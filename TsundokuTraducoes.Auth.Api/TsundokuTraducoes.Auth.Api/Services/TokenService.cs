using FluentResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Web;
using TsundokuTraducoes.Auth.Api.DTOs;
using TsundokuTraducoes.Auth.Api.DTOs.Request;
using TsundokuTraducoes.Auth.Api.DTOs.Response;
using TsundokuTraducoes.Auth.Api.Entities;
using TsundokuTraducoes.Auth.Api.Services.Interfaces;
using TsundokuTraducoes.Helpers.Configuration;

namespace TsundokuTraducoes.Auth.Api.Services;

public class TokenService : ITokenService
{
    private readonly SignInManager<CustomIdentityUser> _signInManager;
    private readonly UserManager<CustomIdentityUser> _userManager;

    public TokenService(SignInManager<CustomIdentityUser> signInManager, UserManager<CustomIdentityUser> userManager, IEmailService emailServices)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }

    public Token CreateToken(CustomIdentityUser usuario, List<string> roles)
    {
        var token = CreateJwtSecurityToken(usuario, roles);
        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
        return new Token(tokenString);
    }

    private static JwtSecurityToken CreateJwtSecurityToken(CustomIdentityUser usuario, List<string> roles)
    {
        List<Claim> claims =
        [
            new Claim("username", usuario.UserName),
            new Claim("id", usuario.Id.ToString())
        ];

        roles.ForEach(
            role => claims.Add(new Claim(ClaimTypes.Role, role))
        );

        var chave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ConfigurationAutenticacaoExternal.RetornaJwtSecretToken()));
        var credenciais = new SigningCredentials(chave, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            claims: claims,
            signingCredentials: credenciais,
            expires: DateTime.UtcNow.AddHours(1)
        );
        return token;
    }

    public async Task<Result<TokenResponse>> RefreshToken(TokenRequest tokenRequest)
    {
        string accessToken = tokenRequest.AccessToken;
        string refreshToken = tokenRequest.RefreshToken;

        var principal = GetPrincipalFromExpiredToken(accessToken);
        if (principal.IsFailed)
            return Result.Fail(principal.Errors[0]);

        if (principal.ValueOrDefault is null)
            return Result.Fail("Access token/refresh token inválido.");

        var username = principal.ValueOrDefault.Claims.FirstOrDefault(x => x.Type == "username")?.Value;

        var usuario = await _userManager.FindByNameAsync(username);

        if (usuario == null || usuario.RefreshToken != refreshToken ||
                   usuario.RefreshTokenExpiryTime <= DateTime.Now)
        {
            return Result.Fail("Access token/refresh token inválido.");
        }

        var newAccessToken = CreateJwtSecurityToken(usuario, _signInManager.UserManager.GetRolesAsync(usuario).Result.ToList());
        var newRefreshToken = GenerateRefreshToken();

        usuario.RefreshToken = newRefreshToken;
        await _userManager.UpdateAsync(usuario);

        return Result.Ok(new TokenResponse
        {
            AccessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
            RefreshToken = newRefreshToken
        });
    }

    private static Result<ClaimsPrincipal> GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                               .GetBytes(ConfigurationAutenticacaoExternal.RetornaJwtSecretToken())),
            ValidateLifetime = false
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters,
                         out SecurityToken securityToken);

        if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                  !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                                 StringComparison.InvariantCultureIgnoreCase))
            return Result.Fail("Token inválido");

        return Result.Ok(principal);
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Uri.EscapeDataString(Convert.ToBase64String(randomNumber));
    }

    public string GenerateResetToken(int userId)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(ConfigurationAutenticacaoExternal.RetornaJwtSecretTokenReset());
        var expires = DateTime.UtcNow.AddMinutes(ConfigurationAutenticacaoExternal.RetornaResetTokenValidityInMinutes());

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim("Purpose", "PasswordReset")
            }),
            Expires = expires,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public TokenRecuperacaoSenha ValidateTokenAndGetUserId(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(ConfigurationAutenticacaoExternal.RetornaJwtSecretTokenReset());

        try
        {
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            };

            var codigoConfirmacaoEncododa = HttpUtility.UrlDecode(token);

            var encodedToken = WebEncoders.Base64UrlDecode(codigoConfirmacaoEncododa);

            var tokenRecuperacao = ByteArrayToObject<TokenRecuperacaoSenha>(encodedToken);


            var principal = tokenHandler.ValidateToken(tokenRecuperacao.TokenResetSenha, validationParameters, out SecurityToken validatedToken);

            var purposeClaim = principal.FindFirst("Purpose");
            if (purposeClaim == null || purposeClaim.Value != "PasswordReset")
            {
                return null;
            }

            var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                tokenRecuperacao.Id = userId;
                return tokenRecuperacao;
            }

            return null;
        }
        catch
        {
            return null;
        }
    }

    public static T ByteArrayToObject<T>(byte[] bytes)
    {        
        string json = Encoding.UTF8.GetString(bytes);
        return JsonSerializer.Deserialize<T>(json);
    }
}
