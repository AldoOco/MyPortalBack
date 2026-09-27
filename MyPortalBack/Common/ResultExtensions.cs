using Microsoft.AspNetCore.Mvc;
using MyPortalBack.Application.Common.Result;

namespace MyPortalBack.Api.Common;

/// <summary>
/// Métodos de extensión para convertir resultados de la aplicación
/// en respuestas HTTP de ASP.NET Core.
/// </summary>
public static class ResultExtensions
{

    /// <summary>
    /// Convierte un resultado de la aplicación en una respuesta HTTP.
    /// </summary>
    /// <typeparam name="T">Tipo de dato contenido en el resultado.</typeparam>
    /// <param name="controller">
    /// Controlador desde el cual se genera la respuesta.
    /// </param>
    /// <param name="result">
    /// Resultado de la operación de aplicación.
    /// </param>
    /// <returns>
    /// Una respuesta HTTP acorde al resultado de la operación.
    /// </returns>
    public static IActionResult ToActionResult<T>(
        this ControllerBase controller,
        Result<T> result)
    {
        if (result.Success)
        {
            return controller.Ok(result);
        }

        return result.ErrorCode switch
        {
            ErrorCodes.UserNotFound =>
                controller.NotFound(result),

            ErrorCodes.EmailAlreadyExists =>
                controller.Conflict(result),

            ErrorCodes.InvalidCredentials =>
                controller.Unauthorized(result),

            ErrorCodes.ValidationError =>
                controller.BadRequest(result),

            _ =>
                controller.BadRequest(result)
        };
    }

    /// <summary>
    /// Convierte un resultado exitoso en una respuesta HTTP 201 Created,
    /// utilizando la acción indicada para generar la ubicación del recurso.
    /// </summary>
    /// <typeparam name="T">Tipo de dato contenido en el resultado.</typeparam>
    /// <param name="controller">
    /// Controlador desde el cual se genera la respuesta.
    /// </param>
    /// <param name="result">
    /// Resultado de la operación de aplicación.
    /// </param>
    /// <param name="actionName">
    /// Nombre de la acción utilizada para generar la ubicación del recurso.
    /// </param>
    /// <param name="routeValues">
    /// Valores utilizados para construir la ruta del recurso creado.
    /// </param>
    /// <returns>
    /// Una respuesta HTTP 201 Created si la operación fue exitosa;
    /// en caso contrario, la respuesta HTTP correspondiente al error.
    /// </returns>
    public static IActionResult ToCreatedAtActionResult<T>(
        this ControllerBase controller,
        Result<T> result,
        string actionName,
        object routeValues)
    {
        if (!result.Success)
        {
            return controller.ToActionResult(result);
        }

        return controller.CreatedAtAction(
            actionName,
            routeValues,
            result);
    }
}
