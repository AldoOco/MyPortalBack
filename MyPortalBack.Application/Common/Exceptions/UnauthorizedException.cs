using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPortalBack.Application.Common.Exceptions;

public sealed class UnauthorizedException : ApplicationException
{
    public UnauthorizedException(string message = "No autenticado.")
        : base(message)
    {
    }
}
