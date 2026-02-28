using Api.Domain.Items.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Api.Domain.Items.DTOs;
using Api.Domain.Shared.DTOs;
using Api.Domain.Shared.Utils;

namespace Api.Domain.Items.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ToolsController(IToolService service, IMapper mapper) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ICollection<ToolResponseDto>>> GetAllTools([FromQuery] ToolFilterDto? filterDto = null, [FromQuery] PaginationRequestDto? paginationDto = null)
    {
        var filteredTools = await service.GetAllAsync(filterDto, paginationDto);
        var mappedTools = mapper.Map<ICollection<ToolResponseDto>>(filteredTools);

        return Ok(PaginationUtil.BuildPaginationResponse(mappedTools, paginationDto, "api/tools"));
    }

    [HttpGet("{toolId}")]
    public async Task<ActionResult<ToolResponseDto>> GetTool(int toolId)
    {
        var tool = await service.GetByIdAsync(toolId);
        return Ok(mapper.Map<ToolResponseDto>(tool));
    }

    [HttpPost]
    public async Task<ActionResult<ToolResponseDto>> CreateTool([FromBody] CreateToolRequestDto dto)
    {
        var tool = await service.CreateAsync(dto);
        return Ok(mapper.Map<ToolResponseDto>(tool));
    }

    [HttpPatch("{toolId}")]
    public async Task<ActionResult<ToolResponseDto>> UpdateTool(int toolId, [FromBody] UpdateToolRequestDto dto)
    {
        var updatedTool = await service.UpdateAsync(dto, toolId);
        return Ok(mapper.Map<ToolResponseDto>(updatedTool));
    }


    [HttpDelete("{toolId}")]
    public async Task<ActionResult> DeleteTool(int toolId)
    {
        await service.DeleteAsync(toolId);
        return Ok();
    }

    [HttpPatch("{toolId}/properties")]
    public async Task<ActionResult<ToolResponseDto>> AddProperty(int toolId, [FromBody] ToolPropertyDto dto)
    {
        var updatedTool = await service.AddProperty(dto, toolId);
        return Ok(mapper.Map<ToolResponseDto>(updatedTool));
    }

    [HttpPatch("{toolId}/activities")]
    public async Task<ActionResult<ToolResponseDto>> AddActivity(int toolId, [FromBody] ToolActivityDto dto)
    {
        var updatedTool = await service.AddActivity(dto, toolId);
        return Ok(mapper.Map<ToolResponseDto>(updatedTool));
    }
}