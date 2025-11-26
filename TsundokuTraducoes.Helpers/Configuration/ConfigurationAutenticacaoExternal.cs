
using System.Configuration;

namespace TsundokuTraducoes.Helpers.Configuration;

public static class ConfigurationAutenticacaoExternal
{
    private static AcessoEmail _acessoEmail;
    private static JwtConfiguration _jwtConfiguration;
    private static IntegrationUrlBase _integrationUrlBase;

    public static void SetaAcessoExterno(AcessoEmail acessoEmail, JwtConfiguration jwtConfiguration, IntegrationUrlBase integrationUrlBase)
    {
        _acessoEmail = acessoEmail;
        _jwtConfiguration = jwtConfiguration;
        _integrationUrlBase = integrationUrlBase;
    }

    public static string RetornaRemetente()
    {
        var remetente = ConfigurationManager.AppSettings["Remetente"];
        remetente ??= _acessoEmail.Remetente;

        return remetente;
    }

    public static string RetornaSmtpServer()
    {
        var smtpServer = ConfigurationManager.AppSettings["SmtpServer"];
        smtpServer ??= _acessoEmail.SmtpServer;

        return smtpServer;
    }

    public static int RetornaPort()
    {
        var smtpServer = Convert.ToInt32(ConfigurationManager.AppSettings["Port"]);
        smtpServer = smtpServer > 0 ? smtpServer : _acessoEmail.Port;

        return smtpServer;
    }

    public static string RetornaPassword()
    {
        var password = ConfigurationManager.AppSettings["Password"];
        password ??= _acessoEmail.Password;

        return password;
    }

    public static string RetornaSenhaAdminInicial()
    {
        var senhaAdminInicial = ConfigurationManager.AppSettings["SenhaAdminInicial"];
        senhaAdminInicial ??= _acessoEmail.SenhaAdminInicial;

        return senhaAdminInicial;
    }

    public static string RetornaEmailAdminInicial()
    {
        var email = ConfigurationManager.AppSettings["EmailAdminInicial"];
        email ??= _acessoEmail.EmailAdminInicial;

        return email;
    }

    public static string RetornaJwtSecretToken()
    {
        var secretToken = ConfigurationManager.AppSettings["SecretToken"];
        secretToken ??= _jwtConfiguration.SecretToken;

        return secretToken;
    }

    public static string RetornaJwtSecretTokenReset()
    {
        var secretToken = ConfigurationManager.AppSettings["SecretTokenReset"];
        secretToken ??= _jwtConfiguration.SecretTokenReset;

        return secretToken;
    }

    public static int RetornaRefreshTokenValidityInMinutes()
    {
        _ = int.TryParse(ConfigurationManager.AppSettings["RefreshTokenValidityInMinutes"],
        out int refreshTokenValidityInMinutes);        

        return refreshTokenValidityInMinutes > 0 ? refreshTokenValidityInMinutes : _jwtConfiguration.RefreshTokenValidityInMinutes;
    }

    public static int RetornaResetTokenValidityInMinutes()
    {
        _ = int.TryParse(ConfigurationManager.AppSettings["ResetTokenValidityInMinutes"],
        out int resetTokenValidityInMinutes);

        return resetTokenValidityInMinutes > 0 ? resetTokenValidityInMinutes : _jwtConfiguration.ResetTokenValidityInMinutes;
    }

    public static string RetornaUrlBaseApi()
    {
        var urlBaseApi = ConfigurationManager.AppSettings["UrlBaseTsundokuApi"];
        urlBaseApi ??= _integrationUrlBase.UrlBaseTsundokuApi;

        return urlBaseApi;
    }

    public static string RetornaUrlBaseWeb()
    {
        var urlBaseFrontEnd = ConfigurationManager.AppSettings["UrlBaseTsundokuWeb"];
        urlBaseFrontEnd ??= _integrationUrlBase.UrlBaseTsundokuWeb;

        return urlBaseFrontEnd;
    }
}

public class AcessoEmail
{
    public string Remetente { get; set; }
    public string SmtpServer { get; set; }
    public int Port { get; set; }
    public string Password { get; set; }
    public string SenhaAdminInicial { get; set; }
    public string EmailAdminInicial { get; set; }
}

public class JwtConfiguration
{
    public string SecretToken { get; set; }
    public int RefreshTokenValidityInMinutes { get; set; }
    public string SecretTokenReset { get; set; }
    public int ResetTokenValidityInMinutes { get; set; }
}

public class IntegrationUrlBase
{
    public string UrlBaseTsundokuApi { get; set; }
    public string UrlBaseTsundokuWeb { get; set; }
}
