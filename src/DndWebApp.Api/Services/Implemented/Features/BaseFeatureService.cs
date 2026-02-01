using DndWebApp.Api.Middlewares.ExceptionHandling;
using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs.Features;
using DndWebApp.Api.Models.Features;
using DndWebApp.Api.Models.Items.Constants;
using DndWebApp.Api.Repositories.Interfaces;
using DndWebApp.Api.Services.Interfaces.Features;
using DndWebApp.Api.Services.Util;
using DndWebApp.Api.Services.Util.Interfaces;

namespace DndWebApp.Api.Services.Implemented.Features;

public abstract class BaseFeatureService<T>(
    IRepository<T> repo,
    ISpellRepository spellRepo,
    ISkillRepository skillRepo,
    IAbilityRepository abilityRepo,
    ILanguageRepository languageRepo,
    ILogger<BaseFeatureService<T>> logger) : IBaseFeatureService<T> where T : AFeature
{
    public async Task AddSpell(int spellId, int featureId)
    {
        var spell = await spellRepo.GetByIdAsync(spellId)
            ?? throw new NotFoundException($"Spell with id {spellId} could not be found");

        var feature = await repo.GetByIdAsync(featureId)
            ?? throw new NotFoundException($"Feature with id {featureId} could not be found");

        feature.SpellsGained.Add(spell);
        await repo.UpdateAsync(feature);
    }

    public async Task RemoveSpell(int spellId, int featureId)
    {
        var feature = await repo.GetByIdAsync(featureId)
            ?? throw new NotFoundException($"Feature with id {featureId} could not be found");

        var spell = feature.SpellsGained.FirstOrDefault(s => s.Id == spellId)
            ?? throw new NotFoundException($"Spell with id {spellId} was not in the list of spells");

        feature.SpellsGained.Remove(spell);
        await repo.UpdateAsync(feature);
    }

    public async Task AddProficiency(ProficiencyDto dto, int featureId)
    {
        var feature = await repo.GetByIdAsync(featureId)
            ?? throw new NotFoundException($"Feature with id {featureId} could not be found");
        
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
    }

    public async Task RemoveProficiency(ProficiencyDto dto, int featureId)
    {
        var feature = await repo.GetByIdAsync(featureId)
            ?? throw new NotFoundException($"Feature with id {featureId} could not be found");
                
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
    }

    public async Task AddAbilityIncrease(int abilityId, int value, int featureId)
    {
        var feature = await repo.GetByIdAsync(featureId)
            ?? throw new NotFoundException($"Feature with id {featureId} could not be found");

        var ability = await abilityRepo.GetByIdAsync(abilityId)
            ?? throw new NotFoundException($"Ability with id {abilityId} could not be found");

        feature.AbilityIncreases.Add(new() { Ability = ability, AbilityId = abilityId, Value = value });
        await repo.UpdateAsync(feature);
    }

    public async Task RemoveAbilityIncrease(int abilityId, int featureId)
    {
        var feature = await repo.GetByIdAsync(featureId)
            ?? throw new NotFoundException($"Feature with id {featureId} could not be found");

        var abilityIncrease = feature.AbilityIncreases.FirstOrDefault(a => a.AbilityId == abilityId)
            ?? throw new NotFoundException($"AbilityIncrease with Ability id {abilityId} was not in the list of Ability Increases");
        
        feature.AbilityIncreases.Remove(abilityIncrease);
        await repo.UpdateAsync(feature);
    }

    public async Task ClearAbilityIncreaseChoices(int featureId)
    {
        var feature = await repo.GetByIdAsync(featureId)
            ?? throw new NotFoundException($"Feature with id {featureId} could not be found");

        feature.AbilityIncreaseChoices.Clear();
        await repo.UpdateAsync(feature);
    }

    public async Task AddProficiencyChoice(ProficiencyChoiceDto dto, int featureId)
    {
        var feature = await repo.GetByIdAsync(featureId)
            ?? throw new NotFoundException($"Feature with id {featureId} could not be found");

        switch (dto.Type)
        {
            case "Weapon Category":
                var weaponCategories = ConstantsUtil.ResolveOptionOrThrow(dto.Options, WeaponCategory.AllowedValues, "Weapon Category");
                feature.WeaponCategoryProficiencyChoices.Add(new() { Description = dto.Description, Options = weaponCategories });
                break;
            case "Weapon Type":
                var weaponType = ConstantsUtil.ResolveOptionOrThrow(dto.Options, WeaponType.AllowedValues, "Weapon Type");
                feature.WeaponTypeProficiencyChoices.Add(new() { Description = dto.Description, Options = weaponType });
                break;
            case "Armor Category":
                var armorCategory = ConstantsUtil.ResolveOptionOrThrow(dto.Options, ArmorCategory.AllowedValues, "Armor Category");
                feature.ArmorProficiencyChoices.Add(new() { Description = dto.Description, Options = armorCategory });
                break;
            case "Tool Category":
                var toolCategory = ConstantsUtil.ResolveOptionOrThrow(dto.Options, ToolCategory.AllowedValues, "Tool Category");
                feature.ToolProficiencyChoices.Add(new() { Description = dto.Description, Options = toolCategory });
                break;
            case "Skill":
                var skillOptions = await GetProficiencyOptionsById(dto, skillRepo);
                feature.SkillProficiencyChoices.Add(new() { Description = dto.Description, Options = skillOptions });
                break;
            case "Language":
                var languageOptions = await GetProficiencyOptionsById(dto, languageRepo);
                feature.LanguageChoices.Add(new() { Description = dto.Description, Options = languageOptions });
                break;
        }

        await repo.UpdateAsync(feature);
    }

    public async Task AddAbilityIncreaseChoice(AbilityIncreaseChoiceDto dto, int featureId)
    {
        var feature = await repo.GetByIdAsync(featureId)
            ?? throw new NotFoundException($"Feature with id {featureId} could not be found");
        
        ICollection<AbilityValue> options = [];

        foreach (var abilityValueDto in dto.Options)
        {
            var ability = await abilityRepo.GetByIdAsync(abilityValueDto.AbilityId)
                ?? throw new NotFoundException($"Ability with id {abilityValueDto.AbilityId} could not be found");

            options.Add(new AbilityValue
            {
                Ability = ability,
                AbilityId = ability.Id,
                Value = abilityValueDto.Value
            });
        }
        feature.AbilityIncreaseChoices.Add(new() { Description = dto.Description, Options = options });
        await repo.UpdateAsync(feature);
    }

    public async Task RemoveProficiencyChoice(string type, int choiceIndex, int featureId)
    {
        var feature = await repo.GetByIdAsync(featureId)
            ?? throw new NotFoundException($"Feature with id {featureId} could not be found");
        
        void RemoveChoiceAtIndex<PC>(ICollection<PC> choices, string choiceType)
        {
            if (choiceIndex < 0 || choiceIndex >= choices.Count)
                throw new ArgumentOutOfRangeException(nameof(choiceIndex), $"Index is out of range. No {choiceType.ToLower()} choice at index {choiceIndex}.");
            
            choices.RemoveAt(choiceIndex);
        }

        switch (type)
        {
            case "Skill":
                RemoveChoiceAtIndex(feature.SkillProficiencyChoices, type);
                break;
            case "Weapon Category":
                RemoveChoiceAtIndex(feature.WeaponCategoryProficiencyChoices, type);
                break;
            case "Weapon Type":
                RemoveChoiceAtIndex(feature.WeaponTypeProficiencyChoices, type);
                break;
            case "Armor Category":
                RemoveChoiceAtIndex(feature.ArmorProficiencyChoices, type);
                break;
            case "Tool Category":
                RemoveChoiceAtIndex(feature.ToolProficiencyChoices, type);
                break;
            case "Language":
                RemoveChoiceAtIndex(feature.LanguageChoices, type);
                break;
            default:
                throw new InvalidOperationException($"Unknown Choice type");
        }

        await repo.UpdateAsync(feature);
    }

    public async Task RemoveAbilityIncreaseChoice(int choiceIndex, int featureId)
    {
        var feature = await repo.GetByIdAsync(featureId)
            ?? throw new NotFoundException($"Feature with id {featureId} could not be found");

        if (choiceIndex < 0 || choiceIndex >= feature.AbilityIncreaseChoices.Count)
                throw new ArgumentOutOfRangeException($"Index is out of range. No ability increase choice at index {choiceIndex}.");

        feature.AbilityIncreaseChoices.RemoveAt(choiceIndex);
        await repo.UpdateAsync(feature);
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

    private async Task<ICollection<P>> GetProficiencyOptionsById<P>(ProficiencyChoiceDto dto, IRepository<P> repository) where P : class
    {
        ICollection<P> options = [];

        foreach (var optionId in dto.Options)
        {
            if (!int.TryParse(optionId, out var id))
                throw new ValidationException($"{dto.Type} id {optionId} is not a valid integer");

            options.Add(await repository.GetByIdAsync(id)
                ?? throw new NotFoundException($"{dto.Type} with id {optionId} could not be found"));
        }
        return options;
    }
}