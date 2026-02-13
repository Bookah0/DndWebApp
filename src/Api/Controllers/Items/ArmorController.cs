using AutoMapper;
using Api.Models.DTOs.ResponseDtos;
using Microsoft.AspNetCore.Mvc;
using Api.Models.DTOs.RequestDtos.Inventory;
using Api.Services.Interfaces.Items;
using Api.Services.Util;
using Api.Models.DTOs.Items;

namespace Api.Controllers.Items;

[ApiController]
[Route("api/[controller]")]
public class ArmorController(IArmorService service, IMapper mapper) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ICollection<ArmorResponseDto>>> GetAllArmor([FromQuery] ArmorFilterDto filterDto, [FromQuery] PaginationRequestDto paginationDto)
    {
        var (totalArmor, filteredArmor) = await service.GetFilteredAsync(filterDto, paginationDto);
        
        return Ok(new PaginationResponseDto<ArmorResponseDto>
        {
            Items = mapper.Map<ICollection<ArmorResponseDto>>(filteredArmor),
            ItemCount = totalArmor,
            Page = paginationDto.Page,
            PageSize = paginationDto.PageSize,
            Next = PaginationUtil.GetNext(paginationDto, totalArmor, "api/armor"),
            Prev = PaginationUtil.GetPrev(paginationDto, "api/armor")
        });
    }

    [HttpGet("{armorId}")]
    public async Task<ActionResult<ArmorResponseDto>> GetArmor(int armorId)
    {
        var armor = await service.GetByIdAsync(armorId);
        return Ok(mapper.Map<ArmorResponseDto>(armor));
    }

    [HttpPost]
    public async Task<ActionResult<ArmorResponseDto>> CreateArmor([FromBody] CreateArmorRequestDto dto)
    {
        var armor = await service.CreateAsync(dto);
        return Ok(mapper.Map<ArmorResponseDto>(armor));
    }

    [HttpPatch("{armorId}")]
    public async Task<ActionResult<ArmorResponseDto>> UpdateArmor(int armorId, [FromBody] UpdateArmorRequestDto dto)
    {
        var updatedArmor = await service.UpdateAsync(dto, armorId);
        return Ok(mapper.Map<ArmorResponseDto>(updatedArmor));
    }


    [HttpDelete("{armorId}")]
    public async Task<ActionResult> DeleteArmor(int armorId)
    {
        await service.DeleteAsync(armorId);
        return Ok();
    }
}