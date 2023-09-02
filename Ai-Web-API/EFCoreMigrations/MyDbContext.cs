using Microsoft.EntityFrameworkCore;
using Model.Entitys;

namespace EFCoreMigrations;

public class MyDbContext : DbContext
{
    public DbSet<Users> Users { get; set; }

    public DbSet<Yolotbs> yolotbs { get; set; }
    
    public DbSet<Photos> Photos { get; set; }

    //注入方式配置
    public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
    {
    }
}