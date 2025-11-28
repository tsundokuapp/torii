# Esta fase é usada para baixar o .net runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8082
EXPOSE 8083

# Esta fase é usada para compilar o projeto de serviço
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["TsundokuTraducoes.Auth.Api/TsundokuTraducoes.Auth.Api.csproj", "TsundokuTraducoes.Auth.Api/"]
COPY ["TsundokuTraducoes.Helpers/TsundokuTraducoes.Helpers.csproj", "TsundokuTraducoes.Helpers/"]

RUN dotnet restore "./TsundokuTraducoes.Auth.Api/TsundokuTraducoes.Auth.Api.csproj"

COPY . .

WORKDIR "/src/TsundokuTraducoes.Auth.Api"
RUN dotnet build -c $BUILD_CONFIGURATION -o /app/build

FROM build as publish
ARG BUILD_CONFIGURATION=Release
# Execute publish no diretório correto
RUN dotnet publish -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# CORREÇÃO AQUI: Ajuste o caminho de origem do certificado para ser relativo ao contexto (se estiver na raiz)
# Se a pasta TsundokuTraducoes.Helpers estiver na raiz do seu build context:
RUN mkdir -p /app/certificados
COPY TsundokuTraducoes.Helpers/Certificados/aspnetapp.pfx /app/certificados

ENTRYPOINT [ "dotnet", "TsundokuTraducoes.Auth.Api.dll" ]
