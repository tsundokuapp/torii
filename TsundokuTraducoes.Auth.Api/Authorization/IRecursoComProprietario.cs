namespace TsundokuTraducoes.Auth.Api.Authorization;

/// <summary>
/// Interface para recursos que possuem um proprietário
/// Permite verificar se o usuário é dono do recurso
/// </summary>
public interface IRecursoComProprietario
{
    int ProprietarioId { get; }
}
