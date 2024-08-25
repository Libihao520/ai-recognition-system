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
}