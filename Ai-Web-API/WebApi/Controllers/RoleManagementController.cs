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


    [HttpPost]
    public async Task<ApiResult> GetUserRole(RoleReq req)
    {
        return await _managementService.GetUserRole(req);
    }
    [HttpDelete]
    public async Task<ApiResult> Deleted(long id)
    {
        return await _managementService.DeleteAsync(id);
    }

    [HttpPut]
    public async Task<ApiResult> PutPasswAsync(long id)
    {
        return await _managementService.PutPasswAsync(id);
    }
}