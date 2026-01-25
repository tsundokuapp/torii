using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TsundokuTraducoes.Auth.Api.Entities;
using TsundokuTraducoes.Helpers.Configuration;

namespace TsundokuTraducoes.Auth.Api.Data.Context;

public class UsuarioContext : IdentityDbContext<CustomIdentityUser, IdentityRole<int>, int>
{
    public UsuarioContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
    {
    }

    // DbSets para RBAC/ABAC
    public DbSet<ClaimDinamica> ClaimsDinamicas { get; set; }
    public DbSet<Permissao> Permissoes { get; set; }
    public DbSet<RolePermissao> RolePermissoes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            string stringconexao = SourceConnection.RetornaConnectionStringConfig();
            optionsBuilder.UseMySql(stringconexao, ServerVersion.AutoDetect(stringconexao));
            base.OnConfiguring(optionsBuilder);
        }
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        var admin = new CustomIdentityUser
        {
            UserName = "admin",
            NormalizedUserName = "ADMIN",
            Email = ConfigurationAutenticacaoExternal.RetornaEmailAdminInicial(),
            NormalizedEmail = ConfigurationAutenticacaoExternal.RetornaEmailAdminInicial().ToUpper(),
            EmailConfirmed = true,
            SecurityStamp = Guid.NewGuid().ToString(),
            Id = 1
        };

        //builder.Entity<CustomIdentityUser>(entity => { entity.HasIndex(e => e.Id); });

        builder.Entity<IdentityRole<int>>().HasData(
            new IdentityRole<int> { Id = 1, Name = "admin", NormalizedName = "ADMIN" },
            new IdentityRole<int> { Id = 2, Name = "leitor", NormalizedName = "LEITOR" },
            new IdentityRole<int> { Id = 3, Name = "tradutor", NormalizedName = "TRADUTOR" }
        );

        var hasher = new PasswordHasher<CustomIdentityUser>();
        admin.PasswordHash = hasher.HashPassword(admin, ConfigurationAutenticacaoExternal.RetornaSenhaAdminInicial());
        builder.Entity<CustomIdentityUser>().HasData(admin);

        builder.Entity<IdentityUserRole<int>>().HasData(
            new IdentityUserRole<int> { RoleId = 1, UserId = admin.Id }
        );

        // Configuração ClaimDinamica
        builder.Entity<ClaimDinamica>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.HasIndex(c => new { c.UserId, c.Tipo, c.Valor }).IsUnique();
            entity.HasOne(c => c.Usuario)
                  .WithMany()
                  .HasForeignKey(c => c.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Configuração RolePermissao
        builder.Entity<RolePermissao>(entity =>
        {
            entity.HasKey(rp => new { rp.RoleId, rp.PermissaoId });
            entity.HasOne(rp => rp.Role)
                  .WithMany()
                  .HasForeignKey(rp => rp.RoleId);
            entity.HasOne(rp => rp.Permissao)
                  .WithMany(p => p.RolePermissoes)
                  .HasForeignKey(rp => rp.PermissaoId);
        });

        // Seed de Permissões (formato hierárquico: recurso.acao)
        builder.Entity<Permissao>().HasData(
            // Obras
            new Permissao { Id = 1, Valor = "obra.visualizar", Descricao = "Visualizar qualquer obra" },
            new Permissao { Id = 2, Valor = "obra.criar", Descricao = "Criar obras" },
            new Permissao { Id = 3, Valor = "obra.editar", Descricao = "Editar qualquer obra" },
            new Permissao { Id = 4, Valor = "obra.deletar", Descricao = "Deletar qualquer obra" },
            // Capítulos
            new Permissao { Id = 5, Valor = "capitulo.visualizar", Descricao = "Visualizar qualquer capítulo" },
            new Permissao { Id = 6, Valor = "capitulo.criar", Descricao = "Criar capítulos" },
            new Permissao { Id = 7, Valor = "capitulo.editar", Descricao = "Editar qualquer capítulo" },
            new Permissao { Id = 8, Valor = "capitulo.deletar", Descricao = "Deletar qualquer capítulo" },
            // Usuários
            new Permissao { Id = 9, Valor = "usuario.gerenciar", Descricao = "Gerenciar usuários" },
            // Super permissão (admin)
            new Permissao { Id = 10, Valor = "*.*", Descricao = "Acesso total a todos os recursos" }
        );

        // Seed de RolePermissoes
        builder.Entity<RolePermissao>().HasData(
            // Admin tem permissão total (*.*)
            new RolePermissao { RoleId = 1, PermissaoId = 10 },
            // Tradutor pode visualizar/criar/editar obras e capítulos
            new RolePermissao { RoleId = 3, PermissaoId = 1 },
            new RolePermissao { RoleId = 3, PermissaoId = 2 },
            new RolePermissao { RoleId = 3, PermissaoId = 3 },
            new RolePermissao { RoleId = 3, PermissaoId = 5 },
            new RolePermissao { RoleId = 3, PermissaoId = 6 },
            new RolePermissao { RoleId = 3, PermissaoId = 7 },
            // Leitor pode apenas visualizar
            new RolePermissao { RoleId = 2, PermissaoId = 1 },
            new RolePermissao { RoleId = 2, PermissaoId = 5 }
        );
    }
}
