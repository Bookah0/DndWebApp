using AutoMapper;
using DndWebApp.Api.Models.DTOs.RequestDtos.Character;
using DndWebApp.Api.Models.DTOs.ResponseDtos;
using DndWebApp.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DndWebApp.Api.Controllers;

[ApiController]
[Route("api/[controller]s")]
public class AlignmentController(IAlignmentService service, IMapper mapper) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ICollection<AlignmentResponseDto>>> GetAlignments()
    {
        var alignments = await service.GetAllAsync();
        return Ok(mapper.Map<ICollection<AlignmentResponseDto>>(alignments));
    }

    [HttpGet("{alignmentId}")]
    public async Task<ActionResult<AlignmentResponseDto>> GetAlignment(int alignmentId)
    {
        var alignment = await service.GetByIdAsync(alignmentId);
        return Ok(mapper.Map<AlignmentResponseDto>(alignment));
    }
}