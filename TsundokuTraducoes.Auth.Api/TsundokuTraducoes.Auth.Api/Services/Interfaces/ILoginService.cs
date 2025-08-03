﻿using FluentResults;
using TsundokuTraducoes.Auth.Api.DTOs.Request;
using TsundokuTraducoes.Auth.Api.DTOs.Response;

namespace TsundokuTraducoes.Auth.Api.Services.Interfaces;

public interface ILoginService
{
    Task<Result<TokenResponse>> LogaUsuario(LoginRequest loginRequest);
}
