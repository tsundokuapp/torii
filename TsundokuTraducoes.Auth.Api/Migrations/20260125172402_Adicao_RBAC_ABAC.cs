using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TsundokuTraducoes.Auth.Api.Migrations
{
    /// <inheritdoc />
    public partial class Adicao_RBAC_ABAC : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Usar SQL condicional para criar tabelas apenas se não existirem
            // (tabelas podem já existir de tentativa anterior de migration)

            migrationBuilder.Sql(@"
                CREATE TABLE IF NOT EXISTS `ClaimsDinamicas` (
                    `Id` int NOT NULL AUTO_INCREMENT,
                    `UserId` int NOT NULL,
                    `Tipo` varchar(255) CHARACTER SET utf8mb4 NULL,
                    `Valor` varchar(255) CHARACTER SET utf8mb4 NULL,
                    `Descricao` longtext CHARACTER SET utf8mb4 NULL,
                    `CriadoEm` datetime(6) NOT NULL,
                    `ExpiraEm` datetime(6) NULL,
                    `Ativo` tinyint(1) NOT NULL,
                    CONSTRAINT `PK_ClaimsDinamicas` PRIMARY KEY (`Id`),
                    CONSTRAINT `FK_ClaimsDinamicas_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
                ) CHARACTER SET=utf8mb4;
            ");

            migrationBuilder.Sql(@"
                CREATE TABLE IF NOT EXISTS `Permissoes` (
                    `Id` int NOT NULL AUTO_INCREMENT,
                    `Nome` longtext CHARACTER SET utf8mb4 NULL,
                    `Descricao` longtext CHARACTER SET utf8mb4 NULL,
                    `Recurso` longtext CHARACTER SET utf8mb4 NULL,
                    `Acao` longtext CHARACTER SET utf8mb4 NULL,
                    `CriadoEm` datetime(6) NOT NULL,
                    CONSTRAINT `PK_Permissoes` PRIMARY KEY (`Id`)
                ) CHARACTER SET=utf8mb4;
            ");

            migrationBuilder.Sql(@"
                CREATE TABLE IF NOT EXISTS `RolePermissoes` (
                    `RoleId` int NOT NULL,
                    `PermissaoId` int NOT NULL,
                    `AtribuidoEm` datetime(6) NOT NULL,
                    CONSTRAINT `PK_RolePermissoes` PRIMARY KEY (`RoleId`, `PermissaoId`),
                    CONSTRAINT `FK_RolePermissoes_AspNetRoles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `AspNetRoles` (`Id`) ON DELETE CASCADE,
                    CONSTRAINT `FK_RolePermissoes_Permissoes_PermissaoId` FOREIGN KEY (`PermissaoId`) REFERENCES `Permissoes` (`Id`) ON DELETE CASCADE
                ) CHARACTER SET=utf8mb4;
            ");

            // Criar índice se não existir
            migrationBuilder.Sql(@"
                CREATE INDEX IF NOT EXISTS `IX_ClaimsDinamicas_UserId_Tipo_Valor` ON `ClaimsDinamicas` (`UserId`, `Tipo`, `Valor`);
            ");

            migrationBuilder.Sql(@"
                CREATE INDEX IF NOT EXISTS `IX_RolePermissoes_PermissaoId` ON `RolePermissoes` (`PermissaoId`);
            ");

            // Role tradutor ID=3 já existe no banco de produção, inserir apenas se não existir
            migrationBuilder.Sql(@"
                INSERT IGNORE INTO `AspNetRoles` (`Id`, `ConcurrencyStamp`, `Name`, `NormalizedName`)
                VALUES (3, NULL, 'tradutor', 'TRADUTOR');
            ");

            // Inserir Permissões apenas se não existirem
            migrationBuilder.Sql(@"
                INSERT IGNORE INTO `Permissoes` (`Id`, `Acao`, `CriadoEm`, `Descricao`, `Nome`, `Recurso`) VALUES
                (1, 'Visualizar', NOW(), 'Permite visualizar obras', 'VisualizarObras', 'Obras'),
                (2, 'Criar', NOW(), 'Permite criar obras', 'CriarObras', 'Obras'),
                (3, 'Editar', NOW(), 'Permite editar obras', 'EditarObras', 'Obras'),
                (4, 'Deletar', NOW(), 'Permite deletar obras', 'DeletarObras', 'Obras'),
                (5, 'Visualizar', NOW(), 'Permite visualizar capítulos', 'VisualizarCapitulos', 'Capitulos'),
                (6, 'Criar', NOW(), 'Permite criar capítulos', 'CriarCapitulos', 'Capitulos'),
                (7, 'Editar', NOW(), 'Permite editar capítulos', 'EditarCapitulos', 'Capitulos'),
                (8, 'Deletar', NOW(), 'Permite deletar capítulos', 'DeletarCapitulos', 'Capitulos'),
                (9, 'Gerenciar', NOW(), 'Permite gerenciar usuários', 'GerenciarUsuarios', 'Usuarios');
            ");

            // Inserir RolePermissões apenas se não existirem
            migrationBuilder.Sql(@"
                INSERT IGNORE INTO `RolePermissoes` (`RoleId`, `PermissaoId`, `AtribuidoEm`) VALUES
                (1, 1, NOW()), (1, 2, NOW()), (1, 3, NOW()), (1, 4, NOW()), (1, 5, NOW()),
                (1, 6, NOW()), (1, 7, NOW()), (1, 8, NOW()), (1, 9, NOW()),
                (2, 1, NOW()), (2, 5, NOW()),
                (3, 1, NOW()), (3, 2, NOW()), (3, 3, NOW()), (3, 5, NOW()), (3, 6, NOW()), (3, 7, NOW());
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClaimsDinamicas");

            migrationBuilder.DropTable(
                name: "RolePermissoes");

            migrationBuilder.DropTable(
                name: "Permissoes");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "62c70243-1da1-4636-b7c9-9c416dba6f41", "AQAAAAIAAYagAAAAEJp7aJHYzhmPlCyN8ppm8sxaRrOSGgSuhgDAn8hFwKqlwHe5RbT4h3GrZQ7Jz2DDxA==", "300370f6-4b73-4e12-97bd-9aaf614a5f0e" });
        }
    }
}
