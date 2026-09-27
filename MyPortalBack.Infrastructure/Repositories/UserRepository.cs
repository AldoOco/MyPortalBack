using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyPortalBack.Application.Common.Interfaces;
using MyPortalBack.Domain.Entities;
using MyPortalBack.Infrastructure.Persistence.Context;

namespace MyPortalBack.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .FirstOrDefaultAsync(
                user => user.Uuid == id && !user.IsDeleted,
                cancellationToken);
    }

    public async Task<User?> GetByIdIntAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .FirstOrDefaultAsync(
                user => user.Id == id && !user.IsDeleted,
                cancellationToken);
    }

    public async Task<User?> GetByEmailAsync(
        string email,
        CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .FirstOrDefaultAsync(
                user => user.Email == email && !user.IsDeleted,
                cancellationToken);
    }

    public async Task<bool> ExistsByEmailAsync(
        string email,
        CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .AnyAsync(
                x => x.Email == email,
                cancellationToken);
    }

    public async Task AddAsync(
        User user,
        CancellationToken cancellationToken = default)
    {
        await _context.Users.AddAsync(user, cancellationToken);
    }

    public Task UpdateAsync(
        User user,
        CancellationToken cancellationToken = default)
    {
        _context.Users.Update(user);

        return Task.CompletedTask;
    }

    public Task UpdateTokenAsync(
        User user,
        CancellationToken cancellationToken = default)
    {
        _context.Entry(user).Property(u => u.Id).IsModified = false;
        _context.Users.Update(user);

        return Task.CompletedTask;
    }

    public async Task<IReadOnlyList<User>> GetAllAsync(
    CancellationToken cancellationToken)
    {
        return await _context.Users
            .Where(user => !user.IsDeleted)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task SaveChangesAsync(
    CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
