using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPortalBack.Application.Common.Exceptions;

public sealed class NotFoundException : ApplicationException
{
    public NotFoundException(string resourceName)
        : base($"{resourceName} no fue encontrado.")
    {
    }

    public NotFoundException(string resourceName, object key)
        : base($"{resourceName} con identificador '{key}' no fue encontrado.")
    {
    }
}
