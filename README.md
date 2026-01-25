![screenshot](https://raw.githubusercontent.com/tsundokuapp/torii/refs/heads/main/public/torii-design-615.png)

## Tsundoku Traduções V2 - API Auth
Tsundoku é um site voltado para leitura de Light Novels e Mangás, que já está ativo no link [Tsundoku](https://tsundoku.com.br/).
Este repo é a atualização backend do site para uma estrutura mais moderna e única com objetivos de solucionar problemas e limitações enfrentados no site atual.

## Contribuindo

A Tsundoku adoraria sua ajuda, apesar do esforço, o projeto é grande e muitos pontos ainda estão por serem decididos. Toda a ajuda é bem vinda e caso se interesse, segue alguns passos necessários para a contribuição:

### Primeiros passos

- Fazer um fork do projeto
- Clonar o projeto em sua máquina
- Rodar o comando "git pull" para se certificar das alterações

## Orientações sobre Pull Requests

Tente seguir essas orientações o tanto quanto possível para minimizar o trabalho de todos.

**1.** Antes de iniciar o processo de contribuição,  **crie uma nova branch**  para fazer suas alterações.

Alguns exemplos:
-   Para novas features:  `git checkout -b feat/tabela-usuarios`
-   Para correções:  `git checkout -b fix/campo-retornando-null`

**2.**  Após realizar as alterações, é hora de fazer um commit com uma mensagem coerente do que foi feito. Exemplo:
```
git add --all
git commit -am ‘fix(usuario): adiciona campo id externo na tabela user’
git push origin fix/usuario
```
o exemplo segue o padrão do Conventional Commits, se não estiver habituado com Conventional Commits, não se preocupe, basta que a mensagem seja clara e direta.
 Leia sobre em: [Conventional Commits](https://www.conventionalcommits.org/pt-br/v1.0.0/)

**3.**  Envie um  _Pull Request_  com as alterações feitas, fazendo referência para o  `main`  do repositório oficial.

**4.**  Sua contribuição será analisada. Em alguns casos pediremos algumas alterações antes de dar merge.

Após o merge:
-   Delete a branch utilizada:
```
git checkout main
git push origin --delete <nome_da_branch>
git branch -D <nome_da_branch>
```
-   Atualize seu repositório com o repositório oficial:
```
git fetch upstream
git rebase upstream/main
git push -f origin main
```
**5.**  Quando iniciar uma nova contribuição, repita o processo do inicio, criando uma nova branch.


## Lembrente

- Evite mexer em arquivos desconexos dentro de um mesmo commit
- Não altere versões de libs ou ferramentas.
- Se precisar de ajuda, pergunte diretamente informando o problema que está tendo.


## Postman

- Para realizar os testes de crud é necessário usar o Postman (Ou algum app de requisição de seu interesse). Caso use o Postman, existe uma collection e variáveis de ambiente disponível.
  - Diretório das collections no Drive: [Collections](https://drive.google.com/drive/folders/1bhmK9wYH26zVlEMj0mxudu7KL6fvR8-t?usp=sharing)
- Basta importar, no Postam, o arquivo json disponível e utilizar.

<br />
<br />

# Iniciando o projeto (ambiente windows)

## Configuração de Variáveis de Ambiente

O projeto utiliza variáveis de ambiente para dados sensíveis. Siga os passos:

### Opção 1: Arquivo .env (Recomendado para desenvolvimento)

1. Copie o arquivo de exemplo:
```sh
cp TsundokuTraducoes.Auth.Api/.env.example TsundokuTraducoes.Auth.Api/.env
```

2. Edite o `.env` com seus valores reais:
```env
ConnectionStrings__UsuarioConnection=Server=localhost;Database=DbTsundokuAuth;user=root;password=SUA_SENHA;
AcessoEmail__Password=sua_senha_smtp
AcessoEmail__EmailAdminInicial=admin@email.com
AcessoEmail__SenhaAdminInicial=SenhaForte123!
JwtConfiguration__SecretToken=gere_com_openssl_rand_hex_32
JwtConfiguration__SecretTokenReset=gere_outro_token_diferente
```

3. Carregue as variáveis antes de rodar:
```sh
# Linux/Mac
export $(cat .env | xargs)

# Windows PowerShell
Get-Content .env | ForEach-Object { $var = $_.Split('='); [Environment]::SetEnvironmentVariable($var[0], $var[1]) }
```

### Opção 2: Variáveis de ambiente do sistema

Configure diretamente no seu sistema operacional ou no seu ambiente de deploy (Docker, Kubernetes, etc).

### Gerar tokens JWT seguros

```sh
# Linux/Mac
openssl rand -hex 32

# Ou online: https://generate-secret.vercel.app/64
```

### Variáveis Obrigatórias

| Variável | Descrição |
|----------|-----------|
| `ConnectionStrings__UsuarioConnection` | Connection string do banco MySQL |
| `AcessoEmail__Password` | Senha do servidor SMTP |
| `AcessoEmail__EmailAdminInicial` | Email do admin inicial |
| `AcessoEmail__SenhaAdminInicial` | Senha do admin inicial |
| `JwtConfiguration__SecretToken` | Secret para tokens JWT (64 chars hex) |
| `JwtConfiguration__SecretTokenReset` | Secret para reset de senha (64 chars hex) |

## Executando o projeto

- instalar o pacote dotnet-ef >
```sh
dotnet tool install --global dotnet-ef
```
(_Geralmente é necessário no VSCode_)
  - Rodar o comando ```update-database```
    - Visual Studio Code > ```dotnet ef database update```

- E em seguida rodar o projeto para subir a api
  - Visual Studio Code > ```dotnet run```

<br />

# Iniciando o projeto (ambiente docker)

Comandos docker:

- Após realizar o clone dentro do ambiente Docker, acessar a seguinte pasta:
  - cd torii/

- Criando uma rede para comunicação entre os containers (opcional)
```sh
docker network create tsundoku
```

- Subindo banco MySql (opcional)<br />
  Criando um container MySql com a nova rede, lembrando que esse nome vai no arquivo appconfig que está no Drive
```sh
docker run --name=mysql -e MYSQL_ROOT_PASSWORD=1234 -p 3306:3306 -d --network tsundoku mysql
```

- Antes de gerar o build da imagem
  - Baixar o arquivo **"appsettings.json"** que se encontra no drive ``15 - Tsun Dev > Arquivos Config Api Tsun > TsundokuTraducoes `` e adicionar na pasta **"TsundokuTraducoes"**, dentro do projeto. (link da pasta acima)

- Buildar imagem
  ```sh
  docker build -t tsundokuapi_auth:1.0 .
  ```
     - Onde "tsundokuapi_auth:1.0" seria o nome da imagem e versão

- criando um container tsundokuapi com a nova rede
```sh
docker run --rm -it -p 8082:80 -p 8083:443 -e ASPNETCORE_URLS="https://+;http://+" -e ASPNETCORE_HTTPS_PORTS=8083 -e ASPNETCORE_Kestrel__Certificates__Default__Password="tsundokuapi" -e ASPNETCORE_ENVIRONMENT=Development -e ASPNETCORE_Kestrel__Certificates__Default__Path=/app/certificados/aspnetapp.pfx -v \TsundokuTraducoes.Auth.Api\.aspnet\https:/https/ --name tsundoku-api-auth tsundokuapi_auth:1.0
```

- Observação: O link para o local host da api de auth é:

- Acessar BD
   - Usar id do container do banco de dados
      - ```docker exec -it id_imagem bash```

   - Onde -p1234 seria a senha do banco já definida para acessar
      - ```mysql -u root -p1234```

  - Lista as tabelas
     - ```show databases;```

  - Seta a tabela para poder realizar as consultas sql (Caso banco local)
    - ```use DbTsundoku;```

  - Seta a tabela para poder realizar as consultas sql (Caso banco remoto)
    - ```use u322048751_tsun_bd;```

  - Lista as tabelas
    - ```show tables;```

  - Consulta de tabelas
    - ```SELECT * FROM AspNetUsers;```

<br />
<br />

## Testes Unitários (Em construção)

## Testes de Integração (Em construção)

## Observações sobre os testes

- Até o momento eu não achei informações de rodar os testes de forma separada via comando.
- Para mais informações, consultar a documentação do pacote: [Xunit.Net](https://xunit.net/docs/getting-started/netcore/cmdline)
