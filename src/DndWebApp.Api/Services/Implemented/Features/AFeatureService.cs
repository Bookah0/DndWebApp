using System.Threading.Tasks;
using DndWebApp.Api.Middlewares.ExceptionHandling;
using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs.Features;
using DndWebApp.Api.Models.Features;
using DndWebApp.Api.Models.Items.Constants;
using DndWebApp.Api.Repositories.Interfaces;
using DndWebApp.Api.Services.Interfaces.Features;
using DndWebApp.Api.Services.Util;

namespace DndWebApp.Api.Services.Implemented.Features;

public abstract class AFeatureService<T>(
    IRepository<T> repo,
    ISpellRepository spellRepo,
    ISkillRepository skillRepo,
    IAbilityRepository abilityRepo,
    ILanguageRepository languageRepo,
    IChoiceService<T> choiceService,
    ILogger<AFeatureService<T>> logger) : IBaseFeatureService<T> where T : AFeature
{
    public abstract Task<T> CreateAsync(AFeatureDto dto);
    public abstract Task DeleteAsync(int id);
    public abstract Task<ICollection<T>> GetAllAsync();
    public abstract Task<T> GetByIdAsync(int id);
    public abstract Task UpdateAsync(int id, AFeatureDto dto);

    public async Task AddSpell(int spellId, int featureId)
    {
        var spell = await spellRepo.GetByIdAsync(spellId)
            ?? throw new NotFoundException($"Spell with id {spellId} could not be found");

        var feature = await repo.GetByIdAsync(featureId)
            ?? throw new NotFoundException($"Feature with id {featureId} could not be found");

        logger.LogInformation("Adding spell with Name: {SpellName}, ID: {SpellId} to feature with Name: {FeatureName}, ID: {FeatureId}", spell.Name, spell.Id, feature.Name, feature.Id);
        feature.SpellsGained.Add(spell);
        await repo.UpdateAsync(feature);
        logger.LogInformation("Successfully added spell with Name: {SpellName}, ID: {SpellId} to feature with Name: {FeatureName}, ID: {FeatureId}", spell.Name, spell.Id, feature.Name, feature.Id);
    }

    public async Task RemoveSpell(int spellId, int featureId)
    {
        var feature = await repo.GetByIdAsync(featureId)
            ?? throw new NotFoundException($"Feature with id {featureId} could not be found");

        var spell = feature.SpellsGained.FirstOrDefault(s => s.Id == spellId)
            ?? throw new NotFoundException($"Spell with id {spellId} was not in the list of spells");

        logger.LogInformation("Removing spell with Name: {SpellName}, ID: {SpellId} from feature with Name: {FeatureName}, ID: {FeatureId}", spell.Name, spell.Id, feature.Name, feature.Id);
        feature.SpellsGained.Remove(spell);
        await repo.UpdateAsync(feature);
        logger.LogInformation("Successfully removed spell with Name: {SpellName}, ID: {SpellId} from feature with Name: {FeatureName}, ID: {FeatureId}", spell.Name, spell.Id, feature.Name, feature.Id);
    }

    public async Task AddProficiency(ProficiencyDto dto, int featureId)
    {
        var feature = await repo.GetByIdAsync(featureId)
            ?? throw new NotFoundException($"Feature with id {featureId} could not be found");

        logger.LogInformation("Adding proficiency of type {ProficiencyType} with value {ProficiencyValue} to feature with Name: {FeatureName}, ID: {FeatureId}", dto.Type, dto.Value, feature.Name, feature.Id);
        
        switch (dto.Type)
        {
            case "WeaponCategory":
                var weaponCategory = ConstantsUtil.ResolveOptionOrThrow(dto.Value, WeaponCategory.AllowedValues, "Weapon Category");
                feature.WeaponCategoryProficiencies.Add(weaponCategory);
                break;
            case "WeaponType":
                var weaponType = ConstantsUtil.ResolveOptionOrThrow(dto.Value, WeaponType.AllowedValues, "Weapon Type");
                feature.WeaponTypeProficiencies.Add(weaponType);
                break;
            case "ArmorCategory":
                var armorCategory = ConstantsUtil.ResolveOptionOrThrow(dto.Value, ArmorCategory.AllowedValues, "Armor Category");
                feature.ArmorProficiencies.Add(armorCategory);
                break;
            case "ToolCategory":
                var toolCategory = ConstantsUtil.ResolveOptionOrThrow(dto.Value, ToolCategory.AllowedValues, "Tool Category");
                feature.ToolProficiencies.Add(toolCategory);
                break;
            case "Skill":
                var skill = await GetProficiencyById(dto, skillRepo);
                feature.SkillProficiencies.Add(skill);
                break;
            case "Language":
                var language = await GetProficiencyById(dto, languageRepo);
                feature.Languages.Add(language);
                break;
            case "SavingThrow":
            case "Ability":
                var ability = await GetProficiencyById(dto, abilityRepo);
                feature.SavingThrowProficiencies.Add(ability);
                break;
            case "Resistance":
                var damageType = ConstantsUtil.ResolveOptionOrThrow(dto.Value, DamageType.AllowedValues, "Damage Type");
                feature.DamageResistanceGained.Add(damageType);
                break;
            case "Immunity":
                var immuneDamageType = ConstantsUtil.ResolveOptionOrThrow(dto.Value, DamageType.AllowedValues, "Damage Type");
                feature.DamageImmunityGained.Add(immuneDamageType);
                break;
            case "Weakness":
                var weaknessDamageType = ConstantsUtil.ResolveOptionOrThrow(dto.Value, DamageType.AllowedValues, "Damage Type");
                feature.DamageWeaknessGained.Add(weaknessDamageType);
                break;
            default:
                throw new InvalidOperationException($"Unknown Proficiency type: {dto.Type}");
        }

        await repo.UpdateAsync(feature);
        logger.LogInformation("Successfully added proficiency of type {ProficiencyType} with value {ProficiencyValue} to feature with Name: {FeatureName}, ID: {FeatureId}", dto.Type, dto.Value, feature.Name, feature.Id);
    }

    public async Task RemoveProficiency(ProficiencyDto dto, int featureId)
    {
        var feature = await repo.GetByIdAsync(featureId)
            ?? throw new NotFoundException($"Feature with id {featureId} could not be found");
                
        logger.LogInformation("Removing proficiency of type {ProficiencyType} with value {ProficiencyValue} from feature with Name: {FeatureName}, ID: {FeatureId}", dto.Type, dto.Value, feature.Name, feature.Id);

        switch (dto.Type)
        {
            case "WeaponCategory":
                var weaponCategory = ConstantsUtil.ResolveOptionOrThrow(dto.Value, WeaponCategory.AllowedValues, "Weapon Category");
                feature.WeaponCategoryProficiencies.Remove(weaponCategory);
                break;
            case "WeaponType":
                var weaponType = ConstantsUtil.ResolveOptionOrThrow(dto.Value, WeaponType.AllowedValues, "Weapon Type");
                feature.WeaponTypeProficiencies.Remove(weaponType);
                break;
            case "ArmorCategory":
                var armorCategory = ConstantsUtil.ResolveOptionOrThrow(dto.Value, ArmorCategory.AllowedValues, "Armor Category");
                feature.ArmorProficiencies.Remove(armorCategory);
                break;
            case "ToolCategory":
                var toolCategory = ConstantsUtil.ResolveOptionOrThrow(dto.Value, ToolCategory.AllowedValues, "Tool Category");
                feature.ToolProficiencies.Remove(toolCategory);
                break;
            case "Resistance":
                var damageType = ConstantsUtil.ResolveOptionOrThrow(dto.Value, DamageType.AllowedValues, "Damage Type");
                feature.DamageResistanceGained.Remove(damageType);
                break;
            case "Immunity":
                var immuneDamageType = ConstantsUtil.ResolveOptionOrThrow(dto.Value, DamageType.AllowedValues, "Damage Type");
                feature.DamageImmunityGained.Remove(immuneDamageType);
                break;
            case "Weakness":
                var weaknessDamageType = ConstantsUtil.ResolveOptionOrThrow(dto.Value, DamageType.AllowedValues, "Damage Type");
                feature.DamageWeaknessGained.Remove(weaknessDamageType);
                break;
            case "Skill":
                var skill = GetProficiencyById(dto, feature.SkillProficiencies);
                feature.SkillProficiencies.Remove(skill);
                break;
            case "Language":
                var language = GetProficiencyById(dto, feature.Languages);
                feature.Languages.Remove(language);
                break;
            case "SavingThrow":
            case "Ability":
                var ability = GetProficiencyById(dto, feature.SavingThrowProficiencies);
                feature.SavingThrowProficiencies.Remove(ability);
                break;
            default:
                throw new InvalidOperationException($"Unknown Proficiency type: {dto.Type}");
        }

        await repo.UpdateAsync(feature);
        logger.LogInformation("Successfully removed proficiency of type {ProficiencyType} with value {ProficiencyValue} from feature with Name: {FeatureName}, ID: {FeatureId}", dto.Type, dto.Value, feature.Name, feature.Id);
    }

    public async Task AddAbilityIncrease(int abilityId, int value, int featureId)
    {
        var feature = await repo.GetByIdAsync(featureId)
            ?? throw new NotFoundException($"Feature with id {featureId} could not be found");

        var ability = await abilityRepo.GetByIdAsync(abilityId)
            ?? throw new NotFoundException($"Ability with id {abilityId} could not be found");

        logger.LogInformation("Adding ability increase of {IncreaseValue} to ability with Name: {AbilityName}, ID: {AbilityId} for feature with Name: {FeatureName}, ID: {FeatureId}", value, ability.FullName, ability.Id, feature.Name, feature.Id);
        feature.AbilityIncreases.Add(new() { Ability = ability, AbilityId = abilityId, Value = value });
        await repo.UpdateAsync(feature);
        logger.LogInformation("Successfully added ability increase of {IncreaseValue} to ability with Name: {AbilityName}, ID: {AbilityId} for feature with Name: {FeatureName}, ID: {FeatureId}", value, ability.FullName, ability.Id, feature.Name, feature.Id);
    }

    public async Task RemoveAbilityIncrease(int abilityId, int featureId)
    {
        var feature = await repo.GetByIdAsync(featureId)
            ?? throw new NotFoundException($"Feature with id {featureId} could not be found");

        var abilityIncrease = feature.AbilityIncreases.FirstOrDefault(a => a.AbilityId == abilityId)
            ?? throw new NotFoundException($"AbilityIncrease with Ability id {abilityId} was not in the list of Ability Increases");
        
        logger.LogInformation("Removing ability increase of {IncreaseValue} to ability with Name: {AbilityName}, ID: {AbilityId} for feature with Name: {FeatureName}, ID: {FeatureId}", abilityIncrease.Value, abilityIncrease.Ability.FullName, abilityIncrease.Ability.Id, feature.Name, feature.Id);
        feature.AbilityIncreases.Remove(abilityIncrease);
        await repo.UpdateAsync(feature);
        logger.LogInformation("Successfully removed ability increase of {IncreaseValue} to ability with Name: {AbilityName}, ID: {AbilityId} for feature with Name: {FeatureName}, ID: {FeatureId}", abilityIncrease.Value, abilityIncrease.Ability.FullName, abilityIncrease.Ability.Id, feature.Name, feature.Id);
    }

    // Helpers
    private static async Task<P> GetProficiencyById<P>(ProficiencyDto dto, IRepository<P> repository) where P : class
    {
        if (!int.TryParse(dto.Value, out var id))
            throw new ValidationException($"{dto.Type} id {dto.Value} is not a valid integer");

        return await repository.GetByIdAsync(id)
            ?? throw new NotFoundException($"{dto.Type} with id {dto.Value} could not be found");
    }

    private static P GetProficiencyById<P>(ProficiencyDto dto, ICollection<P> collection) where P : class
    {
        if (!int.TryParse(dto.Value, out var id))
            throw new ValidationException($"{dto.Type} id {dto.Value} is not a valid integer");

        var proficiency = collection.FirstOrDefault(s => s.Equals(id))
            ?? throw new NotFoundException($"{dto.Type} with id {dto.Value} was not in the list of proficiencies");

        return proficiency;
    }
}