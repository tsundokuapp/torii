namespace TsundokuTraducoes.Auth.Api.Services.Interfaces
{
    public interface IEmailService
    {
        void EnviaEmail(string[] destinatarios, string assunto, int usuarioId, string codigoConfirmacao);
    }
}
