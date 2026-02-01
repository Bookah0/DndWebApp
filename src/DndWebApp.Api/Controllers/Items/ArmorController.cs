using AutoMapper;
using DndWebApp.Api.Models.DTOs.ResponseDtos;
using Microsoft.AspNetCore.Mvc;
using DndWebApp.Api.Models.DTOs.Inventory;
using DndWebApp.Api.Services.Interfaces.Items;

namespace DndWebApp.Api.Controllers.Items;

[ApiController]
[Route("api/[controller]s")]
public class ArmorController(IArmorService service, IMapper mapper) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ICollection<ArmorResponseDto>>> GetAllArmor([FromQuery] string? sort = null, [FromQuery] string? order = null)
    {
        var armor = await service.GetAllAsync();
        return Ok(mapper.Map<ICollection<ArmorResponseDto>>(armor));
    }

    [HttpGet("{armorId}")]
    public async Task<ActionResult<ArmorResponseDto>> GetArmor(int armorId)
    {
        var armor = await service.GetByIdAsync(armorId);
        return Ok(mapper.Map<ArmorResponseDto>(armor));
    }

    [HttpPost]
    public async Task<ActionResult<ArmorResponseDto>> CreateArmor([FromBody] ArmorDto dto)
    {
        var armor = await service.CreateAsync(dto);
        return Ok(mapper.Map<ArmorResponseDto>(armor));
    }

    [HttpPatch("{armorId}")]
    public async Task<ActionResult> UpdateArmor(int armorId, [FromBody] ArmorDto dto)
    {
        await service.UpdateAsync(dto, armorId);
        return Ok();
    }


    [HttpDelete("{armorId}")]
    public async Task<ActionResult> DeleteArmor(int armorId)
    {
        await service.DeleteAsync(armorId);
        return Ok();
    }
}