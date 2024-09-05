using Interface;
using Microsoft.AspNetCore.Mvc;
using Model.Dto.Role;
using Model.Other;

namespace WebApi.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
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
        return await _managementService.DeleteAsync(id);
    }

    /// <summary>
    /// 更新用户角色（有id是编辑，无id是添加）
    /// </summary>
    /// <param name="res"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task<ApiResult> PutUserRole(PutUserRoleRes res)
    {
        return await _managementService.PutUserRole(res);
    }
}