namespace TsundokuTraducoes.Auth.Api.Entities;

/// <summary>
/// Claim dinâmica para permissões específicas de usuário.
/// Sempre usa Tipo = "Permission" e Valor no formato hierárquico.
/// Exemplos de Valor:
/// - capitulo.deletar.obra.minha-novel.5
/// - obra.editar.minha-novel
/// - capitulo.*.obra.minha-novel (wildcard)
/// </summary>
public class ClaimDinamica
{
    public int Id { get; set; }
    public int UserId { get; set; }

    /// <summary>
    /// Sempre "Permission" para permissões hierárquicas
    /// </summary>
    public string Tipo { get; set; } = "Permission";

    /// <summary>
    /// Permissão hierárquica (ex: "capitulo.deletar.obra.teste.5")
    /// </summary>
    public string Valor { get; set; } = string.Empty;

    public string? Descricao { get; set; }
    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;
    public DateTime? ExpiraEm { get; set; }
    public bool Ativo { get; set; } = true;

    public CustomIdentityUser? Usuario { get; set; }
}
