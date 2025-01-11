namespace Model.Dto.TestPapers;

public class SubMitExercisesReq
{
    /// <summary>
    /// 试卷ID
    /// </summary>
    public long? TestPapersManageId { get; set; }
    public List<int>? SingleChoice { get; set; }
    public List<List<int>>? MultipleChoice { get; set; }
    public List<string>? TrueFalse { get; set; }
}