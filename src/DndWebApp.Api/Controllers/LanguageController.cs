using DndWebApp.Api.Models.DTOs.Character;
using DndWebApp.Api.Models.DTOs.ResponseDtos;
using DndWebApp.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DndWebApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LanguageController : ControllerBase
{
    public ILanguageService service;

    public LanguageController(ILanguageService service)
    {
        this.service = service;
    }
    
    [HttpGet]
    public async Task<ActionResult<ICollection<LanguageResponseDto>>> GetLanguages()
    {
        var languages = await service.GetAllAsync();
        return Ok(languages);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<LanguageResponseDto>> GetLanguage(int id)
    {
        var language = await service.GetByIdAsync(id);
        return Ok(language);
    }

    [HttpPost]
    public async Task<ActionResult<LanguageResponseDto>> CreateLanguage([FromBody] LanguageDto dto)
    {
        var language = await service.CreateAsync(dto);
        return Ok(language);
    }

    [HttpPatch("{id}")]
    public async Task<ActionResult> UpdateLanguage(int id, [FromBody] LanguageDto dto)
    {
        await service.UpdateAsync(id, dto);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteLanguage(int id)
    {
        await service.DeleteAsync(id);
        return Ok();
    }
}