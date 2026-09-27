using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyPortalBack.Application.Common.Contracts.Requests;
using MyPortalBack.Application.Features;
using MyPortalBack.Application.Common.Result;
using MyPortalBack.Api.Common;

namespace MyPortalBack.Api.Controllers;

/// <summary>
/// Controlador para la gestión de roles de los usuarios.
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class RolesController : ControllerBase
{
    private readonly RoleOperations _roleOperations;

    /// <summary>
    /// Declaracion de clase contenedora de roles.
    /// </summary>
    public RolesController(RoleOperations roleOperations)
    {
        _roleOperations = roleOperations;
    }

    /// <summary>
    /// Obtiene todos los roles disponibles.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll(
        CancellationToken cancellationToken)
    {
        var result = await _roleOperations.GetAllAsync(
            cancellationToken);

        return this.ToActionResult(result);
    }

    /// <summary>
    /// Obtiene un rol por su ID.
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(
        int id,
        CancellationToken cancellationToken)
    {
        var result = await _roleOperations.GetByIdAsync(
            id,
            cancellationToken);

        return this.ToActionResult(result);
    }

    /// <summary>
    /// Obtiene un rol por su nombre.
    /// </summary>
    [HttpGet("name/{name}")]
    public async Task<IActionResult> GetByName(
        string name,
        CancellationToken cancellationToken)
    {
        var result = await _roleOperations.GetByNameAsync(
            name,
            cancellationToken);

        return this.ToActionResult(result);
    }

    /// <summary>
    /// Crea un nuevo rol.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create(
        CreateRoleRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _roleOperations.CreateAsync(
            request.Name,
            request.Description,
            cancellationToken);

        return this.ToActionResult(result);
    }

    /// <summary>
    /// Actualiza un rol existente.
    /// </summary>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        UpdateRoleRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _roleOperations.UpdateAsync(
            id,
            request.Name,
            request.Description,
            request.IsActive,
            cancellationToken);

        return this.ToActionResult(result);
    }

    /// <summary>
    /// Elimina un rol existente.
    /// </summary>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(
        int id,
        CancellationToken cancellationToken)
    {
        var result = await _roleOperations.DeleteAsync(
            id,
            cancellationToken);

        return this.ToActionResult(result);
    }
}