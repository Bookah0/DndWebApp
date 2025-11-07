using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs;
using DndWebApp.Api.Repositories.Interfaces;
using DndWebApp.Api.Services.Interfaces;
using DndWebApp.Api.Services.Util;
namespace DndWebApp.Api.Services.Implemented.Classes;

public partial class ClassLevelService : IClassLevelService
{
    private readonly IClassLevelRepository levelRepo;
    private readonly IClassRepository classRepo;
    private readonly ISubclassRepository subclassRepo;
    private readonly IClassFeatureRepository featureRepo;
    private readonly ILogger<ClassService> logger;

    public ClassLevelService(IClassRepository classRepo, ISubclassRepository subclassRepo, IClassLevelRepository levelRepo, IClassFeatureRepository featureRepo, ILogger<ClassService> logger)
    {
        this.levelRepo = levelRepo;
        this.classRepo = classRepo;
        this.subclassRepo = subclassRepo;
        this.featureRepo = featureRepo;
        this.logger = logger;
    }

    public async Task<ClassLevel> AddLevelToClassAsync(ClassLevelDto dto)
    {
        ValidationUtil.AboveZeroOrThrow(dto.Level);
        ValidationUtil.AboveZeroOrThrow(dto.ClassId);
        ValidationUtil.AboveZeroOrThrow(dto.ProficiencyBonus);

        AClass? clss;

        if (!dto.isSubclassLevel)
            clss = await classRepo.GetByIdAsync(dto.ClassId)
            ?? throw new NullReferenceException($"No class with id {dto.ClassId} can be found");
        else
            clss = await subclassRepo.GetByIdAsync(dto.ClassId)
            ?? throw new NullReferenceException($"No subclass with id {dto.ClassId} can be found");

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
            var feature = await featureRepo.GetByIdAsync(featureId) ?? throw new NullReferenceException($"Feature with id {featureId} could not be found");
            level.NewFeatures.Add(feature);
        }

        foreach (var slot in dto.ClassSpecificSlotsAtLevel)
        {
            level.ClassSpecificSlotsAtLevel.Add(new ClassSpecificSlot { Name = slot.Name, Quantity = slot.Quantity });
        }

        clss.ClassLevels.Add(level);
        return await levelRepo.CreateAsync(level);
    }

    public async Task EditClassLevelAsync(ClassLevelDto dto)
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
            AClass? newClass;

            if (!dto.isSubclassLevel)
            {
                newClass = await classRepo.GetByIdAsync(dto.ClassId) ?? throw new NullReferenceException($"No class with id {dto.ClassId} can be found");
                level.Class = newClass;
                newClass.ClassLevels.Add(level);
                await classRepo.UpdateAsync((Class)newClass);
            }
            else
            {
                newClass = await subclassRepo.GetByIdAsync(dto.ClassId) ?? throw new NullReferenceException($"No subclass with id {dto.ClassId} can be found");
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
    }

    public async Task DeleteClassLevelAsync(int id)
    {
        var level = await levelRepo.GetByIdAsync(id) ?? throw new NullReferenceException($"Class level with id {id} could not be found");
        await levelRepo.DeleteAsync(level);
    }

    public async Task<ICollection<ClassLevel>> GetAllLevelsAsync(int classId, bool isSubclass)
    {
        if (isSubclass)
        {
            var classWithLevels = await subclassRepo.GetByIdAsync(classId) ?? throw new NullReferenceException($"No subclass with id {classId} can be found");
            return classWithLevels.ClassLevels;
        }
        else
        {
            var classWithLevels = await classRepo.GetByIdAsync(classId) ?? throw new NullReferenceException($"No class with id {classId} can be found");
            return classWithLevels.ClassLevels;
        }
    }

    public async Task<ClassLevel> GetLevelByIdAsync(int id)
    {
        return await levelRepo.GetByIdAsync(id) ?? throw new NullReferenceException($"Class level with id {id} could not be found");
    }

    public ICollection<ClassLevel> SortByLevel(ICollection<ClassLevel> levels, bool descending = false)
    {
        return SortUtil.OrderByMany(levels, [(l => l.Level)], descending);
    }
}