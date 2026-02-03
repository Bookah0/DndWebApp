using AutoMapper;
using DndWebApp.Api.Models.DTOs.RequestDtos.Character;
using DndWebApp.Api.Models.DTOs.ResponseDtos;
using DndWebApp.Api.Models.DTOs.Spells;
using DndWebApp.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DndWebApp.Api.Controllers;

[ApiController]
[Route("api/[controller]s")]
public class SpellController(ISpellService service, IMapper mapper) : ControllerBase
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
    public async Task<ActionResult<SpellResponseDto>> CreateSpell([FromBody] SpellDto dto)
    {
        var spell = await service.CreateAsync(dto);
        return Ok(mapper.Map<SpellResponseDto>(spell));
    }

    [HttpPatch("{spellId}")]
    public async Task<ActionResult<SpellResponseDto>> UpdateSpell(int spellId, [FromBody] SpellDto dto)
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