namespace Model.Dto.TestPapers;

public class AchievementCenterRes
{
    /// <summary>
    /// 科目
    /// </summary>
    public string? subject { get; set; }

    /// <summary>
    /// 总分
    /// </summary>
    public int totalPoints { get; set; }

    /// <summary>
    /// 总题目数量
    /// </summary>
    public int NumberOfQuestions { get; set; }

    /// <summary>
    /// 答对数量
    /// </summary>
    public int CorrectQuantity { get; set; }
}