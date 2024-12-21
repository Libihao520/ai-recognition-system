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
    private readonly IAigcSerevice _aigcSerevice;

    public AigcController(IAigcSerevice aigcSerevice)
    {
        _aigcSerevice = aigcSerevice;
    }

    [HttpGet]
    public Task<ApiResult> GetModelService([FromQuery] GetModelReq req)
    {
        return _aigcSerevice.GetModelService(req);
    }

    [HttpPut]
    public Task<ApiResult> PutModelService([FromForm] PutModelReq req)
    {
        return _aigcSerevice.PutModelService(req);
    }

    [HttpDelete]
    public Task<ApiResult> DelModelService(long id)
    {
        return _aigcSerevice.DelModelService(id);
    }
}