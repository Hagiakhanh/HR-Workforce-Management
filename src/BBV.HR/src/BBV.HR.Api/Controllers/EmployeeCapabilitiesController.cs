using BBV.HR.Application.Common.Exceptions;
using BBV.HR.Application.DTOs.EmployeeCapability;
using BBV.HR.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace BBV.HR.Api.Controllers;

[ApiController]
[Route("api/employees/{employeeId:guid}/capabilities")]
public class EmployeeCapabilitiesController : ControllerBase
{
    private readonly IEmployeeCapabilityService _employeeCapabilityService;

    public EmployeeCapabilitiesController(IEmployeeCapabilityService employeeCapabilityService)
    {
        _employeeCapabilityService = employeeCapabilityService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<EmployeeCapabilityDto>>> GetEmployeeCapabilities(Guid employeeId)
    {
        var capabilities = await _employeeCapabilityService.GetEmployeeCapabilitiesAsync(employeeId);
        return Ok(capabilities);
    }

    [HttpPost]
    public async Task<ActionResult<EmployeeCapabilityDto>> AddEmployeeCapability(
        Guid employeeId,
        [FromBody] AddEmployeeCapabilityDto dto)
    {
        var capability = await _employeeCapabilityService.AddEmployeeCapabilityAsync(employeeId, dto);
        return CreatedAtAction(
            nameof(GetEmployeeCapabilities),
            new { employeeId },
            capability);
    }

    [HttpPatch("{capabilityId:guid}")]
    public async Task<ActionResult<EmployeeCapabilityDto>> UpdateEmployeeCapability(
        Guid employeeId,
        Guid capabilityId,
        [FromBody] UpdateEmployeeCapabilityDto dto)
    {
        var updated = await _employeeCapabilityService.UpdateEmployeeCapabilityAsync(employeeId, capabilityId, dto);
        if (updated == null)
        {
            throw new NotFoundException("EmployeeCapability", capabilityId);
        }
        return Ok(updated);
    }

    [HttpDelete("{capabilityId:guid}")]
    public async Task<IActionResult> RemoveEmployeeCapability(Guid employeeId, Guid capabilityId)
    {
        var removed = await _employeeCapabilityService.RemoveEmployeeCapabilityAsync(employeeId, capabilityId);
        if (!removed)
        {
            throw new NotFoundException("EmployeeCapability", capabilityId);
        }
        return NoContent();
    }
}
