using AutoMapper;
using DndWebApp.Api.Models.DTOs.Inventory;
using DndWebApp.Api.Models.DTOs.ResponseDtos;
using DndWebApp.Api.Services.Interfaces.Items;
using Microsoft.AspNetCore.Mvc;

namespace DndWebApp.Api.Controllers.Items;

[ApiController]
[Route("api/[controller]s")]
public class ToolController(IToolService service, IMapper mapper) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ICollection<ToolResponseDto>>> GetAllTools([FromQuery] string? sort = null, [FromQuery] string? order = null)
    {
        var tools = await service.GetAllAsync();
        return Ok(mapper.Map<ICollection<ToolResponseDto>>(tools));
    }

    [HttpGet("{toolId}")]
    public async Task<ActionResult<ToolResponseDto>> GetTool(int toolId)
    {
        var tool = await service.GetByIdAsync(toolId);
        return Ok(mapper.Map<ToolResponseDto>(tool));
    }

    [HttpPost]
    public async Task<ActionResult<ToolResponseDto>> CreateTool([FromBody] ToolDto dto)
    {
        var tool = await service.CreateAsync(dto);
        return Ok(mapper.Map<ToolResponseDto>(tool));
    }

    [HttpPatch("{toolId}")]
    public async Task<ActionResult> UpdateTool(int toolId, [FromBody] ToolDto dto)
    {
        await service.UpdateAsync(dto, toolId);
        return Ok();
    }


    [HttpDelete("{toolId}")]
    public async Task<ActionResult> DeleteTrait(int toolId)
    {
        await service.DeleteAsync(toolId);
        return Ok();
    }

    [HttpPatch("{toolId}/properties")]
    public async Task<ActionResult> AddProperty(int toolId, [FromBody] ToolPropertyDto dto)
    {
        await service.AddProperty(dto, toolId);
        return Ok();
    }

    [HttpPatch("{toolId}/activities")]
    public async Task<ActionResult> AddActivity(int toolId, [FromBody] ToolActivityDto dto)
    {
        await service.AddActivity(dto, toolId);
        return Ok();
    }
}