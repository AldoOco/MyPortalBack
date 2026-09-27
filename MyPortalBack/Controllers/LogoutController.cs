using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyPortalBack.Application.Common.Result;

namespace MyPortalBack.Api.Controllers;

/// <summary>
/// Controlador para el cierre de sesion de los usuarios.
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
public class LogoutController : ControllerBase
{
    /// <summary>
    /// Cierra la sesión del usuario autenticado.
    /// </summary>
    [Authorize]
    [HttpPost]
    public IActionResult Logout()
    {
        var result = Result<string>.Ok(
            "La sesión se cerró correctamente.");

        return Ok(result);
    }
}
