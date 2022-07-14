using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeChatApp.WebApp.Migrations
{
    public partial class InitDb04 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MaxPickUpCount",
                table: "WorkTask",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkPublishType",
                table: "WorkTask",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxPickUpCount",
                table: "WorkTask");

            migrationBuilder.DropColumn(
                name: "WorkPublishType",
                table: "WorkTask");
        }
    }
}
