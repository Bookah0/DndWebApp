using AutoMapper;
using Api.Models.DTOs.RequestDtos.Inventory;
using Api.Models.DTOs.ResponseDtos;
using Api.Services.Interfaces.Items;
using Microsoft.AspNetCore.Mvc;
using Api.Models.DTOs.Items;
using Api.Services.Util;

namespace Api.Controllers.Items;

[ApiController]
[Route("api/[controller]")]
public class ItemsController(IItemService service, IMapper mapper) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ICollection<ItemResponseDto>>> GetAllItems([FromQuery] ItemFilterDto filterDto, [FromQuery] PaginationRequestDto paginationDto)
    {
        var (totalItems, filteredItems) = await service.GetFilteredAsync(filterDto, paginationDto);
        
        return Ok(new PaginationResponseDto<ItemResponseDto>
        {
            Items = mapper.Map<ICollection<ItemResponseDto>>(filteredItems),
            ItemCount = totalItems,
            Page = paginationDto.Page,
            PageSize = paginationDto.PageSize,
            Next = PaginationUtil.GetNext(paginationDto, totalItems, "api/items"),
            Prev = PaginationUtil.GetPrev(paginationDto, "api/items")
        });
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