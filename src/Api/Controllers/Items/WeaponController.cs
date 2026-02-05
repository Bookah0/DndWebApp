using AutoMapper;
using Api.Models.DTOs.Inventory;
using Api.Models.DTOs.ResponseDtos;
using Api.Services.Interfaces.Items;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Items;

[ApiController]
[Route("api/[controller]s")]
public class WeaponController(IWeaponService service, IMapper mapper) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ICollection<WeaponResponseDto>>> GetAllWeapons([FromQuery] string? sort = null, [FromQuery] string? order = null)
    {
        var weapons = await service.GetAllAsync();
        return Ok(mapper.Map<ICollection<WeaponResponseDto>>(weapons));
    }

    [HttpGet("{weaponId}")]
    public async Task<ActionResult<WeaponResponseDto>> GetWeapon(int weaponId)
    {
        var weapon = await service.GetByIdAsync(weaponId);
        return Ok(mapper.Map<WeaponResponseDto>(weapon));
    }

    [HttpPost]
    public async Task<ActionResult<WeaponResponseDto>> CreateWeapon([FromBody] WeaponDto dto)
    {
        var weapon = await service.CreateAsync(dto);
        return Ok(mapper.Map<WeaponResponseDto>(weapon));
    }

    [HttpPatch("{weaponId}")]
    public async Task<ActionResult<WeaponResponseDto>> UpdateWeapon(int weaponId, [FromBody] WeaponDto dto)
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