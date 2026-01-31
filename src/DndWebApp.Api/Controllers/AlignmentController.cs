using DndWebApp.Api.Models.DTOs.Character;
using DndWebApp.Api.Models.DTOs.ResponseDtos;
using DndWebApp.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DndWebApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AlignmentController : ControllerBase
{
    public IAlignmentService service;

    public AlignmentController(IAlignmentService service)
    {
        this.service = service;
    }
    
    [HttpGet]
    public async Task<ActionResult<ICollection<AlignmentResponseDto>>> GetAlignments()
    {
        var alignments = await service.GetAllAsync();
        return Ok(alignments);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AlignmentResponseDto>> GetAlignment(int id)
    {
        var alignment = await service.GetByIdAsync(id);
        return Ok(alignment);
    }
}