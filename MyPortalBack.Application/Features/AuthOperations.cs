using MyPortalBack.Application.Common.Contracts.Requests;
using MyPortalBack.Application.Common.Contracts.Responses;
using MyPortalBack.Application.Common.Interfaces;
using MyPortalBack.Application.Common.Result;
using MyPortalBack.Domain.Entities;
using MyPortalBack.Application.Common.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace MyPortalBack.Application.Features;

public class AuthOperations
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;
    private readonly IRefreshTokenRepository _refreshTokenRepository;

    public AuthOperations(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        ITokenService tokenService,
        IRefreshTokenRepository refreshTokenRepository)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
        _refreshTokenRepository = refreshTokenRepository;
    }

    public async Task<Result<LoginResponse>> LoginAsync(
        LoginRequest request,
        CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(
            request.Email,
            cancellationToken);

        if (user is null)
        {
            return Result<LoginResponse>.FailureWithCode(
                ErrorCodes.InvalidCredentials,
                "El correo electrónico o la contraseña son incorrectos.");
        }

        if (!user.IsActive)
        {
            return Result<LoginResponse>.FailureWithCode(
                ErrorCodes.InvalidCredentials,
                "El correo electrónico o la contraseña son incorrectos.");
        }

        var passwordValid = _passwordHasher.Verify(
            request.Password,
            user.PasswordHash);

        if (!passwordValid)
        {
            return Result<LoginResponse>.FailureWithCode(
                ErrorCodes.InvalidCredentials,
                "El correo electrónico o la contraseña son incorrectos.");
        }

        var (accessToken, expiresAt) = _tokenService.GenerateToken(user);

        var tokenFamilyUuid = Guid.NewGuid();

        var refreshToken = _tokenService.GenerateRefreshToken();

        var refreshTokenEntity = new RefreshToken
        {
            UserId = user.Id,
            TokenHash = _tokenService.HashToken(refreshToken),
            ExpiresAt = DateTimeOffset.UtcNow.AddDays(7), //DateTimeOffset.UtcNow.AddMinutes(1),
            TokenFamilyUuid = tokenFamilyUuid
        };

        await _refreshTokenRepository.AddAsync(
            refreshTokenEntity,
            cancellationToken);

        user.LastLoginAt = DateTimeOffset.UtcNow;

        await _userRepository.UpdateAsync(user);

        await _userRepository.SaveChangesAsync(
            cancellationToken);

        var response = new LoginResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            ExpiresAt = expiresAt,
            User = UserOperationsMapping.ToResponse(user)
        };

        return Result<LoginResponse>.Ok(response);
    }

    public async Task<Result<LoginResponse>> RefreshAsync(
    RefreshTokenRequest request,
    CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.RefreshToken))
        {
            return Result<LoginResponse>.Failure(
                ErrorCodes.InvalidCredentials,
                "El refresh token es obligatorio.");
        }

        var tokenHash = _tokenService.HashToken(request.RefreshToken);

        var refreshToken = await _refreshTokenRepository
            .GetByTokenHashAsync(tokenHash, cancellationToken);

        if (refreshToken is null)
        {
            return Result<LoginResponse>.FailureWithCode(
                ErrorCodes.InvalidCredentials,
                "El refresh token no es válido.");
        }

        if (refreshToken.IsRevoked)
        {
            await _refreshTokenRepository.RevokeFamilyAsync(refreshToken.TokenFamilyUuid,cancellationToken);

            await _refreshTokenRepository.SaveChangesAsync(cancellationToken);
            return Result<LoginResponse>.FailureWithCode(
                ErrorCodes.InvalidCredentials,
                "El refresh token ya fue revocado.");
        }

        if (refreshToken.ExpiresAt <= DateTimeOffset.UtcNow)
        {
            return Result<LoginResponse>.FailureWithCode(
                ErrorCodes.InvalidCredentials,
                "El refresh token ha expirado.");
        }

        var user = refreshToken.User;

        //var user = await _userRepository.GetByIdAsync(
        //refreshToken.UserId,
        //cancellationToken);

        if (user is null)
        {
            return Result<LoginResponse>.FailureWithCode(
                ErrorCodes.InvalidCredentials,
                "El usuario asociado al refresh token no existe.");
        }

        if (!user.IsActive)
        {
            return Result<LoginResponse>.FailureWithCode(
                ErrorCodes.InvalidCredentials,
                "El usuario está inactivo.");
        }

        var (accessToken, expiresAt) =
            _tokenService.GenerateToken(user);

        var newRefreshToken =
            _tokenService.GenerateRefreshToken();

        refreshToken.RevokedAt =
            DateTimeOffset.UtcNow;

        var newRefreshTokenEntity = new RefreshToken
        {
            UserId = user.Id,
            TokenFamilyUuid = refreshToken.TokenFamilyUuid,
            TokenHash = _tokenService.HashToken(newRefreshToken),
            ExpiresAt = DateTimeOffset.UtcNow.AddDays(7)  //DateTimeOffset.UtcNow.AddMinutes(1)
                
        };

        await _refreshTokenRepository.AddAsync(
            newRefreshTokenEntity,
            cancellationToken);

        await _refreshTokenRepository.UpdateAsync(
            refreshToken,
            cancellationToken);

        await _refreshTokenRepository.SaveChangesAsync(
            cancellationToken);

        var response = new LoginResponse
        {
            AccessToken = accessToken,
            RefreshToken = newRefreshToken,
            ExpiresAt = expiresAt,
            User = UserOperationsMapping.ToResponse(user)
        };

        return Result<LoginResponse>.Ok(response);
    }

    public async Task<Result<bool>> LogoutAsync(
    LogoutRequest request,
    CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.RefreshToken))
        {
            return Result<bool>.Failure(
                ErrorCodes.InvalidCredentials,
                "El refresh token es obligatorio.");
        }

        var tokenHash = _tokenService.HashToken(
            request.RefreshToken);

        var refreshToken =
            await _refreshTokenRepository.GetByTokenHashAsync(
                tokenHash,
                cancellationToken);

        if (refreshToken is null)
        {
            return Result<bool>.Failure(
                ErrorCodes.InvalidCredentials,
                "El refresh token no es válido.");
        }

        if (refreshToken.IsRevoked)
        {
            return Result<bool>.Failure(
                ErrorCodes.InvalidCredentials,
                "El refresh token ya fue revocado.");
        }

        refreshToken.RevokedAt = DateTimeOffset.UtcNow;

        //await _refreshTokenRepository.UpdateAsync(refreshToken,cancellationToken);
        await _refreshTokenRepository.RevokeFamilyAsync(refreshToken.TokenFamilyUuid,cancellationToken);

        await _refreshTokenRepository.SaveChangesAsync(
            cancellationToken);

        return Result<bool>.Ok(true);
    }
}
