using EFCoreMigrations;
using Microsoft.AspNetCore.Mvc;
using Model.Entitys;

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
        //创建初始化值
        Users user = new Users()
        {
            Name = "lbh",
            Password = "123456",
            CreateDate = DateTime.Now,
            CreateUserId = 0,
            IsDeleted = 0
        };
        var yolotbs = new Yolotbs()
        {
            Cls = "皮卡丘",
            sbjgCount = 10,
            IsManualReview = false,
            sbzqCount = 10,
            rgmsCount = 10,
            zql = 99.99,
            zhl = 99.99
        };

        _context.Users.Add(user);
        _context.yolotbs.Add(yolotbs);
        _context.SaveChanges();
        return "ok";
    }
}