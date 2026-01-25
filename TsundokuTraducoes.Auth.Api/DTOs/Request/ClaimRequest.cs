using System.ComponentModel.DataAnnotations;

namespace TsundokuTraducoes.Auth.Api.DTOs.Request;

/// <summary>
/// Request para criar/atualizar uma permissão específica.
/// Formato: recurso.acao[.contexto.id]
/// Exemplos: capitulo.deletar.obra.teste.5, obra.editar.minha-novel
/// </summary>
public class PermissaoUsuarioRequest
{
    /// <summary>
    /// Permissão hierárquica (ex: "capitulo.deletar.obra.teste.5")
    /// </summary>
    [Required]
    public string Permissao { get; set; } = string.Empty;

    /// <summary>
    /// Descrição opcional da permissão
    /// </summary>
    public string? Descricao { get; set; }

    /// <summary>
    /// Data de expiração (null = permanente)
    /// </summary>
    public DateTime? ExpiraEm { get; set; }
}

/// <summary>
/// Request para atribuir permissão a um usuário específico
/// </summary>
public class AtribuirPermissaoRequest
{
    [Required]
    public int UserId { get; set; }

    /// <summary>
    /// Permissão hierárquica (ex: "capitulo.deletar.obra.teste.5")
    /// </summary>
    [Required]
    public string Permissao { get; set; } = string.Empty;

    public string? Descricao { get; set; }
    public DateTime? ExpiraEm { get; set; }
}
