# Esta fase � usada para baixar o .net runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8082
EXPOSE 8083

# Esta fase � usada para compilar o projeto de servi�o
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["TsundokuTraducoes.Auth.Api/TsundokuTraducoes.Auth.Api.csproj", "TsundokuTraducoes.Auth.Api/"]
COPY ["TsundokuTraducoes.Helpers/TsundokuTraducoes.Helpers.csproj", "TsundokuTraducoes.Helpers/"]

RUN dotnet restore "./TsundokuTraducoes.Auth.Api/TsundokuTraducoes.Auth.Api.csproj"

COPY . .

WORKDIR "/src/TsundokuTraducoes.Auth.Api"
RUN dotnet build -c $BUILD_CONFIGURATION -o /app/build /p:UseAppHost=false

FROM build as publish
ARG BUILD_CONFIGURATION=Release
# Execute publish no diret�rio correto
RUN dotnet publish -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# CORRE��O AQUI: Ajuste o caminho de origem do certificado para ser relativo ao contexto (se estiver na raiz)
# Se a pasta TsundokuTraducoes.Helpers estiver na raiz do seu build context:
RUN mkdir -p /app/certificados
COPY TsundokuTraducoes.Helpers/Certificados/aspnetapp.pfx /app/certificados

# Variáveis de ambiente (valores padrão, sobrescreva no docker run ou docker-compose)
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ConnectionStrings__UsuarioConnection=""
ENV AcessoEmail__SmtpServer=""
ENV AcessoEmail__Port="465"
ENV AcessoEmail__Password=""
ENV AcessoEmail__Remetente=""
ENV AcessoEmail__EmailAdminInicial=""
ENV AcessoEmail__SenhaAdminInicial=""
ENV JwtConfiguration__SecretToken=""
ENV JwtConfiguration__SecretTokenReset=""

ENTRYPOINT [ "dotnet", "TsundokuTraducoes.Auth.Api.dll" ]
