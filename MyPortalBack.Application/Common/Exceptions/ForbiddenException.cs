using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPortalBack.Application.Common.Exceptions;

public sealed class ForbiddenException : ApplicationException
{
    public ForbiddenException(string message = "Acceso denegado.")
        : base(message)
    {
    }
}
