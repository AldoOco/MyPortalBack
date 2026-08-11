using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPortalBack.Application.Common.Exceptions;

public sealed class ConflictException : ApplicationException
{
    public ConflictException(string message)
        : base(message)
    {
    }
}
