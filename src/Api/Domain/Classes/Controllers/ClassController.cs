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
    public async Task<ActionResult<ICollection<ClassResponseDto>>> GetClasses([FromQuery] ClassFilterDto filter, [FromQuery] PaginationRequestDto pagination)
    {
        var (totalCount, classes) = await service.GetFilteredAsync(filter, pagination);
        
        return Ok(new PaginationResponseDto<ClassResponseDto>
        {
            Items = mapper.Map<ICollection<ClassResponseDto>>(classes),
            ItemCount = totalCount,
            Page = pagination.Page,
            PageSize = pagination.PageSize,
            Next = PaginationUtil.GetNext(pagination, totalCount, "api/classes"),
            Prev = PaginationUtil.GetPrev(pagination, "api/classes")
        });
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