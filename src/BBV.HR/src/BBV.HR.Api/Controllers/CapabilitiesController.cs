using BBV.HR.Application.Common.Exceptions;
using BBV.HR.Application.DTOs.Capability;
using BBV.HR.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace BBV.HR.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CapabilitiesController : ControllerBase
{
    private readonly ICapabilityService _capabilityService;

    public CapabilitiesController(ICapabilityService capabilityService)
    {
        _capabilityService = capabilityService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CapabilityDto>>> GetAll(
        [FromQuery] string? search,
        [FromQuery] string? category)
    {
        var capabilities = await _capabilityService.GetAllCapabilitiesAsync(search, category);
        return Ok(capabilities);
    }

    [HttpGet("{capabilityId:guid}")]
    public async Task<ActionResult<CapabilityDto>> GetById(Guid capabilityId)
    {
        var capability = await _capabilityService.GetCapabilityByIdAsync(capabilityId);
        if (capability == null) throw new NotFoundException("Capability", capabilityId);
        return Ok(capability);
    }

    [HttpPost]
    public async Task<ActionResult<CapabilityDto>> Create([FromBody] CreateCapabilityDto dto)
    {
        var capability = await _capabilityService.CreateCapabilityAsync(dto);
        return CreatedAtAction(nameof(GetById), new { capabilityId = capability.Id }, capability);
    }

    [HttpPatch("{capabilityId:guid}")]
    public async Task<ActionResult<CapabilityDto>> Update(Guid capabilityId, [FromBody] UpdateCapabilityDto dto)
    {
        var updated = await _capabilityService.UpdateCapabilityAsync(capabilityId, dto);
        if (updated == null) throw new NotFoundException("Capability", capabilityId);
        return Ok(updated);
    }

    [HttpDelete("{capabilityId:guid}")]
    public async Task<IActionResult> Delete(Guid capabilityId)
    {
        var deleted = await _capabilityService.DeleteCapabilityAsync(capabilityId);
        if (!deleted) throw new NotFoundException("Capability", capabilityId);
        return NoContent();
    }
}
