using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TsundokuTraducoes.Auth.Api.Controllers;

[ApiController]
[Route("api/roles")]
[Authorize(Roles = "admin")]
public class RolesController : ControllerBase
{
    private readonly RoleManager<IdentityRole<int>> _roleManager;

    public RolesController(RoleManager<IdentityRole<int>> roleManager)
    {
        _roleManager = roleManager;
    }

    /// <summary>
    /// Lista todas as roles
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> ListarRoles()
    {
        var roles = await _roleManager.Roles.ToListAsync();
        return Ok(roles.Select(r => new { r.Id, r.Name }));
    }

    /// <summary>
    /// Cria uma nova role
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CriarRole([FromBody] string roleName)
    {
        if (string.IsNullOrWhiteSpace(roleName))
            return BadRequest("Nome da role é obrigatório.");

        var result = await _roleManager.CreateAsync(new IdentityRole<int>(roleName));
        if (!result.Succeeded)
            return BadRequest(result.Errors);

        return Created($"api/roles/{roleName}", new { Name = roleName });
    }

    /// <summary>
    /// Deleta uma role
    /// </summary>
    [HttpDelete("{roleName}")]
    public async Task<IActionResult> DeletarRole(string roleName)
    {
        var role = await _roleManager.FindByNameAsync(roleName);
        if (role == null)
            return NotFound("Role não encontrada.");

        var result = await _roleManager.DeleteAsync(role);
        if (!result.Succeeded)
            return BadRequest(result.Errors);

        return NoContent();
    }
}
