namespace Model.Entitys;

public class TestPapers
{
    /// <summary>
    /// 试卷ID
    /// </summary>
    public int? testPapersManageId { get; set; }
    
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
    /// 题目类型 0单选，1多选，2判断题
    /// </summary>
    public int type { get; set; }

    /// <summary>
    /// 选择 A B C D
    /// </summary>
    public string? Choice1 { get; set; }

    public string? Choice2 { get; set; }
    public string? Choice3 { get; set; }
    public string? Choice4 { get; set; }

    /// <summary>
    /// 答案
    /// </summary>
    public List<int>? answer { get; set; }

    /// <summary>
    /// 分数
    /// </summary>
    public int Grade { get; set; }
}