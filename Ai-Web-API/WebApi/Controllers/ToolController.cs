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
        var user2 = new Users()
        {
            Name = "yhj",
            Password = "1234567",
            CreateDate = DateTime.Today,
            CreateUserId = 1,
            IsDeleted = 0
        };
        var papers = new TestPapers()
        {
            id = 1,
            subject = "数学",
            TopicNumber = 1,
            Topic = "计算表达式 (3×5)+(4÷2)-(6-2) 的结果，选择正确的选项",
            type = 1,
            Choice1 = "A:13",
            Choice2 = "A:12",
            Choice3 = "A:11",
            Choice4 = "A:14",
            answer = 1
            
        };
        // var yolotbs = new Yolotbs()
        // {
        //     Cls = "皮卡丘",
        //     sbjgCount = 10,
        //     IsManualReview = false,
        //     sbzqCount = 10,
        //     rgmsCount = 10,
        //     zql = 99.99,
        //     zhl = 99.99,
        //     CreateDate = DateTime.Now,
        //     CreateUserId = 0,
        //     IsDeleted = 0
        // };
        _context.testpapers.Add(papers);
        
        _context.Users.Add(user);
        // _context.yolotbs.Add(yolotbs);
        _context.SaveChanges();
        return "ok";
    }
}