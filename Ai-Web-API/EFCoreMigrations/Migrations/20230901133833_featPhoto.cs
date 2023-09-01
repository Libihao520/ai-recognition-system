using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFCoreMigrations.Migrations
{
    public partial class featPhoto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Photo",
                table: "yolotbs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Photo",
                table: "yolotbs");
        }
    }
}
