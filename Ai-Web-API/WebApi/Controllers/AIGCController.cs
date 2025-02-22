using Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model.Dto.AiModel;
using Model.Other;

namespace WebApi.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]/[action]")]
public class AigcController : ControllerBase
{
    private readonly IAiGcService _aiGcService;

    public AigcController(IAiGcService aiGcService)
    {
        _aiGcService = aiGcService;
    }

    [HttpGet]
    public Task<ApiResult> GetModelService([FromQuery] GetModelReq req)
    {
        return _aiGcService.GetModelService(req);
    }

    [HttpPost]
    [RequestFormLimits(MultipartBodyLengthLimit = 262144000)] // 设置表单数据的最大长度为250MB
    [RequestSizeLimit(262144000)] // 设置整个HTTP请求的最大大小为250MB
    public async Task<ApiResult> PutModelService([FromForm] PutModelReq req)
    {
        return await _aiGcService.PutModelService(req);
    }

    [HttpDelete]
    public Task<ApiResult> DelModelService(long id)
    {
        return _aiGcService.DelModelService(id);
    }

    [HttpGet]
    public Task<ApiResult> QuestionsAndAnswers([FromQuery]string q)
    {
        return _aiGcService.QuestionsAndAnswers(q);
    }
}