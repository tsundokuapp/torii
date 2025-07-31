namespace TsundokuTraducoes.Auth.Api.Services.Interfaces
{
    public interface IEmailMimeService
    {
        void EnviaEmail(string[] destinatarios, string assunto, int usuarioId, string codigoConfirmacao);
    }
}
