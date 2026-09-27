using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPortalBack.Application.Common.Result;

public static class ErrorCodes
{
    public const string UserNotFound = "USER_NOT_FOUND";

    public const string EmailAlreadyExists = "EMAIL_ALREADY_EXISTS";

    public const string NotFound = "NOT_FOUND";

    public const string Conflict = "CONFLICT";

    public const string ValidationError = "VALIDATION_ERROR";

    public const string InvalidCredentials = "INVALID_CREDENTIALS";
}