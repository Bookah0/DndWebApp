using Api.Domain.Items.DTOs;
using Api.Domain.Items.Services;
using Api.Domain.Shared.DTOs;
using Api.Domain.Shared.Utils;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Api.Domain.Items.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ItemsController(IItemService service, IMapper mapper) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ICollection<ItemResponseDto>>> GetAllItems([FromQuery] ItemFilterDto? filterDto = null, [FromQuery] PaginationRequestDto? paginationDto = null)
    {
        var filteredItems = await service.GetAllAsync(filterDto, paginationDto);
        var mappedItems = mapper.Map<ICollection<ItemResponseDto>>(filteredItems);

		return Ok(PaginationUtil.BuildPaginationResponse(mappedItems, paginationDto, "api/items"));
    }

    [HttpGet("{itemId}")]
    public async Task<ActionResult<ItemResponseDto>> GetItem(int itemId)
    {
        var item = await service.GetByIdAsync(itemId);
        return Ok(mapper.Map<ItemResponseDto>(item));
    }

    [HttpPost]
    public async Task<ActionResult<ItemResponseDto>> CreateItem([FromBody] CreateItemRequestDto dto)
    {
        var item = await service.CreateAsync(dto);
        return Ok(mapper.Map<ItemResponseDto>(item));
    }

    [HttpPatch("{itemId}")]
    public async Task<ActionResult<ItemResponseDto>> UpdateItem(int itemId, [FromBody] UpdateItemRequestDto dto)
    {
        var updatedItem = await service.UpdateAsync(dto, itemId);
        return Ok(mapper.Map<ItemResponseDto>(updatedItem));
    }


    [HttpDelete("{itemId}")]
    public async Task<ActionResult> DeleteItem(int itemId)
    {
        await service.DeleteAsync(itemId);
        return Ok();
    }
}