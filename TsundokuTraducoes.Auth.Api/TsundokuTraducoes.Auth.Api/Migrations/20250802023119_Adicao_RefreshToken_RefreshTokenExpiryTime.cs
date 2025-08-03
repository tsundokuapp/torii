using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TsundokuTraducoes.Auth.Api.Migrations
{
    /// <inheritdoc />
    public partial class Adicao_RefreshToken_RefreshTokenExpiryTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RefreshToken",
                table: "AspNetUsers",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "RefreshTokenExpiryTime",
                table: "AspNetUsers",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "RefreshToken", "RefreshTokenExpiryTime", "SecurityStamp" },
                values: new object[] { "e68506f3-6687-4065-95a4-5cb2fd37bc45", "AQAAAAIAAYagAAAAEHp8RZ1mkMub7Y81b+VXYNMTfpkydEgdHolbPUiIKI3BUxIGpP+Kho4ndoYnEcFqyg==", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "bc775f94-0c4f-4184-a259-cb5cb96425ac" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RefreshToken",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "RefreshTokenExpiryTime",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "8e365dc2-bbdb-4eb4-a356-49bb6b43e0ff", "AQAAAAIAAYagAAAAEMEn37y/N9CENeFEBKX5y1mcKsoe2YBAGJbq1TRhQ8IUJgG142REyA97IlK0FCqIOQ==", "8befbde3-d31a-4053-a312-9237339b4389" });
        }
    }
}
