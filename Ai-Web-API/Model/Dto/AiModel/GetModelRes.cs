namespace Model.Dto.AiModel;

public class GetModelRes
{
    /// <summary>
    /// 主键
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 创建人Id
    /// </summary>
    public long CreateUserId { get; set; }

    /// <summary>
    /// 创建日期
    /// </summary>
    public DateTime CreateDate { get; set; }

    /// <summary>
    /// 模型类型
    /// </summary>
    public string? ModleCls { get; set; }

    /// <summary>
    /// 模型名称
    /// </summary>
    public string? ModelName { get; set; }

    /// <summary>
    /// 模型大小
    /// </summary>
    public float? ModelSizee { get; set; }

    /// <summary>
    /// 模型地址
    /// </summary>
    public string? Path { get; set; }
}