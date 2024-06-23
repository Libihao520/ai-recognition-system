namespace Model.Dto.TestPapers;

public class MathematicsRes
{
    /// <summary>
    /// 单选题
    /// </summary>
    public List<SingleChoice> singleChoice { get; set; }

    /// <summary>
    /// 多选题
    /// </summary>
    public List<MultipleChoice> multipleChoice { get; set; }

    /// <summary>
    /// 判断题
    /// </summary>
    public List<TrueFalse> trueFalse { get; set; }
}

public class SingleChoice
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