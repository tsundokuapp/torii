
using System.Configuration;

namespace TsundokuTraducoes.Helpers.Configuration;

public static class ConfigurationAutenticacaoExternal
{
    private static AcessoEmail _acessoEmail;
    private static JwtConfiguration _jwtConfiguration;

    public static void SetaAcessoExterno(AcessoEmail acessoEmail, JwtConfiguration jwtConfiguration)
    {
        _acessoEmail = acessoEmail;
        _jwtConfiguration = jwtConfiguration;
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

    public static string RetornaJwtTokenSecret()
    {
        var tokenSecret = ConfigurationManager.AppSettings["TokenSecret"];
        tokenSecret ??= _jwtConfiguration.TokenSecret;

        return tokenSecret;
    }

    public static int RetornaRefreshTokenValidityInMinutes()
    {
        _ = int.TryParse(ConfigurationManager.AppSettings["RefreshTokenValidityInMinutes"],
        out int refreshTokenValidityInMinutes);        

        return refreshTokenValidityInMinutes > 0 ? refreshTokenValidityInMinutes : _jwtConfiguration.RefreshTokenValidityInMinutes;
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
    public string TokenSecret { get; set; }
    public int RefreshTokenValidityInMinutes { get; set; }
}
