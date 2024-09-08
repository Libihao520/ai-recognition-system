using System.Text.Json.Serialization;
using Model.Enum;

namespace Model.Dto.Role;

public class PutUserRoleRes
{
    public long? Id { get; set; }
    public string Name { get; set; }

    [JsonConverter(typeof(EnumJsonConverter<AuthorizeRoleName>))]
    public AuthorizeRoleName role { get; set; }

    public string Password { get; set; }

    /// <summary>
    /// 邮箱
    /// </summary>
    public string Email { get; set; }
}