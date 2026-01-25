using Microsoft.AspNetCore.Authorization;
using TsundokuTraducoes.Auth.Api.AppServices;
using TsundokuTraducoes.Auth.Api.AppServices.Interfaces;
using TsundokuTraducoes.Auth.Api.Authorization.Handlers;
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
        services.AddScoped<ICadastroAppService, CadastroAppService>();
        services.AddScoped<ICadastroService, CadastroService>();

        services.AddScoped<ILoginAppService, LoginAppService>();
        services.AddScoped<ILoginService, LoginService>();

        services.AddScoped<ITokenAppService, TokenAppService>();
        services.AddScoped<ITokenService, TokenService>();

        services.AddScoped<IEmailService, EmailService>();

        // RBAC/ABAC Services
        services.AddScoped<IPermissionService, PermissionService>();
        services.AddScoped<IClaimsService, ClaimsService>();

        // Authorization Handlers
        services.AddScoped<IAuthorizationHandler, RecursoProprioHandler>();
        services.AddScoped<IAuthorizationHandler, PermissaoHandler>();
    }
}
