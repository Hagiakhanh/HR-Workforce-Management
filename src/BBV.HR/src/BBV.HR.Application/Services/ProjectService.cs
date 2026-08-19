using BBV.HR.Application.DTOs.Projects;
using BBV.HR.Application.Entities;
using BBV.HR.Application.Interfaces.Repositories;
using BBV.HR.Application.Interfaces.Services;
using BBV.HR.Application.Mappings;
using FluentValidation;

namespace BBV.HR.Application.Services;

public class ProjectService : IProjectService
{
    private readonly IProjectRepository _projectRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CreateProjectDto> _createValidator;
    private readonly IValidator<UpdateProjectDto> _updateValidator;

    public ProjectService(
        IProjectRepository projectRepository,
        IUnitOfWork unitOfWork,
        IValidator<CreateProjectDto> createValidator,
        IValidator<UpdateProjectDto> updateValidator)
    {
        _projectRepository = projectRepository;
        _unitOfWork = unitOfWork;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    public async Task<IEnumerable<ProjectDto>> GetAllProjectsAsync(string? search, string? status, Guid? managerId)
    {
        var projects = await _projectRepository.GetAllAsync(search, status, managerId);
        return projects.Select(p => p.ToDto());
    }

    public async Task<ProjectDto?> GetProjectByIdAsync(Guid projectId)
    {
        var project = await _projectRepository.GetByIdAsync(projectId);
        return project?.ToDto();
    }

    public async Task<ProjectDto> CreateProjectAsync(CreateProjectDto dto)
    {
        await _createValidator.ValidateAndThrowAsync(dto);

        var existingCode = await _projectRepository.ExistsCodeAsync(dto.Code);
        if (existingCode)
        {
            throw new InvalidOperationException($"Project code '{dto.Code}' already exists.");
        }

        var project = new Project
        {
            Id = Guid.NewGuid(),
            Code = dto.Code,
            Name = dto.Name,
            Description = dto.Description,
            Status = dto.Status,
            ManagerId = dto.ManagerId,
            CreatedBy = dto.CreatedBy,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            TotalBudget = dto.TotalBudget,
            LaborBudget = dto.LaborBudget,
            BudgetWarningThreshold = dto.BudgetWarningThreshold,
            ExpectedTeamSizeFte = dto.ExpectedTeamSizeFte,
            DefaultWeeklyCapacityH = dto.DefaultWeeklyCapacityH,
            Client = dto.Client,
            Priority = dto.Priority,
            TechStack = dto.TechStack,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var created = await _projectRepository.AddAsync(project);
        await _unitOfWork.SaveChangesAsync();

        return await GetProjectByIdAsync(created.Id) ?? created.ToDto();
    }

    public async Task<ProjectDto?> UpdateProjectAsync(Guid projectId, UpdateProjectDto dto)
    {
        await _updateValidator.ValidateAndThrowAsync(dto);

        var project = await _projectRepository.GetByIdAsync(projectId);
        if (project == null) return null;

        if (!string.IsNullOrWhiteSpace(dto.Code) && !dto.Code.Equals(project.Code, StringComparison.OrdinalIgnoreCase))
        {
            var existingCode = await _projectRepository.ExistsCodeAsync(dto.Code, projectId);
            if (existingCode)
            {
                throw new InvalidOperationException($"Project code '{dto.Code}' already exists.");
            }
            project.Code = dto.Code;
        }

        if (dto.Name != null) project.Name = dto.Name;
        if (dto.Description != null) project.Description = dto.Description;
        if (dto.Status != null) project.Status = dto.Status;
        if (dto.ManagerId.HasValue) project.ManagerId = dto.ManagerId;
        if (dto.StartDate.HasValue) project.StartDate = dto.StartDate;
        if (dto.EndDate.HasValue) project.EndDate = dto.EndDate;
        if (dto.TotalBudget.HasValue) project.TotalBudget = dto.TotalBudget;
        if (dto.LaborBudget.HasValue) project.LaborBudget = dto.LaborBudget;
        if (dto.BudgetWarningThreshold.HasValue) project.BudgetWarningThreshold = dto.BudgetWarningThreshold;
        if (dto.ExpectedTeamSizeFte.HasValue) project.ExpectedTeamSizeFte = dto.ExpectedTeamSizeFte;
        if (dto.DefaultWeeklyCapacityH.HasValue) project.DefaultWeeklyCapacityH = dto.DefaultWeeklyCapacityH;
        if (dto.Client != null) project.Client = dto.Client;
        if (dto.Priority != null) project.Priority = dto.Priority;
        if (dto.TechStack != null) project.TechStack = dto.TechStack;

        project.UpdatedAt = DateTime.UtcNow;

        await _projectRepository.UpdateAsync(project);
        await _unitOfWork.SaveChangesAsync();

        return await GetProjectByIdAsync(projectId);
    }

    public async Task<bool> DeleteProjectAsync(Guid projectId)
    {
        var project = await _projectRepository.GetByIdAsync(projectId);
        if (project == null) return false;

        await _projectRepository.DeleteAsync(project);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}
