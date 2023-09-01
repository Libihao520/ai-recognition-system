using Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model.Dto.photo;
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

    /// <summary>
    /// 获取皮卡丘表单数据
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public ApiResult yolopkq()
    {
        return ResultHelper.Success("获取成功！", _yoloService.getpkqTb());
    }
    /// <summary>
    /// 传入照片识别接口，返回照片
    /// </summary>
    /// <param name="po"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task<ApiResult> PutPhoto(PhotoAdd po)
    {

        return ResultHelper.Success("识别成功！",await _yoloService.PutPhoto(po));
    }
}