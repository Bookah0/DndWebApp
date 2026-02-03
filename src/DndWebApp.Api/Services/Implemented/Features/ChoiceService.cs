using AutoMapper;
using DndWebApp.Api.Middlewares.ExceptionHandling;
using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs.Features;
using DndWebApp.Api.Models.DTOs.RequestDtos.Character;
using DndWebApp.Api.Models.Features;
using DndWebApp.Api.Models.Items.Constants;
using DndWebApp.Api.Repositories.Interfaces;
using DndWebApp.Api.Services.Interfaces.Features;
using DndWebApp.Api.Services.Util;
using static DndWebApp.Api.Services.Util.ConstantsUtil;

namespace DndWebApp.Api.Services.Implemented.Features;

public class ChoiceService<T>
( 
    IAbilityRepository abilityRepo,
    IAbilityValueRepository abilityValueRepo,
    ILanguageRepository languageRepo,
    ISkillRepository skillRepo,
    IFeatureService<T, AFeatureDto> featureService,

    IChoiceRepository<AbilityIncreaseChoice> abilityChoiceRepo,
    IChoiceRepository<SkillProficiencyChoice> skillChoiceRepo,
    IChoiceRepository<LanguageChoice> languageChoiceRepo,
    IChoiceRepository<ToolProficiencyChoice> toolChoiceRepo,
    IChoiceRepository<ArmorProficiencyChoice> armorChoiceRepo,
    IChoiceRepository<WeaponCategoryProficiencyChoice> weaponCategoryChoiceRepo,
    IChoiceRepository<WeaponTypeProficiencyChoice> weaponTypeChoiceRepo,
    ILogger<ChoiceService<T>> logger,
    IMapper mapper) 
    : IChoiceService<T> where T : AFeature
{

    public async Task ClearChoices<C>(int featureId) where C : IFeatureChoice
    {
        var feature = await featureService.GetByIdAsync(featureId)
            ?? throw new NotFoundException($"Feature with id {featureId} could not be found");

        var repo = GetRepositoryOrThrow<C>();
        var choices = GetCollectionOrThrow<C>(feature);

        logger.LogInformation("Clearing all proficiency choices from feature with Name: {FeatureName}, ID: {FeatureId}", feature.Name, feature.Id);

        foreach (var choice in choices)
        {
            if(choice.FeatureId != feature.Id) 
                throw new ValidationException($"Choice with id {choice.Id} does not belong to feature with id {feature.Id}");

            await repo.DeleteAsync(choice);
        }
        logger.LogInformation("Successfully cleared all proficiency choices from feature with Name: {FeatureName}, ID: {FeatureId}", feature.Name, feature.Id);
    }

    public async Task AddChoice<CDto>(CDto dto, int featureId) where CDto : AChoiceDto
    {
        var feature = await featureService.GetByIdAsync(featureId)
            ?? throw new NotFoundException($"Feature with id {featureId} could not be found");
        
        var entityType = GetEntityTypeThrow<CDto>();
        var repo = (IChoiceRepository<IFeatureChoice>)GetDtoRepositoryOrThrow<SkillProficiencyChoiceDto>();

        logger.LogInformation("Adding proficiency choice {ProficiencyChoiceDto} to feature with Name: {FeatureName}, ID: {FeatureId}", dto, feature.Name, feature.Id);

        var choice = mapper.Map(dto, entityType) as IFeatureChoice;
        choice!.FeatureId = feature.Id;
        await repo.CreateAsync(choice);

        logger.LogInformation("Successfully added proficiency choice {ProficiencyChoiceDto} to feature with Name: {FeatureName}, ID: {FeatureId}", dto, feature.Name, feature.Id);
    }

    public async Task RemoveChoice<C>(int choiceId, int featureId) where C : IFeatureChoice
    {
        var feature = await featureService.GetByIdAsync(featureId)
            ?? throw new NotFoundException($"Feature with id {featureId} could not be found");
        
        var repo = GetRepositoryOrThrow<C>();

        var choice = await repo.GetByIdAsync(choiceId) 
            ?? throw new NotFoundException($"Choice with id {choiceId} could not be found");

        if (choice.FeatureId != feature.Id)
            throw new ValidationException($"Choice with id {choiceId} does not belong to feature with id {feature.Id}");

        logger.LogInformation("Removing proficiency choice option with ID: {ChoiceId} from feature with Name: {FeatureName}, ID: {FeatureId}", choiceId, feature.Name, feature.Id);
        await repo.DeleteAsync(choice);
        logger.LogInformation("Successfully removed proficiency choice option with ID: {ChoiceId} from feature with Name: {FeatureName}, ID: {FeatureId}", choiceId, feature.Name, feature.Id);
    }

    public async Task AddSkillOptions(int choiceId, ICollection<int> newSkillIds)
    {
        var choice = await skillChoiceRepo.GetByIdAsync(choiceId)
            ?? throw new NotFoundException($"Skill choice with id {choiceId} could not be found");

        foreach (var id in newSkillIds)
        {
            var skill = await skillRepo.GetByIdAsync(id)
                ?? throw new NotFoundException($"Skill with id {id} could not be found");

            choice.Options.Add(skill);
        }
        await skillChoiceRepo.UpdateAsync(choice);
    }

    public async Task AddLanguageOptions(int choiceId, ICollection<int> newLanguageIds)
    {
        var choice = await languageChoiceRepo.GetByIdAsync(choiceId)
            ?? throw new NotFoundException($"Language choice with id {choiceId} could not be found");

        foreach (var id in newLanguageIds)
        {
            var language = await languageRepo.GetByIdAsync(id)
                ?? throw new NotFoundException($"Language with id {id} could not be found");

            choice.Options.Add(language);
        }
        await languageChoiceRepo.UpdateAsync(choice);
    }

    public async Task AddAbilityOptions(int choiceId, ICollection<AbilityValueDto> newAbilityValues)
    {
        var choice = await abilityChoiceRepo.GetByIdAsync(choiceId)
            ?? throw new NotFoundException($"Ability Increase choice with id {choiceId} could not be found");

        foreach (var dto in newAbilityValues)
        {
            var ability = await abilityRepo.GetByIdAsync(dto.AbilityId)
                ?? throw new NotFoundException($"Ability with id {dto.AbilityId} could not be found");

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
    }

    public async Task AddWeaponCategoryOptions(int choiceId, ICollection<string> newCategories)
    {
        var choice = await weaponCategoryChoiceRepo.GetByIdAsync(choiceId)
            ?? throw new NotFoundException($"Weapon Category choice with id {choiceId} could not be found");

        foreach (var category in newCategories)
        {
            var resolvedCategory = ResolveOptionOrThrow(category, WeaponCategory.AllowedValues, "Weapon Category");
            choice.Options.Add(resolvedCategory);
        }
        await weaponCategoryChoiceRepo.UpdateAsync(choice);
    }

    public async Task AddWeaponTypeOptions(int choiceId, ICollection<string> newTypes)
    {
        var choice = await weaponTypeChoiceRepo.GetByIdAsync(choiceId)
            ?? throw new NotFoundException($"Weapon Type choice with id {choiceId} could not be found");

        foreach (var type in newTypes)
        {
            var resolvedType = ResolveOptionOrThrow(type, WeaponType.AllowedValues, "Weapon Type");
            choice.Options.Add(resolvedType);
        }
        await weaponTypeChoiceRepo.UpdateAsync(choice);
    }

    public async Task AddToolCategoryOptions(int choiceId, ICollection<string> newCategories)
    {
        var choice = await toolChoiceRepo.GetByIdAsync(choiceId)
            ?? throw new NotFoundException($"Tool choice with id {choiceId} could not be found");

        foreach (var category in newCategories)
        {
            var resolvedCategory = ResolveOptionOrThrow(category, ToolCategory.AllowedValues, "Tool Category");
            choice.Options.Add(resolvedCategory);
        }
        await toolChoiceRepo.UpdateAsync(choice);
    }

    public async Task AddArmorCategoryOptions(int choiceId, ICollection<string> newCategories)
    {
        var choice = await armorChoiceRepo.GetByIdAsync(choiceId)
            ?? throw new NotFoundException($"Armor choice with id {choiceId} could not be found");

        foreach (var category in newCategories)
        {
            var resolvedCategory = ResolveOptionOrThrow(category, ArmorCategory.AllowedValues, "Armor Category");
            choice.Options.Add(resolvedCategory);
        }
        await armorChoiceRepo.UpdateAsync(choice);
    }

    public async Task RemoveSkillOptions(int choiceId, ICollection<int> skillIdsToRemove)
    {
        var choice = await skillChoiceRepo.GetByIdAsync(choiceId)
            ?? throw new NotFoundException($"Skill choice with id {choiceId} could not be found");

        foreach (var id in skillIdsToRemove)
        {
            var skill = await skillRepo.GetByIdAsync(id)
                ?? throw new NotFoundException($"Skill with id {id} could not be found");

            var removed = choice.Options.Remove(skill);

            if (!removed)
                throw new ValidationException($"Skill with id {id} is not an option of choice with id {choiceId}");
        }

        await skillChoiceRepo.UpdateAsync(choice);
    }

    public async Task RemoveLanguageOptions(int choiceId, ICollection<int> languageIdsToRemove)
    {
        var choice = await languageChoiceRepo.GetByIdAsync(choiceId)
            ?? throw new NotFoundException($"Language choice with id {choiceId} could not be found");

        foreach (var id in languageIdsToRemove)
        {
            var language = await languageRepo.GetByIdAsync(id)
                ?? throw new NotFoundException($"Language with id {id} could not be found");

            var removed = choice.Options.Remove(language);

            if (!removed)
                throw new ValidationException($"Language with id {id} is not an option of choice with id {choiceId}");
        }
        await languageChoiceRepo.UpdateAsync(choice);
    }

    public async Task RemoveAbilityOptions(int choiceId, ICollection<int> idsToRemove)
    {
        var choice = await abilityChoiceRepo.GetByIdAsync(choiceId)
            ?? throw new NotFoundException($"Ability Increase choice with id {choiceId} could not be found");

        foreach (var id in idsToRemove)
        {
            var ability = await abilityRepo.GetByIdAsync(id)
                ?? throw new NotFoundException($"Ability with id {id} could not be found");

            var abilityValue = choice.Options.FirstOrDefault(av => av.AbilityId == id) 
                ?? throw new NotFoundException($"Ability value with AbilityId {id} could not be found in choice with id {choiceId}");

            var removed = choice.Options.Remove(abilityValue);

            if (!removed)
                throw new ValidationException($"Ability with id {id} is not an option of choice with id {choiceId}");
        }
        await abilityChoiceRepo.UpdateAsync(choice);
    }

    public async Task RemoveWeaponCategoryOptions(int choiceId, ICollection<string> categoriesToRemove)
    {
        var choice = await weaponCategoryChoiceRepo.GetByIdAsync(choiceId)
            ?? throw new NotFoundException($"Weapon Category choice with id {choiceId} could not be found");

        choice.Options.RemoveMany(categoriesToRemove);
        await weaponCategoryChoiceRepo.UpdateAsync(choice);
    }

    public async Task RemoveWeaponTypeOptions(int choiceId, ICollection<string> typesToRemove)
    {
        var choice = await weaponTypeChoiceRepo.GetByIdAsync(choiceId)
            ?? throw new NotFoundException($"Weapon Type choice with id {choiceId} could not be found");

        choice.Options.RemoveMany(typesToRemove);
        await weaponTypeChoiceRepo.UpdateAsync(choice);
    }

    public async Task RemoveToolCategoryOptions(int choiceId, ICollection<string> categoriesToRemove)
    {
        var choice = await toolChoiceRepo.GetByIdAsync(choiceId)
            ?? throw new NotFoundException($"Tool choice with id {choiceId} could not be found");

        choice.Options.RemoveMany(categoriesToRemove);
        await toolChoiceRepo.UpdateAsync(choice);
    }

    public async Task RemoveArmorCategoryOptions(int choiceId, ICollection<string> categoriesToRemove)
    {
        var choice = await armorChoiceRepo.GetByIdAsync(choiceId)
            ?? throw new NotFoundException($"Armor choice with id {choiceId} could not be found");

        choice.Options.RemoveMany(categoriesToRemove);
        await armorChoiceRepo.UpdateAsync(choice);
    }

    // Helpers
    private IChoiceRepository<C> GetRepositoryOrThrow<C>() where C : IFeatureChoice 
    {
        if (!repositoryGetters.TryGetValue(typeof(C), out var repo))
            throw new InvalidOperationException($"Unknown Choice type: {typeof(C).Name}");
        
        return (IChoiceRepository<C>)repo;
    }

    private object GetDtoRepositoryOrThrow<CDto>() where CDto : AChoiceDto
    {
        if (!repositoryGetters.TryGetValue(typeof(CDto), out var repo))
            throw new InvalidOperationException($"Unknown Choice type: {typeof(CDto).Name}");
        
        return repo;
    }

    private Type GetEntityTypeThrow<CDto>() where CDto : AChoiceDto
    {
        if (!dtoToEntityMap.TryGetValue(typeof(CDto), out var entityType))
            throw new InvalidOperationException($"Unknown Choice type: {typeof(CDto).Name}");
        
        return entityType;
    } 

    private ICollection<C> GetCollectionOrThrow<C>(AFeature feature) where C : IFeatureChoice
    {
        if (!collectionGetters.TryGetValue(typeof(C), out var collectionGetter))
            throw new InvalidOperationException($"Unknown Choice type: {typeof(C).Name}");

        return (ICollection<C>)collectionGetter.DynamicInvoke(feature)!;
    } 


    private static async Task RemoveChoiceFromFeature<I>(IChoiceRepository<I> choiceRepo, int choiceId, int featureId) where I : IFeatureChoice
    {
        var choice = await choiceRepo.GetByIdAsync(choiceId)
            ?? throw new NotFoundException($"Choice with id {choiceId} could not be found");

        if (choice.FeatureId != featureId)
            throw new ValidationException($"Choice with id {choiceId} does not belong to feature with id {featureId}");

        await choiceRepo.DeleteAsync(choice);
    }
    // TODO Maps (might switch to switches later)
    private readonly Dictionary<Type, Delegate> collectionGetters = new()
    {
        { typeof(SkillProficiencyChoice), (AFeature f) => f.SkillProficiencyChoices },
        { typeof(WeaponCategoryProficiencyChoice), (AFeature f) => f.WeaponCategoryProficiencyChoices },
        { typeof(ArmorProficiencyChoice), (AFeature f) => f.ArmorProficiencyChoices },
        { typeof(ToolProficiencyChoice), (AFeature f) => f.ToolProficiencyChoices },
        { typeof(LanguageChoice), (AFeature f) => f.LanguageChoices },
        { typeof(WeaponTypeProficiencyChoice), (AFeature f) => f.WeaponTypeProficiencyChoices },
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