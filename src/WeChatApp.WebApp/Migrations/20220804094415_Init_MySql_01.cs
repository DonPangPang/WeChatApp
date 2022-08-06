using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeChatApp.WebApp.Migrations
{
    public partial class Init_MySql_01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "BonusPointRecord",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    BonusPoints = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    PickUpUserId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    PickUpUserName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    WorkTaskId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreateUserUid = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreateUserId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    CreateUserName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreateTime = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BonusPointRecord", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Department",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    DepartmentName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TreeIds = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ParentId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    CreateUserUid = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreateUserId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    CreateUserName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreateTime = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Department", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Department_Department_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Department",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "LogRecord",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    LogLevel = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Module = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Content = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LogTime = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogRecord", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "MessageToast",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Content = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsPush = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    UserId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageToast", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Uid = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Password = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    Tel = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Role = table.Column<int>(type: "int", nullable: false),
                    IsSuper = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    DepartmentId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "WorkTask",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    DepartmentId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Level = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Content = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    WorkPublishType = table.Column<int>(type: "int", nullable: false),
                    MaxPickUpCount = table.Column<int>(type: "int", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    PointsRewards = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    PointsSettlement = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    PickUpUserIds = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PickUpUserNames = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Timespan = table.Column<DateTime>(type: "timestamp", rowVersion: true, nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn),
                    CreateUserUid = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreateUserName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    IsPublicNodes = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    PublicStartTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    PublicEndTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModifyUserUid = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreateUserId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    ModifyUserId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    ModifyUserName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ModifyTime = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkTask", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "WorkTaskNode",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    WorkTaskId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Content = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ImageSources = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NodeTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreateUserUid = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreateUserId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    CreateUserName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreateTime = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkTaskNode", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkTaskNode_WorkTask_WorkTaskId",
                        column: x => x.WorkTaskId,
                        principalTable: "WorkTask",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "WorkTaskNodeItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    WorkTaskId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    WorkTaskNodeId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Title = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Content = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ImageSources = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreateUserUid = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreateUserId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    CreateUserName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreateTime = table.Column<DateTime>(type: "datetime", nullable: false)
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
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Department",
                columns: new[] { "Id", "CreateTime", "CreateUserId", "CreateUserName", "CreateUserUid", "DepartmentName", "ParentId", "TreeIds" },
                values: new object[] { new Guid("02516c95-8cc3-4643-ad6e-de08fbf5cd31"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "三国集团", null, "02516C95-8CC3-4643-AD6E-DE08FBF5CD31" });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "DepartmentId", "Email", "Gender", "IsSuper", "Name", "Password", "Role", "Tel", "Uid" },
                values: new object[,]
                {
                    { new Guid("45d16a4c-4a1c-4cd9-a0ba-6af0b86bbaf6"), new Guid("eab4c639-14a5-4458-91ba-ed77f6a955f5"), "liubei@qq.com", 1, false, "刘备", "123456", 2, "13900000000", "liubei" },
                    { new Guid("45d16a4c-4a1c-4cd9-a0ba-6af0b86bbaf7"), new Guid("eab4c639-14a5-4458-91ba-ed77f6a955f5"), "guanyu@qq.com", 1, false, "关羽", "123456", 1, "13900000000", "guanyu" },
                    { new Guid("45d16a4c-4a1c-4cd9-a0ba-6af0b86bbaf8"), new Guid("eab4c639-14a5-4458-91ba-ed77f6a955f5"), "zhangfei@qq.com", 1, false, "张飞", "123456", 1, "13900000000", "zhangfei" },
                    { new Guid("45d16a4c-4a1c-4cd9-a0ba-6af0b86bbaf9"), new Guid("eab4c639-14a5-4458-91ba-ed77f6a955f5"), "zhugeliang@qq.com", 1, false, "诸葛亮", "123456", 0, "13900000000", "zhugeliang" },
                    { new Guid("45d16a4c-4a1c-4cd9-a0ba-7af0b86bbaf8"), new Guid("170758b4-c800-4895-b373-02488fd9c6c0"), "zhaozilong@qq.com", 1, false, "赵子龙", "123456", 1, "13900000000", "zhaozilong" },
                    { new Guid("8898ae8f-78c0-48f8-8e6a-f5894e9a4ec4"), new Guid("170758b4-c800-4895-b373-02488fd9c6c0"), "zhangsan@qq.com", 1, false, "张三", "123456", 0, "13900000000", "zhangsan" },
                    { new Guid("8898ae8f-78c0-48f8-8e6a-f5894e9a4ec5"), new Guid("170758b4-c800-4895-b373-02488fd9c6c0"), "zhaozilong@qq.com", 1, false, "lisi", "123456", 0, "13900000000", "lisi" }
                });

            migrationBuilder.InsertData(
                table: "Department",
                columns: new[] { "Id", "CreateTime", "CreateUserId", "CreateUserName", "CreateUserUid", "DepartmentName", "ParentId", "TreeIds" },
                values: new object[] { new Guid("eab4c639-14a5-4458-91ba-ed77f6a955f4"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "蜀汉", new Guid("02516c95-8cc3-4643-ad6e-de08fbf5cd31"), "02516C95-8CC3-4643-AD6E-DE08FBF5CD31,EAB4C639-14A5-4458-91BA-ED77F6A955F4" });

            migrationBuilder.InsertData(
                table: "Department",
                columns: new[] { "Id", "CreateTime", "CreateUserId", "CreateUserName", "CreateUserUid", "DepartmentName", "ParentId", "TreeIds" },
                values: new object[] { new Guid("eab4c639-14a5-4458-91ba-ed77f6a955f6"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "魏国", new Guid("02516c95-8cc3-4643-ad6e-de08fbf5cd31"), "02516C95-8CC3-4643-AD6E-DE08FBF5CD31,EAB4C639-14A5-4458-91BA-ED77F6A955F6" });

            migrationBuilder.InsertData(
                table: "Department",
                columns: new[] { "Id", "CreateTime", "CreateUserId", "CreateUserName", "CreateUserUid", "DepartmentName", "ParentId", "TreeIds" },
                values: new object[] { new Guid("eab4c639-14a5-4458-91ba-ed77f6a955f7"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "吴国", new Guid("02516c95-8cc3-4643-ad6e-de08fbf5cd31"), "02516C95-8CC3-4643-AD6E-DE08FBF5CD31,EAB4C639-14A5-4458-91BA-ED77F6A955F7" });

            migrationBuilder.InsertData(
                table: "Department",
                columns: new[] { "Id", "CreateTime", "CreateUserId", "CreateUserName", "CreateUserUid", "DepartmentName", "ParentId", "TreeIds" },
                values: new object[,]
                {
                    { new Guid("00663153-ca24-4e78-aee1-8beafcd045c7"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "魏国打工仔", new Guid("eab4c639-14a5-4458-91ba-ed77f6a955f6"), "02516C95-8CC3-4643-AD6E-DE08FBF5CD31,EAB4C639-14A5-4458-91BA-ED77F6A955F6,00663153-CA24-4E78-AEE1-8BEAFCD045C7" },
                    { new Guid("170758b4-c800-4895-b373-02488fd9c6c0"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "蜀汉打工仔", new Guid("eab4c639-14a5-4458-91ba-ed77f6a955f4"), "02516C95-8CC3-4643-AD6E-DE08FBF5CD31,EAB4C639-14A5-4458-91BA-ED77F6A955F4,170758B4-C800-4895-B373-02488FD9C6C0" },
                    { new Guid("b8d77ddd-a4a3-4ad0-a834-e2e7dfbaf9ef"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "吴国打工仔", new Guid("eab4c639-14a5-4458-91ba-ed77f6a955f7"), "02516C95-8CC3-4643-AD6E-DE08FBF5CD31,EAB4C639-14A5-4458-91BA-ED77F6A955F7,B8D77DDD-A4A3-4AD0-A834-E2E7DFBAF9EF" },
                    { new Guid("eab4c639-14a5-4458-91ba-ed77f6a955f5"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "桃园三小伙", new Guid("eab4c639-14a5-4458-91ba-ed77f6a955f4"), "02516C95-8CC3-4643-AD6E-DE08FBF5CD31,EAB4C639-14A5-4458-91BA-ED77F6A955F4,EAB4C639-14A5-4458-91BA-ED77F6A955F5" }
                });

            migrationBuilder.InsertData(
                table: "Department",
                columns: new[] { "Id", "CreateTime", "CreateUserId", "CreateUserName", "CreateUserUid", "DepartmentName", "ParentId", "TreeIds" },
                values: new object[] { new Guid("00663153-ca24-4e78-aee1-8beafcd045c8"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "虎豹骑", new Guid("00663153-ca24-4e78-aee1-8beafcd045c7"), "02516C95-8CC3-4643-AD6E-DE08FBF5CD31,EAB4C639-14A5-4458-91BA-ED77F6A955F6,00663153-CA24-4E78-AEE1-8BEAFCD045C7,00663153-CA24-4E78-AEE1-8BEAFCD045C8" });

            migrationBuilder.CreateIndex(
                name: "IX_Department_ParentId",
                table: "Department",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkTaskNode_WorkTaskId",
                table: "WorkTaskNode",
                column: "WorkTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkTaskNodeItem_WorkTaskNodeId",
                table: "WorkTaskNodeItem",
                column: "WorkTaskNodeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BonusPointRecord");

            migrationBuilder.DropTable(
                name: "Department");

            migrationBuilder.DropTable(
                name: "LogRecord");

            migrationBuilder.DropTable(
                name: "MessageToast");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "WorkTaskNodeItem");

            migrationBuilder.DropTable(
                name: "WorkTaskNode");

            migrationBuilder.DropTable(
                name: "WorkTask");
        }
    }
}
