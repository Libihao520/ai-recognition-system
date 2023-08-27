using Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model.Other;
using WebApi.Config;

namespace WebApi.Controllers;
[Route("api/[controller]/[action]")]
[ApiController]
[Authorize]
public class YoloController : ControllerBase
{
    private IYoloService _yoloService;

    public YoloController(IYoloService yoloService)
    {
        _yoloService = yoloService;
    }

    [HttpGet]
    public ApiResult yolopkq()
    {
        return ResultHelper.Success("获取成功！",_yoloService.getpkqTb());
    }
}