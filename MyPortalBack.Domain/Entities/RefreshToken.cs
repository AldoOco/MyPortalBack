using MyPortalBack.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPortalBack.Domain.Entities;

public class RefreshToken : AuditableEntity
{
    public int UserId { get; set; }

    public Guid TokenFamilyUuid { get; set; }

    public string TokenHash { get; set; } = string.Empty;

    public DateTimeOffset ExpiresAt { get; set; }

    public DateTimeOffset? RevokedAt { get; set; }

    public bool IsRevoked => RevokedAt.HasValue;

    public User? User { get; set; }
}
