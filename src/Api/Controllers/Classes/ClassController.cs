using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Api.Models.DTOs.RequestDtos.Character;
using Api.Models.DTOs.ResponseDtos;
using Api.Models.DTOs.Spells;
using Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Classes;

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
    public async Task<ActionResult<ClassResponseDto>> CreateClass([FromBody] CreateClassRequestDto dto)
    {
        var clss = await service.CreateAsync(dto);
        return Ok(mapper.Map<ClassResponseDto>(clss));
    }

    [HttpPatch("{classId}")]
    public async Task<ActionResult<ClassResponseDto>> UpdateClass(int classId, [FromBody] UpdateClassRequestDto dto)
    {
        var updatedClass = await service.UpdateAsync(classId, dto);
        return Ok(mapper.Map<ClassResponseDto>(updatedClass));
    }

    [HttpDelete("{classId}")]
    public async Task<ActionResult> DeleteClass(int classId)
    {
        await service.DeleteAsync(classId);
        return Ok();
    }
}