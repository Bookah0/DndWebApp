using AutoMapper;
using DndWebApp.Api.Models.DTOs.Features;
using DndWebApp.Api.Models.DTOs.ResponseDtos;
using DndWebApp.Api.Services.Interfaces.Features;
using Microsoft.AspNetCore.Mvc;
using DndWebApp.Api.Middlewares.ExceptionHandling;
using DndWebApp.Api.Services.Interfaces.Species;

namespace DndWebApp.Api.Controllers.Features;

[ApiController]
[Route("api/races/{raceId}/subraces/{subraceId}/traits")]
public class SubraceTraitController(ITraitService service, ISubraceService subraceService, IRaceService raceService, IMapper mapper) : ControllerBase
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
    public async Task<ActionResult<TraitResponseDto>> CreateTrait(int raceId, int subraceId, [FromBody] TraitDto dto)
    {
        await EnsureSubraceBelongsToParentRace(raceId, subraceId);

        var trait = await service.CreateAsync(dto);
        return Ok(mapper.Map<TraitResponseDto>(trait));
    }

    [HttpPatch("{traitId}")]
    public async Task<ActionResult> UpdateTrait(int raceId, int subraceId, int traitId, [FromBody] TraitDto dto)
    {
        await EnsureSubraceBelongsToParentRace(raceId, subraceId);
        await EnsureTraitBelongsToSubrace(raceId, traitId);

        await service.UpdateAsync(dto, traitId);
        return Ok();
    }

    [HttpDelete("{traitId}")]
    public async Task<ActionResult> DeleteTrait(int raceId, int subraceId, int traitId)
    {
        await EnsureSubraceBelongsToParentRace(raceId, subraceId);
        await EnsureTraitBelongsToSubrace(raceId, traitId);

        await service.DeleteAsync(traitId);
        return Ok();
    }

    private async Task EnsureSubraceBelongsToParentRace(int raceId, int subraceId)
    {
        var parentRace = await raceService.GetWithSubracesAsync(raceId);

        if(!parentRace.SubRaces.Any(sr => sr.Id == subraceId))
            throw new NotFoundException($"Subrace with ID {subraceId} does not belong to Race with ID {raceId}.");
    }

    private async Task EnsureTraitBelongsToSubrace(int raceId, int traitId)
    {
        var trait = await service.GetByIdAsync(traitId);

        if(trait.RaceId != raceId)
            throw new ValidationException($"Trait with id {trait.Id} does not belong to race with id {raceId}");  
    }
}