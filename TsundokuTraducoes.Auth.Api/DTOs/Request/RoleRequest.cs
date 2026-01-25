namespace TsundokuTraducoes.Auth.Api.DTOs.Request;

public class RoleRequest
{
    public string Nome { get; set; } = string.Empty;
}

public class AtribuirRoleRequest
{
    public string RoleName { get; set; } = string.Empty;
}
