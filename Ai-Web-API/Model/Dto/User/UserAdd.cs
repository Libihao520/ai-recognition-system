using System.ComponentModel.DataAnnotations;

namespace Model.Dto.User;

public class UserAdd
{
    /// <summary>
    /// id
    /// </summary>
    [Required]
    public long Id { get; set; }

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