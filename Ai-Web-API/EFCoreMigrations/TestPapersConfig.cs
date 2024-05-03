using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Model.Entitys;

namespace EFCoreMigrations;


class TestPapersConfig:IEntityTypeConfiguration<TestPapers>
{
    public void Configure(EntityTypeBuilder<TestPapers> builder)
    {
        builder.ToTable("testpapers");
    }
}