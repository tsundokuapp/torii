using Microsoft.AspNetCore.Identity;

namespace TsundokuTraducoes.Auth.Api.Entities;

public class CustomIdentityUser : IdentityUser<int>
{
    public string RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }
    public Guid TsunId { get; set; }
}