namespace TsundokuTraducoes.Auth.Api.DTOs.Request;

public class TokenRequest
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}
