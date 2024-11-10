namespace Model.Dto.TestPapers;

public class AchievementCenterReq
{
    /// <summary>
    /// 当前页码
    /// </summary>
    public int pagenum { get; set; }

    /// <summary>
    /// 每页条数
    /// </summary>
    public int pagesize { get; set; }
    /// <summary>
    /// 科目
    /// </summary>
    public string? subject { get; set; }
}