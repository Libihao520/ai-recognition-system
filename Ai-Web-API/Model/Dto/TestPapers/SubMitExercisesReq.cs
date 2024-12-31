namespace Model.Dto.TestPapers;

public class SubMitExercisesReq
{
    public List<int>? SingleChoice { get; set; }
    public List<List<int>>? MultipleChoice { get; set; }
    public List<string>? TrueFalse { get; set; }
}