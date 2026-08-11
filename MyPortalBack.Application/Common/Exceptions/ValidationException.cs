using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPortalBack.Application.Common.Exceptions;

public sealed class ValidationException : ApplicationException
{
    public IReadOnlyCollection<string> Errors { get; }

    public ValidationException(IEnumerable<string> errors)
        : base("Uno o más errores de validación ocurrieron.")
    {
        Errors = errors.ToArray();
    }

    public ValidationException(params string[] errors)
        : this(errors.AsEnumerable())
    {
    }
}
