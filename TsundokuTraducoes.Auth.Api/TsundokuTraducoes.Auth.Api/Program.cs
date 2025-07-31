using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using TsundokuTraducoes.Auth.Api;
using TsundokuTraducoes.Auth.Api.Data.Context;
using TsundokuTraducoes.Auth.Api.Entities;
using TsundokuTraducoes.Auth.Api.Extensions;
using TsundokuTraducoes.Helpers.Configuration;

var _connectionStringConfig = new ConnectionStringConfig();
var _acessoEmail = new AcessoEmail();

var builder = WebApplication.CreateBuilder(args);

_acessoEmail.SmtpServer = builder.Configuration.GetSection("AcessoEmail").GetValue<string>("SmtpServer");
_acessoEmail.Port = Convert.ToInt32(builder.Configuration.GetSection("AcessoEmail").GetValue<string>("Port"));
_acessoEmail.Remetente = builder.Configuration.GetSection("AcessoEmail").GetValue<string>("Remetente");
_acessoEmail.Password = builder.Configuration.GetSection("AcessoEmail").GetValue<string>("Password");
_acessoEmail.EmailAdminInicial = builder.Configuration.GetSection("AcessoEmail").GetValue<string>("EmailAdminInicial");
_acessoEmail.SenhaAdminInicial = builder.Configuration.GetSection("AcessoEmail").GetValue<string>("SenhaAdminInicial");

ConfigurationAutenticacaoExternal.SetaAcessoExterno(_acessoEmail);

_connectionStringConfig.ConnectionString = builder.Configuration.GetConnectionString("UsuarioConnection");
SourceConnection.SetaConnectionStringConfig(_connectionStringConfig);

builder.Services.AddSqlConnection(_connectionStringConfig.ConnectionString!);

//Injetando dependências do Identity
builder.Services
    
    .AddIdentity<CustomIdentityUser, IdentityRole<int>>(
        //Habilitando a opção de obrigação da confirmação do e-mail
        opt => opt.SignIn.RequireConfirmedEmail = true)
    .AddEntityFrameworkStores<UsuarioContext>()
    //Adiciona um provedor de tokens para confirmação de acesso e afins
    .AddDefaultTokenProviders();

builder.Services.AddServices();

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
app.UseAuthorization();
app.MapControllers();

app.Run();

static void LoadConfiguration(WebApplication app)
{
    var connectionStrings = new Configuration.ConnectionStrings();
    app.Configuration.GetSection("ConnectionStrings").Bind(connectionStrings);
    Configuration.ConnectionString = connectionStrings;
}
