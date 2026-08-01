using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace MyPortalBack.Domain.Abstractions;

public abstract class BaseEntity : IEntity
{
    public Guid Id { get; protected set; } = Guid.NewGuid();

    //protected BaseEntity()
    //{
    //    Id = Guid.NewGuid();
    //}
}

