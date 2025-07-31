

using System.Configuration;

namespace TsundokuTraducoes.Helpers.Configuration;

public static class ConfigurationAutenticacaoExternal
{
    private static AcessoEmail _acessoEmail;

    public static void SetaAcessoExterno(AcessoEmail acessoEmail)
    {
        _acessoEmail = acessoEmail;
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
        var password = ConfigurationManager.AppSettings["SenhaAdminInicial"];
        password ??= _acessoEmail.SenhaAdminInicial;

        return password;
    }

    public static string RetornaEmailAdminInicial()
    {
        var password = ConfigurationManager.AppSettings["EmailAdminInicial"];
        password ??= _acessoEmail.EmailAdminInicial;

        return password;
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
