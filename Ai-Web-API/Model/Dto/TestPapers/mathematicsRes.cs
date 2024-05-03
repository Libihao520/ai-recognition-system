namespace Model.Dto.TestPapers;

public class MathematicsRes
{

    public List<SingleChoice> singleChoice { get; set; }
    public List<MultipleChoice> multipleChoice { get; set; }
    public List<TrueFalse> trueFalse { get; set; }
}

public  class SingleChoice
{
    public string title { get; set; } 
    
    public List<string> options { get; set; }
}

public class MultipleChoice
{
    public string title { get; set; } 
    public List<string> options { get; set; }
}

public class TrueFalse
{
    public string title { get; set; } 
    public List<string> options { get; set; }
}