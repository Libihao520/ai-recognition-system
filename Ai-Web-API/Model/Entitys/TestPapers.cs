namespace Model.Entitys;

public class TestPapers
{
    public int id { get; set; }

    /// <summary>
    /// 科目
    /// </summary>
    public string subject { get; set; }

    /// <summary>
    /// 题目编号
    /// </summary>
    public int TopicNumber { get; set; }

    /// <summary>
    /// 题目
    /// </summary>
    public string Topic { get; set; }

    /// <summary>
    /// 题目类型
    /// </summary>
    public string type { get; set; } 
    
    public string Choice1 { get; set; }
    public string Choice2 { get; set; }
    public string Choice3 { get; set; }
    public string Choice4 { get; set; }

    public int answer { get; set; }
}