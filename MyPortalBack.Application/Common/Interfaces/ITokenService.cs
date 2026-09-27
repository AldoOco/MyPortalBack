using MyPortalBack.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPortalBack.Application.Common.Interfaces
{
    public interface ITokenService
    {
        (string AccessToken, DateTimeOffset ExpiresAt) GenerateToken(User user);

        string GenerateRefreshToken();

        string HashToken(string token);
    }
}