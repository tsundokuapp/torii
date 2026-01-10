namespace TsundokuTraducoes.Auth.Api.DTOs.Request;

public class LoginInternalResult
{
        public string UserName { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiry { get; set; }
        public Guid TsunId { get; set; }
}