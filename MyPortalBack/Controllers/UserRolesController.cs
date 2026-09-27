using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyPortalBack.Application.Features;
using MyPortalBack.Api.Common;

namespace MyPortalBack.Api.Controllers;

/// <summary>
/// Controlador para la gestión de roles de los usuarios.
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class UserRolesController : ControllerBase
{
    private readonly UserRoleOperations _userRoleOperations;

    /// <summary>
    /// Declaracion de clase contenedora de operaciones.
    /// </summary>
    public UserRolesController(UserRoleOperations userRoleOperations)
    {
        _userRoleOperations = userRoleOperations;
    }

    /// <summary>
    /// Obtiene los roles asignados a un usuario.
    /// </summary>
    [HttpGet("user/{userId:int}")]
    public async Task<IActionResult> GetByUserId(
        int userId,
        CancellationToken cancellationToken)
    {
        var result = await _userRoleOperations.GetByUserIdAsync(
            userId,
            cancellationToken);

        return this.ToActionResult(result);
    }

    /// <summary>
    /// Obtiene los usuarios asignados a un rol.
    /// </summary>
    [HttpGet("role/{roleId:int}")]
    public async Task<IActionResult> GetByRoleId(
        int roleId,
        CancellationToken cancellationToken)
    {
        var result = await _userRoleOperations.GetByRoleIdAsync(
            roleId,
            cancellationToken);

        return this.ToActionResult(result);
    }

    /// <summary>
    /// Asigna un rol a un usuario.
    /// </summary>
    [HttpPost("user/{userId:int}/role/{roleId:int}")]
    public async Task<IActionResult> Assign(
        int userId,
        int roleId,
        CancellationToken cancellationToken)
    {
        var result = await _userRoleOperations.AssignAsync(
            userId,
            roleId,
            cancellationToken);

        return this.ToActionResult(result);
    }

    /// <summary>
    /// Quita un rol a un usuario.
    /// </summary>
    [HttpDelete("user/{userId:int}/role/{roleId:int}")]
    public async Task<IActionResult> Delete(
        int userId,
        int roleId,
        CancellationToken cancellationToken)
    {
        var result = await _userRoleOperations.DeleteAsync(
            userId,
            roleId,
            cancellationToken);

        return this.ToActionResult(result);
    }
}