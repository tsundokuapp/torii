namespace TsundokuTraducoes.Auth.Api.DTOs.Response;

/// <summary>
/// Response de uma permissão (role ou dinâmica)
/// </summary>
public class PermissaoResponse
{
    public int Id { get; set; }

    /// <summary>
    /// Permissão hierárquica (ex: "capitulo.deletar.obra.teste.5")
    /// </summary>
    public string Valor { get; set; } = string.Empty;

    public string Descricao { get; set; } = string.Empty;
    public DateTime CriadoEm { get; set; }
    public DateTime? ExpiraEm { get; set; }
    public bool Ativo { get; set; } = true;
}
