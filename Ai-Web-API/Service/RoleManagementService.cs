using AutoMapper;
using CommonUtil;
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
        if (req.Id == null)
        {
            var usersList = _context.Users.Where(q => q.IsDeleted == 0).ToList();
            if (!string.IsNullOrEmpty(req.username))
            {
                usersList = usersList.Where(q => q.Name.Contains(req.username)).ToList();
            }

            var resList = _mapper.Map<List<RoleRes>>(usersList);
            return ResultHelper.Success("查询成功", resList);
        }
        else
        {
            var usersEnumerable = _context.Users.Where(u => u.Id == req.Id).FirstOrDefault();
            if (usersEnumerable == null)
            {
                return ResultHelper.Error("获取用户信息失败！");
            }

            var res = _mapper.Map<RoleRes>(usersEnumerable);
            res.Password = AesUtilities.Decrypt(res.Password);
            return ResultHelper.Success("查询成功", res);
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

    public async Task<ApiResult> PutUserRole(PutUserRoleRes res)
    {
        try
        {
            //有id是编辑，无id是新增用户
            if (res.Id == null)
            {
                var user = _context.Users.Where(q => q.Name == res.Name).FirstOrDefault();
                if (user != null)
                {
                    return ResultHelper.Error("用户名称已被注册了，请换一个！");
                }

                //TODO 验证邮箱是否合规（最好是将注册的验证邮箱抽出来通用）
                Users insterUser = new Users()
                {
                    Name = res.Name,
                    Password = res.Password,
                    Email = res.Email,
                    CreateDate = DateTime.Now,
                    CreateUserId = 0,
                    IsDeleted = 0
                };
                _context.Users.Add(insterUser);
                _context.SaveChanges();
                return ResultHelper.Success("请求成功！", "新增用户成功！");
            }
            else
            {
                var user = _context.Users.Where(q => q.Id == res.Id).FirstOrDefault();
                if (user == null)
                {
                    return ResultHelper.Error("用户不存在 ");
                }
                else
                {
                    user.Password = res.Password;
                    user.Email = res.Email;
                    await _context.SaveChangesAsync();
                    return ResultHelper.Success("请求成功！", "密码更新完成");
                }
            }
        }
        catch (Exception e)
        {
            return ResultHelper.Error("更新密码时发生错误");
        }
    }
}