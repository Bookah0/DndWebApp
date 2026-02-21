using Api.Domain.Abilities.DTOs;
using Api.Domain.Feats.Models;
using Api.Domain.Shared.DTOs;
using Api.Domain.Shared.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Api.Domain.Feats.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FeatsController(IFeatureService<Feat, CreateFeatRequestDto, UpdateFeatRequestDto> service, IMapper mapper) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ICollection<FeatResponseDto>>> GetFeats()
    {
        var feats = await service.GetAllAsync();
        return Ok(mapper.Map<ICollection<FeatResponseDto>>(feats));
    }

    [HttpGet("{featId}")]
    public async Task<ActionResult<FeatResponseDto>> GetFeat(int featId)
    {
        var feat = await service.GetByIdAsync(featId);
        return Ok(mapper.Map<FeatResponseDto>(feat));
    }

    [HttpPost]
    public async Task<ActionResult<FeatResponseDto>> CreateFeat([FromBody] CreateFeatRequestDto dto)
    {
        var feat = await service.CreateAsync(dto);
        return Ok(mapper.Map<FeatResponseDto>(feat));
    }

    [HttpPatch("{featId}")]
    public async Task<ActionResult<FeatResponseDto>> UpdateFeat(int featId, [FromBody] UpdateFeatRequestDto dto)
    {
        var updatedFeat = await service.UpdateAsync(dto, featId);
        return Ok(mapper.Map<FeatResponseDto>(updatedFeat));
    }

    [HttpDelete("{featId}")]
    public async Task<ActionResult> DeleteFeat(int featId)
    {
        await service.DeleteAsync(featId);
        return Ok();
    }

    // Spell management endpoints
    [HttpPost("{featId}/spells/{spellId}")]
    public async Task<ActionResult<FeatResponseDto>> AddSpell(int featId, int spellId)
    {
        var updatedFeat = await service.AddSpell(spellId, featId);
        return Ok(mapper.Map<FeatResponseDto>(updatedFeat));
    }
    
    [HttpDelete("{featId}/spells/{spellId}")]
    public async Task<ActionResult> RemoveSpell(int featId, int spellId)
    {
        await service.RemoveSpell(spellId, featId);
        return Ok();
    }

    // Proficiency management endpoints
    [HttpPost("{featId}/proficiencies")]
    public async Task<ActionResult<FeatResponseDto>> AddProficiency(int featId, [FromBody] ProficiencyRequestDto proficiency)
    {
        var updatedFeat = await service.AddProficiency(proficiency, featId);
        return Ok(mapper.Map<FeatResponseDto>(updatedFeat));
    }

    [HttpDelete("{featId}/proficiencies")]
    public async Task<ActionResult> RemoveProficiency(int featId, [FromBody] ProficiencyRequestDto proficiency)
    {
        await service.RemoveProficiency(proficiency, featId);
        return Ok();
    }

    // Ability increase management endpoints
    [HttpPost("{featId}/ability-increases")]
    public async Task<ActionResult<FeatResponseDto>> AddAbilityIncrease(int featId, [FromBody] AbilityValueDto increase)
    {
        var updatedFeat = await service.AddAbilityIncrease(increase.AbilityId, increase.Value, featId);
        return Ok(mapper.Map<FeatResponseDto>(updatedFeat));
    }

    [HttpDelete("{featId}/ability-increases")]
    public async Task<ActionResult> RemoveAbilityIncrease(int featId, [FromBody] AbilityValueDto increase)
    {
        await service.RemoveAbilityIncrease(increase.AbilityId, featId);
        return Ok();
    }
}