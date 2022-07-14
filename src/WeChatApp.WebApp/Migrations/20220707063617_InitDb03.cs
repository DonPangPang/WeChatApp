using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeChatApp.WebApp.Migrations
{
    public partial class InitDb03 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WorkPublishType",
                table: "WorkTask");

            migrationBuilder.DropColumn(
                name: "PickUpUserUid",
                table: "BonusPointRecord");

            migrationBuilder.AddColumn<string>(
                name: "ImageSources",
                table: "WorkTaskNode",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "NodeTime",
                table: "WorkTaskNode",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DepartmentId",
                table: "WorkTask",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "PickUpUserId",
                table: "BonusPointRecord",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "WorkTaskId",
                table: "BonusPointRecord",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "WorkTaskNodeItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WorkTaskId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WorkTaskNodeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageSources = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateUserUid = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreateUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkTaskNodeItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkTaskNodeItem_WorkTaskNode_WorkTaskNodeId",
                        column: x => x.WorkTaskNodeId,
                        principalTable: "WorkTaskNode",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkTaskNodeItem_WorkTaskNodeId",
                table: "WorkTaskNodeItem",
                column: "WorkTaskNodeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorkTaskNodeItem");

            migrationBuilder.DropColumn(
                name: "ImageSources",
                table: "WorkTaskNode");

            migrationBuilder.DropColumn(
                name: "NodeTime",
                table: "WorkTaskNode");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "WorkTask");

            migrationBuilder.DropColumn(
                name: "PickUpUserId",
                table: "BonusPointRecord");

            migrationBuilder.DropColumn(
                name: "WorkTaskId",
                table: "BonusPointRecord");

            migrationBuilder.AddColumn<int>(
                name: "WorkPublishType",
                table: "WorkTask",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "PickUpUserUid",
                table: "BonusPointRecord",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
