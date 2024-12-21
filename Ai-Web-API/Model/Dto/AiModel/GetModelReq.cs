using Model.Common;

namespace Model.Dto.AiModel;

public class GetModelReq : paging
{
    /// <summary>
    /// 模型类型
    /// </summary>
    public string? ModleCls { get; set; }

    /// <summary>
    /// 模型名称
    /// </summary>
    public string? ModelName { get; set; }
}