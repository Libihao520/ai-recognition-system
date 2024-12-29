using System.Reflection;
using System.Text.Json;
using AutoMapper;
using CommonUtil.Extensions;
using CommonUtil.YoloUtil;
using EFCoreMigrations;
using Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Model;
using Model.Dto.TestPaperManage;
using Model.Dto.TestPapers;
using Model.Entitys;
using Model.Other;
using OfficeOpenXml;
using Service.Common;
using SharpDocx;

namespace Service;

public class ExercisesService : IExercisesService

{
    private MyDbContext _context;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ExercisesService(MyDbContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<ApiResult> GetmMthematics()
    {
        var mathematicsRes = new MathematicsRes()
        {
            singleChoice = new List<SingleChoice>(),
            multipleChoice = new List<MultipleChoice>(),
            trueFalse = new List<TrueFalse>(),
        };

        var testPapersList = _context.testpapers.Where(x => x.subject == "数学").ToList();
        foreach (var testPapers in testPapersList)
        {
            if (testPapers.type == (int)ExercisesType.单选题)
            {
                var map = _mapper.Map<SingleChoice>(testPapers);
                mathematicsRes.singleChoice.Add(map);
            }
            else if (testPapers.type == (int)ExercisesType.多选题)
            {
                var map = _mapper.Map<MultipleChoice>(testPapers);
                mathematicsRes.multipleChoice.Add(map);
            }
            else if (testPapers.type == (int)ExercisesType.判断题)
            {
                var map = _mapper.Map<TrueFalse>(testPapers);
                mathematicsRes.trueFalse.Add(map);
            }
        }

        return ResultHelper.Success("获取成功！", mathematicsRes);
    }

    public async Task<ApiResult> checkSubmit(SubmitExercisesReq req)
    {
        //得分
        int score = 0;

        var testPapersList = _context.testpapers.Where(x => x.subject == "数学").ToList();
        var singleChoices = testPapersList.Where(q => q.type == (int)ExercisesType.单选题)
            .Select(p => new { p.TopicNumber, answer = p.answer[0], p.Grade })
            .OrderBy(s => s.TopicNumber)
            .ToList();

        var singleChoiceCount = singleChoices.Count();
        for (int i = 0; i < Math.Min(singleChoices.Count, req.singleChoice.Count); i++)
        {
            if (singleChoices[i].answer == req.singleChoice[i])
            {
                score = score + singleChoices[i].Grade;
                singleChoiceCount++;
            }
        }

        // 处理多选题
        var multipleChoices = testPapersList.Where(m => m.type == (int)ExercisesType.多选题)
            .Select(m => new { m.TopicNumber, CorrectAnswer = m.answer, m.Grade })
            .OrderBy(s => s.TopicNumber)
            .ToList();
        int multipleChoiceCount = 0;
        for (int i = 0; i < Math.Min(multipleChoices.Count, req.multipleChoice.Count); i++)
        {
            if (multipleChoices[i].CorrectAnswer.SequenceEqual(req.multipleChoice[i]))
            {
                score = score + multipleChoices[i].Grade;
                multipleChoiceCount++;
            }
        }

        //处理判断题
        var testPapersEnumerable = testPapersList.Where(p => p.type == (int)ExercisesType.判断题)
            .Select(p => new { p.TopicNumber, Answer = p.answer[0] == 1 ? "true" : "false", p.Grade })
            .OrderBy(p => p.TopicNumber)
            .ToList();
        int trueFalseCount = 0;
        for (int i = 0; i < Math.Min(testPapersEnumerable.Count, req.trueFalse.Count); i++)
        {
            if (testPapersEnumerable[i].Answer == req.trueFalse[i])
            {
                score = score + testPapersEnumerable[i].Grade;
                trueFalseCount++;
            }
        }

        var user = _httpContextAccessor.HttpContext.User;
        var createUserId = long.Parse(user.Claims.FirstOrDefault(c => c.Type == "Id").Value);

        ReportCard reportCard1 = new ReportCard
        {
            Id = TimeBasedIdGeneratorUtil.GenerateId(),

            CreateDate = DateTime.Now,

            CreateUserId = createUserId,

            subject = "数学",
            //总分
            totalPoints = score,
            //总数
            NumberOfQuestions = singleChoices.Count + multipleChoices.Count + testPapersEnumerable.Count,
            // 答对数
            CorrectQuantity = multipleChoiceCount + trueFalseCount + singleChoiceCount,
            // 提交的答案
            SubmittedOptions = JsonSerializer.Serialize(req)
        };
        _context.ReportCards.Add(reportCard1);
        _context.SaveChanges();
        return ResultHelper.Success("成功！", @$"本次答题得分为：{score} ,具体情况前往成绩中心查看！");
    }

    public async Task<ApiResult> AchievementCenter(AchievementCenterReq req)
    {
        var query = from reportCards in _context.ReportCards.Where(p => p.IsDeleted == 0)
            join Users in _context.Users on reportCards.CreateUserId equals Users.Id into usersGroup
            from userItem in usersGroup.DefaultIfEmpty()
            select new AchievementCenterRes
            {
                Id = reportCards.Id,
                subject = reportCards.subject,
                totalPoints = reportCards.totalPoints,
                CreateDate = reportCards.CreateDate,
                NumberOfQuestions = reportCards.NumberOfQuestions,
                CorrectQuantity = reportCards.CorrectQuantity,
                CreateName = userItem == null ? null : userItem.Name,
            };

        var total = await query.CountAsync();
        var paginatedResult = await query
            .Skip((req.pagenum - 1) * req.pagesize)
            .Take(req.pagesize)
            .ToListAsync();

        return ResultHelper.Success("查询成功", paginatedResult, total);
    }

    public async Task<ApiResult> DeleteService(long id)
    {
        try
        {
            var reportCard = await _context.ReportCards.FindAsync(id);
            // 不为空则执行软删除并且保存到数据库中
            if (reportCard != null)
            {
                reportCard.IsDeleted = 1;
                await _context.SaveChangesAsync();
            }

            return ResultHelper.Success("请求成功！", "数据已删除");
        }
        catch (Exception e)
        {
            return ResultHelper.Error("数据删除失败!");
        }
    }

    public async Task<byte[]> DownloadWord(long id)
    {
        var reportCard = await _context.ReportCards.FindAsync(id);
        SubmitExercisesReq submitExercisesReq =
            JsonSerializer.Deserialize<SubmitExercisesReq>(reportCard.SubmittedOptions);
        var testPapersList = await _context.testpapers.Where(q => q.subject == reportCard.subject).ToListAsync();
        var user = await _context.Users.FindAsync(reportCard.CreateUserId);


        var model = _mapper.Map<DownloadAchievementWordDto>(reportCard);
        model = _mapper.Map(user, model);

        foreach (var testPapers in testPapersList)
        {
            if (testPapers.type == (int)ExercisesType.单选题)
            {
                var map = _mapper.Map<SingleChoice>(testPapers);
                map.answer = testPapers.answer[0].ToLetter();
                map.subAnswer = submitExercisesReq.singleChoice[testPapers.TopicNumber - 1].ToLetter();
                model.singleChoice.Add(map);
            }
            else if (testPapers.type == (int)ExercisesType.多选题)
            {
                var map = _mapper.Map<MultipleChoice>(testPapers);
                map.answer = string.Join(", ", testPapers.answer.Select(a => a.ToLetter().ToString()));
                map.subAnswer = string.Join(", ",
                    submitExercisesReq.multipleChoice[testPapers.TopicNumber - 1].Select(a => a.ToLetter().ToString()));
                model.multipleChoice.Add(map);
            }
            else if (testPapers.type == (int)ExercisesType.判断题)
            {
                var map = _mapper.Map<TrueFalse>(testPapers);
                map.answer = testPapers.answer[0] == 1 ? "正确" : "错误";
                map.subAnswer = submitExercisesReq.singleChoice[testPapers.TopicNumber - 1] == 1 ? "正确" : "错误";
                model.trueFalse.Add(map);
            }
        }


        var directoryName =
            Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty,
                "ExcelTemplate");
        var document = DocumentFactory.Create(Path.Combine(directoryName, "练题系统答题情况导出模板.docx"), model);
        var generateId = TimeBasedIdGeneratorUtil.GenerateId().ToString();
        document.Generate(Path.Combine(directoryName, @$"{generateId}结果.docx"));

        using var fileStream = new FileStream(Path.Combine(directoryName, @$"{generateId}结果.docx"), FileMode.Open,
            FileAccess.Read);
        using (var ms = new MemoryStream())
        {
            await fileStream.CopyToAsync(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return ms.ToArray();
        }
    }

    public async Task<ApiResult> GetTestPaperManage(GetTestPaperManageReq req)
    {
        try
        {
            var queryable = _context.TestPapersManages.Where(t => t.IsDeleted == 0).AsQueryable();
            //string.IsNullOrEmpty()判断是否空，空返回true,有值是false
            //!string.IsNullOrEmpty()判断是否空，空返回false,有值是true
            //if(true){才执行}
            if (!string.IsNullOrEmpty(req.FileLabel))
            {
                queryable = queryable.Where(t => t.FileLabel.Contains(req.FileLabel));
            }

            if (!string.IsNullOrEmpty(req.QuestionBankCourseTitle))
            {
                queryable = queryable.Where(t => t.QuestionBankCourseTitle.Contains(req.QuestionBankCourseTitle));
            }

            var total = await queryable.CountAsync();

            var listAsync = await queryable
                .Skip((req.pagenum - 1) * req.pagesize)
                .Take(req.pagesize)
                .ToListAsync();

            var resultList = _mapper.Map<List<TestPaperManageRes>>(listAsync);

            return ResultHelper.Success("查询成功", resultList, total);
        }

        catch (Exception e)
        {
            Console.WriteLine("$发生异常");
            return ResultHelper.Error("查询异常，请稍后重试");
        }
    }

    public async Task<ApiResult> AddTestPaperManage(AddTestPaperManageReq req)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        //TODO 在TestPaperManage 新建一条数据 ，然后读取excel，把数据存到TestPaper
        try
        {
            string filePath = "";
            if (req.File != null)
            {
                //得：唯一文件名+扩展名
                string fileName = req.FileLabel + "_" + TimeBasedIdGeneratorUtil.GenerateId() + "_" +
                                  Path.GetExtension(req.File.FileName);
                //获取项目下现在的目录以及TempExcelFiles文件夹
                string saveDirectory = Path.Combine(Directory.GetCurrentDirectory(), "TempExcelFiles");
                Directory.CreateDirectory(saveDirectory);

                filePath = Path.Combine(saveDirectory, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await req.File.CopyToAsync(stream);
                }
            }

            var user = _httpContextAccessor?.HttpContext?.User;
            var createUserId = long.Parse(user.Claims.FirstOrDefault(c => c.Type == "Id").Value);
            var newRecord = new TestPapersManage()
            {
                Id = TimeBasedIdGeneratorUtil.GenerateId(),
                FileLabel = req.FileLabel,
                QuestionBankCourseTitle = req.QuestionBankCourseTitle,
                ExcelFilePath = filePath,
                CreateUserId = createUserId,
                CreateDate = DateTime.Now,
                IsDeleted = 0,
                HasAnsweringStarted = null
            };
            _context.TestPapersManages.Add(newRecord);

            //检查指定路径filePath的文件是否存在==>往下AI--注释占时不要删，还要看

            using (var stream = new MemoryStream())
            {
                await req.File.CopyToAsync(stream);
                stream.Position = 0; // 重置流的位置到起始点  

                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets[0];
                    var rowCount = worksheet.Dimension.Rows;

                    for (int row = 2; row <= rowCount; row++)
                    {
                        var paperData = new TestPapers()
                        {
                            TopicNumber = row - 1,
                            subject = req.QuestionBankCourseTitle,
                            type = (worksheet.Cells[row, 2].Value is int cellValueIntForType) ? cellValueIntForType : 0,
                            Topic = worksheet.Cells[row, 3].Value?.ToString(),
                            Choice1 = worksheet.Cells[row, 4].Value?.ToString(),
                            Choice2 = worksheet.Cells[row, 5].Value?.ToString(),
                            Choice3 = worksheet.Cells[row, 6].Value?.ToString(),
                            Choice4 = worksheet.Cells[row, 7].Value?.ToString(),
                            answer = new List<int>(),
                            Grade = int.Parse(worksheet.Cells[row, 9].Value?.ToString() ?? string.Empty),
                            testPapersManageId = newRecord.Id
                        };
                        paperData.answer =
                            ExcelDataParser.ParseAnswerFromCellValue(worksheet.Cells[row, 8].Value?.ToString());
                        _context.testpapers.Add(paperData);
                    }
                }
            }

            await _context.SaveChangesAsync();

            return ResultHelper.Success("请求成功", "文件上传成功且数据保存完整");
        }
        catch (Exception e)
        {
            return ResultHelper.Error($"添加失败原因：{e.Message}");
        }
    }

    public async Task<ApiResult> GetSubjectsOrFileLabel(string? fileLabel)
    {
        if (string.IsNullOrEmpty(fileLabel))
        {
            var list = _context.TestPapersManages.Select(q => q.QuestionBankCourseTitle).Distinct().ToList();
            var resultList = list.Select((title, index) => new Dictionary<string, object>
            {
                { "label", title },
                { "value", index + 1 }
            }).ToList();
            return ResultHelper.Success("请求成功！", resultList);
        }
        else
        {
            return ResultHelper.Success("", "");

        }
    }
}