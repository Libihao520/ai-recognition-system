using AutoMapper;
using EFCoreMigrations;
using Interface;
using Model;
using Model.Dto.TestPapers;
using Model.Entitys;
using Model.Other;

namespace Service;

public class ExercisesService : IExercisesService

{
    private MyDbContext _context;
    private readonly IMapper _mapper;

    public ExercisesService(MyDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
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
        //做对的题数
        int count = 0;
        var testPapersList = _context.testpapers.Where(x => x.subject == "数学").ToList();
        var singleChoices = testPapersList.Where(q => q.type == (int)ExercisesType.单选题)
            .Select(p => new { p.TopicNumber, answer = p.answer[0] })
            .OrderBy(s => s.TopicNumber)
            .ToList();

        int index = 0;
        foreach (var singleChoice in singleChoices)
        {
            if (singleChoice.answer == req.singleChoice[index])
            {
                count++;
            }

            index++;
        }

        return ResultHelper.Success("成功！", "做对题目为：" + count);
    }
}