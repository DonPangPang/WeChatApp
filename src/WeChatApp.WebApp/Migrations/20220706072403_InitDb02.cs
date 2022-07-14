using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeChatApp.WebApp.Migrations
{
    public partial class InitDb02 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CreateUserId",
                table: "WorkTaskNode",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreateUserId",
                table: "WorkTask",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ModifyUserId",
                table: "WorkTask",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreateUserId",
                table: "Department",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreateUserId",
                table: "BonusPointRecord",
                type: "uniqueidentifier",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreateUserId",
                table: "WorkTaskNode");

            migrationBuilder.DropColumn(
                name: "CreateUserId",
                table: "WorkTask");

            migrationBuilder.DropColumn(
                name: "ModifyUserId",
                table: "WorkTask");

            migrationBuilder.DropColumn(
                name: "CreateUserId",
                table: "Department");

            migrationBuilder.DropColumn(
                name: "CreateUserId",
                table: "BonusPointRecord");
        }
    }
}
