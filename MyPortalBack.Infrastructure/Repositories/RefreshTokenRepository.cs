using MyPortalBack.Application.Common.Interfaces;
using MyPortalBack.Domain.Entities;
using MyPortalBack.Infrastructure.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MyPortalBack.Infrastructure.Repositories;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly ApplicationDbContext _context;

    public RefreshTokenRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(
        RefreshToken refreshToken,
        CancellationToken cancellationToken = default)
    {
        await _context.RefreshTokens.AddAsync(
            refreshToken,
            cancellationToken);
    }

    public async Task<RefreshToken?> GetByTokenHashAsync(
        string tokenHash,
        CancellationToken cancellationToken = default)
    {
        return await _context.RefreshTokens
            .Include(x => x.User)
            .FirstOrDefaultAsync(
                x => x.TokenHash == tokenHash,
                cancellationToken);
    }

    public Task UpdateAsync(
        RefreshToken refreshToken,
        CancellationToken cancellationToken = default)
    {
        _context.RefreshTokens.Update(refreshToken);

        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteExpiredAndRevokedAsync(
    CancellationToken cancellationToken = default)
    {
        var cutoffDate = DateTimeOffset.UtcNow.AddDays(-30);

        var tokens = await _context.RefreshTokens
            .Where(x =>
                x.ExpiresAt < DateTimeOffset.UtcNow ||
                (x.RevokedAt.HasValue &&
                 x.RevokedAt.Value < cutoffDate))
            .ToListAsync(cancellationToken);

        if (tokens.Count == 0)
        {
            return;
        }

        _context.RefreshTokens.RemoveRange(tokens);
    }

    public async Task RevokeFamilyAsync(
    Guid tokenFamilyUuid,
    CancellationToken cancellationToken = default)
    {
        var tokens = await _context.RefreshTokens.Where(x =>x.TokenFamilyUuid == tokenFamilyUuid && !x.RevokedAt.HasValue).ToListAsync(cancellationToken);

        var revokedAt = DateTimeOffset.UtcNow;

        foreach (var token in tokens)
        {
            token.RevokedAt = revokedAt;
        }
    }
}
