using Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model.Dto.TestPapers;
using Model.Entitys;
using Model.Other;

namespace WebApi.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]/[action]")]
public class ExercisesController : ControllerBase

{
    private readonly IExercisesService _exercisesService;

    public ExercisesController(IExercisesService exercisesService)
    {
        _exercisesService = exercisesService;
    }

    /// <summary>
    /// 获取题目
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public Task<ApiResult> GetmMthematics()
    {
        return _exercisesService.GetmMthematics();
    }

    /// <summary>
    /// 成绩提交
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    public Task<ApiResult> Submit(SubmitExercisesReq req)
    {
        return _exercisesService.checkSubmit(req);
    }

    /// <summary>
    /// 成绩中心获取列表
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpGet]
    public Task<ApiResult> GetAchievementCenter([FromQuery] AchievementCenterReq req)
    {
        return _exercisesService.AchievementCenter(req);
    }
    
    [HttpDelete]
    public  async Task<ApiResult> Deleted(long id)
    {
        return await _exercisesService.DeleteService(id);
    }

    [HttpGet]
    public async Task<IActionResult> DownloadWord(long id)
    {
        var byteArray = await _exercisesService.DownloadWord(id);
        return File(byteArray, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "用户作答情况.docx");
    }
}