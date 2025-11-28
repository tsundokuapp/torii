# Esta fase é usada para baixar o .net runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8082
EXPOSE 8083

# Esta fase é usada para compilar o projeto de serviço
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["/TsundokuTraducoes.Auth.Api/TsundokuTraducoes.Auth.Api/TsundokuTraducoes.Auth.Api.csproj", "TsundokuTraducoes.Auth.Api/"]
COPY ["/TsundokuTraducoes.Helpers/TsundokuTraducoes.Helpers.csproj", "TsundokuTraducoes.Helpers/"]

RUN dotnet restore "./TsundokuTraducoes.Auth.Api/TsundokuTraducoes.Auth.Api.csproj"
RUN dotnet restore "./TsundokuTraducoes.Auth.Api/TsundokuTraducoes.Helpers.csproj"
COPY . .

WORKDIR "/src/TsundokuTraducoes.Auth.Api"
RUN dotnet build "./TsundokuTraducoes.Auth.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build as publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TsundokuTraducoes.Auth.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

RUN mkdir -p /app/certificados
COPY TsundokuTraducoes.Helpers/Certificados/aspnetapp.pfx /app/certificados

ENTRYPOINT [ "dotnet", "TsundokuTraducoes.Auth.Api.dll" ]