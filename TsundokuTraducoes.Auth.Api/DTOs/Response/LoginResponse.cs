namespace TsundokuTraducoes.Auth.Api.DTOs.Response;

public class LoginResponse
{
    public string UserName { get; set; }
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}
