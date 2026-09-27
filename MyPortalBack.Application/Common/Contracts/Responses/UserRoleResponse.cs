using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPortalBack.Application.Common.Contracts.Responses;

public class UserRoleResponse
{
    public UserResponse User { get; set; } = null!;
    public RoleResponse Role { get; set; } = null!;

    public DateTimeOffset CreatedAt { get; set; } 

    public Guid? CreatedBy { get; set; }

    public DateTimeOffset? UpdatedAt { get; set; }

    public Guid? UpdatedBy { get; set; }
}

public class RoleResponse
{
    public int? Id { get; set; }
    public Guid? Uuid { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}
