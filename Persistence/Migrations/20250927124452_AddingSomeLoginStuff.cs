using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddingSomeLoginStuff : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RefreshToken",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "RefreshTokenExpiryTime",
                table: "Users",
                type: "datetime2",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("6c8cd30a-cca3-4537-a8dd-7b6ac903bf29"),
                columns: new[] { "RefreshToken", "RefreshTokenExpiryTime" },
                values: new object[] { "", null });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("a1e1c3d2-1b2a-4c3d-9e1f-1a2b3c4d5e6f"),
                columns: new[] { "RefreshToken", "RefreshTokenExpiryTime" },
                values: new object[] { "", null });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("a7c7d9e8-7b8a-1c9d-3e7f-7a8b9c1d2e3f"),
                columns: new[] { "RefreshToken", "RefreshTokenExpiryTime" },
                values: new object[] { "", null });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("b2f2d4e3-2c3b-5d4e-8f2a-2b3c4d5e6f7a"),
                columns: new[] { "RefreshToken", "RefreshTokenExpiryTime" },
                values: new object[] { "", null });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("b8d8e1f9-8c9b-2d1e-4f8a-8b9c1d2e3f4a"),
                columns: new[] { "RefreshToken", "RefreshTokenExpiryTime" },
                values: new object[] { "", null });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("be2f07b8-c92d-4138-b945-8fefecbc3cbe"),
                columns: new[] { "RefreshToken", "RefreshTokenExpiryTime" },
                values: new object[] { "", null });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("c3e3f5a4-3d4c-6e5f-7a3b-3c4d5e6f7a8b"),
                columns: new[] { "RefreshToken", "RefreshTokenExpiryTime" },
                values: new object[] { "", null });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("d4f4a6b5-4e5d-7f6a-9b4c-4d5e6f7a8b9c"),
                columns: new[] { "RefreshToken", "RefreshTokenExpiryTime" },
                values: new object[] { "", null });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("e5a5b7c6-5f6e-8a7b-1c5d-5e6f7a8b9c1d"),
                columns: new[] { "RefreshToken", "RefreshTokenExpiryTime" },
                values: new object[] { "", null });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("f6b6c8d7-6a7f-9b8c-2d6e-6f7a8b9c1d2e"),
                columns: new[] { "RefreshToken", "RefreshTokenExpiryTime" },
                values: new object[] { "", null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RefreshToken",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "RefreshTokenExpiryTime",
                table: "Users");
        }
    }
}
