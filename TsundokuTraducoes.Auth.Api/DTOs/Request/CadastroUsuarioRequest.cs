using System.ComponentModel.DataAnnotations;

namespace TsundokuTraducoes.Auth.Api.DTOs.Request
{
    public class CadastroUsuarioRequest
    {
        [Required]
        public string Usuario { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Senha { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Senha")]
        public string ConfirmaSenha { get; set; }
    }
}
