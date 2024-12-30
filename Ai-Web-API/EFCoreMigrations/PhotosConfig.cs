using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Model.Entities;

namespace EFCoreMigrations;

public class PhotosConfig : IEntityTypeConfiguration<Yolotbs>
{
    public void Configure(EntityTypeBuilder<Yolotbs> builder)
    {
        builder.ToTable("Photos");
    }
}