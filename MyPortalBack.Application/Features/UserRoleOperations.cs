using MyPortalBack.Application.Common.Contracts.Responses;
using MyPortalBack.Application.Common.Interfaces;
using MyPortalBack.Application.Common.Mapping;
using MyPortalBack.Application.Common.Result;
using MyPortalBack.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPortalBack.Application.Features;

public class UserRoleOperations
{
    private readonly IUserRoleRepository _userRoleRepository;
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly ICurrentUser _currentUser;

    public UserRoleOperations(
        IUserRoleRepository userRoleRepository,
        IUserRepository userRepository,
        IRoleRepository roleRepository,
        ICurrentUser currentUser)
    {
        _userRoleRepository = userRoleRepository;
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _currentUser = currentUser;
    }

    public async Task<Result<List<UserRoleResponse>>> GetByUserIdAsync(
        int userId,
        CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdIntAsync(
            userId,
            cancellationToken);

        if (user is null)
        {
            return Result<List<UserRoleResponse>>.Failure(
                ErrorCodes.NotFound,
                "El usuario no existe.");
        }

        var userRoles = await _userRoleRepository.GetByUserIdAsync(
            userId,
            cancellationToken);

        if (userRoles is null) {
            return Result<List<UserRoleResponse>>.Failure(
                ErrorCodes.NotFound,
                "El usuario no tiene roles asignados.");
        }
        var response = userRoles
       .Select(UserRoleOperationsMapping.ToResponse)
       .ToList();

        return Result<List<UserRoleResponse>>.Ok(response);
    }

    public async Task<Result<List<UserRoleResponse>>> GetByRoleIdAsync(
        int roleId,
        CancellationToken cancellationToken = default)
    {
        var role = await _roleRepository.GetByIdAsync(
            roleId,
            cancellationToken);

        if (role is null)
        {
            return Result<List<UserRoleResponse>>.Failure(
                ErrorCodes.NotFound,
                "El rol no existe.");
        }

        var userRoles = await _userRoleRepository.GetByRoleIdAsync(
            roleId,
            cancellationToken);

        if (userRoles is null) {
            return Result<List<UserRoleResponse>>.Failure(
                ErrorCodes.NotFound,
                "No hay usuarios asignados a este rol.");
        }

        var response = userRoles
        .Select(UserRoleOperationsMapping.ToResponse)
        .ToList();

        return Result<List<UserRoleResponse>>.Ok(response);
    }

    public async Task<Result<UserRole>> AssignAsync(
        int userId,
        int roleId,
        CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdIntAsync(
            userId,
            cancellationToken);

        if (user is null)
        {
            return Result<UserRole>.Failure(
                ErrorCodes.NotFound,
                "El usuario no existe.");
        }

        var role = await _roleRepository.GetByIdAsync(
            roleId,
            cancellationToken);

        if (role is null)
        {
            return Result<UserRole>.Failure(
                ErrorCodes.NotFound,
                "El rol no existe.");
        }

        var existingUserRole = await _userRoleRepository.GetAsync(
            userId,
            roleId,
            cancellationToken);

        if (existingUserRole is not null)
        {
            if (!existingUserRole.IsDeleted)
            {
                return Result<UserRole>.Failure(
                    ErrorCodes.Conflict,
                    "El usuario ya tiene asignado este rol.");
            }

            existingUserRole.IsDeleted = false;
            existingUserRole.UpdatedAt = DateTimeOffset.UtcNow;
            existingUserRole.UpdatedBy = _currentUser.UserUuid;

            await _userRoleRepository.UpdateAsync(
                existingUserRole,
                cancellationToken);

            await _userRoleRepository.SaveChangesAsync(
                cancellationToken);

            return Result<UserRole>.Ok(existingUserRole);
        }

        var userRole = new UserRole
        {
            UserId = userId,
            RoleId = roleId,
            CreatedAt = DateTimeOffset.UtcNow,
            CreatedBy = _currentUser.UserUuid,
            UpdatedAt = null,
            UpdatedBy = null,
            IsDeleted = false
        };

        await _userRoleRepository.AddAsync(
            userRole,
            cancellationToken);

        await _userRoleRepository.SaveChangesAsync(
            cancellationToken);

        return Result<UserRole>.Ok(userRole);
    }

    public async Task<Result<bool>> DeleteAsync(
        int userId,
        int roleId,
        CancellationToken cancellationToken = default)
    {
        var userRole = await _userRoleRepository.GetAsync(
            userId,
            roleId,
            cancellationToken);

        if (userRole is null || userRole.IsDeleted)
        {
            return Result<bool>.Failure(
                ErrorCodes.NotFound,
                "La asignación de rol no existe.");
        }

        userRole.IsDeleted = true;
        userRole.UpdatedAt = DateTimeOffset.UtcNow;
        userRole.UpdatedBy = _currentUser.UserUuid;

        await _userRoleRepository.UpdateAsync(
            userRole,
            cancellationToken);

        await _userRoleRepository.SaveChangesAsync(
            cancellationToken);

        return Result<bool>.Ok(true);
    }
}