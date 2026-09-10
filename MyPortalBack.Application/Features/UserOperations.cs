using MediatR;
using MyPortalBack.Application.Common.Contracts.Responses;
using MyPortalBack.Application.Common.Interfaces;
using MyPortalBack.Application.Common.Result;
using MyPortalBack.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPortalBack.Application.Features;

public class UserOperations
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;

    public UserOperations(IUserRepository userRepository, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    // =========================
    // Queries
    // =========================

    public async Task<Result<UserResponse>> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(
            id,
            cancellationToken);

        if (user is null)
        {
            //return Result<UserResponse>.Failure($"No se encontró el usuario con el identificador '{id}'.");
            return Result<UserResponse>.FailureWithCode(
                "USER_NOT_FOUND",
                "El usuario no existe.");
        }

        return Result<UserResponse>.Ok(
            UserOperationsMapping.ToResponse(user));
    }

    public async Task<Result<UserResponse>> GetByEmailAsync(
        string email,
        CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByEmailAsync(
            email,
            cancellationToken);

        if (user is null)
        {
            return Result<UserResponse>.Failure(
                $"No se encontró un usuario con el correo '{email}'.");
        }

        return Result<UserResponse>.Ok(
            UserOperationsMapping.ToResponse(user));
    }

    public async Task<Result<IReadOnlyList<UserResponse>>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        var users = await _userRepository.GetAllAsync(
            cancellationToken);

        var responses = users
            .Select(UserOperationsMapping.ToResponse)
            .ToList();

        return Result<IReadOnlyList<UserResponse>>.Ok(responses);
    }

    public async Task<Result<UserResponse>> CreateAsync(
        string firstName,
        string lastName,
        string email,
        string password,
        CancellationToken cancellationToken = default)
    {
        var exists = await _userRepository.ExistsByEmailAsync(
            email,
            cancellationToken);

        if (exists)
        {
            return Result<UserResponse>.FailureWithCode(
                "EMAIL_ALREADY_EXISTS",
                "El correo electrónico ya está registrado.");
        }
        var passwordHash = _passwordHasher.Hash(password);
        var user = new User
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            PasswordHash = passwordHash,
            IsActive = true,
            LastLoginAt = null,

            CreatedAt = DateTimeOffset.UtcNow,
            CreatedBy = null,

            UpdatedAt = null,
            UpdatedBy = null,

            IsDeleted = false
        };

        await _userRepository.AddAsync(
            user,
            cancellationToken);

        await _userRepository.SaveChangesAsync(
            cancellationToken);

        return Result<UserResponse>.Ok(
            UserOperationsMapping.ToResponse(user));
    }

    public async Task<Result<UserResponse>> UpdateAsync(
    Guid id,
    string firstName,
    string lastName,
    string email,
    bool isActive,
    CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(
            id,
            cancellationToken);

        if (user is null)
        {
            return Result<UserResponse>.FailureWithCode(
                "USER_NOT_FOUND",
                "El usuario no existe.");
        }

        var existingUser = await _userRepository.GetByEmailAsync(
            email,
            cancellationToken);

        if (existingUser is not null && existingUser.Uuid != id)
        {
            return Result<UserResponse>.Failure(
                "El correo electrónico ya está registrado por otro usuario.");
        }

        user.FirstName = firstName;
        user.LastName = lastName;
        user.Email = email;
        user.IsActive = isActive;

        user.UpdatedAt = DateTimeOffset.UtcNow;
        user.UpdatedBy = null;

        await _userRepository.UpdateAsync(
            user,
            cancellationToken);

        await _userRepository.SaveChangesAsync(
            cancellationToken);

        return Result<UserResponse>.Ok(
            UserOperationsMapping.ToResponse(user));
    }

    public async Task<Result<UserResponse>> DeleteAsync(
    Guid id,
    CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(
            id,
            cancellationToken);

        if (user is null)
        {
            //return Result<UserResponse>.Failure("El usuario no fue encontrado.");
            return Result<UserResponse>.FailureWithCode(
                "USER_NOT_FOUND",
                "El usuario no existe.");
        }

        if (user.IsDeleted)
        {
            return Result<UserResponse>.Failure(
                "El usuario ya se encuentra eliminado.");
        }

        user.IsDeleted = true;
        user.IsActive = false;
        user.UpdatedAt = DateTimeOffset.UtcNow;
        user.UpdatedBy = null;

        await _userRepository.UpdateAsync(
            user,
            cancellationToken);

        await _userRepository.SaveChangesAsync(
            cancellationToken);

        return Result<UserResponse>.Ok(
            UserOperationsMapping.ToResponse(user));
    }

}

internal static class UserOperationsMapping
{
    public static UserResponse ToResponse(User user)
    {
        return new UserResponse
        {
            Id = user.Uuid,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            IsActive = user.IsActive,
            LastLoginAt = user.LastLoginAt,
            CreatedAt = user.CreatedAt
        };
    }
}