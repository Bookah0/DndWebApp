using System.ComponentModel.DataAnnotations;
using AutoMapper;
using DndWebApp.Api.Models.DTOs.RequestDtos.Character;
using DndWebApp.Api.Models.DTOs.ResponseDtos;
using DndWebApp.Api.Models.DTOs.Spells;
using DndWebApp.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DndWebApp.Api.Controllers.Classes;

[ApiController]
[Route("api/classes")]
public class ClassController(IClassService service, IMapper mapper) : ControllerBase
{

    [HttpGet]
    public async Task<ActionResult<ICollection<ClassResponseDto>>> GetClasses()
    {
        var classes = await service.GetAllAsync();
        return Ok(mapper.Map<ICollection<ClassResponseDto>>(classes));
    }

    [HttpGet("{classId}")]
    public async Task<ActionResult<ClassResponseDto>> GetClass(int classId)
    {
        var clss = await service.GetByIdAsync(classId);
        return Ok(mapper.Map<ClassResponseDto>(clss));
    }

    [HttpPost]
    public async Task<ActionResult<ClassResponseDto>> CreateClass([FromBody] ClassDto dto)
    {
        var clss = await service.CreateAsync(dto);
        return Ok(mapper.Map<ClassResponseDto>(clss));
    }

    [HttpPatch("{classId}")]
    public async Task<ActionResult> UpdateClass(int classId, [FromBody] ClassDto dto)
    {
        await service.UpdateAsync(classId, dto);
        return Ok();
    }

    [HttpDelete("{classId}")]
    public async Task<ActionResult> DeleteClass(int classId)
    {
        await service.DeleteAsync(classId);
        return Ok();
    }
}