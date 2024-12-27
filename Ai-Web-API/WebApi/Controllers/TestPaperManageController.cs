using Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model.Dto.TestPaperManage;
using Model.Other;

namespace WebApi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]/[action]")]
public class TestPaperManageController:ControllerBase
{
    private readonly ITestPaperManageService _testPaperManageService;

    public TestPaperManageController(ITestPaperManageService testPaperManageService)
    {
        _testPaperManageService = testPaperManageService;
    }

    /// <summary>
    /// 获取Excel题库信息
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ApiResult> GetQuestionBank(TestPaperManageReq req)
    {
        return await _testPaperManageService.GetBankRole(req);
    }
    
}