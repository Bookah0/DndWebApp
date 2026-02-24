using Api.Domain.Abilities.Repositories;
using Api.Domain.Languages.Repositories;
using Api.Domain.Shared.DTOs;
using Api.Domain.Shared.Enums.Damage;
using Api.Domain.Shared.Enums.Items;
using Api.Domain.Shared.Models;
using Api.Domain.Shared.Repositories;
using Api.Domain.Skills.Repositories;
using Api.Domain.Spells.Repositories;
using Api.Infrastructure.Middleware.ExceptionHandling;
using static Api.Infrastructure.Validation.ValuesValidator;    

namespace Api.Domain.Shared.Services;

public abstract class FeatureService<T, CD, UD>(
    IFeatureRepository<T> repo,
    ISpellRepository spellRepo,
    ISkillRepository skillRepo,
    IAbilityRepository abilityRepo,
    ILanguageRepository languageRepo,
    ILogger logger) 
    : IFeatureService<T, CD, UD> where T : Feature where CD : CreateFeatureRequestDto where UD : UpdateFeatureRequestDto
{
    public abstract Task<T> GetByIdAsync(int id);
    public abstract Task<ICollection<T>> GetAllAsync();
    public abstract Task<T> CreateAsync(CD dto);
    public abstract Task<T> UpdateAsync(UD dto, int id);
    public abstract Task DeleteAsync(int id);

    public Task<T> GetWithChoicesAsync(int id) => repo.GetWithChoicesAsync(id);
    public Task<T> GetWithProficienciesAsync(int id) => repo.GetWithProficienciesAsync(id);

    public async Task<T> AddSpell(int spellId, int featureId)
    {
        var spell = await spellRepo.GetByIdAsync(spellId);
        var feature = await repo.GetByIdAsync(featureId);

        logger.LogInformation("Adding spell with Name: {SpellName}, ID: {SpellId} to feature with Name: {FeatureName}, ID: {FeatureId}", spell.Name, spell.Id, feature.Name, feature.Id);
        feature.SpellsGained.Add(spell);
        await repo.UpdateAsync(feature);
        logger.LogInformation("Successfully added spell with Name: {SpellName}, ID: {SpellId} to feature with Name: {FeatureName}, ID: {FeatureId}", spell.Name, spell.Id, feature.Name, feature.Id);
        return feature;
    }

    public async Task RemoveSpell(int spellId, int featureId)
    {
        var feature = await repo.GetByIdAsync(featureId);
        var spell = feature.SpellsGained.FirstOrDefault(s => s.Id == spellId)
            ?? throw new NotFoundException($"Spell with id {spellId} was not found in the feature's spell list");

        logger.LogInformation("Removing spell with Name: {SpellName}, ID: {SpellId} from feature with Name: {FeatureName}, ID: {FeatureId}", spell.Name, spell.Id, feature.Name, feature.Id);
        feature.SpellsGained.Remove(spell);
        await repo.UpdateAsync(feature);
        logger.LogInformation("Successfully removed spell with Name: {SpellName}, ID: {SpellId} from feature with Name: {FeatureName}, ID: {FeatureId}", spell.Name, spell.Id, feature.Name, feature.Id);
    }

    public async Task<T> AddProficiency(ProficiencyRequestDto dto, int featureId)
    {
        var feature = await repo.GetByIdAsync(featureId);

        logger.LogInformation("Adding proficiency of type {ProficiencyType} with value {ProficiencyValue} to feature with Name: {FeatureName}, ID: {FeatureId}", dto.Type, dto.Value, feature.Name, feature.Id);
        
        switch (dto.Type)
        {
            case "WeaponCategory":
                var weaponCategory = NormalizeValue<WeaponCategory>(dto.Value);
                feature.WeaponCategoryProficiencies.Add(weaponCategory);
                break;
            case "WeaponType":
                var weaponType = NormalizeValue<WeaponType>(dto.Value);
                feature.WeaponTypeProficiencies.Add(weaponType);
                break;
            case "ArmorCategory":
                var armorCategory = NormalizeValue<ArmorCategory>(dto.Value);
                feature.ArmorProficiencies.Add(armorCategory);
                break;
            case "ToolCategory":
                var toolCategory = NormalizeValue<ToolCategory>(dto.Value);
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
                var damageType = NormalizeValue<DamageType>(dto.Value);
                feature.DamageResistanceGained.Add(damageType);
                break;
            case "Immunity":
                var immuneDamageType = NormalizeValue<DamageType>(dto.Value);
                feature.DamageImmunityGained.Add(immuneDamageType);
                break;
            case "Weakness":
                var weaknessDamageType = NormalizeValue<DamageType>(dto.Value);
                feature.DamageWeaknessGained.Add(weaknessDamageType);
                break;
            default:
                throw new InvalidOperationException($"Unknown Proficiency type: {dto.Type}");
        }

        await repo.UpdateAsync(feature);
        logger.LogInformation("Successfully added proficiency of type {ProficiencyType} with value {ProficiencyValue} to feature with Name: {FeatureName}, ID: {FeatureId}", dto.Type, dto.Value, feature.Name, feature.Id);
        return feature;
    }

    public async Task RemoveProficiency(ProficiencyRequestDto dto, int featureId)
    {
        var feature = await repo.GetByIdAsync(featureId);
                
        logger.LogInformation("Removing proficiency of type {ProficiencyType} with value {ProficiencyValue} from feature with Name: {FeatureName}, ID: {FeatureId}", dto.Type, dto.Value, feature.Name, feature.Id);

        switch (dto.Type)
        {
            case "WeaponCategory":
                var weaponCategory = NormalizeValue<WeaponCategory>(dto.Value);
                feature.WeaponCategoryProficiencies.Remove(weaponCategory);
                break;
            case "WeaponType":
                var weaponType = NormalizeValue<WeaponType>(dto.Value);
                feature.WeaponTypeProficiencies.Remove(weaponType);
                break;
            case "ArmorCategory":
                var armorCategory = NormalizeValue<ArmorCategory>(dto.Value);
                feature.ArmorProficiencies.Remove(armorCategory);
                break;
            case "ToolCategory":
                var toolCategory = NormalizeValue<ToolCategory>(dto.Value);
                feature.ToolProficiencies.Remove(toolCategory);
                break;
            case "Resistance":
                var damageType = NormalizeValue<DamageType>(dto.Value);
                feature.DamageResistanceGained.Remove(damageType);
                break;
            case "Immunity":
                var immuneDamageType = NormalizeValue<DamageType>(dto.Value);
                feature.DamageImmunityGained.Remove(immuneDamageType);
                break;
            case "Weakness":
                var weaknessDamageType = NormalizeValue<DamageType>(dto.Value);
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

    public async Task<T> AddAbilityIncrease(int abilityId, int value, int featureId)
    {
        var feature = await repo.GetByIdAsync(featureId);
        var ability = await abilityRepo.GetByIdAsync(abilityId);

        logger.LogInformation("Adding ability increase of {IncreaseValue} to ability with Name: {AbilityName}, ID: {AbilityId} for feature with Name: {FeatureName}, ID: {FeatureId}", value, ability.FullName, ability.Id, feature.Name, feature.Id);
        feature.AbilityIncreases.Add(new() { Ability = ability, AbilityId = abilityId, Value = value });
        await repo.UpdateAsync(feature);
        logger.LogInformation("Successfully added ability increase of {IncreaseValue} to ability with Name: {AbilityName}, ID: {AbilityId} for feature with Name: {FeatureName}, ID: {FeatureId}", value, ability.FullName, ability.Id, feature.Name, feature.Id);
        return feature;
    }

    public async Task RemoveAbilityIncrease(int abilityId, int featureId)
    {
        var feature = await repo.GetByIdAsync(featureId);

        var abilityIncrease = feature.AbilityIncreases.FirstOrDefault(a => a.AbilityId == abilityId)
            ?? throw new NotFoundException($"AbilityIncrease with Ability id {abilityId} was not in the list of Ability Increases");
        
        logger.LogInformation("Removing ability increase of {IncreaseValue} to ability with Name: {AbilityName}, ID: {AbilityId} for feature with Name: {FeatureName}, ID: {FeatureId}", abilityIncrease.Value, abilityIncrease.Ability.FullName, abilityIncrease.Ability.Id, feature.Name, feature.Id);
        feature.AbilityIncreases.Remove(abilityIncrease);
        await repo.UpdateAsync(feature);
        logger.LogInformation("Successfully removed ability increase of {IncreaseValue} to ability with Name: {AbilityName}, ID: {AbilityId} for feature with Name: {FeatureName}, ID: {FeatureId}", abilityIncrease.Value, abilityIncrease.Ability.FullName, abilityIncrease.Ability.Id, feature.Name, feature.Id);
    }

    // Helpers
    private static async Task<P> GetProficiencyById<P>(ProficiencyRequestDto dto, IRepository<P> repository) where P : class
    {
        if (!int.TryParse(dto.Value, out var id))
            throw new ValidationException($"{dto.Type} id {dto.Value} is not a valid integer");

        return await repository.GetByIdAsync(id);
    }

    private static P GetProficiencyById<P>(ProficiencyRequestDto dto, ICollection<P> collection) where P : class
    {
        if (!int.TryParse(dto.Value, out var id))
            throw new ValidationException($"{dto.Type} id {dto.Value} is not a valid integer");

        var proficiency = collection.FirstOrDefault(s => s.Equals(id))
            ?? throw new NotFoundException($"{dto.Type} with id {dto.Value} was not in the list of proficiencies");

        return proficiency;
    }
}