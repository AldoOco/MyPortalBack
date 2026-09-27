using MyPortalBack.Application.Common.Interfaces;
using MyPortalBack.Application.Common.Result;
using MyPortalBack.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPortalBack.Application.Features;

public class RoleOperations
{
    private readonly IRoleRepository _roleRepository;
    private readonly ICurrentUser _currentUser;

    public RoleOperations(
        IRoleRepository roleRepository,
        ICurrentUser currentUser)
    {
        _roleRepository = roleRepository;
        _currentUser = currentUser;
    }

    public async Task<Result<Role?>> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var role = await _roleRepository.GetByIdAsync(
            id,
            cancellationToken);

        if (role is null)
        {
            return Result<Role?>.Failure(
                ErrorCodes.NotFound,
                "El rol no fue encontrado.");
        }

        return Result<Role?>.Ok(role);
    }

    public async Task<Result<Role?>> GetByNameAsync(
        string name,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result<Role?>.Failure(
                ErrorCodes.ValidationError,
                "El nombre del rol es obligatorio.");
        }

        var role = await _roleRepository.GetByNameAsync(
            name,
            cancellationToken);

        if (role is null)
        {
            return Result<Role?>.Failure(
                ErrorCodes.NotFound,
                "El rol no fue encontrado.");
        }

        return Result<Role?>.Ok(role);
    }

    public async Task<Result<List<Role>>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        var roles = await _roleRepository.GetAllAsync(
            cancellationToken);

        return Result<List<Role>>.Ok(roles);
    }

    public async Task<Result<Role>> CreateAsync(
        string name,
        string description,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result<Role>.Failure(
                ErrorCodes.ValidationError,
                "El nombre del rol es obligatorio.");
        }

        var exists = await _roleRepository.ExistsByNameAsync(
            name,
            cancellationToken);

        if (exists)
        {
            return Result<Role>.Failure(
                ErrorCodes.Conflict,
                "El rol ya está registrado.");
        }

        var role = new Role
        {
            Name = name.Trim(),
            Description = description?.Trim() ?? string.Empty,
            IsActive = true,
            CreatedAt = DateTimeOffset.UtcNow,
            CreatedBy = _currentUser.UserUuid,
            UpdatedAt = null,
            UpdatedBy = null,
            IsDeleted = false
        };

        await _roleRepository.AddAsync(
            role,
            cancellationToken);

        await _roleRepository.SaveChangesAsync(
            cancellationToken);

        return Result<Role>.Ok(role);
    }

    public async Task<Result<Role>> UpdateAsync(
        int id,
        string name,
        string description,
        bool isActive,
        CancellationToken cancellationToken = default)
    {
        var role = await _roleRepository.GetByIdAsync(
            id,
            cancellationToken);

        if (role is null)
        {
            return Result<Role>.Failure(
                ErrorCodes.NotFound,
                "El rol no fue encontrado.");
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            return Result<Role>.Failure(
                ErrorCodes.ValidationError,
                "El nombre del rol es obligatorio.");
        }

        var existingRole = await _roleRepository.GetByNameAsync(
            name,
            cancellationToken);

        if (existingRole is not null &&
            existingRole.Id != role.Id)
        {
            return Result<Role>.Failure(
                ErrorCodes.Conflict,
                "Ya existe otro rol con ese nombre.");
        }

        role.Name = name.Trim();
        role.Description = description?.Trim() ?? string.Empty;
        role.IsActive = isActive;
        role.UpdatedAt = DateTimeOffset.UtcNow;
        role.UpdatedBy = _currentUser.UserUuid;

        await _roleRepository.UpdateAsync(
            role,
            cancellationToken);

        await _roleRepository.SaveChangesAsync(
            cancellationToken);

        return Result<Role>.Ok(role);
    }

    public async Task<Result<bool>> DeleteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var role = await _roleRepository.GetByIdAsync(
            id,
            cancellationToken);

        if (role is null)
        {
            return Result<bool>.Failure(
                ErrorCodes.NotFound,
                "El rol no fue encontrado.");
        }

        role.IsDeleted = true;
        role.IsActive = false;
        role.UpdatedAt = DateTimeOffset.UtcNow;
        role.UpdatedBy = _currentUser.UserUuid;

        await _roleRepository.UpdateAsync(
            role,
            cancellationToken);

        await _roleRepository.SaveChangesAsync(
            cancellationToken);

        return Result<bool>.Ok(true);
    }
}
