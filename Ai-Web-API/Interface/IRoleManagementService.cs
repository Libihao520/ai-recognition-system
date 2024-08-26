using Model.Dto.Role;
using Model.Other;

namespace Interface;

public interface IRoleManagementService
{
    public Task<ApiResult> GetUserRole(RoleReq req);
    
    Task<ApiResult> DeleteAsync(long id);

    Task<ApiResult> PutPasswAsync(long id);
}