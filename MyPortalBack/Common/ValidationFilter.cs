using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MyPortalBack.Application.Common.Result;

namespace MyPortalBack.Api.Common;

/// <summary>
/// Filtro que ejecuta automáticamente los validadores de FluentValidation
/// asociados a los modelos recibidos por los controladores.
/// </summary>
public class ValidationFilter : IAsyncActionFilter
{
    /// <summary>
    /// Ejecuta la validación antes de ejecutar la acción del controlador.
    /// </summary>
    /// <param name="context">
    /// Contexto de ejecución de la acción.
    /// </param>
    /// <param name="next">
    /// Siguiente elemento del pipeline.
    /// </param>
    public async Task OnActionExecutionAsync(
        ActionExecutingContext context,
        ActionExecutionDelegate next)
    {
        foreach (var argument in context.ActionArguments.Values)
        {
            if (argument is null)
            {
                continue;
            }

            var argumentType = argument.GetType();

            var validatorType = typeof(IValidator<>)
                .MakeGenericType(argumentType);

            var validator = context.HttpContext.RequestServices
                .GetService(validatorType);

            if (validator is null)
            {
                continue;
            }

            var validationResult = await ValidateAsync(
                validator,
                argument,
                context.HttpContext.RequestAborted);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors
                    .Select(error => error.ErrorMessage)
                    .Distinct()
                    .ToArray();

                var failure = Result<object>.FailureWithCode(
                    ErrorCodes.ValidationError,
                    errors);

                context.Result = new BadRequestObjectResult(failure);

                return;
            }
        }

        await next();
    }

    private static async Task<FluentValidation.Results.ValidationResult> ValidateAsync(
        object validator,
        object model,
        CancellationToken cancellationToken)
    {
        return await ((dynamic)validator).ValidateAsync(
            (dynamic)model,
            cancellationToken);
    }
}