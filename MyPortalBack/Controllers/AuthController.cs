using Microsoft.AspNetCore.Mvc;
using MyPortalBack.Application.Common.Contracts.Requests;
using MyPortalBack.Application.Common.Contracts.Responses;
using MyPortalBack.Application.Common.Result;
using MyPortalBack.Application.Features;
using MyPortalBack.Api.Common;

namespace MyPortalBack.Api.Controllers;

/// <summary>
/// Controlador para la autenticación de usuarios.
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthOperations _authOperations;

    /// <summary>
    /// Declaracion de clase contenedora de operaciones.
    /// </summary>
    public AuthController(AuthOperations authOperations)
    {
        _authOperations = authOperations;
    }

    /// <summary>
    /// Autentica un usuario y genera un token JWT.
    /// </summary>
    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [FromBody] LoginRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _authOperations.LoginAsync(
            request,
            cancellationToken);

        return this.ToActionResult(result);
    }

    /// <summary>
    /// Refresca el token JWT utilizando un refresh token válido.
    /// </summary>
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(
    [FromBody] RefreshTokenRequest request,
    CancellationToken cancellationToken)
    {
        var result = await _authOperations.RefreshAsync(
            request,
            cancellationToken);

        return this.ToActionResult(result);
    }

    /// <summary>
    /// Cierra la sesión del usuario y revoca el refresh token.
    /// </summary>
    [HttpPost("logout")]
    public async Task<IActionResult> Logout(
    [FromBody] LogoutRequest request,
    CancellationToken cancellationToken)
    {
        var result = await _authOperations.LogoutAsync(
            request,
            cancellationToken);

        return this.ToActionResult(result);
    }
}