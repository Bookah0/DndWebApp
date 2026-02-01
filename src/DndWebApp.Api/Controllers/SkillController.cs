using AutoMapper;
using DndWebApp.Api.Models.DTOs.RequestDtos.Character;
using DndWebApp.Api.Models.DTOs.ResponseDtos;
using DndWebApp.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DndWebApp.Api.Controllers;

[ApiController]
[Route("api/[controller]s")]
public class SkillController(ISkillService service, IMapper mapper) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ICollection<SkillResponseDto>>> GetSkills()
    {
        var skills = await service.GetAllAsync();
        return Ok(mapper.Map<ICollection<SkillResponseDto>>(skills));
    }

    [HttpGet("{skillId}")]
    public async Task<ActionResult<SkillResponseDto>> GetSkill(int skillId)
    {
        var skill = await service.GetByIdAsync(skillId);
        return Ok(mapper.Map<SkillResponseDto>(skill));
    }

    [HttpPost]
    public async Task<ActionResult<SkillResponseDto>> CreateSkill([FromBody] SkillDto dto)
    {
        var skill = await service.CreateAsync(dto);
        return Ok(mapper.Map<SkillResponseDto>(skill));
    }

    [HttpPatch("{skillId}")]
    public async Task<ActionResult> UpdateSkill(int skillId, [FromBody] SkillDto dto)
    {
        await service.UpdateAsync(skillId, dto);
        return Ok();
    }

    [HttpDelete("{skillId}")]
    public async Task<ActionResult> DeleteSkill(int skillId)
    {
        await service.DeleteAsync(skillId);
        return Ok();
    }
}