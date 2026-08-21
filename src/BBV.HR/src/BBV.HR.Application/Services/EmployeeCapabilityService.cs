using BBV.HR.Application.Common.Exceptions;
using BBV.HR.Application.DTOs.EmployeeCapability;
using BBV.HR.Application.Entities;
using BBV.HR.Application.Interfaces.Repositories;
using BBV.HR.Application.Interfaces.Services;
using BBV.HR.Application.Mappings;
using FluentValidation;

namespace BBV.HR.Application.Services;

public class EmployeeCapabilityService : IEmployeeCapabilityService
{
    private readonly IEmployeeCapabilityRepository _employeeCapabilityRepository;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly ICapabilityRepository _capabilityRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<AddEmployeeCapabilityDto> _addValidator;
    private readonly IValidator<UpdateEmployeeCapabilityDto> _updateValidator;

    public EmployeeCapabilityService(
        IEmployeeCapabilityRepository employeeCapabilityRepository,
        IEmployeeRepository employeeRepository,
        ICapabilityRepository capabilityRepository,
        IUnitOfWork unitOfWork,
        IValidator<AddEmployeeCapabilityDto> addValidator,
        IValidator<UpdateEmployeeCapabilityDto> updateValidator)
    {
        _employeeCapabilityRepository = employeeCapabilityRepository;
        _employeeRepository = employeeRepository;
        _capabilityRepository = capabilityRepository;
        _unitOfWork = unitOfWork;
        _addValidator = addValidator;
        _updateValidator = updateValidator;
    }

    public async Task<IEnumerable<EmployeeCapabilityDto>> GetEmployeeCapabilitiesAsync(Guid employeeId)
    {
        var employeeExists = await _employeeRepository.ExistsAsync(employeeId);
        if (!employeeExists)
        {
            throw new NotFoundException("Employee", employeeId);
        }

        var capabilities = await _employeeCapabilityRepository.GetByEmployeeIdAsync(employeeId);
        return capabilities.Select(ec => ec.ToDto());
    }

    public async Task<EmployeeCapabilityDto> AddEmployeeCapabilityAsync(Guid employeeId, AddEmployeeCapabilityDto dto)
    {
        await _addValidator.ValidateAndThrowAsync(dto);

        var employeeExists = await _employeeRepository.ExistsAsync(employeeId);
        if (!employeeExists)
        {
            throw new NotFoundException("Employee", employeeId);
        }

        var capability = await _capabilityRepository.GetByIdAsync(dto.CapabilityId);
        if (capability == null)
        {
            throw new NotFoundException("Capability", dto.CapabilityId);
        }

        var exists = await _employeeCapabilityRepository.ExistsAsync(employeeId, dto.CapabilityId);
        if (exists)
        {
            throw new InvalidOperationException("Employee already has this capability assigned.");
        }

        var employeeCapability = new EmployeeCapability
        {
            Id = Guid.NewGuid(),
            EmployeeId = employeeId,
            CapabilityId = dto.CapabilityId,
            ProficiencyLevel = dto.ProficiencyLevel,
            YearsExperience = dto.YearsExperience,
            UpdatedAt = DateTime.UtcNow
        };

        var created = await _employeeCapabilityRepository.AddAsync(employeeCapability);
        await _unitOfWork.SaveChangesAsync();

        var reloaded = await _employeeCapabilityRepository.GetByIdAsync(created.Id);
        return (reloaded ?? created).ToDto();
    }

    public async Task<EmployeeCapabilityDto?> UpdateEmployeeCapabilityAsync(Guid employeeId, Guid capabilityId, UpdateEmployeeCapabilityDto dto)
    {
        await _updateValidator.ValidateAndThrowAsync(dto);

        var employeeExists = await _employeeRepository.ExistsAsync(employeeId);
        if (!employeeExists)
        {
            throw new NotFoundException("Employee", employeeId);
        }

        var entity = await _employeeCapabilityRepository.GetByEmployeeAndCapabilityIdAsync(employeeId, capabilityId);
        if (entity == null)
        {
            return null;
        }

        if (dto.ProficiencyLevel.HasValue) entity.ProficiencyLevel = dto.ProficiencyLevel;
        if (dto.YearsExperience.HasValue) entity.YearsExperience = dto.YearsExperience;
        entity.UpdatedAt = DateTime.UtcNow;

        await _employeeCapabilityRepository.UpdateAsync(entity);
        await _unitOfWork.SaveChangesAsync();

        var reloaded = await _employeeCapabilityRepository.GetByIdAsync(entity.Id);
        return (reloaded ?? entity).ToDto();
    }

    public async Task<bool> RemoveEmployeeCapabilityAsync(Guid employeeId, Guid capabilityId)
    {
        var employeeExists = await _employeeRepository.ExistsAsync(employeeId);
        if (!employeeExists)
        {
            throw new NotFoundException("Employee", employeeId);
        }

        var entity = await _employeeCapabilityRepository.GetByEmployeeAndCapabilityIdAsync(employeeId, capabilityId);
        if (entity == null)
        {
            return false;
        }

        await _employeeCapabilityRepository.DeleteAsync(entity);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}
