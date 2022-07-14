using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeChatApp.WebApp.Migrations
{
    public partial class InitDb06 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Gender",
                table: "User",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("45d16a4c-4a1c-4cd9-a0ba-6af0b86bbaf6"),
                column: "Gender",
                value: 1);

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("45d16a4c-4a1c-4cd9-a0ba-6af0b86bbaf7"),
                column: "Gender",
                value: 1);

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("45d16a4c-4a1c-4cd9-a0ba-6af0b86bbaf8"),
                column: "Gender",
                value: 1);

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("45d16a4c-4a1c-4cd9-a0ba-6af0b86bbaf9"),
                column: "Gender",
                value: 1);

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("45d16a4c-4a1c-4cd9-a0ba-7af0b86bbaf8"),
                column: "Gender",
                value: 1);

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("8898ae8f-78c0-48f8-8e6a-f5894e9a4ec4"),
                column: "Gender",
                value: 1);

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("8898ae8f-78c0-48f8-8e6a-f5894e9a4ec5"),
                column: "Gender",
                value: 1);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Gender",
                table: "User");
        }
    }
}
