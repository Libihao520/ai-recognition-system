using Microsoft.EntityFrameworkCore;
using Model.Entitys;

namespace EFCoreMigrations;

public class MyDbContext : DbContext
{
    public DbSet<Users> users { get; set; }

    public DbSet<Scores> scores { get; set; }

    public DbSet<TestPapers> testpapers { get; set; }

    //注入方式配置
    public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
    {
    }
}