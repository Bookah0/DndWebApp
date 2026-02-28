using Api.Domain.Classes.Models;
using Api.Domain.Shared.DTOs;
using Api.Domain.Shared.Models;
using Api.Domain.Shared.Services;
using Microsoft.AspNetCore.Mvc;
using Api.Domain.Species.Models;
using Api.Domain.Backgrounds.Models;
using Api.Domain.Feats.Models;
using Api.Infrastructure.Middleware.ExceptionHandling;

namespace Api.Domain.Shared.Controllers;


[ApiController]
[Route("api/races/{raceId}/traits/{traitId}")]
public class RaceTraitChoicesController(
		IChoiceService<Trait> service,
		IFeatureService<Trait, CreateTraitRequestDto, UpdateTraitRequestDto, TraitFilterDto> featureService) 
		: ChoicesControllerBase<Trait, CreateTraitRequestDto, UpdateTraitRequestDto, TraitFilterDto>(service, featureService)
{ }

[ApiController]
[Route("api/races/{raceId}/subraces/{subraceId}/traits/{traitId}")]
public class SubraceTraitChoicesController(
		IChoiceService<Trait> service,
		IFeatureService<Trait, CreateTraitRequestDto, UpdateTraitRequestDto, TraitFilterDto> featureService) 
		: ChoicesControllerBase<Trait, CreateTraitRequestDto, UpdateTraitRequestDto, TraitFilterDto>(service, featureService)
{ }

[ApiController]
[Route("api/classes/{classId}/features/{featureId}")]
public class ClassChoicesController(
		IChoiceService<ClassFeature> service,
		IFeatureService<ClassFeature, CreateClassFeatureRequestDto, UpdateClassFeatureRequestDto, ClassFeatureFilterDto> featureService) 
		: ChoicesControllerBase<ClassFeature, CreateClassFeatureRequestDto, UpdateClassFeatureRequestDto, ClassFeatureFilterDto>(service, featureService)
{ }

[ApiController]
[Route("api/classes/{classId}/subclasses/{subclassId}/features/{featureId}")]
public class SubclassClassChoicesController(
		IChoiceService<ClassFeature> service,
		IFeatureService<ClassFeature, CreateClassFeatureRequestDto, UpdateClassFeatureRequestDto, ClassFeatureFilterDto> featureService) 
		: ChoicesControllerBase<ClassFeature, CreateClassFeatureRequestDto, UpdateClassFeatureRequestDto, ClassFeatureFilterDto>(service, featureService)
{ }

[ApiController]
[Route("api/backgrounds/{backgroundId}/features/{featureId}")]
public class BackgroundChoicesController(
		IChoiceService<BackgroundFeature> service,
		IFeatureService<BackgroundFeature, CreateBackgroundFeatureRequestDto, UpdateBackgroundFeatureRequestDto, BackgroundFeatureFilterDto> featureService) 
		: ChoicesControllerBase<BackgroundFeature, CreateBackgroundFeatureRequestDto, UpdateBackgroundFeatureRequestDto, BackgroundFeatureFilterDto>(service, featureService)
{ }

[ApiController]
[Route("api/feats/{featId}")]
public class FeatChoicesController(
		IChoiceService<Feat> service,
		IFeatureService<Feat, CreateFeatRequestDto, UpdateFeatRequestDto, FeatFilterDto> featureService) 
		: ChoicesControllerBase<Feat, CreateFeatRequestDto, UpdateFeatRequestDto, FeatFilterDto>(service, featureService)
{ }

public abstract class ChoicesControllerBase<F, CD, UD, FF>(IChoiceService<F> choiceService, IFeatureService<F, CD, UD, FF> featureService) : ControllerBase where F : Feature where CD : CreateFeatureRequestDto where UD : UpdateFeatureRequestDto where FF : FeatureFilterDto
{
    [HttpPost("ability-increases/choices")] 
    public async Task<ActionResult> AddAbilityIncreaseChoice(int featureId, AbilityIncreaseChoiceDto dto)
    {
        await choiceService.AddChoice(dto, featureId);
        return Ok();
    }

    [HttpDelete("ability-increases/choices/{choiceId}")]
    public async Task<ActionResult> RemoveAbilityIncreaseChoice(int featureId, int choiceId)
    {
        await EnsureChoiceBelongsToFeature(featureId, choiceId);
        await choiceService.RemoveChoice<AbilityIncreaseChoice>(choiceId, featureId);
        return Ok();
    }

    [HttpDelete("ability-increases/choices")] 
    public async Task<ActionResult> ClearAbilityIncreaseChoices(int featureId, AbilityIncreaseChoiceDto dto)
    {
        await choiceService.ClearChoices<AbilityIncreaseChoice>(featureId);
        return Ok();
    }

    // SKILLS
    [HttpPost("proficiencies/skills/choices")]
    public async Task<ActionResult> AddSkillProficiencyChoice(int featureId, SkillProficiencyChoiceDto dto)
    {
        await choiceService.AddChoice(dto, featureId);
        return Ok();
    }

    [HttpDelete("proficiencies/skills/choices/{choiceId}")]
    public async Task<ActionResult> RemoveSkillProficiencyChoice(int featureId, int choiceId)
    {
        await EnsureChoiceBelongsToFeature(featureId, choiceId);
        await choiceService.RemoveChoice<SkillProficiencyChoice>(choiceId, featureId);
        return Ok();
    }

    [HttpDelete("proficiencies/skills/choices")] 
    public async Task<ActionResult> ClearSkillProficiencyChoices(int featureId)
    {
        await choiceService.ClearChoices<SkillProficiencyChoice>(featureId);
        return Ok();
    }

    // LANGUAGES
    [HttpPost("proficiencies/languages/choices")]
    public async Task<ActionResult> AddLanguageChoice(int featureId, LanguageProficiencyChoiceDto dto)
    {
        await choiceService.AddChoice(dto, featureId);
        return Ok();
    }

    [HttpDelete("proficiencies/languages/choices/{choiceId}")]
    public async Task<ActionResult> RemoveLanguageChoice(int featureId, int choiceId)
    {
        await EnsureChoiceBelongsToFeature(featureId, choiceId);
        await choiceService.RemoveChoice<LanguageChoice>(choiceId, featureId);
        return Ok();
    }

    [HttpDelete("proficiencies/languages/choices")] 
    public async Task<ActionResult> ClearLanguageChoices(int featureId)
    {
        await choiceService.ClearChoices<LanguageChoice>(featureId);
        return Ok();
    }

    // TOOLS
    [HttpPost("proficiencies/tools/choices")]
    public async Task<ActionResult> AddToolProficiencyChoice(int featureId, ToolProficiencyChoiceDto dto)
    {
        await choiceService.AddChoice(dto, featureId);
        return Ok();
    }

    [HttpDelete("proficiencies/tools/choices/{choiceId}")]
    public async Task<ActionResult> RemoveToolProficiencyChoice(int featureId, int choiceId)
    {
        await EnsureChoiceBelongsToFeature(featureId, choiceId);
        await choiceService.RemoveChoice<ToolProficiencyChoice>(choiceId, featureId);
        return Ok();
    }

    [HttpDelete("proficiencies/tools/choices")] 
    public async Task<ActionResult> ClearToolProficiencyChoices(int featureId)
    {
        await choiceService.ClearChoices<ToolProficiencyChoice>(featureId);
        return Ok();
    }

    // WEAPONS
    [HttpPost("proficiencies/weapon-types/choices")]
    public async Task<ActionResult> AddWeaponTypeProficiencyChoice(int featureId, WeaponTypeProficiencyChoiceDto dto)
    {
        await choiceService.AddChoice(dto, featureId);
        return Ok();
    }

    [HttpDelete("proficiencies/weapon-types/choices/{choiceId}")]
    public async Task<ActionResult> RemoveWeaponTypeProficiencyChoice(int featureId, int choiceId)
    {
        await EnsureChoiceBelongsToFeature(featureId, choiceId);
        await choiceService.RemoveChoice<WeaponTypeProficiencyChoice>(choiceId, featureId);
        return Ok();
    }

    [HttpDelete("proficiencies/weapon-types/choices")] 
    public async Task<ActionResult> ClearWeaponTypeProficiencyChoices(int featureId)
    {
        await choiceService.ClearChoices<WeaponTypeProficiencyChoice>(featureId);
        return Ok();
    }

    [HttpPost("proficiencies/weapon-categories/choices")]
    public async Task<ActionResult> AddWeaponCategoryProficiencyChoice(int featureId, WeaponCategoryProficiencyChoiceDto dto)
    {
        await choiceService.AddChoice(dto, featureId);
        return Ok();
    }

    [HttpDelete("proficiencies/weapon-categories/choices/{choiceId}")]
    public async Task<ActionResult> RemoveWeaponCategoryProficiencyChoice(int featureId, int choiceId)
    {
        await EnsureChoiceBelongsToFeature(featureId, choiceId);
        await choiceService.RemoveChoice<WeaponCategoryProficiencyChoice>(choiceId, featureId);
        return Ok();
    }

    [HttpDelete("proficiencies/weapon-categories/choices")] 
    public async Task<ActionResult> ClearWeaponCategoryProficiencyChoices(int featureId)
    {
        await choiceService.ClearChoices<WeaponCategoryProficiencyChoice>(featureId);
        return Ok();
    }

    [HttpDelete("choices")]
    public async Task<ActionResult> ClearChoices(int featureId)
    {
        await choiceService.ClearChoices<SkillProficiencyChoice>(featureId);
        await choiceService.ClearChoices<LanguageChoice>(featureId);
        await choiceService.ClearChoices<WeaponCategoryProficiencyChoice>(featureId);
        await choiceService.ClearChoices<WeaponTypeProficiencyChoice>(featureId);
        await choiceService.ClearChoices<ToolProficiencyChoice>(featureId);
        await choiceService.ClearChoices<AbilityIncreaseChoice>(featureId);
        await choiceService.ClearChoices<ArmorProficiencyChoice>(featureId);
        return Ok();
    }

    private async Task EnsureChoiceBelongsToFeature(int featureId, int choiceId)
    {
        var feature = await featureService.GetWithChoicesAsync(featureId);
        
        if(feature.LanguageChoices.FirstOrDefault(c => c.Id == choiceId) != null 
        || feature.ToolProficiencyChoices.FirstOrDefault(c => c.Id == choiceId) != null
        || feature.SkillProficiencyChoices.FirstOrDefault(c => c.Id == choiceId) != null
        || feature.ArmorProficiencyChoices.FirstOrDefault(c => c.Id == choiceId) != null
        || feature.WeaponCategoryProficiencyChoices.FirstOrDefault(c => c.Id == choiceId) != null
        || feature.WeaponTypeProficiencyChoices.FirstOrDefault(c => c.Id == choiceId) != null
        || feature.AbilityIncreaseChoices.FirstOrDefault(c => c.Id == choiceId) != null
        )
            return;
        
        throw new NotFoundException($"Feature with ID {featureId} does not have a choice with ID {choiceId}.");
    }
}   