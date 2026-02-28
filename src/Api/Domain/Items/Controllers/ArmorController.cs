using Api.Domain.Items.DTOs;
using Api.Domain.Items.Services;
using Api.Domain.Shared.DTOs;
using Api.Domain.Shared.Utils;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Api.Domain.Items.Controllers;


[ApiController]
[Route("api/[controller]")]
public class ArmorController(IArmorService service, IMapper mapper) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ICollection<ArmorResponseDto>>> GetAllArmor([FromQuery] ArmorFilterDto? filterDto = null, [FromQuery] PaginationRequestDto? paginationDto = null)
    {
        var filteredArmor = await service.GetAllAsync(filterDto, paginationDto);
        var mappedArmor = mapper.Map<ICollection<ArmorResponseDto>>(filteredArmor);

        return Ok(PaginationUtil.BuildPaginationResponse(mappedArmor, paginationDto, "api/armor"));
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