using Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model.Dto.TestPaperManage;
using Model.Dto.TestPapers;
using Model.Entities;
using Model.Other;

namespace WebApi.Controllers;

[ApiController]
/*[Authorize]*/
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
    public Task<ApiResult> Submit(SubMitExercisesReq req)
    {
        return _exercisesService.checkSubmit(req);
    }

    /// <summary>
    /// 成绩中心获取列表
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpGet]
    public Task<ApiResult> GetAchievementCenter([FromQuery] GetAchievementCenterReq req)
    {
        return _exercisesService.AchievementCenter(req);
    }

    [HttpDelete]
    public async Task<ApiResult> Deleted(long id)
    {
        return await _exercisesService.DeleteService(id);
    }

    [HttpGet]
    public async Task<IActionResult> DownloadWord(long id)
    {
        var byteArray = await _exercisesService.DownloadWord(id);
        return File(byteArray, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "用户作答情况.docx");
    }

    /// <summary>
    /// 获取题库信息
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<ApiResult> GetTestPaperManage([FromQuery] GetTestPaperManageReq req)
    {
        return await _exercisesService.GetTestPaperManage(req);
    }

    /// <summary>
    /// 批量导入题库
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ApiResult> AddTestPaperManage([FromForm] AddTestPaperManageReq req)
    {
        return await _exercisesService.AddTestPaperManage(req);
    }

    [HttpGet]
    public async Task<ApiResult> GetSubjectsOrFileLabel([FromQuery] string? subjectName)
    {
        return await _exercisesService.GetSubjectsOrFileLabel(subjectName);
    }

    [HttpGet]
    public async Task<IActionResult> DownloadExcelTemplate()
    {
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "ExcelTemplate", "题库导入模板.xlsx");
        // 检查文件是否存在  
        if (!System.IO.File.Exists(filePath))
        {
            return NotFound("File not found");
        }

        // 读取文件内容  
        var memory = new MemoryStream();
        using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
        {
            await stream.CopyToAsync(memory);
        }

        memory.Position = 0;

        // 返回文件内容作为HTTP响应  
        return File(memory, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            Path.GetFileName(filePath));
    }
}