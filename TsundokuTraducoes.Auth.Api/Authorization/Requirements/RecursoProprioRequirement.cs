using Microsoft.AspNetCore.Authorization;

namespace TsundokuTraducoes.Auth.Api.Authorization.Requirements;

/// <summary>
/// Requirement para verificar se o usuário é dono do recurso
/// </summary>
public class RecursoProprioRequirement : IAuthorizationRequirement { }
