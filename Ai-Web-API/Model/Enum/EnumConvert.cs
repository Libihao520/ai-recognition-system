namespace Model.Enum;

public class EnumConvert
{
    
    public static string ConvertRoleNameToString(AuthorizeRoleName role)
    {
        switch (role)
        {
            case AuthorizeRoleName.Administrator:
                return "超级管理员";
            case AuthorizeRoleName.Editor:
                return "编辑者";
            case AuthorizeRoleName.Ordinary:
                return "普通用户";
            default:
                throw new ArgumentOutOfRangeException(nameof(role), role, null);
        }
    }
}