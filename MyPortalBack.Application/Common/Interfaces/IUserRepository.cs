using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyPortalBack.Domain.Entities;

namespace MyPortalBack.Application.Common.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<User?> GetByEmailAsync(
        string email,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByEmailAsync(
        string email,
        CancellationToken cancellationToken = default);

    Task AddAsync(
        User user,
        CancellationToken cancellationToken = default);

    Task UpdateAsync(
        User user,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<User>> GetAllAsync(
        CancellationToken cancellationToken = default);

    Task SaveChangesAsync(
        CancellationToken cancellationToken = default);
}
