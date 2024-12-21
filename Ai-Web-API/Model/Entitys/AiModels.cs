using Model.Common;

namespace Model.Entitys;

public class AiModels : IEntity
{
    /// <summary>
    /// 模型地址
    /// </summary>
    public string? Path { get; set; }

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
}