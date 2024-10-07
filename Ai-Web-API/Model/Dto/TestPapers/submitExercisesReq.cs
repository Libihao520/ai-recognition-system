namespace Model.Dto.TestPapers;

public class SubmitExercisesReq
{
    public List<int> singleChoice { get; set; }
    public List<List<int>> multipleChoice { get; set; }
    public List<string> trueFalse { get; set; }
}