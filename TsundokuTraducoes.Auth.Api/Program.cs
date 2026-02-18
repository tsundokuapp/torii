using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TsundokuTraducoes.Auth.Api;
using TsundokuTraducoes.Auth.Api.Authorization.Requirements;
using TsundokuTraducoes.Auth.Api.Data.Context;
using TsundokuTraducoes.Auth.Api.Entities;
using TsundokuTraducoes.Auth.Api.Extensions;
using TsundokuTraducoes.Helpers.Configuration;

var _connectionStringConfig = new ConnectionStringConfig();
var _acessoEmail = new AcessoEmail();
var _jwtConfiguration = new JwtConfiguration();
var _integrationUrlBase = new IntegrationUrlBase();

var builder = WebApplication.CreateBuilder(args);

var allowedOrigins = builder.Configuration
    .GetSection("Cors:AllowedOrigins")
    .Get<string[]>();

var CertificatePassword = builder.Configuration
    .GetSection("CertificateSettings")
    .GetValue<string>("Password");
var CertificatePath = builder.Configuration
    .GetSection("CertificateSettings")
    .GetValue<string>("Path");

_acessoEmail.SmtpServer = builder.Configuration.GetSection("AcessoEmail").GetValue<string>("SmtpServer");
_acessoEmail.Port = Convert.ToInt32(builder.Configuration.GetSection("AcessoEmail").GetValue<string>("Port"));
_acessoEmail.Remetente = builder.Configuration.GetSection("AcessoEmail").GetValue<string>("Remetente");
_acessoEmail.Password = builder.Configuration.GetSection("AcessoEmail").GetValue<string>("Password");
_acessoEmail.EmailAdminInicial = builder.Configuration.GetSection("AcessoEmail").GetValue<string>("EmailAdminInicial");
_acessoEmail.SenhaAdminInicial = builder.Configuration.GetSection("AcessoEmail").GetValue<string>("SenhaAdminInicial");

_jwtConfiguration.SecretToken = builder.Configuration.GetSection("JwtConfiguration").GetValue<string>("SecretToken");
_jwtConfiguration.RefreshTokenValidityInMinutes = Convert.ToInt32(builder.Configuration.GetSection("JwtConfiguration").GetValue<string>("RefreshTokenValidityInMinutes"));
_jwtConfiguration.SecretTokenReset = builder.Configuration.GetSection("JwtConfiguration").GetValue<string>("SecretTokenReset");
_jwtConfiguration.ResetTokenValidityInMinutes = Convert.ToInt32(builder.Configuration.GetSection("JwtConfiguration").GetValue<string>("ResetTokenValidityInMinutes"));

_integrationUrlBase.UrlBaseTsundokuApi = builder.Configuration.GetSection("IntegrationUrlBase").GetValue<string>("UrlBaseTsundokuApi");
_integrationUrlBase.UrlBaseTsundokuWeb = builder.Configuration.GetSection("IntegrationUrlBase").GetValue<string>("UrlBaseTsundokuWeb");

ConfigurationAutenticacaoExternal.SetaAcessoExterno(_acessoEmail, _jwtConfiguration, _integrationUrlBase);

_connectionStringConfig.ConnectionString = builder.Configuration.GetConnectionString("UsuarioConnection");
SourceConnection.SetaConnectionStringConfig(_connectionStringConfig);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .WithOrigins(allowedOrigins ?? Array.Empty<string>())
            .AllowCredentials()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddSqlConnection(_connectionStringConfig.ConnectionString!);

//Injetando depend�ncias do Identity
builder.Services

    .AddIdentity<CustomIdentityUser, IdentityRole<int>>(
        //Habilitando a op��o de obriga��o da confirma��o do e-mail
        opt => opt.SignIn.RequireConfirmedEmail = true)
    .AddEntityFrameworkStores<UsuarioContext>()
    //Adiciona um provedor de tokens para confirma��o de acesso e afins
    .AddDefaultTokenProviders();

builder.Services.AddServices();

// Configuração de políticas de autorização RBAC/ABAC
builder.Services.AddAuthorization(options =>
{
    // Políticas RBAC
    options.AddPolicy("ApenasAdmin", policy =>
        policy.RequireRole("admin"));

    options.AddPolicy("TradutorOuAdmin", policy =>
        policy.RequireRole("admin", "tradutor"));

    options.AddPolicy("UsuarioAutenticado", policy =>
        policy.RequireAuthenticatedUser());

    // Políticas ABAC
    options.AddPolicy("RecursoProprio", policy =>
        policy.Requirements.Add(new RecursoProprioRequirement()));

    // Políticas baseadas em permissões
    options.AddPolicy("PodeCriarObra", policy =>
        policy.RequireAssertion(context =>
            context.User.IsInRole("admin") ||
            context.User.HasClaim("permissao", "Obras:Criar")));

    options.AddPolicy("PodeEditarObra", policy =>
        policy.RequireAssertion(context =>
            context.User.IsInRole("admin") ||
            context.User.HasClaim("permissao", "Obras:Editar")));

    options.AddPolicy("PodeDeletarObra", policy =>
        policy.RequireAssertion(context =>
            context.User.IsInRole("admin") ||
            context.User.HasClaim("permissao", "Obras:Deletar")));

    options.AddPolicy("PodeCriarCapitulo", policy =>
        policy.RequireAssertion(context =>
            context.User.IsInRole("admin") ||
            context.User.HasClaim("permissao", "Capitulos:Criar")));

    options.AddPolicy("PodeEditarCapitulo", policy =>
        policy.RequireAssertion(context =>
            context.User.IsInRole("admin") ||
            context.User.HasClaim("permissao", "Capitulos:Editar")));

    options.AddPolicy("PodeDeletarCapitulo", policy =>
        policy.RequireAssertion(context =>
            context.User.IsInRole("admin") ||
            context.User.HasClaim("permissao", "Capitulos:Deletar")));

    options.AddPolicy("PodeGerenciarUsuarios", policy =>
        policy.RequireAssertion(context =>
            context.User.IsInRole("admin") ||
            context.User.HasClaim("permissao", "Usuarios:Gerenciar")));
});

//definindo configura��es de autentica��o
builder.Services.AddAuthentication(auth =>
{
    auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(token =>
{
    token.RequireHttpsMetadata = false;
    token.SaveToken = true;
    token.MapInboundClaims = false; // Evita mapeamento automático de claims
    token.TokenValidationParameters = new TokenValidationParameters
    {
        // Essa opção permite que o middleware entenda que agora será usado o claim "roles" e não o do schema do identity (ClaimTypes.Role)
        RoleClaimType = "roles",
        ValidateIssuerSigningKey = true,
        //Mesma chave feita na classe Token Service
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_jwtConfiguration.SecretToken)
        ),
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = TimeSpan.Zero
    };

    // Processa arrays de claims (roles e Permission) corretamente
    token.Events = new JwtBearerEvents
    {
        OnTokenValidated = context =>
        {
            var identity = context.Principal?.Identity as System.Security.Claims.ClaimsIdentity;
            if (identity != null)
            {
                // Expande claims que são arrays JSON em claims individuais
                var claimsToAdd = new List<System.Security.Claims.Claim>();
                var claimsToRemove = new List<System.Security.Claims.Claim>();

                foreach (var claim in identity.Claims.ToList())
                {
                    if (claim.Value.StartsWith("[") && claim.Value.EndsWith("]"))
                    {
                        try
                        {
                            var values = System.Text.Json.JsonSerializer.Deserialize<string[]>(claim.Value);
                            if (values != null)
                            {
                                claimsToRemove.Add(claim);
                                foreach (var value in values)
                                {
                                    claimsToAdd.Add(new System.Security.Claims.Claim(claim.Type, value));
                                }
                            }
                        }
                        catch { /* Não é um array JSON válido, mantém a claim original */ }
                    }
                }

                foreach (var claim in claimsToRemove)
                    identity.RemoveClaim(claim);
                foreach (var claim in claimsToAdd)
                    identity.AddClaim(claim);
            }
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddControllers().AddNewtonsoftJson(
    option => option.SerializerSettings.ReferenceLoopHandling =
                Newtonsoft.Json.ReferenceLoopHandling.Ignore
);

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.Configure<IISServerOptions>(options =>
{
    options.MaxRequestBodySize = int.MaxValue;
});

builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.Limits.MaxRequestBodySize = int.MaxValue;
});

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState
            .Where(e => e.Value?.Errors.Count > 0)
            .ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value?.Errors.Select(er => er.ErrorMessage).ToArray()
            );

        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "Erro de Validação",
            Detail = "Um ou mais campos estão incorretos.",
            Instance = context.HttpContext.Request.Path
        };

        // Adiciona os erros específicos do Identity/DataAnnotations
        problemDetails.Extensions.Add("errors", errors);

        return new BadRequestObjectResult(problemDetails);
    };
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Adiciona suporte ao padrão ProblemDetails
builder.Services.AddProblemDetails();

if (builder.Environment.IsProduction())
{
    builder.WebHost.ConfigureKestrel(options =>
    {
        options.ListenAnyIP(8080, listenOptions =>
        {
            listenOptions.UseHttps(CertificatePath, CertificatePassword);
        });
    });
}

var app = builder.Build();

// Ativa o middleware de tratamento de exceção
app.UseExceptionHandler();

LoadConfiguration(app);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowFrontend");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<UsuarioContext>();
    if (context.Database.GetPendingMigrations().Any())
    {
        context.Database.Migrate();
    }
}

app.Run();

static void LoadConfiguration(WebApplication app)
{
    var connectionStrings = new Configuration.ConnectionStrings();
    app.Configuration.GetSection("ConnectionStrings").Bind(connectionStrings);
    Configuration.ConnectionString = connectionStrings;
}
