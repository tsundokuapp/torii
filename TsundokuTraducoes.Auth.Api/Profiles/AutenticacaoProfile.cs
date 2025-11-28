using AutoMapper;
using Microsoft.AspNetCore.Identity;
using TsundokuTraducoes.Auth.Api.DTOs.Request;
using TsundokuTraducoes.Auth.Api.Entities;

namespace TsundokuTraducoes.Auth.Api.Profiles;

public class AutenticacaoProfile : Profile
{
    public AutenticacaoProfile()
    {
        CreateMap<CadastroUsuarioRequest, Usuario>();
        CreateMap<Usuario, IdentityUser<int>>();
        CreateMap<Usuario, CustomIdentityUser>();
    }
}
