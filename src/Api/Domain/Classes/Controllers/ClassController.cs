using Api.Domain.Classes.DTOs;
using Api.Domain.Classes.Services;
using Api.Domain.Shared.DTOs;
using Api.Domain.Shared.Utils;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Api.Domain.Classes.Controllers;

[ApiController]
[Route("api/classes")]
public class ClassController(IBaseClassService service, IMapper mapper) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PaginationResponseDto<ClassResponseDto>>> GetClasses([FromQuery] ClassFilterDto? filter, [FromQuery] PaginationRequestDto? pagination)
    {
        var classes = await service.GetAllAsync(filter, pagination);
        var mappedClasses = mapper.Map<ICollection<ClassResponseDto>>(classes);
        
		return Ok(PaginationUtil.BuildPaginationResponse(mappedClasses, pagination, "api/classes"));
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