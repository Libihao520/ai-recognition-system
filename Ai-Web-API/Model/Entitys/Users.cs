using System.ComponentModel.DataAnnotations;
using Model.Common;
using Model.Enum;

namespace Model.Entitys;

public class Users : IEntity
{
    /// <summary>
    /// 用户名
    /// </summary>
    [Required]
    public string Name { get; set; }

    /// <summary>
    /// 密码
    /// </summary>
    [Required]
    public string Password { get; set; }

    /// <summary>
    /// 用户角色
    /// </summary>
    public AuthorizeRoleName Role { get; set; } = AuthorizeRoleName.Ordinary;

    /// <summary>
    /// 邮箱
    /// </summary>
    [Required]
    public string Email { get; set; }
}