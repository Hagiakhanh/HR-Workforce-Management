using BBV.HR.Application.DTOs.ProjectMembers;
using BBV.HR.Application.Entities;
using BBV.HR.Application.Interfaces.Repositories;
using BBV.HR.Application.Interfaces.Services;
using BBV.HR.Application.Mappings;
using FluentValidation;

namespace BBV.HR.Application.Services;

public class ProjectMemberService : IProjectMemberService
{
    private readonly IProjectMemberRepository _memberRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<AddProjectMemberDto> _addMemberValidator;
    private readonly IValidator<UpdateMemberAllocationDto> _allocationValidator;

    public ProjectMemberService(
        IProjectMemberRepository memberRepository,
        IProjectRepository projectRepository,
        IEmployeeRepository employeeRepository,
        IUnitOfWork unitOfWork,
        IValidator<AddProjectMemberDto> addMemberValidator,
        IValidator<UpdateMemberAllocationDto> allocationValidator)
    {
        _memberRepository = memberRepository;
        _projectRepository = projectRepository;
        _employeeRepository = employeeRepository;
        _unitOfWork = unitOfWork;
        _addMemberValidator = addMemberValidator;
        _allocationValidator = allocationValidator;
    }

    public async Task<IEnumerable<ProjectMemberDto>> GetProjectMembersAsync(Guid projectId)
    {
        var members = await _memberRepository.GetByProjectIdAsync(projectId);
        return members.Select(pm => pm.ToDto());
    }

    public async Task<ProjectMemberDto?> GetProjectMemberByIdAsync(Guid projectId, Guid memberId)
    {
        var member = await _memberRepository.GetByIdAsync(projectId, memberId);
        return member?.ToDto();
    }

    public async Task<ProjectMemberDto> AddProjectMemberAsync(Guid projectId, AddProjectMemberDto dto)
    {
        await _addMemberValidator.ValidateAndThrowAsync(dto);

        var projectExists = await _projectRepository.ExistsAsync(projectId);
        if (!projectExists)
        {
            throw new KeyNotFoundException($"Project with ID '{projectId}' not found.");
        }

        var employeeExists = await _employeeRepository.ExistsAsync(dto.EmployeeId);
        if (!employeeExists)
        {
            throw new KeyNotFoundException($"Employee with ID '{dto.EmployeeId}' not found.");
        }

        var alreadyMember = await _memberRepository.IsMemberAsync(projectId, dto.EmployeeId);
        if (alreadyMember)
        {
            throw new InvalidOperationException("Employee is already a member of this project.");
        }

        var member = new ProjectMember
        {
            Id = Guid.NewGuid(),
            ProjectId = projectId,
            EmployeeId = dto.EmployeeId,
            ProjectRole = dto.ProjectRole,
            AllocationPct = dto.AllocationPct,
            Status = dto.Status ?? "Active",
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            JoinedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var created = await _memberRepository.AddAsync(member);
        await _unitOfWork.SaveChangesAsync();

        return await GetProjectMemberByIdAsync(projectId, created.Id) ?? created.ToDto();
    }

    public async Task<ProjectMemberDto?> UpdateProjectMemberAsync(Guid projectId, Guid memberId, UpdateProjectMemberDto dto)
    {
        var member = await _memberRepository.GetByIdAsync(projectId, memberId);
        if (member == null) return null;

        if (dto.ProjectRole != null) member.ProjectRole = dto.ProjectRole;
        if (dto.AllocationPct.HasValue) member.AllocationPct = dto.AllocationPct;
        if (dto.Status != null) member.Status = dto.Status;
        if (dto.StartDate.HasValue) member.StartDate = dto.StartDate;
        if (dto.EndDate.HasValue) member.EndDate = dto.EndDate;

        member.UpdatedAt = DateTime.UtcNow;

        await _memberRepository.UpdateAsync(member);
        await _unitOfWork.SaveChangesAsync();

        return await GetProjectMemberByIdAsync(projectId, memberId);
    }

    public async Task<bool> RemoveProjectMemberAsync(Guid projectId, Guid memberId)
    {
        var member = await _memberRepository.GetByIdAsync(projectId, memberId);
        if (member == null) return false;

        await _memberRepository.DeleteAsync(member);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task<ProjectMemberDto?> UpdateMemberAllocationAsync(Guid projectId, Guid memberId, UpdateMemberAllocationDto dto)
    {
        await _allocationValidator.ValidateAndThrowAsync(dto);

        var member = await _memberRepository.GetByIdAsync(projectId, memberId);
        if (member == null) return null;

        member.AllocationPct = dto.AllocationPct;
        member.UpdatedAt = DateTime.UtcNow;

        await _memberRepository.UpdateAsync(member);
        await _unitOfWork.SaveChangesAsync();

        return await GetProjectMemberByIdAsync(projectId, memberId);
    }
}
