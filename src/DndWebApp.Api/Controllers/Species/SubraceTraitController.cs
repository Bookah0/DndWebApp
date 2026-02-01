using AutoMapper;
using DndWebApp.Api.Models.DTOs.RequestDtos.Character;
using DndWebApp.Api.Models.DTOs.Features;
using DndWebApp.Api.Models.DTOs.ResponseDtos;
using DndWebApp.Api.Services.Constants;
using DndWebApp.Api.Services.Interfaces.Features;
using DndWebApp.Api.Services.Util;
using Microsoft.AspNetCore.Mvc;

namespace DndWebApp.Api.Controllers.Features;

[ApiController]
[Route("api/races/{raceId}/subraces/{subraceId}/traits")]
public class SubraceTraitController(ITraitService service, IMapper mapper) : ControllerBase
{
    
    [HttpGet]
    public Task<ActionResult<ICollection<TraitResponseDto>>> GetTraits(int raceId, int subraceId, [FromQuery] string? sort = null, [FromQuery] string? order = null)
    {
        throw new NotImplementedException();
    }

    [HttpPost]
    public Task<ActionResult<TraitResponseDto>> AddTrait(int raceId, int subraceId, [FromBody] TraitDto dto)
    {
        throw new NotImplementedException();
    }

    [HttpPatch("{traitId}")]
    public Task<ActionResult> RemoveTrait(int raceId, int subraceId, int traitId, [FromBody] TraitDto dto)
    {
        throw new NotImplementedException();
    }
}