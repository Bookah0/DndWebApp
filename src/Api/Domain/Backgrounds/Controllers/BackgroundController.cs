using Api.Domain.Backgrounds.DTOs;
using Api.Domain.Backgrounds.Services;
using Api.Domain.Shared.DTOs;
using Api.Domain.Shared.Utils;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Api.Domain.Backgrounds.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BackgroundsController(IBackgroundService service, IMapper mapper) : ControllerBase
    {

    [HttpGet]
    public async Task<ActionResult<PaginationResponseDto<BackgroundResponseDto>>> GetAllBackgrounds([FromQuery] BackgroundFilterDto? filterDto = null, [FromQuery] PaginationRequestDto? paginationDto = null)
    {
        var filteredBackgrounds = await service.GetAllAsync(filterDto, paginationDto);
        var mappedBackgrounds = mapper.Map<ICollection<BackgroundResponseDto>>(filteredBackgrounds);

		return Ok(PaginationUtil.BuildPaginationResponse(mappedBackgrounds, paginationDto, "api/backgrounds"));
    }

    [HttpGet("{backgroundId}")]
    public async Task<ActionResult<BackgroundResponseDto>> GetBackground(int backgroundId)
    {
        var background = await service.GetWithFeaturesAsync(backgroundId);
        return Ok(mapper.Map<BackgroundResponseDto>(background));
    }

    [HttpPost]
    public async Task<ActionResult<BackgroundResponseDto>> CreateBackground([FromBody] CreateBackgroundRequestDto dto)
    {
        var background = await service.CreateAsync(dto);
        return Ok(mapper.Map<BackgroundResponseDto>(background));
    }

    [HttpPatch("{backgroundId}")]
    public async Task<ActionResult<BackgroundResponseDto>> UpdateBackground(int backgroundId, [FromBody] UpdateBackgroundRequestDto dto)
    {
        var updatedBackground = await service.UpdateAsync(backgroundId, dto);
        return Ok(mapper.Map<BackgroundResponseDto>(updatedBackground));
    }

    [HttpDelete("{backgroundId}")]
    public async Task<ActionResult> DeleteBackground(int backgroundId)
    {
        await service.DeleteAsync(backgroundId);
        return Ok();
    }

    [HttpPost("{backgroundId}/starting-items")]
    public async Task<ActionResult<BackgroundResponseDto>> AddStartingItem(int backgroundId, [FromBody] int itemId)
    {
        var updatedBackground = await service.AddStartingItemsAsync(backgroundId, itemId);
        return Ok(mapper.Map<BackgroundResponseDto>(updatedBackground));
    }

    [HttpDelete("{backgroundId}/starting-items/{itemId}")]
    public async Task<ActionResult> DeleteStartingItem(int backgroundId, int itemId)
    {
        await service.RemoveStartingItemsAsync(backgroundId, itemId);
        return Ok();
    }

    [HttpPost("{backgroundId}/starting-items/options")]
    public async Task<ActionResult<BackgroundResponseDto>> AddStartingItemOption(int backgroundId, [FromBody] StartingItemOptionDto dto)
    {
        var updatedBackground = await service.AddStartingItemChoiceAsync(backgroundId, dto);
        return Ok(mapper.Map<BackgroundResponseDto>(updatedBackground));
    }

    [HttpDelete("{backgroundId}/starting-items/options/{optionId}")]
    public async Task<ActionResult> DeleteStartingItemOption(int backgroundId, int optionId)
    {
        await service.RemoveStartingItemChoiceAsync(backgroundId, optionId);
        return Ok();
    }
}