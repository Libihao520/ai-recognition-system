using EFCoreMigrations;
using Microsoft.AspNetCore.Mvc;
using Model.Entities;

namespace WebApi.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class ToolController : ControllerBase
{
    private MyDbContext _context;

    public ToolController(MyDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public string InitDateBase()
    {
        #region 创建初始化值

        var users = new List<Users>()
        {
            new Users
            {
                Name = "lbh",
                PassWord = "123456",
                CreateDate = DateTime.Now,
                CreateUserId = 0,
                IsDeleted = 0
            },
            new Users
            {
                Name = "yhj",
                PassWord = "1234567",
                CreateDate = DateTime.Now,
                CreateUserId = 1,
                IsDeleted = 0
            }
        };

        #endregion


        #region 添加到数据库

        _context.Users.AddRange(users);
        //_context.Users.AddRange(new[] { user1, user2 });
        // _context.yolotbs.Add(yolotbs);

        #endregion

        #region 保存

        _context.SaveChanges();

        #endregion

        return "ok";
    }
}