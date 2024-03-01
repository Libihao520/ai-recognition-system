using System.Text.RegularExpressions;
using AutoMapper;
using EFCoreMigrations;
using Interface;
using Model.Dto.User;
using Model.Entitys;
using Model.Other;
using Service.Utils;
using WebApi.Config;

namespace Service;

public class UserService : IUserService
{
    private readonly IMapper _mapper;

    private MyDbContext _context;

    public UserService(IMapper mapper, MyDbContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public UserRes GetUser(string userName, string passWord)
    {
        var users = _context.users.Where(u => u.Name == userName && u.Password == passWord).FirstOrDefault();
        if (users != null)
        {
            return _mapper.Map<UserRes>(users);
        }

        return new UserRes();
    }

    public async Task<string> add(UserAdd userAdd)
    {
        if (string.IsNullOrWhiteSpace(userAdd.Password) || string.IsNullOrWhiteSpace(userAdd.RePassword))
        {
            return "密码为空";
        }

        if (userAdd.Password != userAdd.RePassword)
        {
            return "两次输入的密码不一致";
        }

        if (userAdd.Email != null && userAdd.Username != null)
        {
            var password = AesUtilities.Decrypt(userAdd.Password);
            var rePassword = AesUtilities.Decrypt(userAdd.RePassword);
            var email = AesUtilities.Decrypt(userAdd.Email);
        }

        users user = new users()
        {
            Name = userAdd.Username,
            Password = userAdd.Password,
            CreateDate = DateTime.Now,
            CreateUserId = 0,
            IsDeleted = 0
        };
        _context.users.Add(user);
        _context.SaveChanges();
        return "注册成功！";
    }

    /// <summary>
    /// 发送邮箱验证码
    /// </summary>
    /// <returns></returns>
    public async Task<ApiResult> SendVerificationCode(string email)
    {
        var decodeEmail = AesUtilities.Decrypt(email);
        if (!IsValidEmail(decodeEmail))
        {
            return ResultHelper.Error("请输入正确格式的邮箱!");
        }
        string randomId = RandomIdGenerator.GenerateRandomId(6);  
        //发送邮箱
        EmailUtil.NetSendEmail($"欢迎注册通用管理系统,您的激活码是：{randomId},激活码有效期至-{DateTime.Now}", "通用管理系统注册",
            decodeEmail);

        return ResultHelper.Success("注册成功，尽快激活！", $"验证码已经发送到您输入的邮箱{decodeEmail}中！");
    }

    /// <summary>
    /// 验证邮箱格式工具
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    static bool IsValidEmail(string email)
    {
        string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        return Regex.IsMatch(email, pattern);
    }
}