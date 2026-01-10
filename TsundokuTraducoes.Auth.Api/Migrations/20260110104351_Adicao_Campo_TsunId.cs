using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TsundokuTraducoes.Auth.Api.Migrations
{
    /// <inheritdoc />
    public partial class Adicao_Campo_TsunId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TsunId",
                table: "AspNetUsers",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp", "TsunId" },
                values: new object[] { "62c70243-1da1-4636-b7c9-9c416dba6f41", "AQAAAAIAAYagAAAAEJp7aJHYzhmPlCyN8ppm8sxaRrOSGgSuhgDAn8hFwKqlwHe5RbT4h3GrZQ7Jz2DDxA==", "300370f6-4b73-4e12-97bd-9aaf614a5f0e", new Guid("00000000-0000-0000-0000-000000000000") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TsunId",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e68506f3-6687-4065-95a4-5cb2fd37bc45", "AQAAAAIAAYagAAAAEHp8RZ1mkMub7Y81b+VXYNMTfpkydEgdHolbPUiIKI3BUxIGpP+Kho4ndoYnEcFqyg==", "bc775f94-0c4f-4184-a259-cb5cb96425ac" });
        }
    }
}
