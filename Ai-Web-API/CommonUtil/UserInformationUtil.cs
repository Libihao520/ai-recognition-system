using EFCoreMigrations;

namespace CommonUtil;

public class UserInformationUtil
{
    private readonly MyDbContext _context;

    public UserInformationUtil(MyDbContext context)
    {
        _context = context;
    }

    public async Task<string> GetUserNameByIdAsync(long userId)
    {
        var findAsync = await _context.Users.FindAsync(userId);
        // 模拟从数据库获取用户名
        // 在实际应用中，这里应该是数据库查询
        if (findAsync != null)
        {
            return findAsync.Name;
        }
        else
        {
            return "用户不存在！";
        }
    }
}