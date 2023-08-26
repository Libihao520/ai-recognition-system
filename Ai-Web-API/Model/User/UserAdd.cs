using System.ComponentModel.DataAnnotations;

namespace Model.User;

public class UserAdd
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
}