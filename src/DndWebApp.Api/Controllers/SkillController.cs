using DndWebApp.Api.Models.DTOs.Character;
using DndWebApp.Api.Models.DTOs.ResponseDtos;
using DndWebApp.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DndWebApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SkillController : ControllerBase
    {
    public ISkillService service;

    public SkillController(ISkillService service)
    {
        this.service = service;
    }
    
    [HttpGet]
    public async Task<ActionResult<ICollection<SkillResponseDto>>> GetSkills()
    {
        var skills = await service.GetAllAsync();
        return Ok(skills);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<SkillResponseDto>> GetSkill(int id)
    {
        var skill = await service.GetByIdAsync(id);
        return Ok(skill);
    }

    [HttpPost]
    public async Task<ActionResult<SkillResponseDto>> CreateSkill([FromBody] SkillDto dto)
    {
        var skill = await service.CreateAsync(dto);
        return Ok(skill);
    }

    [HttpPatch("{id}")]
    public async Task<ActionResult> UpdateSkill(int id, [FromBody] SkillDto dto)
    {
        await service.UpdateAsync(id, dto);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteSkill(int id)
    {
        await service.DeleteAsync(id);
        return Ok();
    }
}