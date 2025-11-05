using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.Characters.Enums;
using DndWebApp.Api.Models.Features;
using DndWebApp.Api.Models.Items.Enums;
using DndWebApp.Api.Models.World.Enums;
using DndWebApp.Api.Repositories;
using DndWebApp.Api.Repositories.Spells;
namespace DndWebApp.Api.Services.Features;

public abstract class BaseFeatureService<T> where T : AFeature
{
    internal readonly IRepository<T> repo;
    internal readonly ISpellRepository spellRepo;
    internal readonly ILogger<BaseFeatureService<T>> logger;

    public BaseFeatureService(IRepository<T> repo, ISpellRepository spellRepo, ILogger<BaseFeatureService<T>> logger)
    {
        this.repo = repo;
        this.spellRepo = spellRepo;
        this.logger = logger;
    }

    public async Task AddSpell(int spellId, int featureId)
    {
        var spell = await spellRepo.GetByIdAsync(spellId)
            ?? throw new NullReferenceException($"Spell with id {spellId} could not be found");

        var feature = await repo.GetByIdAsync(featureId)
            ?? throw new NullReferenceException($"Background Feature with id {featureId} could not be found");

        feature.SpellsGained.Add(spell);
        await repo.UpdateAsync(feature);
    }

    public async Task RemoveSpell(int spellId, int featureId)
    {
        var feature = await repo.GetByIdAsync(featureId)
            ?? throw new NullReferenceException($"Background Feature with id {featureId} could not be found");

        var spell = feature.SpellsGained.FirstOrDefault(s => s.Id == spellId)
            ?? throw new NullReferenceException($"Spell with id {spellId} was not in the list of spells");

        feature.SpellsGained.Remove(spell);
        await repo.UpdateAsync(feature);
    }

    public async Task AddProficiency<TEnum>(TEnum proficiency, int featureId) where TEnum : struct, Enum
    {
        var feature = await repo.GetByIdAsync(featureId)
            ?? throw new NullReferenceException($"Background Feature with id {featureId} could not be found");

        switch (proficiency)
        {
            case SkillType skillType:
                feature.SkillProficiencies.Add(skillType);
                break;
            case WeaponCategory weaponCategory:
                feature.WeaponProficiencies.Add(weaponCategory);
                break;
            case ArmorCategory armorCategory:
                feature.ArmorProficiencies.Add(armorCategory);
                break;
            case ToolCategory toolCategory:
                feature.ToolProficiencies.Add(toolCategory);
                break;
            case LanguageType languageType:
                feature.Languages.Add(languageType);
                break;
            case AbilityType savingThrowAbility:
                feature.SavingThrows.Add(savingThrowAbility);
                break;
        }

        await repo.UpdateAsync(feature);
    }

    public async Task RemoveProficiency<TEnum>(TEnum proficiency, int featureId) where TEnum : struct, Enum
    {
        var feature = await repo.GetByIdAsync(featureId)
            ?? throw new NullReferenceException($"Background Feature with id {featureId} could not be found");

        var wasRemoved = proficiency switch
        {
            SkillType skillType => feature.SkillProficiencies.Remove(skillType),
            WeaponCategory weaponCategory => feature.WeaponProficiencies.Remove(weaponCategory),
            ArmorCategory armorCategory => feature.ArmorProficiencies.Remove(armorCategory),
            ToolCategory toolCategory => feature.ToolProficiencies.Remove(toolCategory),
            LanguageType languageType => feature.Languages.Remove(languageType),
            AbilityType savingThrowAbility => feature.SavingThrows.Remove(savingThrowAbility),
            _ => throw new InvalidOperationException($"Unknown proficiency type: {proficiency.GetType().Name}")
        };

        if (!wasRemoved)
            throw new NullReferenceException($"Proificiency {proficiency} could not be found in the list of type {proficiency.GetType().Name}");

        await repo.UpdateAsync(feature);
    }

    public async Task AddDamageAffinity(AffinityType affinityType, DamageType damageType, int featureId)
    {
        var feature = await repo.GetByIdAsync(featureId)
            ?? throw new NullReferenceException($"Background Feature with id {featureId} could not be found");

        switch (affinityType)
        {
            case AffinityType.Resistant:
                feature.DamageResistanceGained.Add(damageType);
                break;
            case AffinityType.Immune:
                feature.DamageImmunityGained.Add(damageType);
                break;
            case AffinityType.Weakness:
                feature.DamageWeaknessGained.Add(damageType);
                break;
            default:
                throw new InvalidOperationException($"Unknown Affinity type: {affinityType.GetType().Name}");
        }

        await repo.UpdateAsync(feature);
    }

    public async Task RemoveDamageAffinity(AffinityType affinityType, DamageType damageType, int featureId)
    {
        var feature = await repo.GetByIdAsync(featureId)
            ?? throw new NullReferenceException($"Background Feature with id {featureId} could not be found");

        var wasRemoved = affinityType switch
        {
            AffinityType.Resistant => feature.DamageResistanceGained.Remove(damageType),
            AffinityType.Immune => feature.DamageImmunityGained.Remove(damageType),
            AffinityType.Weakness => feature.DamageWeaknessGained.Remove(damageType),
            _ => throw new InvalidOperationException($"Unknown Affinity type: {affinityType.GetType().Name}"),
        };

        if (!wasRemoved)
            throw new NullReferenceException($"Damage type {damageType} could not be found in the list of type {affinityType}");

        await repo.UpdateAsync(feature);
    }

    public async Task AddAbilityIncrease(int abilityId, int value, int featureId)
    {
        var feature = await repo.GetByIdAsync(featureId)
            ?? throw new NullReferenceException($"Background Feature with id {featureId} could not be found");

        feature.AbilityIncreases.Add(new() { AbilityId = abilityId, Value = value });
    }

    public async Task RemoveAbilityIncrease(int abilityId, int featureId)
    {
        var feature = await repo.GetByIdAsync(featureId)
            ?? throw new NullReferenceException($"Background Feature with id {featureId} could not be found");

        var abilityIncrease = feature.AbilityIncreases.FirstOrDefault(a => a.AbilityId == abilityId)
            ?? throw new NullReferenceException($"AbilityIncrease with Ability id {abilityId} was not in the list of Ability Increases");

        feature.AbilityIncreases.Remove(abilityIncrease);
        await repo.UpdateAsync(feature);
    }

    public async Task AddAbilityIncreaseChoice(List<AbilityValue> options, string description, int featureId)
    {
        var feature = await repo.GetByIdAsync(featureId)
            ?? throw new NullReferenceException($"Background Feature with id {featureId} could not be found");

        feature.AbilityIncreaseChoices.Add(new() { Description = description, Options = options });
    }

    public async Task RemoveAbilityIncreaseChoice(int choiceId, int featureId)
    {
        var feature = await repo.GetByIdAsync(featureId)
            ?? throw new NullReferenceException($"Background Feature with id {featureId} could not be found");

        var choice = feature.AbilityIncreaseChoices.FirstOrDefault(a => a.Id == choiceId)
            ?? throw new NullReferenceException($"AbilityIncreaseChoice with id {choiceId} was not in the list of choices");

        feature.AbilityIncreaseChoices.Remove(choice);
        await repo.UpdateAsync(feature);
    }

    public async Task AddProficiencyChoice<TEnum>(List<TEnum> options, string description, int featureId) where TEnum : struct, Enum
    {
        var feature = await repo.GetByIdAsync(featureId)
            ?? throw new NullReferenceException($"Background Feature with id {featureId} could not be found");

        switch (options[0])
        {
            case SkillType:
                feature.SkillProficiencyChoices.Add(new() { Description = description, Options = (ICollection<SkillType>)options });
                break;
            case WeaponCategory:
                feature.WeaponCategoryProficiencyChoices.Add(new() { Description = description, Options = (ICollection<WeaponCategory>)options });
                break;
            case WeaponType:
                feature.WeaponTypeProficiencyChoices.Add(new() { Description = description, Options = (ICollection<WeaponType>)options });
                break;
            case ArmorCategory:
                feature.ArmorProficiencyChoices.Add(new() { Description = description, Options = (ICollection<ArmorCategory>)options });
                break;
            case ToolCategory:
                feature.ToolProficiencyChoices.Add(new() { Description = description, Options = (ICollection<ToolCategory>)options });
                break;
            case LanguageType:
                feature.LanguageChoices.Add(new() { Description = description, Options = (ICollection<LanguageType>)options });
                break;
            default:
                throw new InvalidOperationException($"Unknown Choice type: {options[0].GetType().Name}");
        }

        await repo.UpdateAsync(feature);
    }

    public async Task RemoveProficiencyChoice<TEnum>(int choiceId, int featureId) where TEnum : struct, Enum
    {
        var feature = await repo.GetByIdAsync(featureId)
            ?? throw new NullReferenceException($"Background Feature with id {featureId} could not be found");

        switch (typeof(TEnum).Name)
        {
            case nameof(SkillType):
                RemoveChoice(feature.SkillProficiencyChoices, choiceId);
                break;
            case nameof(WeaponCategory):
                RemoveChoice(feature.WeaponCategoryProficiencyChoices, choiceId);
                break;
            case nameof(WeaponType):
                RemoveChoice(feature.WeaponTypeProficiencyChoices, choiceId);
                break;
            case nameof(ArmorCategory):
                RemoveChoice(feature.ArmorProficiencyChoices, choiceId);
                break;
            case nameof(ToolCategory):
                RemoveChoice(feature.ToolProficiencyChoices, choiceId);
                break;
            case nameof(LanguageType):
                RemoveChoice(feature.LanguageChoices, choiceId);
                break;
            default:
                throw new InvalidOperationException($"Unknown Choice type");
        }

        await repo.UpdateAsync(feature);
    }

    private static void RemoveChoice<C>(ICollection<C> collection, int choiceId) where C : class
    {
        var choice = collection.FirstOrDefault(c => (c as dynamic).Id == choiceId)
            ?? throw new NullReferenceException($"Choice with id {choiceId} could not be found");
        collection.Remove(choice);
    }
}