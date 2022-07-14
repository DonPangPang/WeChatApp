using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeChatApp.WebApp.Migrations
{
    public partial class InitDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BonusPointRecord",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BonusPoints = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    PickUpUserUid = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PickUpUserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateUserUid = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BonusPointRecord", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Department",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DepartmentName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TreeIds = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreateUserUid = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Department", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Department_Department_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Department",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Uid = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Tel = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false),
                    IsSuper = table.Column<bool>(type: "bit", nullable: false),
                    DepartmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkTask",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WorkPublishType = table.Column<int>(type: "int", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PointsRewards = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    PointsSettlement = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    PickUpUserIds = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PickUpUserNames = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Timespan = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    CreateUserUid = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsPublicNodes = table.Column<bool>(type: "bit", nullable: false),
                    PublicStartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PublicEndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifyUserUid = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifyUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifyTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkTask", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkTaskNode",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WorkTaskId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateUserUid = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
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
                });

            migrationBuilder.InsertData(
                table: "Department",
                columns: new[] { "Id", "CreateTime", "CreateUserName", "CreateUserUid", "DepartmentName", "ParentId", "TreeIds" },
                values: new object[] { new Guid("02516c95-8cc3-4643-ad6e-de08fbf5cd31"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "三国集团", null, "02516C95-8CC3-4643-AD6E-DE08FBF5CD31" });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "DepartmentId", "Email", "IsSuper", "Name", "Password", "Role", "Tel", "Uid" },
                values: new object[,]
                {
                    { new Guid("45d16a4c-4a1c-4cd9-a0ba-6af0b86bbaf6"), new Guid("eab4c639-14a5-4458-91ba-ed77f6a955f5"), "liubei@qq.com", false, "刘备", "123456", 2, "13900000000", "liubei" },
                    { new Guid("45d16a4c-4a1c-4cd9-a0ba-6af0b86bbaf7"), new Guid("eab4c639-14a5-4458-91ba-ed77f6a955f5"), "guanyu@qq.com", false, "关羽", "123456", 1, "13900000000", "guanyu" },
                    { new Guid("45d16a4c-4a1c-4cd9-a0ba-6af0b86bbaf8"), new Guid("eab4c639-14a5-4458-91ba-ed77f6a955f5"), "zhangfei@qq.com", false, "张飞", "123456", 1, "13900000000", "zhangfei" },
                    { new Guid("45d16a4c-4a1c-4cd9-a0ba-6af0b86bbaf9"), new Guid("eab4c639-14a5-4458-91ba-ed77f6a955f5"), "zhugeliang@qq.com", false, "诸葛亮", "123456", 0, "13900000000", "zhugeliang" },
                    { new Guid("45d16a4c-4a1c-4cd9-a0ba-7af0b86bbaf8"), new Guid("170758b4-c800-4895-b373-02488fd9c6c0"), "zhaozilong@qq.com", false, "赵子龙", "123456", 1, "13900000000", "zhaozilong" },
                    { new Guid("8898ae8f-78c0-48f8-8e6a-f5894e9a4ec4"), new Guid("170758b4-c800-4895-b373-02488fd9c6c0"), "zhangsan@qq.com", false, "张三", "123456", 0, "13900000000", "zhangsan" },
                    { new Guid("8898ae8f-78c0-48f8-8e6a-f5894e9a4ec5"), new Guid("170758b4-c800-4895-b373-02488fd9c6c0"), "zhaozilong@qq.com", false, "lisi", "123456", 0, "13900000000", "lisi" }
                });

            migrationBuilder.InsertData(
                table: "Department",
                columns: new[] { "Id", "CreateTime", "CreateUserName", "CreateUserUid", "DepartmentName", "ParentId", "TreeIds" },
                values: new object[] { new Guid("eab4c639-14a5-4458-91ba-ed77f6a955f4"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "蜀汉", new Guid("02516c95-8cc3-4643-ad6e-de08fbf5cd31"), "02516C95-8CC3-4643-AD6E-DE08FBF5CD31,EAB4C639-14A5-4458-91BA-ED77F6A955F4" });

            migrationBuilder.InsertData(
                table: "Department",
                columns: new[] { "Id", "CreateTime", "CreateUserName", "CreateUserUid", "DepartmentName", "ParentId", "TreeIds" },
                values: new object[] { new Guid("eab4c639-14a5-4458-91ba-ed77f6a955f6"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "魏国", new Guid("02516c95-8cc3-4643-ad6e-de08fbf5cd31"), "02516C95-8CC3-4643-AD6E-DE08FBF5CD31,EAB4C639-14A5-4458-91BA-ED77F6A955F6" });

            migrationBuilder.InsertData(
                table: "Department",
                columns: new[] { "Id", "CreateTime", "CreateUserName", "CreateUserUid", "DepartmentName", "ParentId", "TreeIds" },
                values: new object[] { new Guid("eab4c639-14a5-4458-91ba-ed77f6a955f7"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "吴国", new Guid("02516c95-8cc3-4643-ad6e-de08fbf5cd31"), "02516C95-8CC3-4643-AD6E-DE08FBF5CD31,EAB4C639-14A5-4458-91BA-ED77F6A955F7" });

            migrationBuilder.InsertData(
                table: "Department",
                columns: new[] { "Id", "CreateTime", "CreateUserName", "CreateUserUid", "DepartmentName", "ParentId", "TreeIds" },
                values: new object[,]
                {
                    { new Guid("00663153-ca24-4e78-aee1-8beafcd045c7"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "魏国打工仔", new Guid("eab4c639-14a5-4458-91ba-ed77f6a955f6"), "02516C95-8CC3-4643-AD6E-DE08FBF5CD31,EAB4C639-14A5-4458-91BA-ED77F6A955F6,00663153-CA24-4E78-AEE1-8BEAFCD045C7" },
                    { new Guid("170758b4-c800-4895-b373-02488fd9c6c0"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "蜀汉打工仔", new Guid("eab4c639-14a5-4458-91ba-ed77f6a955f4"), "02516C95-8CC3-4643-AD6E-DE08FBF5CD31,EAB4C639-14A5-4458-91BA-ED77F6A955F4,170758B4-C800-4895-B373-02488FD9C6C0" },
                    { new Guid("b8d77ddd-a4a3-4ad0-a834-e2e7dfbaf9ef"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "吴国打工仔", new Guid("eab4c639-14a5-4458-91ba-ed77f6a955f7"), "02516C95-8CC3-4643-AD6E-DE08FBF5CD31,EAB4C639-14A5-4458-91BA-ED77F6A955F7,B8D77DDD-A4A3-4AD0-A834-E2E7DFBAF9EF" },
                    { new Guid("eab4c639-14a5-4458-91ba-ed77f6a955f5"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "桃园三小伙", new Guid("eab4c639-14a5-4458-91ba-ed77f6a955f4"), "02516C95-8CC3-4643-AD6E-DE08FBF5CD31,EAB4C639-14A5-4458-91BA-ED77F6A955F4,EAB4C639-14A5-4458-91BA-ED77F6A955F5" }
                });

            migrationBuilder.InsertData(
                table: "Department",
                columns: new[] { "Id", "CreateTime", "CreateUserName", "CreateUserUid", "DepartmentName", "ParentId", "TreeIds" },
                values: new object[] { new Guid("00663153-ca24-4e78-aee1-8beafcd045c8"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "虎豹骑", new Guid("00663153-ca24-4e78-aee1-8beafcd045c7"), "02516C95-8CC3-4643-AD6E-DE08FBF5CD31,EAB4C639-14A5-4458-91BA-ED77F6A955F6,00663153-CA24-4E78-AEE1-8BEAFCD045C7,00663153-CA24-4E78-AEE1-8BEAFCD045C8" });

            migrationBuilder.CreateIndex(
                name: "IX_Department_ParentId",
                table: "Department",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkTaskNode_WorkTaskId",
                table: "WorkTaskNode",
                column: "WorkTaskId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BonusPointRecord");

            migrationBuilder.DropTable(
                name: "Department");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "WorkTaskNode");

            migrationBuilder.DropTable(
                name: "WorkTask");
        }
    }
}
