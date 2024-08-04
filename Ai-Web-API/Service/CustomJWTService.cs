using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Interface;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Model.Dto.User;
using Model.Other;

namespace Service;

public class CustomJWTService : ICustomJWTService
{
    private readonly JWTTokenOptions _jwtTokenOptions;


    public CustomJWTService(IOptionsMonitor<JWTTokenOptions> jwtTokenOptions)
    {
        _jwtTokenOptions = jwtTokenOptions.CurrentValue;
    }

    public string GetToken(UserRes user)
    {
        #region 有效载荷，想写多少写多少，但尽量避免敏感信息

        var claims = new[]
        {
            new Claim("Id", user.Id.ToString()),
            new Claim("Name", user.Name),
            new Claim("RoleName",user.Role.ToString()),
        };

        //需要加密：需要加密key:
        //Nuget引入：Microsoft.IdentityModel.Tokens
        SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtTokenOptions.SecurityKey));

        SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        //token过期时间，10分钟有效期
        var expires = DateTime.Now.AddMinutes(10);
#if DEBUG
        //调试期间过期时间为100分钟
        expires = DateTime.Now.AddMinutes(100);
#endif

        //Nuget引入：System.IdentityModel.Tokens.Jwt
        JwtSecurityToken token = new JwtSecurityToken(
            issuer: _jwtTokenOptions.Issuer,
            audience: _jwtTokenOptions.Audience,
            claims: claims,
            expires: expires,
            signingCredentials: creds);

        string returnToken = new JwtSecurityTokenHandler().WriteToken(token);
        return returnToken;

        #endregion
    }
}