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
public class WeaponsController(IWeaponService service, IMapper mapper) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ICollection<WeaponResponseDto>>> GetAllWeapons([FromQuery] WeaponFilterDto filterDto, [FromQuery] PaginationRequestDto paginationDto)
    {
        var (totalWeapons, filteredWeapons) = await service.GetFilteredAsync(filterDto, paginationDto);
        
        return Ok(new PaginationResponseDto<WeaponResponseDto>
        {
            Items = mapper.Map<ICollection<WeaponResponseDto>>(filteredWeapons),
            ItemCount = totalWeapons,
            Page = paginationDto.Page,
            PageSize = paginationDto.PageSize,
            Next = PaginationUtil.GetNext(paginationDto, totalWeapons, "api/weapons"),
            Prev = PaginationUtil.GetPrev(paginationDto, "api/weapons")
        });
    }

    [HttpGet("{weaponId}")]
    public async Task<ActionResult<WeaponResponseDto>> GetWeapon(int weaponId)
    {
        var weapon = await service.GetByIdAsync(weaponId);
        return Ok(mapper.Map<WeaponResponseDto>(weapon));
    }

    [HttpPost]
    public async Task<ActionResult<WeaponResponseDto>> CreateWeapon([FromBody] CreateWeaponRequestDto dto)
    {
        var weapon = await service.CreateAsync(dto);
        return Ok(mapper.Map<WeaponResponseDto>(weapon));
    }

    [HttpPatch("{weaponId}")]
    public async Task<ActionResult<WeaponResponseDto>> UpdateWeapon(int weaponId, [FromBody] UpdateWeaponRequestDto dto)
    {
        var updatedWeapon = await service.UpdateAsync(dto, weaponId);
        return Ok(mapper.Map<WeaponResponseDto>(updatedWeapon));
    }


    [HttpDelete("{weaponId}")]
    public async Task<ActionResult> DeleteWeapon(int weaponId)
    {
        await service.DeleteAsync(weaponId);
        return Ok();
    }
}