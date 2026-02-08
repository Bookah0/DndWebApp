using AutoMapper;
using Api.Models.DTOs.RequestDtos.Character;
using Api.Models.DTOs.ResponseDtos;
using Api.Models.DTOs.Spells;
using Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SpellsController(ISpellService service, IMapper mapper) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ICollection<SpellResponseDto>>> GetSpells()
    {
        var spells = await service.GetAllAsync();
        return Ok(mapper.Map<ICollection<SpellResponseDto>>(spells));
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