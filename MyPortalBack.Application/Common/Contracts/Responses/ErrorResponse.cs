using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPortalBack.Application.Common.Contracts.Responses;

public sealed class ErrorResponse : ApiResponse<object>
{
    public ErrorResponse(
        string message,
        IEnumerable<string>? errors = null)
    {
        Success = false;
        Message = message;
        Data = null;
        Errors = errors?.ToArray() ?? Array.Empty<string>();
    }
}