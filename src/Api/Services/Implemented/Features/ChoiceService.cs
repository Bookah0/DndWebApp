using AutoMapper;
using Api.Middlewares.ExceptionHandling;
using Api.Models.Characters;
using Api.Models.DTOs.Features;
using Api.Models.DTOs.RequestDtos.Character;
using Api.Models.Features;
using Api.Repositories.Interfaces;
using Api.Services.Interfaces.Features;
using Api.Services.Util;
using static Api.Validation.AllowedValues.ValuesValidator;
using Api.Validation.AllowedValues.Items;

namespace Api.Services.Implemented.Features;

public class ChoiceService<T>
( 
    IAbilityRepository abilityRepo,
    IAbilityValueRepository abilityValueRepo,
    ILanguageRepository languageRepo,
    ISkillRepository skillRepo,
    IFeatureService<T, CreateFeatureRequestDto, UpdateFeatureRequestDto> featureService,

    IChoiceRepository<AbilityIncreaseChoice> abilityChoiceRepo,
    IChoiceRepository<SkillProficiencyChoice> skillChoiceRepo,
    IChoiceRepository<LanguageChoice> languageChoiceRepo,
    IChoiceRepository<ToolProficiencyChoice> toolChoiceRepo,
    IChoiceRepository<ArmorProficiencyChoice> armorChoiceRepo,
    IChoiceRepository<WeaponCategoryProficiencyChoice> weaponCategoryChoiceRepo,
    IChoiceRepository<WeaponTypeProficiencyChoice> weaponTypeChoiceRepo,
    ILogger<ChoiceService<T>> logger,
    IMapper mapper) 
    : IChoiceService<T> where T : Feature
{

    public async Task ClearChoices<C>(int featureId) where C : IFeatureChoice
    {
        var feature = await featureService.GetByIdAsync(featureId);

        if (!repositoryGetters.TryGetValue(typeof(C), out var repo))
            throw new ValidationException($"Unknown Choice type: {typeof(C).Name}");
        
        if (!collectionGetters.TryGetValue(typeof(C), out var collectionGetter))
            throw new ValidationException($"Unknown Choice type: {typeof(C).Name}");

        var choices = (ICollection<C>)collectionGetter.DynamicInvoke(feature)!;

        logger.LogInformation("Clearing all proficiency choices from feature with Name: {FeatureName}, ID: {FeatureId}", feature.Name, feature.Id);

        foreach (var choice in choices)
        {
            if(choice.FeatureId != feature.Id) 
                throw new ValidationException($"Choice with id {choice.Id} does not belong to feature with id {feature.Id}");

            await ((IChoiceRepository<C>)repo).DeleteAsync(choice);
        }
        logger.LogInformation("Successfully cleared all proficiency choices from feature with Name: {FeatureName}, ID: {FeatureId}", feature.Name, feature.Id);
    }

    public async Task<T> AddChoice<CDto>(CDto dto, int featureId) where CDto : ChoiceDto
    {
        var feature = await featureService.GetByIdAsync(featureId);
        
        if (!dtoToEntityMap.TryGetValue(typeof(CDto), out var entityType))
            throw new InvalidOperationException($"Unknown Choice type: {typeof(CDto).Name}");
        
        if (!dtoRepositoryGetters.TryGetValue(typeof(CDto), out var repo))
            throw new InvalidOperationException($"Unknown Choice type: {typeof(CDto).Name}");

        logger.LogInformation("Adding proficiency choice {ProficiencyChoiceDto} to feature with Name: {FeatureName}, ID: {FeatureId}", dto, feature.Name, feature.Id);

        var choice = mapper.Map(dto, entityType) as IFeatureChoice;
        choice!.FeatureId = feature.Id;
        await ((IChoiceRepository<IFeatureChoice>)repo).CreateAsync(choice);

        logger.LogInformation("Successfully added proficiency choice {ProficiencyChoiceDto} to feature with Name: {FeatureName}, ID: {FeatureId}", dto, feature.Name, feature.Id);
        return feature;
    }

    public async Task RemoveChoice<C>(int choiceId, int featureId) where C : IFeatureChoice
    {
        var feature = await featureService.GetByIdAsync(featureId);
        
        if (!repositoryGetters.TryGetValue(typeof(C), out var repo))
            throw new InvalidOperationException($"Unknown Choice type: {typeof(C).Name}");

        var choice = await ((IChoiceRepository<C>)repo).GetByIdAsync(choiceId) ;

        if (choice.FeatureId != feature.Id)
            throw new ValidationException($"Choice with id {choiceId} does not belong to feature with id {feature.Id}");

        logger.LogInformation("Removing proficiency choice option with ID: {ChoiceId} from feature with Name: {FeatureName}, ID: {FeatureId}", choiceId, feature.Name, feature.Id);
        await ((IChoiceRepository<C>)repo).DeleteAsync(choice);
        logger.LogInformation("Successfully removed proficiency choice option with ID: {ChoiceId} from feature with Name: {FeatureName}, ID: {FeatureId}", choiceId, feature.Name, feature.Id);
    }

    public async Task<SkillProficiencyChoice> AddSkillOptions(int choiceId, ICollection<int> newSkillIds)
    {
        var choice = await skillChoiceRepo.GetByIdAsync(choiceId);

        foreach (var id in newSkillIds)
        {
            var skill = await skillRepo.GetByIdAsync(id);
            choice.Options.Add(skill);
        }
        await skillChoiceRepo.UpdateAsync(choice);
        return choice;
    }

    public async Task<LanguageChoice> AddLanguageOptions(int choiceId, ICollection<int> newLanguageIds)
    {
        var choice = await languageChoiceRepo.GetByIdAsync(choiceId);

        foreach (var id in newLanguageIds)
        {
            var language = await languageRepo.GetByIdAsync(id);
            choice.Options.Add(language);
        }
        await languageChoiceRepo.UpdateAsync(choice);
        return choice;
    }

    public async Task<AbilityIncreaseChoice> AddAbilityOptions(int choiceId, ICollection<AbilityValueDto> newAbilityValues)
    {
        var choice = await abilityChoiceRepo.GetByIdAsync(choiceId);

        foreach (var dto in newAbilityValues)
        {
            var ability = await abilityRepo.GetByIdAsync(dto.AbilityId);

            var abilityValue = new AbilityValue
            {
                Ability = ability,
                AbilityId = ability.Id,
                Value = dto.Value
            };
            await abilityValueRepo.CreateAsync(abilityValue);
            choice.Options.Add(abilityValue);
        }
        await abilityChoiceRepo.UpdateAsync(choice);
        return choice;
    }

    public async Task<WeaponCategoryProficiencyChoice> AddWeaponCategoryOptions(int choiceId, ICollection<string> newCategories)
    {
        var choice = await weaponCategoryChoiceRepo.GetByIdAsync(choiceId);

        foreach (var category in newCategories)
        {
            var normalizedCategory = NormalizeValueOrThrow<WeaponCategory>(category);
            choice.Options.Add(normalizedCategory);
        }
        await weaponCategoryChoiceRepo.UpdateAsync(choice);
        return choice;
    }

    public async Task<WeaponTypeProficiencyChoice> AddWeaponTypeOptions(int choiceId, ICollection<string> newTypes)
    {
        var choice = await weaponTypeChoiceRepo.GetByIdAsync(choiceId);

        foreach (var type in newTypes)
        {
            var normalizedType = NormalizeValueOrThrow<WeaponType>(type);
            choice.Options.Add(normalizedType);
        }
        await weaponTypeChoiceRepo.UpdateAsync(choice);
        return choice;
    }

    public async Task<ToolProficiencyChoice> AddToolCategoryOptions(int choiceId, ICollection<string> newCategories)
    {
        var choice = await toolChoiceRepo.GetByIdAsync(choiceId);

        foreach (var category in newCategories)
        {
            var normalizedCategory = NormalizeValueOrThrow<ToolCategory>(category);
            choice.Options.Add(normalizedCategory);
        }
        await toolChoiceRepo.UpdateAsync(choice);
        return choice;
    }

    public async Task<ArmorProficiencyChoice> AddArmorCategoryOptions(int choiceId, ICollection<string> newCategories)
    {
        var choice = await armorChoiceRepo.GetByIdAsync(choiceId);

        foreach (var category in newCategories)
        {
            var normalizedCategory = NormalizeValueOrThrow<ArmorCategory>(category);
            choice.Options.Add(normalizedCategory);
        }
        await armorChoiceRepo.UpdateAsync(choice);
        return choice;
    }

    public async Task RemoveSkillOptions(int choiceId, ICollection<int> skillIdsToRemove)
    {
        var choice = await skillChoiceRepo.GetByIdAsync(choiceId);

        foreach (var id in skillIdsToRemove)
        {
            var skill = await skillRepo.GetByIdAsync(id);

            if (!choice.Options.Remove(skill))
                throw new ValidationException($"Skill with id {id} is not an option of choice with id {choiceId}");
        }

        await skillChoiceRepo.UpdateAsync(choice);
    }

    public async Task RemoveLanguageOptions(int choiceId, ICollection<int> languageIdsToRemove)
    {
        var choice = await languageChoiceRepo.GetByIdAsync(choiceId);

        foreach (var id in languageIdsToRemove)
        {
            var language = await languageRepo.GetByIdAsync(id);

            if (!choice.Options.Remove(language))
                throw new ValidationException($"Language with id {id} is not an option of choice with id {choiceId}");
        }
        await languageChoiceRepo.UpdateAsync(choice);
    }

    public async Task RemoveAbilityOptions(int choiceId, ICollection<int> idsToRemove)
    {
        var choice = await abilityChoiceRepo.GetByIdAsync(choiceId);

        foreach (var id in idsToRemove)
        {
            var ability = await abilityRepo.GetByIdAsync(id);

            var abilityValue = choice.Options.FirstOrDefault(av => av.AbilityId == id) 
                ?? throw new NotFoundException($"Ability value with AbilityId {id} could not be found in choice with id {choiceId}");

            if (!choice.Options.Remove(abilityValue))
                throw new ValidationException($"Ability with id {id} is not an option of choice with id {choiceId}");
        }
        await abilityChoiceRepo.UpdateAsync(choice);
    }

    public async Task RemoveWeaponCategoryOptions(int choiceId, ICollection<string> categoriesToRemove)
    {
        var choice = await weaponCategoryChoiceRepo.GetByIdAsync(choiceId);
        choice.Options.RemoveMany(categoriesToRemove);
        await weaponCategoryChoiceRepo.UpdateAsync(choice);
    }

    public async Task RemoveWeaponTypeOptions(int choiceId, ICollection<string> typesToRemove)
    {
        var choice = await weaponTypeChoiceRepo.GetByIdAsync(choiceId);
        choice.Options.RemoveMany(typesToRemove);
        await weaponTypeChoiceRepo.UpdateAsync(choice);
    }

    public async Task RemoveToolCategoryOptions(int choiceId, ICollection<string> categoriesToRemove)
    {
        var choice = await toolChoiceRepo.GetByIdAsync(choiceId);
        choice.Options.RemoveMany(categoriesToRemove);
        await toolChoiceRepo.UpdateAsync(choice);
    }

    public async Task RemoveArmorCategoryOptions(int choiceId, ICollection<string> categoriesToRemove)
    {
        var choice = await armorChoiceRepo.GetByIdAsync(choiceId);
        choice.Options.RemoveMany(categoriesToRemove);
        await armorChoiceRepo.UpdateAsync(choice);
    }

    // Helpers
    // TODO might switch maps to switches later
    private readonly Dictionary<Type, Delegate> collectionGetters = new()
    {
        { typeof(SkillProficiencyChoice), (Feature f) => f.SkillProficiencyChoices },
        { typeof(WeaponCategoryProficiencyChoice), (Feature f) => f.WeaponCategoryProficiencyChoices },
        { typeof(ArmorProficiencyChoice), (Feature f) => f.ArmorProficiencyChoices },
        { typeof(ToolProficiencyChoice), (Feature f) => f.ToolProficiencyChoices },
        { typeof(LanguageChoice), (Feature f) => f.LanguageChoices },
        { typeof(WeaponTypeProficiencyChoice), (Feature f) => f.WeaponTypeProficiencyChoices },
    };

    private readonly Dictionary<Type, object> repositoryGetters = new()
    {
        { typeof(SkillProficiencyChoice), skillChoiceRepo },
        { typeof(WeaponCategoryProficiencyChoice), weaponCategoryChoiceRepo },
        { typeof(ArmorProficiencyChoice), armorChoiceRepo },
        { typeof(ToolProficiencyChoice), toolChoiceRepo },
        { typeof(LanguageChoice), languageChoiceRepo },
        { typeof(WeaponTypeProficiencyChoice), weaponTypeChoiceRepo },
    };

    private readonly Dictionary<Type, object> dtoRepositoryGetters = new()
    {
        { typeof(SkillProficiencyChoiceDto), skillChoiceRepo },
        { typeof(WeaponCategoryProficiencyChoiceDto), weaponCategoryChoiceRepo },
        { typeof(ArmorProficiencyChoiceDto), armorChoiceRepo },
        { typeof(ToolProficiencyChoiceDto), toolChoiceRepo },
        { typeof(LanguageProficiencyChoiceDto), languageChoiceRepo },
        { typeof(WeaponTypeProficiencyChoiceDto), weaponTypeChoiceRepo },
    };

    private readonly Dictionary<Type, Type> dtoToEntityMap = new()
    {
        { typeof(SkillProficiencyChoiceDto), typeof(SkillProficiencyChoice) },
        { typeof(LanguageProficiencyChoiceDto), typeof(LanguageChoice) },
        { typeof(ArmorProficiencyChoiceDto), typeof(ArmorProficiencyChoice) },
        { typeof(ToolProficiencyChoiceDto), typeof(ToolProficiencyChoice) },
        { typeof(WeaponCategoryProficiencyChoiceDto), typeof(WeaponCategoryProficiencyChoice) },
        { typeof(WeaponTypeProficiencyChoiceDto), typeof(WeaponTypeProficiencyChoice) },
        { typeof(AbilityIncreaseChoiceDto), typeof(AbilityIncreaseChoice) },
    };
}