using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TsundokuTraducoes.Auth.Api.Migrations
{
    /// <inheritdoc />
    public partial class Refatoracao_Permissoes_Hierarquicas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RolePermissoes",
                keyColumns: new[] { "PermissaoId", "RoleId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "RolePermissoes",
                keyColumns: new[] { "PermissaoId", "RoleId" },
                keyValues: new object[] { 2, 1 });

            migrationBuilder.DeleteData(
                table: "RolePermissoes",
                keyColumns: new[] { "PermissaoId", "RoleId" },
                keyValues: new object[] { 3, 1 });

            migrationBuilder.DeleteData(
                table: "RolePermissoes",
                keyColumns: new[] { "PermissaoId", "RoleId" },
                keyValues: new object[] { 4, 1 });

            migrationBuilder.DeleteData(
                table: "RolePermissoes",
                keyColumns: new[] { "PermissaoId", "RoleId" },
                keyValues: new object[] { 5, 1 });

            migrationBuilder.DeleteData(
                table: "RolePermissoes",
                keyColumns: new[] { "PermissaoId", "RoleId" },
                keyValues: new object[] { 6, 1 });

            migrationBuilder.DeleteData(
                table: "RolePermissoes",
                keyColumns: new[] { "PermissaoId", "RoleId" },
                keyValues: new object[] { 7, 1 });

            migrationBuilder.DeleteData(
                table: "RolePermissoes",
                keyColumns: new[] { "PermissaoId", "RoleId" },
                keyValues: new object[] { 8, 1 });

            migrationBuilder.DeleteData(
                table: "RolePermissoes",
                keyColumns: new[] { "PermissaoId", "RoleId" },
                keyValues: new object[] { 9, 1 });

            migrationBuilder.DropColumn(
                name: "Acao",
                table: "Permissoes");

            migrationBuilder.DropColumn(
                name: "Nome",
                table: "Permissoes");

            migrationBuilder.RenameColumn(
                name: "Recurso",
                table: "Permissoes",
                newName: "Valor");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "dd9bc4d0-ce65-4fa3-afef-290275b34e66", "AQAAAAIAAYagAAAAEH8wf2r+DeNfsbTfoNoGgIZXmlkysfNvRwVcVq26gz26nPQH9kr5CZ0BbdRtR8NjQw==", "291a2949-64f1-4ed6-a5a9-eb2b1931a5f7" });

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CriadoEm", "Descricao", "Valor" },
                values: new object[] { new DateTime(2026, 1, 25, 17, 46, 40, 802, DateTimeKind.Utc).AddTicks(8460), "Visualizar qualquer obra", "obra.visualizar" });

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CriadoEm", "Descricao", "Valor" },
                values: new object[] { new DateTime(2026, 1, 25, 17, 46, 40, 802, DateTimeKind.Utc).AddTicks(8460), "Criar obras", "obra.criar" });

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CriadoEm", "Descricao", "Valor" },
                values: new object[] { new DateTime(2026, 1, 25, 17, 46, 40, 802, DateTimeKind.Utc).AddTicks(8460), "Editar qualquer obra", "obra.editar" });

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CriadoEm", "Descricao", "Valor" },
                values: new object[] { new DateTime(2026, 1, 25, 17, 46, 40, 802, DateTimeKind.Utc).AddTicks(8470), "Deletar qualquer obra", "obra.deletar" });

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CriadoEm", "Descricao", "Valor" },
                values: new object[] { new DateTime(2026, 1, 25, 17, 46, 40, 802, DateTimeKind.Utc).AddTicks(8470), "Visualizar qualquer capítulo", "capitulo.visualizar" });

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CriadoEm", "Descricao", "Valor" },
                values: new object[] { new DateTime(2026, 1, 25, 17, 46, 40, 802, DateTimeKind.Utc).AddTicks(8470), "Criar capítulos", "capitulo.criar" });

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CriadoEm", "Descricao", "Valor" },
                values: new object[] { new DateTime(2026, 1, 25, 17, 46, 40, 802, DateTimeKind.Utc).AddTicks(8470), "Editar qualquer capítulo", "capitulo.editar" });

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CriadoEm", "Descricao", "Valor" },
                values: new object[] { new DateTime(2026, 1, 25, 17, 46, 40, 802, DateTimeKind.Utc).AddTicks(8470), "Deletar qualquer capítulo", "capitulo.deletar" });

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CriadoEm", "Descricao", "Valor" },
                values: new object[] { new DateTime(2026, 1, 25, 17, 46, 40, 802, DateTimeKind.Utc).AddTicks(8470), "Gerenciar usuários", "usuario.gerenciar" });

            migrationBuilder.InsertData(
                table: "Permissoes",
                columns: new[] { "Id", "CriadoEm", "Descricao", "Valor" },
                values: new object[] { 10, new DateTime(2026, 1, 25, 17, 46, 40, 802, DateTimeKind.Utc).AddTicks(8470), "Acesso total a todos os recursos", "*.*" });

            migrationBuilder.UpdateData(
                table: "RolePermissoes",
                keyColumns: new[] { "PermissaoId", "RoleId" },
                keyValues: new object[] { 1, 2 },
                column: "AtribuidoEm",
                value: new DateTime(2026, 1, 25, 17, 46, 40, 802, DateTimeKind.Utc).AddTicks(8500));

            migrationBuilder.UpdateData(
                table: "RolePermissoes",
                keyColumns: new[] { "PermissaoId", "RoleId" },
                keyValues: new object[] { 5, 2 },
                column: "AtribuidoEm",
                value: new DateTime(2026, 1, 25, 17, 46, 40, 802, DateTimeKind.Utc).AddTicks(8500));

            migrationBuilder.UpdateData(
                table: "RolePermissoes",
                keyColumns: new[] { "PermissaoId", "RoleId" },
                keyValues: new object[] { 1, 3 },
                column: "AtribuidoEm",
                value: new DateTime(2026, 1, 25, 17, 46, 40, 802, DateTimeKind.Utc).AddTicks(8500));

            migrationBuilder.UpdateData(
                table: "RolePermissoes",
                keyColumns: new[] { "PermissaoId", "RoleId" },
                keyValues: new object[] { 2, 3 },
                column: "AtribuidoEm",
                value: new DateTime(2026, 1, 25, 17, 46, 40, 802, DateTimeKind.Utc).AddTicks(8500));

            migrationBuilder.UpdateData(
                table: "RolePermissoes",
                keyColumns: new[] { "PermissaoId", "RoleId" },
                keyValues: new object[] { 3, 3 },
                column: "AtribuidoEm",
                value: new DateTime(2026, 1, 25, 17, 46, 40, 802, DateTimeKind.Utc).AddTicks(8500));

            migrationBuilder.UpdateData(
                table: "RolePermissoes",
                keyColumns: new[] { "PermissaoId", "RoleId" },
                keyValues: new object[] { 5, 3 },
                column: "AtribuidoEm",
                value: new DateTime(2026, 1, 25, 17, 46, 40, 802, DateTimeKind.Utc).AddTicks(8500));

            migrationBuilder.UpdateData(
                table: "RolePermissoes",
                keyColumns: new[] { "PermissaoId", "RoleId" },
                keyValues: new object[] { 6, 3 },
                column: "AtribuidoEm",
                value: new DateTime(2026, 1, 25, 17, 46, 40, 802, DateTimeKind.Utc).AddTicks(8500));

            migrationBuilder.UpdateData(
                table: "RolePermissoes",
                keyColumns: new[] { "PermissaoId", "RoleId" },
                keyValues: new object[] { 7, 3 },
                column: "AtribuidoEm",
                value: new DateTime(2026, 1, 25, 17, 46, 40, 802, DateTimeKind.Utc).AddTicks(8500));

            migrationBuilder.InsertData(
                table: "RolePermissoes",
                columns: new[] { "PermissaoId", "RoleId", "AtribuidoEm" },
                values: new object[] { 10, 1, new DateTime(2026, 1, 25, 17, 46, 40, 802, DateTimeKind.Utc).AddTicks(8490) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RolePermissoes",
                keyColumns: new[] { "PermissaoId", "RoleId" },
                keyValues: new object[] { 10, 1 });

            migrationBuilder.DeleteData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.RenameColumn(
                name: "Valor",
                table: "Permissoes",
                newName: "Recurso");

            migrationBuilder.AddColumn<string>(
                name: "Acao",
                table: "Permissoes",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Nome",
                table: "Permissoes",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "804ef45c-db06-4dd6-8a81-451fd10c2dd1", "AQAAAAIAAYagAAAAEPV2efEDVfxEY9RE+0OU8CKEjqeRf1vNEDEZV7D8g9aofAPlFQaHY12LMIdSlA9NFA==", "635e141a-4563-4936-a743-bd877794624d" });

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Acao", "CriadoEm", "Descricao", "Nome", "Recurso" },
                values: new object[] { "Visualizar", new DateTime(2026, 1, 25, 17, 24, 2, 386, DateTimeKind.Utc).AddTicks(3500), "Permite visualizar obras", "VisualizarObras", "Obras" });

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Acao", "CriadoEm", "Descricao", "Nome", "Recurso" },
                values: new object[] { "Criar", new DateTime(2026, 1, 25, 17, 24, 2, 386, DateTimeKind.Utc).AddTicks(3500), "Permite criar obras", "CriarObras", "Obras" });

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Acao", "CriadoEm", "Descricao", "Nome", "Recurso" },
                values: new object[] { "Editar", new DateTime(2026, 1, 25, 17, 24, 2, 386, DateTimeKind.Utc).AddTicks(3500), "Permite editar obras", "EditarObras", "Obras" });

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Acao", "CriadoEm", "Descricao", "Nome", "Recurso" },
                values: new object[] { "Deletar", new DateTime(2026, 1, 25, 17, 24, 2, 386, DateTimeKind.Utc).AddTicks(3510), "Permite deletar obras", "DeletarObras", "Obras" });

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Acao", "CriadoEm", "Descricao", "Nome", "Recurso" },
                values: new object[] { "Visualizar", new DateTime(2026, 1, 25, 17, 24, 2, 386, DateTimeKind.Utc).AddTicks(3510), "Permite visualizar capítulos", "VisualizarCapitulos", "Capitulos" });

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Acao", "CriadoEm", "Descricao", "Nome", "Recurso" },
                values: new object[] { "Criar", new DateTime(2026, 1, 25, 17, 24, 2, 386, DateTimeKind.Utc).AddTicks(3510), "Permite criar capítulos", "CriarCapitulos", "Capitulos" });

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Acao", "CriadoEm", "Descricao", "Nome", "Recurso" },
                values: new object[] { "Editar", new DateTime(2026, 1, 25, 17, 24, 2, 386, DateTimeKind.Utc).AddTicks(3510), "Permite editar capítulos", "EditarCapitulos", "Capitulos" });

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Acao", "CriadoEm", "Descricao", "Nome", "Recurso" },
                values: new object[] { "Deletar", new DateTime(2026, 1, 25, 17, 24, 2, 386, DateTimeKind.Utc).AddTicks(3510), "Permite deletar capítulos", "DeletarCapitulos", "Capitulos" });

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "Acao", "CriadoEm", "Descricao", "Nome", "Recurso" },
                values: new object[] { "Gerenciar", new DateTime(2026, 1, 25, 17, 24, 2, 386, DateTimeKind.Utc).AddTicks(3510), "Permite gerenciar usuários", "GerenciarUsuarios", "Usuarios" });

            migrationBuilder.UpdateData(
                table: "RolePermissoes",
                keyColumns: new[] { "PermissaoId", "RoleId" },
                keyValues: new object[] { 1, 2 },
                column: "AtribuidoEm",
                value: new DateTime(2026, 1, 25, 17, 24, 2, 386, DateTimeKind.Utc).AddTicks(3550));

            migrationBuilder.UpdateData(
                table: "RolePermissoes",
                keyColumns: new[] { "PermissaoId", "RoleId" },
                keyValues: new object[] { 5, 2 },
                column: "AtribuidoEm",
                value: new DateTime(2026, 1, 25, 17, 24, 2, 386, DateTimeKind.Utc).AddTicks(3550));

            migrationBuilder.UpdateData(
                table: "RolePermissoes",
                keyColumns: new[] { "PermissaoId", "RoleId" },
                keyValues: new object[] { 1, 3 },
                column: "AtribuidoEm",
                value: new DateTime(2026, 1, 25, 17, 24, 2, 386, DateTimeKind.Utc).AddTicks(3540));

            migrationBuilder.UpdateData(
                table: "RolePermissoes",
                keyColumns: new[] { "PermissaoId", "RoleId" },
                keyValues: new object[] { 2, 3 },
                column: "AtribuidoEm",
                value: new DateTime(2026, 1, 25, 17, 24, 2, 386, DateTimeKind.Utc).AddTicks(3540));

            migrationBuilder.UpdateData(
                table: "RolePermissoes",
                keyColumns: new[] { "PermissaoId", "RoleId" },
                keyValues: new object[] { 3, 3 },
                column: "AtribuidoEm",
                value: new DateTime(2026, 1, 25, 17, 24, 2, 386, DateTimeKind.Utc).AddTicks(3550));

            migrationBuilder.UpdateData(
                table: "RolePermissoes",
                keyColumns: new[] { "PermissaoId", "RoleId" },
                keyValues: new object[] { 5, 3 },
                column: "AtribuidoEm",
                value: new DateTime(2026, 1, 25, 17, 24, 2, 386, DateTimeKind.Utc).AddTicks(3550));

            migrationBuilder.UpdateData(
                table: "RolePermissoes",
                keyColumns: new[] { "PermissaoId", "RoleId" },
                keyValues: new object[] { 6, 3 },
                column: "AtribuidoEm",
                value: new DateTime(2026, 1, 25, 17, 24, 2, 386, DateTimeKind.Utc).AddTicks(3550));

            migrationBuilder.UpdateData(
                table: "RolePermissoes",
                keyColumns: new[] { "PermissaoId", "RoleId" },
                keyValues: new object[] { 7, 3 },
                column: "AtribuidoEm",
                value: new DateTime(2026, 1, 25, 17, 24, 2, 386, DateTimeKind.Utc).AddTicks(3550));

            migrationBuilder.InsertData(
                table: "RolePermissoes",
                columns: new[] { "PermissaoId", "RoleId", "AtribuidoEm" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2026, 1, 25, 17, 24, 2, 386, DateTimeKind.Utc).AddTicks(3540) },
                    { 2, 1, new DateTime(2026, 1, 25, 17, 24, 2, 386, DateTimeKind.Utc).AddTicks(3540) },
                    { 3, 1, new DateTime(2026, 1, 25, 17, 24, 2, 386, DateTimeKind.Utc).AddTicks(3540) },
                    { 4, 1, new DateTime(2026, 1, 25, 17, 24, 2, 386, DateTimeKind.Utc).AddTicks(3540) },
                    { 5, 1, new DateTime(2026, 1, 25, 17, 24, 2, 386, DateTimeKind.Utc).AddTicks(3540) },
                    { 6, 1, new DateTime(2026, 1, 25, 17, 24, 2, 386, DateTimeKind.Utc).AddTicks(3540) },
                    { 7, 1, new DateTime(2026, 1, 25, 17, 24, 2, 386, DateTimeKind.Utc).AddTicks(3540) },
                    { 8, 1, new DateTime(2026, 1, 25, 17, 24, 2, 386, DateTimeKind.Utc).AddTicks(3540) },
                    { 9, 1, new DateTime(2026, 1, 25, 17, 24, 2, 386, DateTimeKind.Utc).AddTicks(3540) }
                });
        }
    }
}
