using BBV.HR.Application.Common.Exceptions;
using BBV.HR.Application.DTOs.ProjectMembers;
using BBV.HR.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace BBV.HR.Api.Controllers;

[ApiController]
[Route("api/projects/{projectId:guid}/members")]
public class ProjectMembersController : ControllerBase
{
    private readonly IProjectMemberService _memberService;

    public ProjectMembersController(IProjectMemberService memberService)
    {
        _memberService = memberService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProjectMemberDto>>> GetMembers(Guid projectId)
    {
        var members = await _memberService.GetProjectMembersAsync(projectId);
        return Ok(members);
    }

    [HttpGet("{memberId:guid}")]
    public async Task<ActionResult<ProjectMemberDto>> GetMemberById(Guid projectId, Guid memberId)
    {
        var member = await _memberService.GetProjectMemberByIdAsync(projectId, memberId);
        if (member == null) throw new NotFoundException("ProjectMember", memberId);
        return Ok(member);
    }

    [HttpPost]
    public async Task<ActionResult<ProjectMemberDto>> AddMember(Guid projectId, [FromBody] AddProjectMemberDto dto)
    {
        var member = await _memberService.AddProjectMemberAsync(projectId, dto);
        return CreatedAtAction(nameof(GetMemberById), new { projectId, memberId = member.Id }, member);
    }

    [HttpPatch("{memberId:guid}")]
    public async Task<ActionResult<ProjectMemberDto>> UpdateMember(Guid projectId, Guid memberId, [FromBody] UpdateProjectMemberDto dto)
    {
        var updated = await _memberService.UpdateProjectMemberAsync(projectId, memberId, dto);
        if (updated == null) throw new NotFoundException("ProjectMember", memberId);
        return Ok(updated);
    }

    [HttpDelete("{memberId:guid}")]
    public async Task<IActionResult> RemoveMember(Guid projectId, Guid memberId)
    {
        var removed = await _memberService.RemoveProjectMemberAsync(projectId, memberId);
        if (!removed) throw new NotFoundException("ProjectMember", memberId);
        return NoContent();
    }

    [HttpPatch("{memberId:guid}/allocation")]
    public async Task<ActionResult<ProjectMemberDto>> UpdateAllocation(Guid projectId, Guid memberId, [FromBody] UpdateMemberAllocationDto dto)
    {
        var updated = await _memberService.UpdateMemberAllocationAsync(projectId, memberId, dto);
        if (updated == null) throw new NotFoundException("ProjectMember", memberId);
        return Ok(updated);
    }
}
