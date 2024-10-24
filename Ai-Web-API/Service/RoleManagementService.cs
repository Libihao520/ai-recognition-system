using System.Net;
using AutoMapper;
using CommonUtil;
using EFCoreMigrations;
using Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Model;
using Model.Dto.Role;
using Model.Dto.User;
using Model.Entitys;
using Model.Enum;
using Model.Other;
using OfficeOpenXml;
using Service.Common;

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
            var userstb = _context.Users.Where(q => q.IsDeleted == 0);
            if (!string.IsNullOrEmpty(req.username))
            {
                userstb = userstb.Where(q => q.Name.Contains(req.username));
            }

            var total = await userstb.CountAsync();

            var paginatedResult = await userstb
                .Skip((req.pagenum - 1) * req.pagesize) // 跳过前面的记录  
                .Take(req.pagesize) // 取接下来的指定数量的记录  
                .ToListAsync(); // 转换为列表  

            var resList = _mapper.Map<List<RoleRes>>(paginatedResult);
            return ResultHelper.Success("查询成功", resList, total);
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

    public async Task<ApiResult> DeleteUserRoleAsync(long id)
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

    public async Task<ApiResult> AddOrUpdateUserRole(PutUserRoleRes res)
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

                //TODO 验证邮箱是否合规（最好是将注册的验证邮箱抽出来通用） CreateUserId采用当前操作角色的id 
                Users insterUser = new Users()
                {
                    Id = TimeBasedIdGeneratorUtil.GenerateId(),
                    Name = res.Name,
                    Password = res.Password,
                    Email = res.Email,
                    CreateDate = DateTime.Now,
                    CreateUserId = 0,
                    Role = res.role,
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
                    user.Role = res.role;
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

    public async Task<ApiResult> ImportUsersFromExcel(IFormFile file,long createUserId)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using (var stream = new MemoryStream())
        {
            await file.CopyToAsync(stream);
            stream.Position = 0; // 重置流的位置到起始点  

            using (var package = new ExcelPackage(stream))
            {
                var worksheet = package.Workbook.Worksheets[0];
                var rowCount = worksheet.Dimension.Rows;

                for (int row = 2; row <= rowCount; row++)
                {
                    var userName = worksheet.Cells[row, 1].Text;
                    var role = EnumConvert.ConvertStringToRoleName(worksheet.Cells[row, 2].Text);
                    var userEmail = worksheet.Cells[row, 3].Text;
                    var userPassword = AesUtilities.Encrypt(worksheet.Cells[row, 4].Text);

                    Users insterUser = new Users()
                    {
                        Id = TimeBasedIdGeneratorUtil.GenerateId(),
                        Name = userName,
                        Password = userPassword,
                        Email = userEmail,
                        CreateDate = DateTime.Now,
                        CreateUserId = createUserId,
                        Role = role,
                        IsDeleted = 0
                    };
                    _context.Users.Add(insterUser);
                }
            }
        }

        _context.SaveChanges();
        return ResultHelper.Success("请求成功！", "导入用户成功！");
    }

    public async Task<byte[]> DownloadExcelUsersFromExcel()
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        var users = await _context.Users.Where(q=>q.IsDeleted == 0).ToListAsync();

        using (var ms = new MemoryStream())
        {
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Users");

                worksheet.Cells[1, 1].Value = "ID";
                worksheet.Cells[1, 2].Value = "用户名";
                worksheet.Cells[1, 3].Value = "邮箱";
                worksheet.Cells[1, 4].Value = "创建时间";

                int row = 2;
                foreach (var user in users)
                {
                    worksheet.Cells[row, 1].Value = user.Id;
                    worksheet.Cells[row, 2].Value = user.Name;
                    worksheet.Cells[row, 3].Value = user.Email;
                    worksheet.Cells[row, 4].Value = user.CreateDate;

                    row++;
                }

                package.SaveAs(ms);
            }

            ms.Seek(0, SeekOrigin.Begin);
            return ms.ToArray();
        }
    }
}