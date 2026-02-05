using DndWebApp.Api.Middlewares.ExceptionHandling;
using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs.RequestDtos.Character;
using DndWebApp.Api.Models.Features;
using DndWebApp.Api.Repositories.Interfaces;
using DndWebApp.Api.Services.Interfaces;
using static DndWebApp.Api.Services.Util.SortUtil;
namespace DndWebApp.Api.Services.Implemented.Classes;

public partial class ClassLevelService(
    IClassRepository classRepo,
    ISubclassRepository subclassRepo,
    IClassLevelRepository levelRepo,
    IFeatureRepository<ClassFeature> featureRepo,
    ILogger<ClassService> logger) : IClassLevelService
{
    public async Task<ClassLevel> CreateAsync(ClassLevelDto dto)
    {
        AClass clss = dto.IsSubclassLevel 
            ?  await classRepo.GetByIdAsync(dto.ClassId)
            : await subclassRepo.GetByIdAsync(dto.ClassId);
        
        logger.LogInformation("Creating class level, Level: {ClassLevel}, ClassId: {ClassId}", dto.Level, dto.ClassId);

        ClassLevel level = new()
        {
            Level = dto.Level,
            ClassId = dto.ClassId,
            ProficiencyBonus = dto.ProficiencyBonus,
            Class = clss,
            SpellsKnown = dto.SpellsKnown,
            CantripsKnown = dto.CantripsKnown,
            SpellSlots = dto.SpellSlotsAtLevel
        };

        foreach (var featureId in dto.NewFeatureIds)
        {
            var feature = await featureRepo.GetByIdAsync(featureId);
            level.NewFeatures.Add(feature);
        }

        foreach (var slot in dto.ClassSpecificSlotsAtLevel)
        {
            level.ClassSpecificSlotsAtLevel.Add(new ClassSpecificSlot { Name = slot.Name, Quantity = slot.Quantity });
        }

        clss.ClassLevels.Add(level);
        level = await levelRepo.CreateAsync(level);
        logger.LogInformation("Successfully created class level, Level: {ClassLevel}, ClassId: {ClassId}, ID: {ClassLevelId}", level.Level, level.ClassId, level.Id);
        return level;
    }

    public async Task<ClassLevel> UpdateAsync(int id, ClassLevelDto dto)
    {
        var level = await levelRepo.GetByIdAsync(id) ;

        logger.LogInformation("Updating class level, Level: {ClassLevel}, ClassId: {ClassId}, ID: {ClassLevelId}", level.Level, level.ClassId, id);

        level.Level = dto.Level;
        level.ClassId = dto.ClassId;
        level.ProficiencyBonus = dto.ProficiencyBonus;

        if (level.ClassId != dto.ClassId)
        {
            AClass? newClass;

            if (!dto.IsSubclassLevel)
            {
                newClass = await classRepo.GetByIdAsync(dto.ClassId);
                level.Class = newClass;
                newClass.ClassLevels.Add(level);
                await classRepo.UpdateAsync((Class)newClass);
            }
            else
            {
                newClass = await subclassRepo.GetByIdAsync(dto.ClassId);
                level.Class = newClass;
                newClass.ClassLevels.Add(level);
                await subclassRepo.UpdateAsync((Subclass)newClass);
            }
        }

        level.ClassSpecificSlotsAtLevel.Clear();
        foreach (var slot in dto.ClassSpecificSlotsAtLevel)
        {
            level.ClassSpecificSlotsAtLevel.Add(new ClassSpecificSlot { Name = slot.Name, Quantity = slot.Quantity });
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

    public async Task<ClassLevel> GetByIdAsync(int id) => await levelRepo.GetByIdAsync(id);

    public ICollection<ClassLevel> SortByLevel(ICollection<ClassLevel> levels, bool descending = false)
    {
        return OrderByMany(levels, [(l => l.Level)], descending);
    }
}