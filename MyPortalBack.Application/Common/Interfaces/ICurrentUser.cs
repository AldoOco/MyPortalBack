using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPortalBack.Application.Common.Interfaces;

public interface ICurrentUser
{
    Guid? UserUuid { get; }

    bool IsAuthenticated { get; }
}
