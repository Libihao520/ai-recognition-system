using Model.Dto.AiModel;
using Model.Other;

namespace Interface;

public interface IAigcSerevice
{
    /// <summary>
    /// 查询所有模型
    /// </summary>
    /// <returns></returns>
    Task<ApiResult> GetModelService(GetModelReq req);

    /// <summary>
    /// 上传模型
    /// </summary>
    /// <returns></returns>
    Task<ApiResult> PutModelService(PutModelReq req);

    /// <summary>
    /// 删除模型
    /// </summary>
    /// <returns></returns>
    Task<ApiResult> DelModelService(long id);
}