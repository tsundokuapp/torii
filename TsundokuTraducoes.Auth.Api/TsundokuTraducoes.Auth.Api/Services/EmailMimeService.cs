﻿using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;
using System.Web;
using TsundokuTraducoes.Auth.Api.Entities;
using TsundokuTraducoes.Auth.Api.Services.Interfaces;
using TsundokuTraducoes.Helpers.Configuration;

namespace TsundokuTraducoes.Auth.Api.Services;

public class EmailMimeService : IEmailMimeService
{
    public void EnviaEmail(string[] destinatarios, string assunto, int usuarioId, string codigoConfirmacao)
    {
        //Encodando o codigoConfirmacao para não ter problemas na hora de passar a variável pela url como parâmetro
        var mensagem = new Mensagem(destinatarios, assunto, usuarioId, HttpUtility.UrlEncode(codigoConfirmacao));
        var memsagemDeEmail = RetornaCorpoDoEmail(mensagem);
        Enviar(memsagemDeEmail);
    }

    private static MimeMessage RetornaCorpoDoEmail(Mensagem mensagem)
    {
        var mensagemDeEmail = new MimeMessage();
        mensagemDeEmail.From.Add(new MailboxAddress("Remetente", ConfigurationAutenticacaoExternal.RetornaRemetente()));
        mensagemDeEmail.To.AddRange(mensagem.Destinatarios);
        mensagemDeEmail.Subject = mensagem.Assunto;
        //Necessário conversão da string para um tipo TextPart (MIME), é o que o e-mail aceita
        mensagemDeEmail.Body = new TextPart(TextFormat.Html) { Text = mensagem.Conteudo };
        return mensagemDeEmail;
    }

    private static void Enviar(MimeMessage memsagemDeEmail)
    {
        using (var smtpClient = new SmtpClient())
        {
            try
            {

                var smptServer = ConfigurationAutenticacaoExternal.RetornaSmtpServer();
                var port = ConfigurationAutenticacaoExternal.RetornaPort();
                var remetente = ConfigurationAutenticacaoExternal.RetornaRemetente();
                var password = ConfigurationAutenticacaoExternal.RetornaPassword();

                //Conectando com o servidor
                smtpClient.Connect(smptServer, port);                
                smtpClient.AuthenticationMechanisms.Remove("XOUATH2");
                //Autenticação
                smtpClient.Authenticate(remetente, password);
                //TODO Auth do e-mail
                smtpClient.Send(memsagemDeEmail);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //Independente se deu certo ou não é necessário desconectar e liberar os recursos do smtpClient (cliente)
                smtpClient.Disconnect(true);
                smtpClient.Dispose();
            }
        }
    }
}
