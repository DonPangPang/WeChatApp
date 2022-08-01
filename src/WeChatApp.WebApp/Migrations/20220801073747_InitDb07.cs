using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeChatApp.WebApp.Migrations
{
    public partial class InitDb07 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Level",
                table: "WorkTask",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Level",
                table: "WorkTask");
        }
    }
}
