using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TsundokuTraducoes.Auth.Api;
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

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("tsundokuAuthApp", builder =>
    {
        builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

var app = builder.Build();
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
