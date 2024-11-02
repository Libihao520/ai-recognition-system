﻿using System.IdentityModel.Tokens.Jwt;
using Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model.Dto.Role;
using Model.Enum;
using Model.Other;
using WebApi.Config.Authorization;

namespace WebApi.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
[AdminAuthorize(AuthorizeRoleName.Administrator)]
[Authorize]
public class RoleManagementController : ControllerBase
{
    private readonly IRoleManagementService _managementService;

    public RoleManagementController(IRoleManagementService managementService)
    {
        _managementService = managementService;
    }

    /// <summary>
    /// 获取用户角色信息（无id获取列表list，有id获取单个对象）
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ApiResult> GetUserRole(RoleReq req)
    {
        return await _managementService.GetUserRole(req);
    }

    /// <summary>
    /// 删除用户
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete]
    public async Task<ApiResult> Deleted(long id)
    {
        return await _managementService.DeleteUserRoleAsync(id);
    }

    /// <summary>
    /// 更新用户角色（有id是编辑，无id是添加）
    /// </summary>
    /// <param name="res"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task<ApiResult> PutUserRole(PutUserRoleRes res)
    {
        return await _managementService.AddOrUpdateUserRole(res);
    }

    /// <summary>
    /// 批量导入用户
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ApiResult> UploadUserFile(IFormFile file)
    {
        //解析token
        string token = Request.Headers["Authorization"];
        if (token.StartsWith("Bearer "))
        {
            token = token.Substring("Bearer ".Length);
        }
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);
        var CreateUserId = long.Parse( jwtToken.Claims.FirstOrDefault(claim => claim.Type == "Id").Value);
        return await _managementService.ImportUsersFromExcel(file,CreateUserId);
    }

    /// <summary>
    /// 批量导出用户
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> DownloadExcelUsersFromExcel()
    {
        var byteArray = await _managementService.DownloadExcelUsersFromExcel();  
        return File(byteArray, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "用户数据.xlsx"); 
    }

    /// <summary>
    /// 下载导入模板
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> downloadExcelTemplate()
    {
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "ExcelTemplate", "用户管理导入模板.xlsx");
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