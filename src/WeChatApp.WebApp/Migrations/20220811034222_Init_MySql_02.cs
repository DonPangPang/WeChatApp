using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeChatApp.WebApp.Migrations
{
    public partial class Init_MySql_02 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CanPickUserIds",
                table: "WorkTask",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "CanPickUserNames",
                table: "WorkTask",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CanPickUserIds",
                table: "WorkTask");

            migrationBuilder.DropColumn(
                name: "CanPickUserNames",
                table: "WorkTask");
        }
    }
}
