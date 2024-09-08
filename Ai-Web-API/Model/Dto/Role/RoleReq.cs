namespace Model.Dto.Role;

/// <summary>
/// 角色管理获取用户请求
/// </summary>
public class RoleReq
{
    /// <summary>
    /// 当前页码
    /// </summary>
    public int pagenum { get; set; }

    /// <summary>
    /// 每页条数
    /// </summary>
    public int pagesize { get; set; }
    /// <summary>
    /// 用户id
    /// </summary>
    public long? Id { get; set; }

    /// <summary>
    /// 用户名
    /// </summary>
    public string? username { get; set; }
}