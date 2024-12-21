namespace Model.Dto.AiModel;

public class GetModelReq
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
    /// 模型类型
    /// </summary>
    public string? ModleCls { get; set; }

    /// <summary>
    /// 模型名称
    /// </summary>
    public string? ModelName { get; set; }
}