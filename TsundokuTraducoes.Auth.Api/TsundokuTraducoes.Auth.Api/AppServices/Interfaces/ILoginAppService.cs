﻿using FluentResults;
using TsundokuTraducoes.Auth.Api.DTOs.Request;
using TsundokuTraducoes.Auth.Api.DTOs.Response;

namespace TsundokuTraducoes.Auth.Api.AppServices.Interfaces;

public interface ILoginAppService
{
    Task<Result<TokenResponse>> LogaUsuario(LoginRequest loginRequest);
}
