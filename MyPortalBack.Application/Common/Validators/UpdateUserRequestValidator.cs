using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using MyPortalBack.Application.Common.Contracts.Requests;

namespace MyPortalBack.Application.Common.Validators;

public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
{
    public UpdateUserRequestValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage("El nombre es obligatorio.")
            .MaximumLength(100)
            .WithMessage("El nombre no puede superar los 100 caracteres.");

        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage("El apellido es obligatorio.")
            .MaximumLength(100)
            .WithMessage("El apellido no puede superar los 100 caracteres.");

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("El correo electrónico es obligatorio.")
            .EmailAddress()
            .WithMessage("El correo electrónico no tiene un formato válido.")
            .MaximumLength(150)
            .WithMessage("El correo electrónico no puede superar los 150 caracteres.");
    }
}
