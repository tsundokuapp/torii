using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TsundokuTraducoes.Auth.Api.DTOs.Request;
using TsundokuTraducoes.Auth.Api.Entities;

namespace TsundokuTraducoes.Auth.Api.Controllers;

[ApiController]
[Route("api/admin")]
[Authorize(Roles = "admin")]
public class AdminController : ControllerBase
{
    private readonly UserManager<CustomIdentityUser> _userManager;

    public AdminController(UserManager<CustomIdentityUser> userManager)
    {
        _userManager = userManager;
    }

    /// <summary>
    /// Obtém as roles de um usuário
    /// </summary>
    [HttpGet("usuarios/{userId}/roles")]
    public async Task<IActionResult> ObterRolesUsuario(int userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
            return NotFound("Usuário não encontrado.");

        var roles = await _userManager.GetRolesAsync(user);
        return Ok(roles);
    }

    /// <summary>
    /// Atribui uma role a um usuário
    /// </summary>
    [HttpPost("usuarios/{userId}/roles")]
    public async Task<IActionResult> AtribuirRole(int userId, [FromBody] AtribuirRoleRequest request)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
            return NotFound("Usuário não encontrado.");

        var result = await _userManager.AddToRoleAsync(user, request.RoleName);
        if (!result.Succeeded)
            return BadRequest(result.Errors);

        return Ok(new { Message = $"Role '{request.RoleName}' atribuída com sucesso." });
    }

    /// <summary>
    /// Remove uma role de um usuário
    /// </summary>
    [HttpDelete("usuarios/{userId}/roles/{role}")]
    public async Task<IActionResult> RemoverRole(int userId, string role)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
            return NotFound("Usuário não encontrado.");

        var result = await _userManager.RemoveFromRoleAsync(user, role);
        if (!result.Succeeded)
            return BadRequest(result.Errors);

        return NoContent();
    }

    /// <summary>
    /// Lista todos os usuários com suas roles
    /// </summary>
    [HttpGet("usuarios")]
    public async Task<IActionResult> ListarUsuarios()
    {
        var users = _userManager.Users.ToList();
        var usersWithRoles = new List<object>();

        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            usersWithRoles.Add(new
            {
                user.Id,
                user.UserName,
                user.Email,
                user.TsunId,
                Roles = roles
            });
        }

        return Ok(usersWithRoles);
    }
}
