using Api.Domain.Abilities.DTOs;
using Api.Domain.Shared.DTOs;
using Api.Domain.Shared.Services;
using Api.Domain.Species.Models;
using Api.Domain.Species.Services;
using Api.Infrastructure.Middleware.ExceptionHandling;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Api.Domain.Species.Controllers;

[ApiController]
[Route("api/races/{raceId}/subraces/{subraceId}/traits")]
public class SubraceTraitController(IFeatureService<Trait, CreateTraitRequestDto, UpdateTraitRequestDto, TraitFilterDto> service, ISubraceService subraceService, IRaceService raceService, IMapper mapper) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ICollection<TraitResponseDto>>> GetTraits(int raceId, int subraceId)
    {
        await EnsureSubraceBelongsToParentRace(raceId, subraceId);
        var subrace = await subraceService.GetWithTraitsAsync(subraceId);
        return Ok(mapper.Map<ICollection<TraitResponseDto>>(subrace.Traits));
    }

    [HttpGet("{traitId}")]
    public async Task<ActionResult<TraitResponseDto>> GetTrait(int traitId, int raceId, int subraceId)
    {
        await EnsureSubraceBelongsToParentRace(raceId, subraceId);
        var trait = await service.GetByIdAsync(traitId);

        if(trait.RaceId != subraceId)
            throw new ValidationException($"Trait with id {trait.Id} does not belong to subrace with id {subraceId}");

        return Ok(mapper.Map<TraitResponseDto>(trait));
    }

    [HttpPost]
    public async Task<ActionResult<TraitResponseDto>> CreateTrait(int raceId, int subraceId, [FromBody] CreateTraitRequestDto dto)
    {
        await EnsureSubraceBelongsToParentRace(raceId, subraceId);

        var trait = await service.CreateAsync(dto);
        return Ok(mapper.Map<TraitResponseDto>(trait));
    }

    [HttpPatch("{traitId}")]
    public async Task<ActionResult<TraitResponseDto>> UpdateTrait(int raceId, int subraceId, int traitId, [FromBody] UpdateTraitRequestDto dto)
    {
        await EnsureSubraceBelongsToParentRace(raceId, subraceId);
        await EnsureTraitBelongsToSubrace(subraceId, traitId);

        var updatedTrait = await service.UpdateAsync(dto, traitId);
        return Ok(mapper.Map<TraitResponseDto>(updatedTrait));
    }

    [HttpDelete("{traitId}")]
    public async Task<ActionResult> DeleteTrait(int raceId, int subraceId, int traitId)
    {
        await EnsureSubraceBelongsToParentRace(raceId, subraceId);
        await EnsureTraitBelongsToSubrace(subraceId, traitId);

        await service.DeleteAsync(traitId);
        return Ok();
    }

        // Spell management endpoints
    [HttpPost("{traitId}/spells/{spellId}")]
    public async Task<ActionResult<TraitResponseDto>> AddSpell(int raceId, int subraceId, int traitId, int spellId)
    {
        await EnsureSubraceBelongsToParentRace(raceId, subraceId);
        await EnsureTraitBelongsToSubrace(subraceId, traitId);
        var updatedTrait = await service.AddSpell(spellId, traitId);
        return Ok(mapper.Map<TraitResponseDto>(updatedTrait));
    }
    
    [HttpDelete("{traitId}/spells/{spellId}")]
    public async Task<ActionResult> RemoveSpell(int raceId, int subraceId, int traitId, int spellId)
    {
        await EnsureSubraceBelongsToParentRace(raceId, subraceId);
        await EnsureTraitBelongsToSubrace(subraceId, traitId);
        await service.RemoveSpell(spellId, traitId);
        return Ok();
    }

    // Proficiency management endpoints
    [HttpPost("{traitId}/proficiencies")]
    public async Task<ActionResult<TraitResponseDto>> AddProficiency(int raceId, int subraceId, int traitId, [FromBody] ProficiencyRequestDto proficiency)
    {
        await EnsureSubraceBelongsToParentRace(raceId, subraceId);
        await EnsureTraitBelongsToSubrace(subraceId, traitId);
        var updatedTrait = await service.AddProficiency(proficiency, traitId);
        return Ok(mapper.Map<TraitResponseDto>(updatedTrait));
    }

    [HttpDelete("{traitId}/proficiencies")]
    public async Task<ActionResult> RemoveProficiency(int raceId, int subraceId, int traitId, [FromBody] ProficiencyRequestDto proficiency)
    {
        await EnsureSubraceBelongsToParentRace(raceId, subraceId);
        await EnsureTraitBelongsToSubrace(subraceId, traitId);
        await service.RemoveProficiency(proficiency, traitId);
        return Ok();
    }

    // Ability increase management endpoints
    [HttpPost("{traitId}/ability-increases")]
    public async Task<ActionResult<TraitResponseDto>> AddAbilityIncrease(int raceId, int subraceId, int traitId, [FromBody] AbilityValueDto increase)
    {
        await EnsureSubraceBelongsToParentRace(raceId, subraceId);
        await EnsureTraitBelongsToSubrace(subraceId, traitId);
        var updatedTrait = await service.AddAbilityIncrease(increase.AbilityId, increase.Value, traitId);
        return Ok(mapper.Map<TraitResponseDto>(updatedTrait));
    }

    [HttpDelete("{traitId}/ability-increases")]
    public async Task<ActionResult> RemoveAbilityIncrease(int raceId, int subraceId, int traitId, [FromBody] AbilityValueDto increase)
    {
        await EnsureSubraceBelongsToParentRace(raceId, subraceId);
        await EnsureTraitBelongsToSubrace(subraceId, traitId);
        await service.RemoveAbilityIncrease(increase.AbilityId, traitId);
        return Ok();
    }

    private async Task EnsureSubraceBelongsToParentRace(int raceId, int subraceId)
    {
        var parentRace = await raceService.GetWithSubracesAsync(raceId);

        if(!parentRace.SubRaces.Any(sr => sr.Id == subraceId))
            throw new NotFoundException($"Subrace with ID {subraceId} does not belong to Race with ID {raceId}.");
    }

    private async Task EnsureTraitBelongsToSubrace(int subraceId, int traitId)
    {
        var trait = await service.GetByIdAsync(traitId);

        if(trait.RaceId != subraceId)
            throw new ValidationException($"Trait with id {trait.Id} does not belong to race with id {subraceId}");  
    }
}