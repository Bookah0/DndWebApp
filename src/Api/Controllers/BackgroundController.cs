using AutoMapper;
using Api.Models.DTOs.RequestDtos.Character;
using Api.Models.DTOs.ResponseDtos;
using Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Backgrounds;

[ApiController]
[Route("api/backgrounds")]
public class BackgroundController(IBackgroundService service, IMapper mapper) : ControllerBase
    {

    [HttpGet]
    public async Task<ActionResult<ICollection<BackgroundResponseDto>>> GetBackgrounds()
    {
        var backgrounds = await service.GetAllAsync();
        return Ok(mapper.Map<ICollection<BackgroundResponseDto>>(backgrounds));
    }

    [HttpGet("{backgroundId}")]
    public async Task<ActionResult<BackgroundResponseDto>> GetBackground(int backgroundId)
    {
        var background = await service.GetByIdAsync(backgroundId);
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