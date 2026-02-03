using AutoMapper;
using DndWebApp.Api.Models.DTOs.RequestDtos.Character;
using DndWebApp.Api.Models.DTOs.Features;
using DndWebApp.Api.Models.DTOs.ResponseDtos;
using DndWebApp.Api.Services.Interfaces.Features;
using Microsoft.AspNetCore.Mvc;
using DndWebApp.Api.Models.Features;
using DndWebApp.Api.Models.Items;

namespace DndWebApp.Api.Controllers;

[ApiController]
[Route("api/feats/{featId}/")]
public class FeatChoiceController(IChoiceService<Feat> choiceService) : ControllerBase
{
    [HttpPost("ability-increases/choices")]
    public async Task<ActionResult> AddAbilityIncreaseChoice(int featId, AbilityIncreaseChoiceDto dto)
    {
        await choiceService.AddChoice(dto, featId);
        return Ok();
    }

    [HttpDelete("ability-increases/choices/{choiceId}")]
    public async Task<ActionResult> RemoveAbilityIncreaseChoice(int featId, int choiceId)
    {
        await choiceService.RemoveChoice<AbilityIncreaseChoice>(choiceId, featId);
        return Ok();
    }

    // Skill proficiency management endpoints
    [HttpPost("proficiencies/skills/choices")]
    public async Task<ActionResult> AddSkillProficiencyChoice(int featId, SkillProficiencyChoiceDto dto)
    {
        await choiceService.AddChoice(dto, featId);
        return Ok();
    }

    [HttpDelete("proficiencies/skills/choices/{choiceId}")]
    public async Task<ActionResult> RemoveSkillProficiencyChoice(int featId, int choiceId)
    {
        await choiceService.RemoveChoice<SkillProficiencyChoice>(choiceId, featId);
        return Ok();
    }

    // Language proficiency management endpoints
    [HttpPost("proficiencies/languages/choices")]
    public async Task<ActionResult> AddLanguageChoice(int featId, LanguageProficiencyChoiceDto dto)
    {
        await choiceService.AddChoice(dto, featId);
        return Ok();
    }

    [HttpDelete("proficiencies/languages/choices/{choiceId}")]
    public async Task<ActionResult> RemoveLanguageChoice(int featId, int choiceId)
    {
        await choiceService.RemoveChoice<LanguageChoice>(choiceId, featId);
        return Ok();
    }

    // Tool proficiency management endpoints
    [HttpPost("proficiencies/tools/choices")]
    public async Task<ActionResult> AddToolProficiencyChoice(int featId, ToolProficiencyChoiceDto dto)
    {
        await choiceService.AddChoice(dto, featId);
        return Ok();
    }

    [HttpDelete("proficiencies/tools/choices/{choiceId}")]
    public async Task<ActionResult> RemoveToolProficiencyChoice(int featId, int choiceId)
    {
        await choiceService.RemoveChoice<ToolProficiencyChoice>(choiceId, featId);
        return Ok();
    }

    // Weapon type proficiency management endpoints
    [HttpPost("proficiencies/weapon-types/choices")]
    public async Task<ActionResult> AddWeaponTypeProficiencyChoice(int featId, WeaponTypeProficiencyChoiceDto dto)
    {
        await choiceService.AddChoice(dto, featId);
        return Ok();
    }

    [HttpDelete("proficiencies/weapon-types/choices/{choiceId}")]
    public async Task<ActionResult> RemoveWeaponTypeProficiencyChoice(int featId, int choiceId)
    {
        await choiceService.RemoveChoice<WeaponTypeProficiencyChoice>(choiceId, featId);
        return Ok();
    }

    // Weapon category proficiency management endpoints
    [HttpPost("proficiencies/weapon-categories/choices")]
    public async Task<ActionResult> AddWeaponCategoryProficiencyChoice(int featId, WeaponCategoryProficiencyChoiceDto dto)
    {
        await choiceService.AddChoice(dto, featId);
        return Ok();
    }

    [HttpDelete("proficiencies/weapon-categories/choices/{choiceId}")]
    public async Task<ActionResult> RemoveWeaponCategoryProficiencyChoice(int featId, int choiceId)
    {
        await choiceService.RemoveChoice<WeaponCategoryProficiencyChoice>(choiceId, featId);
        return Ok();
    }

    [HttpDelete("choices")]
    public async Task<ActionResult> ClearChoices(int featId)
    {
        await choiceService.ClearChoices<SkillProficiencyChoice>(featId);
        await choiceService.ClearChoices<LanguageChoice>(featId);
        await choiceService.ClearChoices<WeaponCategoryProficiencyChoice>(featId);
        await choiceService.ClearChoices<WeaponTypeProficiencyChoice>(featId);
        await choiceService.ClearChoices<ToolProficiencyChoice>(featId);
        await choiceService.ClearChoices<AbilityIncreaseChoice>(featId);
        await choiceService.ClearChoices<ArmorProficiencyChoice>(featId);
        return Ok();
    }
}