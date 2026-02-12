using AutoMapper;
using Api.Models.DTOs.RequestDtos.Character;
using Api.Models.DTOs.ResponseDtos;
using Api.Models.DTOs.Spells;
using Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Api.Repositories.Implemented.Spells;
using Api.Services.Util;
using Api.Models.Spells;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SpellsController(ISpellService service, IMapper mapper) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ICollection<SpellResponseDto>>> GetSpells([FromQuery] SpellFilterDto filterDto, [FromQuery] PaginationRequestDto paginationDto)
    {
        var (totalSpells, filteredSpells) = await service.GetFilteredAsync(filterDto, paginationDto);
        
        return Ok(new PaginationResponseDto<Spell>
        {
            Items = filteredSpells,
            ItemCount = totalSpells,
            Page = paginationDto.Page,
            PageSize = paginationDto.PageSize,
            Next = PaginationUtil.GetNext(paginationDto, totalSpells, "api/spells"),
            Prev = PaginationUtil.GetPrev(paginationDto, "api/spells")
        });
    }

    [HttpGet("{spellId}")]
    public async Task<ActionResult<SpellResponseDto>> GetSpell(int spellId)
    {
        var spell = await service.GetByIdAsync(spellId);
        return Ok(mapper.Map<SpellResponseDto>(spell));
    }

    [HttpPost]
    public async Task<ActionResult<SpellResponseDto>> CreateSpell([FromBody] CreateSpellRequestDto dto)
    {
        var spell = await service.CreateAsync(dto);
        return Ok(mapper.Map<SpellResponseDto>(spell));
    }

    [HttpPatch("{spellId}")]
    public async Task<ActionResult<SpellResponseDto>> UpdateSpell(int spellId, [FromBody] UpdateSpellRequestDto dto)
    {
        var updatedSpell = await service.UpdateAsync(spellId, dto);
        return Ok(mapper.Map<SpellResponseDto>(updatedSpell));
    }

    [HttpDelete("{spellId}")]
    public async Task<ActionResult> DeleteSpell(int spellId)
    {
        await service.DeleteAsync(spellId);
        return Ok();
    }
}