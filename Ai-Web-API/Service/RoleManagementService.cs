using AutoMapper;
using EFCoreMigrations;
using Interface;
using Model;
using Model.Dto.Role;
using Model.Dto.User;
using Model.Entitys;
using Model.Other;

namespace Service;

public class RoleManagementService : IRoleManagementService
{
    private MyDbContext _context;
    private IMapper _mapper;
    
    public RoleManagementService(MyDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ApiResult> GetUserRole(RoleReq req)
    {
        //TODO
        //根据req里的username返回用户的name,createDate,Tole,email,若id为空则返回所有的
        if (string.IsNullOrEmpty(req.username))
        {
            var usersList = _context.Users.ToList();
            var resList = _mapper.Map<List<RoleRes>>(usersList);
            return ResultHelper.Success("查询成功", resList);
        }
        else
        {
            var usersEnumerable = _context.Users.Where(u => u.Name == req.username).ToList();
            var resList = _mapper.Map<List<RoleRes>>(usersEnumerable);
            return ResultHelper.Success("查询成功", resList);
        }

    }
}