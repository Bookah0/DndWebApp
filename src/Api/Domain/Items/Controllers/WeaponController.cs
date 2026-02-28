using Api.Domain.Items.DTOs;
using Api.Domain.Items.Services;
using Api.Domain.Shared.DTOs;
using Api.Domain.Shared.Utils;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Api.Domain.Items.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WeaponsController(IWeaponService service, IMapper mapper) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ICollection<WeaponResponseDto>>> GetAllWeapons([FromQuery] WeaponFilterDto? filterDto = null, [FromQuery] PaginationRequestDto? paginationDto = null)
    {
        var filteredWeapons = await service.GetAllAsync(filterDto, paginationDto);
        var mappedWeapons = mapper.Map<ICollection<WeaponResponseDto>>(filteredWeapons);

        return Ok(PaginationUtil.BuildPaginationResponse(mappedWeapons, paginationDto, "api/weapons"));
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