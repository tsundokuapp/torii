using Microsoft.AspNetCore.Identity;

namespace TsundokuTraducoes.Auth.Api.Entities;

public class RolePermissao
{
    public int RoleId { get; set; }
    public int PermissaoId { get; set; }
    public DateTime AtribuidoEm { get; set; } = DateTime.UtcNow;

    public IdentityRole<int>? Role { get; set; }
    public Permissao? Permissao { get; set; }
}
