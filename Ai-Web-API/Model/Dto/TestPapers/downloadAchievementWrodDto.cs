namespace Model.Dto.TestPapers;

/// <summary>
/// 练题系统导出Dto
/// </summary>
public class DownloadAchievementWordDto
{
    public string? subject { get; set; }

    /// <summary>
    /// 总分
    /// </summary>
    public int totalPoints { get; set; }

    public string? Name { get; set; }

    public DateTime CreateDate { get; set; }

    /// <summary>
    /// 单选题
    /// </summary>
    public List<SingleChoice> singleChoice { get; set; } = new List<SingleChoice>();

    /// <summary>
    /// 多选题
    /// </summary>
    public List<MultipleChoice> multipleChoice { get; set; } = new List<MultipleChoice>();

    /// <summary>
    /// 判断题
    /// </summary>
    public List<TrueFalse> trueFalse { get; set; } = new List<TrueFalse>();
}

