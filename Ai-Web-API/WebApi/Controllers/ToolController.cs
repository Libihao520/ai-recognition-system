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
        #region 创建初始化值

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

        #endregion

        #region 题型一 ：单选

        var papers = new TestPapers()
        {
            id = 1,
            subject = "数学",
            TopicNumber = 1,
            Topic = "计算表达式 (3×5)+(4÷2)-(6-2) 的结果，选择正确的选项",
            type = 0,
            Choice1 = "A:13",
            Choice2 = "A:12",
            Choice3 = "A:11",
            Choice4 = "A:14",
            answer = new List<int>{1}
        };

        #endregion

        #region 题型二 ：多选

        var papers2 = new TestPapers()
        {
            type = 1,
            id = 2,
            subject = "数学",
            TopicNumber = 2,
            Topic = "以下哪些选项是关于地球的正确陈述？",
            Choice1 = "地球是太阳系中唯一已知存在生命的行星。",
            Choice2 = "地球的自转周期约为24小时。",
            Choice3 = "地球的公转轨道是一个完美的圆形。",
            Choice4 = "地球的大气层主要由氮气和氧气组成。",
            answer = new List<int>{1,2,4}
        };

        #endregion

        #region 题型三 : 判断题

        var papers3 = new TestPapers()
        {
            type = 2,
            id = 3,
            subject = "数学",
            TopicNumber = 3,
            Topic = "地球是太阳系中最大的行星。 ( )",
            Choice1 = "√",
            Choice2 = "×。",
            answer = new List<int>{1}
        };

        #endregion


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

        #region 添加到数据库

        _context.testpapers.AddRange(new List<TestPapers>{papers,papers2,papers3});
        _context.Users.Add(user);
        // _context.yolotbs.Add(yolotbs);

        #endregion

        #region 保存

        _context.SaveChanges();

        #endregion
        return "ok";
    }
}