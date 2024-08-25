using Interface;
using Model;
using Model.Dto.Role;
using Model.Other;

namespace Service;

public class RoleManagementService : IRoleManagementService
{
    public async Task<ApiResult> GetUserRole(RoleReq req)
    {
        //TODO
        //根据req里的username返回用户的name,createDate,Tole,email,若id为空则返回所有的
        return ResultHelper.Success("", "");
    }
}