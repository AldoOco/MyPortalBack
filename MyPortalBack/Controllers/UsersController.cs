using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyPortalBack.Api.Common;
using MyPortalBack.Application.Common.Contracts.Requests;
using MyPortalBack.Application.Common.Contracts.Responses;
using MyPortalBack.Application.Common.Result;
using MyPortalBack.Application.Features;

namespace MyPortalBack.Api.Controllers;

/// <summary>
/// Controlador para la gestión de usuarios.
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class UsersController : ControllerBase
    {
    private readonly UserOperations _userOperations;

    /// <summary>
    /// Declaracion de clase contenedora de operaciones.
    /// </summary>
    public UsersController(UserOperations userOperations)
    {
        _userOperations = userOperations;
    }

    /// <summary>
    /// Crea un nuevo usuario.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateUserRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _userOperations.CreateAsync(
            request.FirstName,
            request.LastName,
            request.Email,
            request.Password,
            cancellationToken);


        //return Ok(result);
        return this.ToCreatedAtActionResult(
            result,
            nameof(GetById),
            new { id = result.Value?.Id });

    }

    /// <summary>
    /// Obtiene todos los usuarios activos.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll(
        CancellationToken cancellationToken)
    {
        var result = await _userOperations.GetAllAsync(
            cancellationToken);

        return this.ToActionResult(result);
    }

    /// <summary>
    /// Obtiene un usuario por su identificador.
    /// </summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _userOperations.GetByIdAsync(
            id,
            cancellationToken);

        return this.ToActionResult(result);

        //return Ok(result);
    }

    /// <summary>
    /// Obtiene un usuario por su correo electrónico.
    /// </summary>
    [HttpGet("email/{email}")]
    public async Task<IActionResult> GetByEmail(
        string email,
        CancellationToken cancellationToken)
    {
        var result = await _userOperations.GetByEmailAsync(
            email,
            cancellationToken);

        return this.ToActionResult(result);

    }

    /// <summary>
    /// Actualiza un usuario existente.
    /// </summary>
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateUserRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _userOperations.UpdateAsync(
            id,
            request.FirstName,
            request.LastName,
            request.Email,
            request.IsActive,
            cancellationToken);

        return this.ToActionResult(result);
    }

    /// <summary>
    /// Elimina lógicamente un usuario.
    /// </summary>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _userOperations.DeleteAsync(
            id,
            cancellationToken);

        return this.ToActionResult(result);
    }
}

