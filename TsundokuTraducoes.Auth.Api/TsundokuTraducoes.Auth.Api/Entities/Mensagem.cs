using MimeKit;

namespace TsundokuTraducoes.Auth.Api.Entities
{
    public class Mensagem
    {
        public List<MailboxAddress> Destinatarios { get; set; }
        public string Assunto { get; set; }
        public string Conteudo { get; set; }

        public Mensagem(IEnumerable<string> destinatarios, string assunto, int usuarioId, string codigoConfirmacao)
        {
            Destinatarios = [.. destinatarios.Select(destinatario => new MailboxAddress("destinatario", destinatario))];
            Assunto = assunto;
            Conteudo = RetornaConteudo(usuarioId, codigoConfirmacao);
        }

        private static string RetornaConteudo(int usuarioId, string codigoConfirmacao)
        {
            return $@"<!DOCTYPE html>
                        <html lang=""PT-br"">
                        <head>   
                            <title>Envio de E-mail Ativação</title>
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
                                <a href=""https://localhost:5002/api/auth/ativar-conta?UsuarioId={usuarioId}&CodigoAtivacao={codigoConfirmacao}"">
                                    <button>Ativar</button>
                                </a> 
                            </div>
                        </body>
                        </html>";
        }
    }
}
