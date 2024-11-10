using System.Text.Json;
using AutoMapper;
using EFCoreMigrations;
using Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Model;
using Model.Dto.TestPapers;
using Model.Entitys;
using Model.Other;
using Service.Common;

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
        var reportCards = _context.ReportCards.Where(p => p.IsDeleted == 0);

        var total = await reportCards.CountAsync();
        var paginatedResult = await reportCards
            .Skip((req.pagenum - 1) * req.pagesize) // 跳过前面的记录  
            .Take(req.pagesize) // 取接下来的指定数量的记录  
            .ToListAsync(); // 转换为列表  
        
        var res = _mapper.Map<List<AchievementCenterRes>>(paginatedResult);
        return ResultHelper.Success("查询成功", res,total);
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

    public async Task<ApiResult> DownloadWrod(long id)
    {
        // string jsonString = reader["SubmissionData"].ToString();  
        // SubmitExercisesReq req = JsonSerializer.Deserialize<SubmitExercisesReq>(jsonString);
        return null;
    }
}