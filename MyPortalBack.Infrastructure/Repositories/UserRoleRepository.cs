using MyPortalBack.Application.Common.Interfaces;
using MyPortalBack.Domain.Entities;
using MyPortalBack.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPortalBack.Infrastructure.Repositories;

public class UserRoleRepository : IUserRoleRepository
{
    private readonly ApplicationDbContext _context;

    public UserRoleRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<UserRole>> GetByUserIdAsync(
        int userId,
        CancellationToken cancellationToken = default)
    {
        return await _context.UserRoles
            .Include(userRole => userRole.Role)
            .Include(userRole => userRole.User)
            .Where(userRole =>
                userRole.UserId == userId &&
                !userRole.IsDeleted)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<List<UserRole>> GetByRoleIdAsync(
        int roleId,
        CancellationToken cancellationToken = default)
    {
        return await _context.UserRoles
            .Include(userRole => userRole.User)
            .Include(userRole => userRole.Role)
            .Where(userRole =>
                userRole.RoleId == roleId &&
                !userRole.IsDeleted)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<UserRole?> GetAsync(
        int userId,
        int roleId,
        CancellationToken cancellationToken = default)
    {
        return await _context.UserRoles
            .FirstOrDefaultAsync(
                userRole =>
                    userRole.UserId == userId &&
                    userRole.RoleId == roleId,
                cancellationToken);
    }

    public async Task<bool> ExistsAsync(
        int userId,
        int roleId,
        CancellationToken cancellationToken = default)
    {
        return await _context.UserRoles
            .AnyAsync(
                userRole =>
                    userRole.UserId == userId &&
                    userRole.RoleId == roleId &&
                    !userRole.IsDeleted,
                cancellationToken);
    }

    public async Task AddAsync(
        UserRole userRole,
        CancellationToken cancellationToken = default)
    {
        await _context.UserRoles.AddAsync(userRole, cancellationToken);
    }

    public Task UpdateAsync(
        UserRole userRole,
        CancellationToken cancellationToken = default)
    {
        _context.UserRoles.Update(userRole);

        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
