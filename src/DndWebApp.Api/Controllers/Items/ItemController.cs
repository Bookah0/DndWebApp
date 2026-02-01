using AutoMapper;
using DndWebApp.Api.Models.DTOs.Inventory;
using DndWebApp.Api.Models.DTOs.ResponseDtos;
using DndWebApp.Api.Services.Interfaces.Items;
using Microsoft.AspNetCore.Mvc;

namespace DndWebApp.Api.Controllers.Items;

[ApiController]
[Route("api/[controller]s")]
public class ItemController(IItemService service, IMapper mapper) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ICollection<ItemResponseDto>>> GetAllItems([FromQuery] string? sort = null, [FromQuery] string? order = null)
    {
        var items = await service.GetAllAsync();
        return Ok(mapper.Map<ICollection<ItemResponseDto>>(items));
    }

    [HttpGet("{itemId}")]
    public async Task<ActionResult<ItemResponseDto>> GetItem(int itemId)
    {
        var item = await service.GetByIdAsync(itemId);
        return Ok(mapper.Map<ItemResponseDto>(item));
    }

    [HttpPost]
    public async Task<ActionResult<ItemResponseDto>> CreateItem([FromBody] ItemDto dto)
    {
        var item = await service.CreateAsync(dto);
        return Ok(mapper.Map<ItemResponseDto>(item));
    }

    [HttpPatch("{itemId}")]
    public async Task<ActionResult> UpdateItem(int itemId, [FromBody] ItemDto dto)
    {
        await service.UpdateAsync(dto, itemId);
        return Ok();
    }


    [HttpDelete("{itemId}")]
    public async Task<ActionResult> DeleteItem(int itemId)
    {
        await service.DeleteAsync(itemId);
        return Ok();
    }
}