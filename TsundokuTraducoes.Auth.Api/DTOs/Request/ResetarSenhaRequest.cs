using System.ComponentModel.DataAnnotations;

namespace TsundokuTraducoes.Auth.Api.DTOs.Request;

public class ResetarSenhaRequest
{
    [Required]
    [DataType(DataType.Password)]
    public string NovaSenha { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Compare("NovaSenha")]
    public string ConfirmaNovaSenha { get; set; }

    [Required]
    public string Token { get; set; }
}
