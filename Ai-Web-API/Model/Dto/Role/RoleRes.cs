namespace Model.Dto.Role;

public class RoleRes
{
    /// <summary>  
    /// 用户名  
    /// </summary>  
    public string? Name { get; set; }  
  
    /// <summary>  
    /// 创建日期  
    /// </summary>  
    public DateTime CreateDate { get; set; }  
  
    /// <summary>  
    /// 角色  
    /// </summary>  
    public string? Role { get; set; }  
  
    /// <summary>  
    /// 电子邮件  
    /// </summary>  
    public string? Email { get; set; }
}