using BBV.HR.Application.Common.Exceptions;
using BBV.HR.Application.DTOs.Projects;
using BBV.HR.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace BBV.HR.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectsController : ControllerBase
{
    private readonly IProjectService _projectService;

    public ProjectsController(IProjectService projectService)
    {
        _projectService = projectService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProjectDto>>> GetAll(
        [FromQuery] string? search,
        [FromQuery] string? status,
        [FromQuery] Guid? managerId)
    {
        var projects = await _projectService.GetAllProjectsAsync(search, status, managerId);
        return Ok(projects);
    }

    [HttpGet("{projectId:guid}")]
    public async Task<ActionResult<ProjectDto>> GetById(Guid projectId)
    {
        var project = await _projectService.GetProjectByIdAsync(projectId);
        if (project == null) throw new NotFoundException("Project", projectId);
        return Ok(project);
    }

    [HttpPost]
    public async Task<ActionResult<ProjectDto>> Create([FromBody] CreateProjectDto dto)
    {
        var project = await _projectService.CreateProjectAsync(dto);
        return CreatedAtAction(nameof(GetById), new { projectId = project.Id }, project);
    }

    [HttpPatch("{projectId:guid}")]
    public async Task<ActionResult<ProjectDto>> Update(Guid projectId, [FromBody] UpdateProjectDto dto)
    {
        var updated = await _projectService.UpdateProjectAsync(projectId, dto);
        if (updated == null) throw new NotFoundException("Project", projectId);
        return Ok(updated);
    }

    [HttpDelete("{projectId:guid}")]
    public async Task<IActionResult> Delete(Guid projectId)
    {
        var deleted = await _projectService.DeleteProjectAsync(projectId);
        if (!deleted) throw new NotFoundException("Project", projectId);
        return NoContent();
    }
}
