namespace TsundokuTraducoes.Auth.Api.Entities;

public class EntidadeUsuario
{
    public int Id { get; set; }
    public string Usuario { get; set; }
    public string Email { get; set; }
    public Guid TsunId { get; set; }
}
