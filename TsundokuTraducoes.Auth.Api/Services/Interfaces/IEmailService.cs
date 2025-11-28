using TsundokuTraducoes.Helpers.Utils.Enums;

namespace TsundokuTraducoes.Auth.Api.Services.Interfaces;

public interface IEmailService
{
    void EnviaEmail(string[] destinatarios, string assunto, string linkServicoSolici, TipoEnvioEmailEnum tipoEnvioEmailEnum);
}
