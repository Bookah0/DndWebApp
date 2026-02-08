using AutoMapper;
using Api.Models.DTOs.RequestDtos.Character;
using Api.Models.DTOs.ResponseDtos;
using Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SkillsController(ISkillService service, IMapper mapper) : ControllerBase
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
    public async Task<ActionResult<SkillResponseDto>> CreateSkill([FromBody] CreateSkillRequestDto dto)
    {
        var skill = await service.CreateAsync(dto);
        return Ok(mapper.Map<SkillResponseDto>(skill));
    }

    [HttpPatch("{skillId}")]
    public async Task<ActionResult<SkillResponseDto>> UpdateSkill(int skillId, [FromBody] UpdateSkillRequestDto dto)
    {
        var updated = await service.UpdateAsync(skillId, dto);
        return Ok(mapper.Map<SkillResponseDto>(updated));
    }

    [HttpDelete("{skillId}")]
    public async Task<ActionResult> DeleteSkill(int skillId)
    {
        await service.DeleteAsync(skillId);
        return Ok();
    }
}