# ========================================
# Build Stage
# ========================================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia os arquivos de projeto primeiro para aproveitar o cache do Docker
COPY TsundokuTraducoes.Auth.Api/TsundokuTraducoes.Auth.Api.csproj TsundokuTraducoes.Auth.Api/
COPY TsundokuTraducoes.Helpers/TsundokuTraducoes.Helpers.csproj TsundokuTraducoes.Helpers/
COPY TsundokuTraducoes.Auth.Api.sln ./

# Restaura as dependências
RUN dotnet restore TsundokuTraducoes.Auth.Api.sln

# Copia todo o código fonte
COPY . .

# Build da aplicação
WORKDIR /src/TsundokuTraducoes.Auth.Api
RUN dotnet build -c Release -o /app/build

# Publica a aplicação
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# ========================================
# Runtime Stage
# ========================================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copia os arquivos publicados
COPY --from=build /app/publish .

# Cria diretório para data protection keys
RUN mkdir -p /app/keys

# Define variáveis de ambiente padrão
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:8080

# Expõe a porta da aplicação
EXPOSE 8080

# Comando de entrada
ENTRYPOINT ["dotnet", "TsundokuTraducoes.Auth.Api.dll"]
