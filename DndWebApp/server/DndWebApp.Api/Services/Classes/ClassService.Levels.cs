using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs;
using DndWebApp.Api.Services.Generic;
using DndWebApp.Api.Services.Util;
namespace DndWebApp.Api.Services.Classes;

public partial class ClassService : IService<Class, ClassDto>
{
    public async Task<ClassLevel> AddLevelToClassAsync(ClassLevelDto dto)
    {
        ValidationUtil.AboveZeroOrThrow(dto.Level);
        ValidationUtil.AboveZeroOrThrow(dto.ClassId);
        ValidationUtil.AboveZeroOrThrow(dto.ProficiencyBonus);

        var clss = await repo.GetByIdAsync(dto.ClassId) ?? throw new NullReferenceException($"Class with id {dto.ClassId} could not be found");

        ClassLevel level = new()
        {
            Level = dto.Level,
            ClassId = dto.ClassId,
            ProficiencyBonus = dto.ProficiencyBonus,
            Class = clss
        };

        foreach (var featureId in dto.NewFeatureIds)
        {
            var feature = await featureRepo.GetByIdAsync(featureId) ?? throw new NullReferenceException($"Feature with id {featureId} could not be found");
            level.NewFeatures.Add(feature);
        }

        foreach (var slot in dto.ClassSpecificSlotsAtLevel)
        {
            level.ClassSpecificSlotsAtLevel.Add(new ClassSpecificSlot { Name = slot.Name, Quantity = slot.Quantity });
        }

        if (dto.SpellSlotsAtLevel is not null)
        {
            level.SpellSlotsAtLevel = new SpellSlotsAtLevel
            {
                SpellsKnown = dto.SpellSlotsAtLevel.SpellsKnown,
                CantripsKnown = dto.SpellSlotsAtLevel.CantripsKnown,
                Lvl1 = dto.SpellSlotsAtLevel.Lvl1,
                Lvl2 = dto.SpellSlotsAtLevel.Lvl2,
                Lvl3 = dto.SpellSlotsAtLevel.Lvl3,
                Lvl4 = dto.SpellSlotsAtLevel.Lvl4,
                Lvl5 = dto.SpellSlotsAtLevel.Lvl5,
                Lvl6 = dto.SpellSlotsAtLevel.Lvl6,
                Lvl7 = dto.SpellSlotsAtLevel.Lvl7,
                Lvl8 = dto.SpellSlotsAtLevel.Lvl8,
                Lvl9 = dto.SpellSlotsAtLevel.Lvl9,
            };
        }

        clss.ClassLevels.Add(level);
        return await levelRepo.CreateAsync(level);
    }

    public async Task UpdateClassLevelAsync(ClassLevelDto dto)
    {
        ValidationUtil.AboveZeroOrThrow(dto.Level);
        ValidationUtil.AboveZeroOrThrow(dto.ClassId);
        ValidationUtil.AboveZeroOrThrow(dto.ProficiencyBonus);

        var level = await levelRepo.GetByIdAsync(dto.Id) ?? throw new NullReferenceException($"Class level with id {dto.Id} could not be found");

        level.Level = dto.Level;
        level.ClassId = dto.ClassId;
        level.ProficiencyBonus = dto.ProficiencyBonus;

        if (level.ClassId != dto.ClassId)
        {
            var oldClass = await repo.GetByIdAsync(level.ClassId) ?? throw new NullReferenceException($"Class with id {level.ClassId} could not be found");
            var newClass = await repo.GetByIdAsync(dto.ClassId) ?? throw new NullReferenceException($"Class with id {dto.ClassId} could not be found");
            level.Class = newClass;
            newClass.ClassLevels.Add(level);
            oldClass.ClassLevels.Remove(level);

            await repo.UpdateAsync(oldClass);
            await repo.UpdateAsync(newClass);
        }

        level.ClassSpecificSlotsAtLevel.Clear();
        foreach (var slot in dto.ClassSpecificSlotsAtLevel)
        {
            level.ClassSpecificSlotsAtLevel.Add(new ClassSpecificSlot { Name = slot.Name, Quantity = slot.Quantity });
        }

        if (dto.SpellSlotsAtLevel is not null)
        {
            level.SpellSlotsAtLevel = new SpellSlotsAtLevel
            {
                SpellsKnown = dto.SpellSlotsAtLevel.SpellsKnown,
                CantripsKnown = dto.SpellSlotsAtLevel.CantripsKnown,
                Lvl1 = dto.SpellSlotsAtLevel.Lvl1,
                Lvl2 = dto.SpellSlotsAtLevel.Lvl2,
                Lvl3 = dto.SpellSlotsAtLevel.Lvl3,
                Lvl4 = dto.SpellSlotsAtLevel.Lvl4,
                Lvl5 = dto.SpellSlotsAtLevel.Lvl5,
                Lvl6 = dto.SpellSlotsAtLevel.Lvl6,
                Lvl7 = dto.SpellSlotsAtLevel.Lvl7,
                Lvl8 = dto.SpellSlotsAtLevel.Lvl8,
                Lvl9 = dto.SpellSlotsAtLevel.Lvl9,
            };
        }

        await levelRepo.UpdateAsync(level);
    }

    public async Task DeleteClassLevelAsync(int id)
    {
        var level = await levelRepo.GetByIdAsync(id) ?? throw new NullReferenceException($"Class level with id {id} could not be found");
        await levelRepo.DeleteAsync(level);
    }

    public async Task<ICollection<ClassLevel>> GetAllLevelsAsync(int classId)
    {
        var classWithLevels = await repo.GetWithClassLevelsAsync(classId) ?? throw new NullReferenceException($"Class with id {classId} could not be found");
        return classWithLevels.ClassLevels;
    }

    public async Task<ClassLevel> GetLevelByIdAsync(int id)
    {
        return await levelRepo.GetByIdAsync(id) ?? throw new NullReferenceException($"Class level with id {id} could not be found");
    }

    public ICollection<ClassLevel> SortBy(ICollection<ClassLevel> levels, bool descending = false)
    {
        return SortUtil.OrderByMany(levels, [(l => l.Level)], descending);
    }
}