using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Api.Models.Characters;
using Api.Models.DTOs.RequestDtos.Character;
using Api.Models.DTOs.Features;
using Api.Models.DTOs.ResponseDtos;
using Api.Models.Features;
using Api.Services.Constants;
using Api.Services.Interfaces.Features;
using Api.Services.Util;
using Microsoft.AspNetCore.Mvc;
using Dndtoolkit.Api.Models.DTOs.RequestDtos.Features;

namespace Api.Controllers.Features;

[ApiController]
[Route("api/races/{raceId}/traits")]
public class RaceTraitController(IFeatureService<Trait, CreateTraitRequestDto, UpdateTraitRequestDto> service, IMapper mapper) : ControllerBase
{
    
    [HttpGet]
    public Task<ActionResult<ICollection<TraitResponseDto>>> GetTraits(int raceId, [FromQuery] string? sort = null, [FromQuery] string? order = null)
    {
        throw new NotImplementedException();
    }

    [HttpGet("{traitId}")]
    public async Task<ActionResult<TraitResponseDto>> GetTrait(int raceId, int traitId)
    {
        var trait = await ValidateAndGetTrait(raceId, traitId);
        return Ok(mapper.Map<TraitResponseDto>(trait));
    }

    [HttpPost]
    public async Task<ActionResult<TraitResponseDto>> CreateTrait(int raceId, [FromBody] CreateTraitRequestDto dto)
    {
        var trait = await service.CreateAsync(dto);

        if(dto.RaceId != raceId)
            throw new ValidationException($"Trait race id {dto.RaceId} does not match route race id {raceId}");

        return Ok(mapper.Map<TraitResponseDto>(trait));
    }

    [HttpPatch("{traitId}")]
    public async Task<ActionResult<TraitResponseDto>> UpdateTrait(int raceId, int traitId, [FromBody] UpdateTraitRequestDto dto)
    {
       await EnsureTraitBelongsToRace(raceId, traitId);
        var updatedTrait = await service.UpdateAsync(dto, traitId);
        return Ok(mapper.Map<TraitResponseDto>(updatedTrait));
    }

    [HttpDelete("{traitId}")]
    public async Task<ActionResult> DeleteTrait(int raceId, int traitId)
    {
        await EnsureTraitBelongsToRace(raceId, traitId);
        await service.DeleteAsync(traitId);
        return Ok();
    }

    // Spell management endpoints
    [HttpPost("{traitId}/spells/{spellId}")]
    public async Task<ActionResult<TraitResponseDto>> AddSpell(int raceId, int traitId, int spellId)
    {
        await EnsureTraitBelongsToRace(raceId, traitId);
        var updatedTrait = await service.AddSpell(spellId, traitId);
        return Ok(mapper.Map<TraitResponseDto>(updatedTrait));
    }
    
    [HttpDelete("{traitId}/spells/{spellId}")]
    public async Task<ActionResult> RemoveSpell(int raceId, int traitId, int spellId)
    {
        await EnsureTraitBelongsToRace(raceId, traitId);
        await service.RemoveSpell(spellId, traitId);
        return Ok();
    }

    // Proficiency management endpoints
    [HttpPost("{traitId}/proficiencies")]
    public async Task<ActionResult<TraitResponseDto>> AddProficiency(int raceId, int traitId, [FromBody] ProficiencyRequestDto proficiency)
    {
        await EnsureTraitBelongsToRace(raceId, traitId);
        var updatedTrait = await service.AddProficiency(proficiency, traitId);
        return Ok(mapper.Map<TraitResponseDto>(updatedTrait));
    }

    [HttpDelete("{traitId}/proficiencies")]
    public async Task<ActionResult> RemoveProficiency(int raceId, int traitId, [FromBody] ProficiencyRequestDto proficiency)
    {
        await EnsureTraitBelongsToRace(raceId, traitId);
        await service.RemoveProficiency(proficiency, traitId);
        return Ok();
    }

    // Ability increase management endpoints
    [HttpPost("{traitId}/ability-increases")]
    public async Task<ActionResult<TraitResponseDto>> AddAbilityIncrease(int raceId, int traitId, [FromBody] AbilityValueDto increase)
    {
        await EnsureTraitBelongsToRace(raceId, traitId);
        var updatedTrait = await service.AddAbilityIncrease(increase.AbilityId, increase.Value, traitId);
        return Ok(mapper.Map<TraitResponseDto>(updatedTrait));
    }

    [HttpDelete("{traitId}/ability-increases")]
    public async Task<ActionResult> RemoveAbilityIncrease(int raceId, int traitId, [FromBody] AbilityValueDto increase)
    {
        await EnsureTraitBelongsToRace(raceId, traitId);
        await service.RemoveAbilityIncrease(increase.AbilityId, traitId);
        return Ok();
    }

    //Helpers
    private async Task<Trait> ValidateAndGetTrait(int raceId, int traitId)
    {
        var trait = await service.GetByIdAsync(traitId);

        if(trait.RaceId != raceId)
            throw new ValidationException($"Trait with id {trait.Id} does not belong to race with id {raceId}");
        return trait;
    }

    private async Task EnsureTraitBelongsToRace(int raceId, int traitId)
    {
        var trait = await service.GetByIdAsync(traitId);

        if(trait.RaceId != raceId)
            throw new ValidationException($"Trait with id {trait.Id} does not belong to race with id {raceId}");  
    }
}