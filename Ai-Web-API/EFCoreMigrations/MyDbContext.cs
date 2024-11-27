using Microsoft.EntityFrameworkCore;
using Model.Entitys;

namespace EFCoreMigrations;

public class MyDbContext : DbContext
{
    public DbSet<Users> Users { get; set; }

    public DbSet<Yolotbs> yolotbs { get; set; }

    public DbSet<Photos> Photos { get; set; }

    public DbSet<TestPapers> testpapers { get; set; }

    /// <summary>
    /// 成绩中心
    /// </summary>
    public DbSet<ReportCard> ReportCards { get; set; }

    public MyDbContext()
    {
    }

    // 注入方式配置
    public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
    {
    }

    //数据库迁移
    // cd EFCoreMigrations
    //dotnet ef migrations add InitialCreate
    //dotnet ef database update
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        //数据库连接字符串
        optionsBuilder.UseMySql("server=47.107.226.106;port=3306;database=aitest;user=root;password=1qazZAQ!hhh333;",
            new MySqlServerVersion(new Version(8, 0, 33)));
    }
}