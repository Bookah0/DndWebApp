using Api.Middlewares.ExceptionHandling;
using Api.Models.DTOs.Features;
using Api.Models.Features;
using Api.Services.Interfaces.Features;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Features;

[ApiController]
[Route("api/races/{raceId}/traits/{traitId}")]
public class RaceTraitChoicesController : ChoicesControllerBase<Trait, CreateTraitRequestDto, UpdateTraitRequestDto>
{
    public RaceTraitChoicesController(IChoiceService<Trait> service, IFeatureService<Trait, CreateTraitRequestDto, UpdateTraitRequestDto> featureService) : base(service, featureService) { }
}

[ApiController]
[Route("api/races/{raceId}/subraces/{subraceId}/traits/{traitId}")]
public class SubraceTraitChoicesController : ChoicesControllerBase<Trait, CreateTraitRequestDto, UpdateTraitRequestDto>
{
    public SubraceTraitChoicesController(IChoiceService<Trait> service, IFeatureService<Trait, CreateTraitRequestDto, UpdateTraitRequestDto> featureService) : base(service, featureService) { }
}

[ApiController]
[Route("api/classes/{classId}/features/{featureId}")]
public class ClassChoicesController : ChoicesControllerBase<ClassFeature, CreateClassFeatureRequestDto, UpdateClassFeatureRequestDto>
{
    public ClassChoicesController(IChoiceService<ClassFeature> service, IFeatureService<ClassFeature, CreateClassFeatureRequestDto, UpdateClassFeatureRequestDto> featureService) : base(service, featureService) { }
}

[ApiController]
[Route("api/classes/{classId}/subclasses/{subclassId}/features/{featureId}")]
public class SubclassClassChoicesController : ChoicesControllerBase<ClassFeature, CreateClassFeatureRequestDto, UpdateClassFeatureRequestDto>
{
    public SubclassClassChoicesController(IChoiceService<ClassFeature> service, IFeatureService<ClassFeature, CreateClassFeatureRequestDto, UpdateClassFeatureRequestDto> featureService) : base(service, featureService) { }
}

[ApiController]
[Route("api/backgrounds/{backgroundId}/features/{featureId}")]
public class BackgroundChoicesController : ChoicesControllerBase<BackgroundFeature, CreateBackgroundFeatureRequestDto, UpdateBackgroundFeatureRequestDto>
{
    public BackgroundChoicesController(IChoiceService<BackgroundFeature> service, IFeatureService<BackgroundFeature, CreateBackgroundFeatureRequestDto, UpdateBackgroundFeatureRequestDto> featureService) : base(service, featureService) { }
}

[ApiController]
[Route("api/feats/{featId}")]
public class FeatChoicesController : ChoicesControllerBase<Feat, CreateFeatRequestDto, UpdateFeatRequestDto>
{
    public FeatChoicesController(IChoiceService<Feat> service, IFeatureService<Feat, CreateFeatRequestDto, UpdateFeatRequestDto> featureService) : base(service, featureService) { }
}

public abstract class ChoicesControllerBase<F, CD, UD>(IChoiceService<F> choiceService, IFeatureService<F, CD, UD> featureService) : ControllerBase where F : Feature where CD : CreateFeatureRequestDto where UD : UpdateFeatureRequestDto
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