using MyPortalBack.Application.Common.Contracts.Responses;
using MyPortalBack.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPortalBack.Application.Common.Mapping;

public static class UserRoleOperationsMapping
{
    public static UserRoleResponse ToResponse(
        UserRole userRole)
    {
        return new UserRoleResponse
        {
            User = UserOperationsMapping.ToBasicResponse(
                userRole.User!),

            Role = new RoleResponse
            {
                Id = userRole.Role!.Id,
                //Uuid = userRole.Role.Uuid,
                Name = userRole.Role.Name,
                Description = userRole.Role.Description,
                IsActive = userRole.Role.IsActive
            },
            CreatedAt = userRole.CreatedAt,
            CreatedBy = userRole.CreatedBy,
            UpdatedAt = userRole.UpdatedAt,
            UpdatedBy = userRole.UpdatedBy

        };
    }
}
