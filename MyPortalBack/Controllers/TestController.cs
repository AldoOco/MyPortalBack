using Microsoft.AspNetCore.Mvc;
using MyPortalBack.Application.Common.Exceptions;

namespace MyPortalBack.Api.Controllers;
//CONTROLADOR DE PRUEBA DE EXCEPCIONES PARA EL MIDDLEWARE DE EXCEPCIONES
[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    [HttpGet("ok")]
    public IActionResult OkResponse()
    {
        return Ok("Middleware funcionando.");
    }

    [HttpGet("error")]
    public IActionResult Error()
    {
        throw new Exception("Error de prueba.");
    }

    [HttpGet("not-found")]
    public IActionResult NotFoundException()
    {
        throw new NotFoundException("Usuario");
    }

    [HttpGet("validation")]
    public IActionResult ValidationException()
    {
        throw new ValidationException(
            "El correo es obligatorio.",
            "La contraseña debe tener al menos 8 caracteres.");
    }

    [HttpGet("conflict")]
    public IActionResult ConflictException()
    {
        throw new ConflictException(
            "El correo ya está registrado.");
    }

    [HttpGet("unauthorized")]
    public IActionResult UnauthorizedException()
    {
        throw new UnauthorizedException();
    }

    [HttpGet("forbidden")]
    public IActionResult ForbiddenException()
    {
        throw new ForbiddenException();
    }
}
