namespace TsundokuTraducoes.Auth.Api.Entities;

/// <summary>
/// Permissão hierárquica no formato: recurso.acao[.contexto.id]
/// Exemplos:
/// - obra.visualizar
/// - obra.deletar
/// - capitulo.criar.obra.minha-novel
/// - capitulo.deletar.obra.minha-novel.5
/// - *.* (todas as permissões)
/// </summary>
public class Permissao
{
    public int Id { get; set; }

    /// <summary>
    /// Permissão hierárquica (ex: "capitulo.deletar.obra.teste.5")
    /// </summary>
    public string Valor { get; set; } = string.Empty;

    /// <summary>
    /// Descrição legível da permissão
    /// </summary>
    public string Descricao { get; set; } = string.Empty;

    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;

    public ICollection<RolePermissao> RolePermissoes { get; set; } = new List<RolePermissao>();
}
