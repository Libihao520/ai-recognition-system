using Model.Dto.Role;
using Model.Other;

namespace Interface;

public interface IRoleManagementService
{
    public Task<ApiResult> GetUserRole(RoleReq req);
}