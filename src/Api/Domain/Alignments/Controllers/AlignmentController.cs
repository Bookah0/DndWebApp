using Api.Domain.Alignments.DTOs;
using Api.Domain.Alignments.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Api.Domain.Alignments.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AlignmentsController(IAlignmentService service, IMapper mapper) : ControllerBase
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