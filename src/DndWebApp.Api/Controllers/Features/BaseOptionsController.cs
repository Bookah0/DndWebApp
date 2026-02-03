using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs.Features;
using DndWebApp.Api.Models.DTOs.RequestDtos.Character;
using DndWebApp.Api.Models.Features;
using DndWebApp.Api.Services.Interfaces.Features;
using Microsoft.AspNetCore.Mvc;

namespace DndWebApp.Api.Controllers.Features;

public abstract class BaseOptionsController<F>(IChoiceService<F> choiceService) : ControllerBase where F : AFeature
{
    [HttpPost("ability-increases/choices/{choiceId}/options")] 
    public async Task<ActionResult> AddAbilityIncreaseOption(int featId, int choiceId, [FromBody] AbilityValueDto dto)
    {
        await choiceService.AddAbilityOptions(choiceId, [dto]);
        return Ok();
    }

    [HttpDelete("ability-increases/choices/{choiceId}/options")]
    public async Task<ActionResult> RemoveAbilityIncreaseOption(int featId, int choiceId, [FromBody] ICollection<int> ids)
    {
        await choiceService.RemoveAbilityOptions(choiceId, ids);
        return Ok();
    }

    // SKILLS
    [HttpPost("proficiencies/skills/choices/{choiceId}/options")]
    public async Task<ActionResult> AddSkillProficiencyOption(int featId, int choiceId, [FromBody] ICollection<int> ids)
    {
        await choiceService.AddSkillOptions(choiceId, ids);
        return Ok();
    }

    [HttpDelete("proficiencies/skills/choices/{choiceId}/options")]
    public async Task<ActionResult> RemoveSkillProficiencyOption(int featId, int choiceId, [FromBody] ICollection<int> ids)
    {
        await choiceService.RemoveSkillOptions(choiceId, ids);
        return Ok();
    }

    // LANGUAGES
    [HttpPost("proficiencies/languages/choices/{choiceId}/options")]
    public async Task<ActionResult> AddLanguageOption(int featId, int choiceId, [FromBody] ICollection<int> ids)
    {
        await choiceService.AddLanguageOptions(choiceId, ids);
        return Ok();
    }

    [HttpDelete("proficiencies/languages/choices/{choiceId}/options")]
    public async Task<ActionResult> RemoveLanguageOption(int featId, int choiceId, [FromBody] ICollection<int> ids)
    {
        await choiceService.RemoveLanguageOptions(choiceId, ids);
        return Ok();
    }

    // TOOLS
    [HttpPost("proficiencies/tools/choices/{choiceId}/options")]
    public async Task<ActionResult> AddToolProficiencyOption(int featId, int choiceId, [FromBody] ICollection<string> categories)
    {
        await choiceService.AddToolCategoryOptions(choiceId, categories);
        return Ok();
    }

    [HttpDelete("proficiencies/tools/choices/{choiceId}/options")]
    public async Task<ActionResult> RemoveToolProficiencyOption(int featId, int choiceId, [FromBody] ICollection<string> categories)
    {
        await choiceService.RemoveToolCategoryOptions(choiceId, categories);
        return Ok();
    }

    // WEAPONS
    [HttpPost("proficiencies/weapon-types/choices/{choiceId}/options")]
    public async Task<ActionResult> AddWeaponTypeProficiencyOption(int featId, int choiceId, [FromBody] ICollection<string> types)
    {
        await choiceService.AddWeaponTypeOptions(choiceId, types);
        return Ok();
    }

    [HttpDelete("proficiencies/weapon-types/choices/{choiceId}/options")]
    public async Task<ActionResult> RemoveWeaponTypeProficiencyOption(int featId, int choiceId, [FromBody] ICollection<string> types)
    {
        await choiceService.RemoveWeaponTypeOptions(choiceId, types);
        return Ok();
    }

    [HttpPost("proficiencies/weapon-categories/choices/{choiceId}/options")]
    public async Task<ActionResult> AddWeaponCategoryProficiencyOption(int featId, int choiceId, [FromBody] ICollection<string> categories)
    {
        await choiceService.AddWeaponCategoryOptions(choiceId, categories);
        return Ok();
    }

    [HttpDelete("proficiencies/weapon-categories/choices/{choiceId}/options")]
    public async Task<ActionResult> RemoveWeaponCategoryProficiencyOption(int featId, int choiceId, [FromBody] ICollection<string> categories)
    {
        await choiceService.RemoveWeaponCategoryOptions(choiceId, categories);
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