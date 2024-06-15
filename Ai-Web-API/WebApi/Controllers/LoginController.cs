using System.IdentityModel.Tokens.Jwt;
using Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model.Dto.User;
using Model.Other;
using WebApi.Config;

namespace WebApi.Controllers;
[Route("api/[controller]/[action]")]
[ApiController]
public class LoginController:ControllerBase
{
    private IUserService _userService;
    private ICustomJWTService _jwtService;
    public LoginController(IUserService userService, ICustomJWTService jwtService)
    {
        _userService = userService;
        _jwtService = jwtService;
    }
    /// <summary>
    /// 登录接口
    /// </summary>
    /// <param name="userReq"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ApiResult> GetToken(UserReq userReq)
    {
        var res = Task.Run(() =>
        {
            if (string.IsNullOrEmpty(userReq.username) || string.IsNullOrEmpty(userReq.Password))
            {
                return ResultHelper.Error("参数不能为空");
            }
            UserRes user = _userService.GetUser(userReq.username, userReq.Password);
            if (string.IsNullOrEmpty(user.Name))
            {
                return ResultHelper.Error("账号不存在，用户名或密码错误！");
            }
            return ResultHelper.Success("登录成功！",_jwtService.GetToken(user));
        });
        return await res;
    }

    /// <summary>
    /// 注册接口
    /// </summary>
    /// <param name="userAdd"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ApiResult> add(UserAdd userAdd)
    {
        var res = await _userService.add(userAdd);
        if (res == "注册成功")
        {
        return ResultHelper.Success("添加成功！",$"添加成功：{userAdd.username}");
            
        }
        else
        {
            return ResultHelper.Error("用户已存在");
        }
    }
    
    [HttpGet]
    [Authorize]
    public async Task<ApiResult> userinfo( )
    {
        //解析token
        string token = Request.Headers["Authorization"];
        if (token.StartsWith("Bearer "))
        {
            token = token.Substring("Bearer ".Length);
        }
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);
        var nameClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "Name").Value;
        var userRes = new UserRes()
        {
            Name = nameClaim
        };
        return ResultHelper.Success("成功！",userRes);
    }
}