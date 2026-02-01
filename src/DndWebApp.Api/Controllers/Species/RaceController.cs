using AutoMapper;
using DndWebApp.Api.Models.DTOs.RequestDtos.Character;
using DndWebApp.Api.Models.DTOs.ResponseDtos;
using DndWebApp.Api.Services.Interfaces.Species;
using Microsoft.AspNetCore.Mvc;

namespace DndWebApp.Api.Controllers.Species;

[ApiController]
[Route("api/races")]
public class RaceController(IRaceService service, IMapper mapper) : ControllerBase
    {

    [HttpGet]
    public async Task<ActionResult<ICollection<RaceResponseDto>>> GetRaces()
    {
        var races = await service.GetAllAsync();
        return Ok(mapper.Map<ICollection<RaceResponseDto>>(races));
    }

    [HttpGet("{raceId}")]
    public async Task<ActionResult<RaceResponseDto>> GetRace(int raceId)
    {
        var race = await service.GetByIdAsync(raceId);
        return Ok(mapper.Map<RaceResponseDto>(race));
    }

    [HttpPost]
    public async Task<ActionResult<RaceResponseDto>> CreateRace([FromBody] RaceDto dto)
    {
        var race = await service.CreateAsync(dto);
        return Ok(mapper.Map<RaceResponseDto>(race));
    }

    [HttpPatch("{raceId}")]
    public async Task<ActionResult> UpdateRace(int raceId, [FromBody] RaceDto dto)
    {
        await service.UpdateAsync(raceId, dto);
        return Ok();
    }

    [HttpDelete("{raceId}")]
    public async Task<ActionResult> DeleteRace(int raceId)
    {
        await service.DeleteAsync(raceId);
        return Ok();
    }
}