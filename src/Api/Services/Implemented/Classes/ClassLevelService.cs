using Api.Middlewares.ExceptionHandling;
using Api.Models.Characters;
using Api.Models.DTOs.RequestDtos.Character;
using Api.Models.Features;
using Api.Repositories.Interfaces;
using Api.Services.Interfaces;
using static Api.Services.Util.QueryUtil;
namespace Api.Services.Implemented.Classes;

public partial class ClassLevelService(
    IBaseClassRepository classRepo,
    ISubclassRepository subclassRepo,
    IClassLevelRepository levelRepo,
    IFeatureRepository<ClassFeature> featureRepo,
    ICurrentUserService currentUserService,
    ILogger<BaseClassService> logger) : IClassLevelService
{
    public async Task<ClassLevel> GetByIdAsync(int id) => await levelRepo.GetByIdAsync(id);

    public async Task<ClassLevel> CreateAsync(CreateClassLevelRequestDto dto)
    {
        Class clss = dto.IsSubclassLevel
            ? await subclassRepo.GetWithLevelsAsync(dto.ClassId)
            : await classRepo.GetWithLevelsAsync(dto.ClassId);
 
        logger.LogInformation("Creating class level, Level: {ClassLevel}, ClassId: {ClassId}", dto.Level, dto.ClassId);

        ClassLevel level = new()
        {
            Level = dto.Level,
            Class = clss,
            ClassId = dto.ClassId,
            ProficiencyBonus = CalculateProficiencyBonus(dto.Level),
            SpellsKnown = dto.SpellsKnown,
            CantripsKnown = dto.CantripsKnown,
            SpellSlots = dto.SpellSlots,
            NewFeatures = [],

            CreatedAt = DateTime.UtcNow,
            CreatedBy = currentUserService.GetCurrentUserId(),
        };

        clss.ClassLevels.Add(level);
        level = await levelRepo.CreateAsync(level);
        logger.LogInformation("Successfully created class level, Level: {ClassLevel}, ClassId: {ClassId}, ID: {ClassLevelId}", level.Level, level.ClassId, level.Id);
        return level;
    }

    private static int CalculateProficiencyBonus(int level) => 1 + (level / 4);

    public async Task<ClassLevel> UpdateAsync(int id, UpdateClassLevelRequestDto dto)
    {
        var level = await levelRepo.GetByIdAsync(id);

        logger.LogInformation("Updating class level, Level: {ClassLevel}, ClassId: {ClassId}, ID: {ClassLevelId}", level.Level, level.ClassId, id);

        level.Level = dto.Level ?? level.Level;
        level.ProficiencyBonus = dto.ProficiencyBonus ?? level.ProficiencyBonus;
        level.CantripsKnown = dto.CantripsKnown ?? level.CantripsKnown;
        level.SpellsKnown = dto.SpellsKnown ?? level.SpellsKnown;
        level.SpellSlots = dto.SpellSlots ?? level.SpellSlots;

        if (dto.NewClassId is not null && level.ClassId != dto.NewClassId)
        {
            if (dto.NewClassIsSubclass == null)
                throw new ValidationException("NewClassIsSubclass must be provided when changing class of a class level");

            level.Class = dto.NewClassIsSubclass == true
                ? await subclassRepo.GetByIdAsync(dto.NewClassId.Value)
                : await classRepo.GetByIdAsync(dto.NewClassId.Value);
            level.Class.ClassLevels.Add(level);
        }

        await levelRepo.UpdateAsync(level);
        logger.LogInformation("Successfully updated class level, Level: {ClassLevel}, ClassId: {ClassId}, ID: {ClassLevelId}", level.Level, level.ClassId, id);
        return level;
    }

    public async Task DeleteAsync(int id)
    {
        var level = await levelRepo.GetByIdAsync(id);

        logger.LogInformation("Deleting class level, Level: {ClassLevel}, ID: {ClassLevelId}", level.Level, id);
        await levelRepo.DeleteAsync(level);
        logger.LogInformation("Successfully deleted class level, Level: {ClassLevel}, ID: {ClassLevelId}", level.Level, id);
    }

    public ICollection<ClassLevel> SortByLevel(ICollection<ClassLevel> levels, bool descending = false)
    {
        return OrderByMany(levels, [(l => l.Level)], descending);
    }

    public async Task<ClassLevel> AddFeatureAsync(int levelId, int featureId)
    {
        var level = await levelRepo.GetByIdAsync(levelId);
        var feature = await featureRepo.GetByIdAsync(featureId);

        logger.LogInformation("Adding feature to class level, FeatureId: {FeatureId}, ClassLevelId: {ClassLevelId}", featureId, levelId);
        level.NewFeatures.Add(feature);
        var updatedLevel = await levelRepo.UpdateAsync(level);
        logger.LogInformation("Successfully added feature to class level, FeatureId: {FeatureId}, ClassLevelId: {ClassLevelId}", featureId, levelId);
        return updatedLevel;
    }

    public async Task<ClassLevel> AddFeatureAsync(ClassLevel level, ClassFeature feature)
    {
        logger.LogInformation("Adding feature to class level, FeatureId: {FeatureId}, ClassLevelId: {ClassLevelId}", feature.Id, level.Id);
        level.NewFeatures.Add(feature);
        var updatedLevel = await levelRepo.UpdateAsync(level);
        logger.LogInformation("Successfully added feature to class level, FeatureId: {FeatureId}, ClassLevelId: {ClassLevelId}", feature.Id, level.Id);
        return updatedLevel;
    }

    public async Task<ClassLevel> RemoveFeatureAsync(int levelId, int featureId)
    {
        var level = await levelRepo.GetByIdAsync(levelId);
        var feature = level.NewFeatures.FirstOrDefault(f => f.Id == featureId)
            ?? throw new ValidationException($"Feature with id {featureId} is not a new feature of class level with id {levelId}");

        logger.LogInformation("Removing feature from class level, FeatureId: {FeatureId}, ClassLevelId: {ClassLevelId}", featureId, levelId);
        level.NewFeatures.Remove(feature);
        var updatedLevel = await levelRepo.UpdateAsync(level);
        logger.LogInformation("Successfully removed feature from class level, FeatureId: {FeatureId}, ClassLevelId: {ClassLevelId}", featureId, levelId);
        return updatedLevel;
    }

    public async Task<ClassLevel> RemoveFeatureAsync(ClassLevel level, ClassFeature feature)
    {
        logger.LogInformation("Removing feature from class level, FeatureId: {FeatureId}, ClassLevelId: {ClassLevelId}", feature.Id, level.Id);
        level.NewFeatures.Remove(feature);
        var updatedLevel = await levelRepo.UpdateAsync(level);
        logger.LogInformation("Successfully removed feature from class level, FeatureId: {FeatureId}, ClassLevelId: {ClassLevelId}", feature.Id, level.Id);
        return updatedLevel;
    }

    public async Task<ClassLevel> AddClassSlotAsync(int levelId, ClassSlotRequestDto dto)
    {
        var level = await levelRepo.GetByIdAsync(levelId);

        logger.LogInformation("Adding class slot to class level, SlotName: {SlotName}, ClassLevelId: {ClassLevelId}", dto.Name, levelId);
        level.ClassSlotsAtLevel.Add(new ClassSlot
        {
            Name = dto.Name,
            Quantity = dto.Quantity,
        });

        var updatedLevel = await levelRepo.UpdateAsync(level);
        logger.LogInformation("Successfully added class slot to class level, SlotName: {SlotName}, ClassLevelId: {ClassLevelId}", dto.Name, levelId);
        return updatedLevel;
    }

    public async Task<ClassLevel> RemoveClassSlotByNameAsync(int levelId, string slotName)
    {
        var level = await levelRepo.GetByIdAsync(levelId);
        var slot = level.ClassSlotsAtLevel.FirstOrDefault(s => s.Name == slotName)
            ?? throw new ValidationException($"Slot with name {slotName} is not a class slot of class level with id {levelId}");

        logger.LogInformation("Removing class slot from class level, SlotName: {SlotName}, ClassLevelId: {ClassLevelId}", slotName, levelId);
        level.ClassSlotsAtLevel.Remove(slot);
        var updatedLevel = await levelRepo.UpdateAsync(level);
        logger.LogInformation("Successfully removed class slot from class level, SlotName: {SlotName}, ClassLevelId: {ClassLevelId}", slotName, levelId);
        return updatedLevel;
    }
}