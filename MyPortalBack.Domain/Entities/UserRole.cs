using MyPortalBack.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPortalBack.Domain.Entities;

public class UserRole : AuditableEntity
{
    public int UserId { get; set; }

    public int RoleId { get; set; }

    public User? User { get; set; }

    public Role? Role { get; set; }
}
