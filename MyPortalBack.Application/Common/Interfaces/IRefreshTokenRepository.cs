using MyPortalBack.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPortalBack.Application.Common.Interfaces;

public interface IRefreshTokenRepository
{
    Task AddAsync(
        RefreshToken refreshToken,
        CancellationToken cancellationToken = default);

    Task<RefreshToken?> GetByTokenHashAsync(string tokenHash,CancellationToken cancellationToken = default);

    Task UpdateAsync(RefreshToken refreshToken,CancellationToken cancellationToken = default);

    Task SaveChangesAsync(CancellationToken cancellationToken = default);

    Task DeleteExpiredAndRevokedAsync(CancellationToken cancellationToken = default);

    Task RevokeFamilyAsync(Guid tokenFamilyUuid, CancellationToken cancellationToken = default);
}
