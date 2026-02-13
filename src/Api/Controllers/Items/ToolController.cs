using AutoMapper;
using Api.Models.DTOs.RequestDtos.Inventory;
using Api.Models.DTOs.ResponseDtos;
using Api.Models.Items;
using Api.Services.Interfaces.Items;
using Microsoft.AspNetCore.Mvc;
using Api.Services.Util;
using Api.Models.DTOs.Items;

namespace Api.Controllers.Items;

[ApiController]
[Route("api/[controller]")]
public class ToolsController(IToolService service, IMapper mapper) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ICollection<ToolResponseDto>>> GetAllTools([FromQuery] ToolFilterDto filterDto, [FromQuery] PaginationRequestDto paginationDto)
    {
        var (totalTools, filteredTools) = await service.GetFilteredAsync(filterDto, paginationDto);
        
        return Ok(new PaginationResponseDto<ToolResponseDto>
        {
            Items = mapper.Map<ICollection<ToolResponseDto>>(filteredTools),
            ItemCount = totalTools,
            Page = paginationDto.Page,
            PageSize = paginationDto.PageSize,
            Next = PaginationUtil.GetNext(paginationDto, totalTools, "api/tools"),
            Prev = PaginationUtil.GetPrev(paginationDto, "api/tools")
        });
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