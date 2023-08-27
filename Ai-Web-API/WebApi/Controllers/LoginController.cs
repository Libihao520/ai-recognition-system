using Interface;
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

    [HttpPost]
    public async Task<ApiResult> add(UserAdd userAdd)
    {
        return ResultHelper.Success("添加成功！",await _userService.add(userAdd));
    }
}