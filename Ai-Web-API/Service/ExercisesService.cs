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
            }else if (testPapers.type == (int)ExercisesType.多选题)
            {
                var map = _mapper.Map<MultipleChoice>(testPapers);
                mathematicsRes.multipleChoice.Add(map);
            }else if (testPapers.type == (int)ExercisesType.判断题)
            {
                var map = _mapper.Map<TrueFalse>(testPapers);
                mathematicsRes.trueFalse.Add(map);
            }
            
        }
            
        return ResultHelper.Success("获取成功！", mathematicsRes);
    }
}