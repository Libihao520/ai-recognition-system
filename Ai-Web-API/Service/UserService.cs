using System.IdentityModel.Tokens.Jwt;
using System.Text.RegularExpressions;
using AutoMapper;
using Azure.Core;
using CommonUtil;
using CommonUtil.RedisUtil;
using EFCoreMigrations;
using Interface;
using Microsoft.AspNetCore.Http;
using Model;
using Model.Consts;
using Model.Dto.photo;
using Model.Dto.User;
using Model.Entitys;
using Model.Enum;
using Model.Other;
using Service.Common;

namespace Service;

public class UserService : IUserService
{
    private readonly IMapper _mapper;
    private MyDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserService(IMapper mapper, MyDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _mapper = mapper;
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public UserRes GetUser(string userName, string passWord)
    {
        var users = _context.Users.Where(u => u.Name == userName && u.Password == passWord).FirstOrDefault();
        if (users != null)
        {
            return _mapper.Map<UserRes>(users);
        }

        return new UserRes();
    }

    public async Task<ApiResult> Add(UserAdd userAdd)
    {
        if (string.IsNullOrWhiteSpace(userAdd.Password) || string.IsNullOrWhiteSpace(userAdd.rePassword))
        {
            return ResultHelper.Error("密码为空!");
        }

        if (userAdd.Password != userAdd.rePassword)
        {
            return ResultHelper.Error("两次输入的密码不一致!");
        }

        if (string.IsNullOrWhiteSpace(userAdd.Email))
        {
            return ResultHelper.Error("邮箱为空！");
        }

        if (string.IsNullOrWhiteSpace(userAdd.username))
        {
            return ResultHelper.Error("用户名为空！");
        }

        if (_context.Users.Any(u => u.Name == userAdd.username && u.IsDeleted == 0))
        {
            return ResultHelper.Error("用户名已被注册，请换一个！");
        }

        var password = AesUtilities.Decrypt(userAdd.Password);
        var decodeEmail = AesUtilities.Decrypt(userAdd.Email);

        if (!IsValidEmail(decodeEmail))
        {
            return ResultHelper.Error("请输入正确格式的邮箱!");
        }

        if (_context.Users.Any(u => u.Email == decodeEmail && u.IsDeleted == 0))
        {
            return ResultHelper.Error("该邮箱已经注册过了！");
        }

        var s = CacheManager.Get<string>(string.Format(RedisKey.UserActiveCode, decodeEmail));
        if (string.IsNullOrWhiteSpace(s))
        {
            return ResultHelper.Error("验证码还未发送或已失效，请再发送一次！");
        }

        if (userAdd.Authcode != s)
        {
            return ResultHelper.Error("验证码错误！");
        }


        var users = _context.Users.Where(u => u.Name == userAdd.username).FirstOrDefault();
        if (users == null)
        {
            Users user = new Users()
            {
                Name = userAdd.username,
                Password = userAdd.Password,
                Email = decodeEmail,
                CreateDate = DateTime.Now,
                CreateUserId = 0,
                IsDeleted = 0
            };
            _context.Users.Add(user);
            _context.SaveChanges();
            return ResultHelper.Success("注册成功！", "验证码正确！已注册成功！  ");
        }
        else
        {
            return ResultHelper.Success("注册失败！", "用户已存在");
        }
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

        if (_context.Users.Any(u => u.Email == decodeEmail && u.IsDeleted == 0))
        {
            return ResultHelper.Error("该邮箱已经注册过了！");
        }

        //查看缓存有没有这条key
        var exist = CacheManager.Exist(string.Format(RedisKey.UserActiveCode, decodeEmail));
        if (exist)
        {
            return ResultHelper.Error("该邮箱的上一条验证码还未失效,请查看您的邮箱继续激活！");
        }

        //将验证码写入缓存，并设置过期时间
        string randomId = RandomIdGenerator.GenerateRandomId(6);
        CacheManager.Set(string.Format(RedisKey.UserActiveCode, decodeEmail), randomId, TimeSpan.FromMinutes(30));

        //发送邮箱
        EmailUtil.NetSendEmail($"欢迎注册AI识别系统,您的验证码是：{randomId},验证码有效期至-{DateTime.Now.AddMinutes(30)}", "AI识别系统注册",
            decodeEmail);

        return ResultHelper.Success("发送成功，尽快验证！", $"验证码已经发送到您的邮箱{decodeEmail}！有效期30分钟");
    }

    /// <summary>
    /// 获取用户信息
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<ApiResult> GetUserInfo()
    {
        var httpContextUser = _httpContextAccessor.HttpContext.User;
        var userId = long.Parse(httpContextUser.Claims.FirstOrDefault(c => c.Type == "Id").Value);
        var user = await _context.Users.FindAsync(userId);
        if (user == null)
        {
            return ResultHelper.Error("用户不存在！");
        }

        var tPhotos = await _context.Photos.FindAsync(user.PhotosId);
        var userRes = new UserRes()
        {
            Id = user.Id,
            Name = user.Name,
            Role = EnumConvert.ConvertRoleNameToString(user.Role),
            Photo = tPhotos?.Photobase64,
            CreateDate = user.CreateDate
        };
        return ResultHelper.Success("成功！", userRes);
    }

    public async Task<ApiResult> PutUserAvatar(PhotoAdd po, CancellationToken cancellationToken)
    {
        var httpContextUser = _httpContextAccessor.HttpContext.User;
        var userIdClaim = httpContextUser.Claims.FirstOrDefault(c => c.Type == "Id");

        if (userIdClaim == null || !long.TryParse(userIdClaim.Value, out long userId))
        {
            return ResultHelper.Error("用户 ID 不存在！");
        }

        var user = await _context.Users.FindAsync(userId, cancellationToken);
        if (user == null)
        {
            return ResultHelper.Error("用户不存在！");
        }

        if (string.IsNullOrEmpty(po.photo))
        {
            return ResultHelper.Error("图片不能为空！");
        }

        // 检查用户是否已经有照片，如果有则更新，没有则创建新的照片实体
        if (user.PhotosId == null)
        {
            var newPhotoId = TimeBasedIdGeneratorUtil.GenerateId();
            user.PhotosId = newPhotoId;

            var newPhoto = new Photos()
            {
                PhotosId = newPhotoId,
                Photobase64 = po.photo
            };
            _context.Photos.Add(newPhoto);
        }
        else
        {
            // 用户已经有照片ID，更新现有照片
            var existingPhoto = await _context.Photos.FindAsync(user.PhotosId, cancellationToken);
            if (existingPhoto == null)
            {
                return ResultHelper.Error("找不到与用户关联的照片！");
            }

            existingPhoto.Photobase64 = po.photo;
        }

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            return ResultHelper.Error("保存头像时出错：" + ex.Message);
        }

        return ResultHelper.Success("请求成功！", "头像更新成功");
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