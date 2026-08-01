using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPortalBack.Domain.Abstractions;

public interface IEntity
{
    Guid Id { get; }
}

