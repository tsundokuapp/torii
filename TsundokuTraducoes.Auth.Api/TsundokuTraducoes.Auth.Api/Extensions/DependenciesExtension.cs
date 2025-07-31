using TsundokuTraducoes.Auth.Api.AppServices;
using TsundokuTraducoes.Auth.Api.AppServices.Interfaces;
using TsundokuTraducoes.Auth.Api.Data.Context;
using TsundokuTraducoes.Auth.Api.Services;
using TsundokuTraducoes.Auth.Api.Services.Interfaces;

namespace TsundokuTraducoes.Auth.Api.Extensions;

public static class DependenciesExtension
{
    public static void AddSqlConnection(
    this IServiceCollection services,
    string stringConnection)
    {
        services.AddDbContext<UsuarioContext>();
    }

    public static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<IUsuarioAppService, UsuarioAppService>();

        services.AddScoped<IEmailMimeService, EmailMimeService>();
        services.AddScoped<IUsuarioService, UsuarioService>();
    }
}
