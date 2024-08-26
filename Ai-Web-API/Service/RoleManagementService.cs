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

    public async Task<ApiResult> DeleteAsync(long id)
    {
        try
        {
            // 根据id查找对象
            var findAsync = await _context.Users.FindAsync(id);
            // 不为空则执行软删除并且保存到数据库中
            if (findAsync != null)
            {
                findAsync.IsDeleted = 1;
                await _context.SaveChangesAsync();
            }

            return ResultHelper.Success("请求成功！", "数据已删除");
        }
        catch (Exception e)
        {
            return ResultHelper.Error("用户数据删除失败");
        }
    }

    public async Task<ApiResult> PutPasswAsync(long id)
    {
        try
        {
            var findAsync = _context.Users.FindAsync(id);
            if (findAsync == null)
            {
                return ResultHelper.Error("用户不存在 ");
            }
          
            await _context.SaveChangesAsync();
            return ResultHelper.Success("请求成功！", "密码更新完成");
        }
        catch (Exception e)
        {
            return ResultHelper.Error("更新密码时发生错误");
        }
    }
}

    