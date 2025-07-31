using System.ComponentModel.DataAnnotations;

namespace TsundokuTraducoes.Auth.Api.DTOs.Request;

public class AtivaUsuarioRequest
{
    [Required]
    public int UsuarioId { get; set; }

    [Required]
    public string CodigoAtivacao { get; set; }
}
