using MyPortalBack.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPortalBack.Application.Common.Interfaces;
public interface IUserRoleRepository
{
    Task<List<UserRole>> GetByUserIdAsync(
        int userId,
        CancellationToken cancellationToken = default);

    Task<List<UserRole>> GetByRoleIdAsync(
        int roleId,
        CancellationToken cancellationToken = default);

    Task<UserRole?> GetAsync(
        int userId,
        int roleId,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(
        int userId,
        int roleId,
        CancellationToken cancellationToken = default);

    Task AddAsync(
        UserRole userRole,
        CancellationToken cancellationToken = default);

    Task UpdateAsync(
        UserRole userRole,
        CancellationToken cancellationToken = default);

    Task SaveChangesAsync(
        CancellationToken cancellationToken = default);
}
