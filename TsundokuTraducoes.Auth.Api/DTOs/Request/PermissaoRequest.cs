using System.ComponentModel.DataAnnotations;

namespace TsundokuTraducoes.Auth.Api.DTOs.Request;

/// <summary>
/// Request para criar uma nova permissão base no sistema
/// </summary>
public class PermissaoRequest
{
    /// <summary>
    /// Permissão hierárquica (ex: "capitulo.deletar")
    /// </summary>
    [Required]
    public string Valor { get; set; } = string.Empty;

    public string Descricao { get; set; } = string.Empty;
}
