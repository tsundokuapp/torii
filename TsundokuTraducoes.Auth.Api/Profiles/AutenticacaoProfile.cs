using AutoMapper;
using Microsoft.AspNetCore.Identity;
using TsundokuTraducoes.Auth.Api.DTOs.Request;
using TsundokuTraducoes.Auth.Api.Entities;

namespace TsundokuTraducoes.Auth.Api.Profiles;

public class AutenticacaoProfile : Profile
{
    public AutenticacaoProfile()
    {
        CreateMap<CadastroUsuarioRequest, EntidadeUsuario>();
        CreateMap<EntidadeUsuario, IdentityUser<int>>()
            .ForMember(x => x.UserName, opt => opt.MapFrom(src => src.Usuario));

        CreateMap<EntidadeUsuario, CustomIdentityUser>();
    }
}
