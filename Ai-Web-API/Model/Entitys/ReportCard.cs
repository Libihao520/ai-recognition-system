using Model.Common;
using Newtonsoft.Json.Linq;

namespace Model.Entitys;

/// <summary>
/// 成绩
/// </summary>
public class ReportCard : IEntity
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

    /// <summary>
    /// 提交的答案
    /// </summary>
    public string?  SubmittedOptions { get; set; }
}