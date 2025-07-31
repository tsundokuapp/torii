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
            new IdentityRole<int> { Id = 1, Name = "admin", NormalizedName = "ADMIN" }
        );
        builder.Entity<IdentityRole<int>>().HasData(
           new IdentityRole<int> { Id = 2, Name = "leitor", NormalizedName = "LEITOR" }
        );

        var hasher = new PasswordHasher<CustomIdentityUser>();
        admin.PasswordHash = hasher.HashPassword(admin, ConfigurationAutenticacaoExternal.RetornaSenhaAdminInicial());
        builder.Entity<CustomIdentityUser>().HasData(admin);

        builder.Entity<IdentityUserRole<int>>().HasData(
            new IdentityUserRole<int> { RoleId = 1, UserId = admin.Id } 
        );
    }
}
