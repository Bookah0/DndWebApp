using AutoMapper;
using Api.Middlewares.ExceptionHandling;
using Api.Models.DTOs.RequestDtos.Character;
using Api.Models.DTOs.ResponseDtos;
using Api.Services.Interfaces.Species;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Species;

[ApiController]
[Route("api/races/{raceId}/[controller]")]
public class SubracesController(ISubraceService service, IRaceService raceService, IMapper mapper) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ICollection<SubraceResponseDto>>> GetSubraces(int raceId)
    {
        var parentRace = await raceService.GetWithSubracesAsync(raceId);
        return Ok(mapper.Map<ICollection<SubraceResponseDto>>(parentRace.SubRaces));
    }

    [HttpGet("{subraceId}")]
    public async Task<ActionResult<SubraceResponseDto>> GetSubrace(int raceId, int subraceId)
    {
        await EnsureSubraceBelongsToParentRace(raceId, subraceId);
        var subrace = await service.GetByIdAsync(subraceId);
        return Ok(mapper.Map<SubraceResponseDto>(subrace));
    }

    [HttpPost]
    public async Task<ActionResult<SubraceResponseDto>> CreateSubrace(int raceId, [FromBody] CreateSubraceRequestDto dto)
    {
        if(dto.ParentRaceId != raceId)
            throw new ValidationException($"Subrace parent id {dto.ParentRaceId} does not match route id {raceId}");

        var subrace = await service.CreateAsync(dto);
        return Ok(mapper.Map<SubraceResponseDto>(subrace));
    }

    [HttpPatch("{subraceId}")]
    public async Task<ActionResult<SubraceResponseDto>> UpdateSubrace(int raceId, int subraceId, [FromBody] UpdateSubraceRequestDto dto)
    {
        await EnsureSubraceBelongsToParentRace(raceId, subraceId);
        var updatedSubrace = await service.UpdateAsync(subraceId, dto);
        return Ok(mapper.Map<SubraceResponseDto>(updatedSubrace));
    }

    [HttpDelete("{subraceId}")]
    public async Task<ActionResult> DeleteSubrace(int raceId, int subraceId)
    {
        await EnsureSubraceBelongsToParentRace(raceId, subraceId);
        await service.DeleteAsync(subraceId);
        return Ok();
    }

    private async Task EnsureSubraceBelongsToParentRace(int raceId, int subraceId)
    {
        var parentRace = await raceService.GetWithSubracesAsync(raceId);

        if(!parentRace.SubRaces.Any(sr => sr.Id == subraceId))
            throw new NotFoundException($"Subrace with ID {subraceId} does not belong to Race with ID {raceId}.");
    }
}