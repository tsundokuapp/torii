namespace TsundokuTraducoes.Auth.Api.DTOs.Response;

public class TokenResponse
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public string UserName { get; set; }
    public DateTime RefreshTokenExpiry { get; set; }
    public Guid TsunId { get; set; }
}
