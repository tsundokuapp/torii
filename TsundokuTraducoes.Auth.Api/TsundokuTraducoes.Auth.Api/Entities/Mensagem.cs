using MimeKit;
using TsundokuTraducoes.Helpers.Utils.Enums;

namespace TsundokuTraducoes.Auth.Api.Entities;

public class Mensagem
{
    public List<MailboxAddress> Destinatarios { get; set; }
    public string Assunto { get; set; }
    public string Conteudo { get; set; }

    public Mensagem(IEnumerable<string> destinatarios, string assunto, string linkServicoSolicitado, TipoEnvioEmailEnum tipoEnvioEmailEnum)
    {
        Destinatarios = [.. destinatarios.Select(destinatario => new MailboxAddress("destinatario", destinatario))];
        Assunto = assunto;
        Conteudo = RetornaConteudo(assunto, linkServicoSolicitado, tipoEnvioEmailEnum);
    }

    private static string RetornaConteudo(string assunto, string linkServicoSolicitado, TipoEnvioEmailEnum tipoEnvioEmailEnum)
    {
        return tipoEnvioEmailEnum switch
        {
            TipoEnvioEmailEnum.AtivacaoConta => RetornaConteudoAtivacaoConta(assunto, linkServicoSolicitado),
            TipoEnvioEmailEnum.RecuperacaoSenha => RetornaConteudoRecuperacaoSenha(assunto, linkServicoSolicitado),
            _ => "Entre em contato com o suporte da Tsun",
        };
    }

    private static string RetornaConteudoAtivacaoConta(string assunto, string linkServicoSolicitado)
    {
        return $@"<!DOCTYPE html>
                        <html lang=""PT-br"">
                        <head>   
                            <title>Envio de E-mail de {assunto}</title>
                            <style>
                                body{{
                                    background-color: #202225;
                                    color: aliceblue;
            
                                }}
                                .principal{{            
                                    display: flex;
                                    flex-direction: column;
                                    align-items: center;
            
                                }}

                                a{{
                                    text-decoration: none;
                                }}

                                button{{
                                    width: 100px;
                                    height: 50px;
                                    background-color: cornflowerblue;
                                    color: aliceblue;
                                    font-size: large;
                                    cursor: pointer;           
                                    text-align: center;
                                    justify-content: center;
                                    border-radius: 7px;
                                }}

                                button:hover{{
                                    opacity: 0.5;
                                }}

                            </style>
                        </head>
                        <body>
                            <div class=""principal"">
                                <h2>Para ativar sua conta clique no botão abaixo:</h2>
                                <br />
                                <a href={linkServicoSolicitado}>
                                    <button>Ativar</button>
                                </a> 
                            </div>
                        </body>
                        </html>";
    }

    private static string RetornaConteudoRecuperacaoSenha(string assunto, string linkServicoSolicitado)
    {
        return $@"<!DOCTYPE html>
                        <html lang=""PT-br"">
                        <head>   
                            <title>Envio de E-mail de {assunto}</title>
                            <style>
                                body{{
                                    background-color: #202225;
                                    color: aliceblue;
            
                                }}
                                .principal{{            
                                    display: flex;
                                    flex-direction: column;
                                    align-items: center;
            
                                }}

                                a{{
                                    text-decoration: none;
                                }}

                                button{{
                                    width: 100px;
                                    height: 50px;
                                    background-color: cornflowerblue;
                                    color: aliceblue;
                                    font-size: large;
                                    cursor: pointer;           
                                    text-align: center;
                                    justify-content: center;
                                    border-radius: 7px;
                                }}

                                button:hover{{
                                    opacity: 0.5;
                                }}

                            </style>
                        </head>
                        <body>
                            <div class=""principal"">
                                <h2>Para recuperar sua senha clique no botão abaixo:</h2>
                                <br />
                                <a href={linkServicoSolicitado}>
                                    <button>Recuperar</button>
                                </a> 
                            </div>
                        </body>
                        </html>";
    }
}
