using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFCoreMigrations.Migrations
{
    /// <inheritdoc />
    public partial class TestPaperManageCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "testPapersManageId",
                table: "testpapers",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "testPapersManageId",
                table: "testpapers");
        }
    }
}
