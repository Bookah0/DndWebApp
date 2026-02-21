using Api.Domain.Classes.DTOs;
using Api.Domain.Classes.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Api.Domain.Classes.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClassesController(IBaseClassService service, IMapper mapper) : ControllerBase
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