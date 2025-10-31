using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs;
using DndWebApp.Api.Models.Features;
using DndWebApp.Api.Repositories;
using DndWebApp.Api.Repositories.Classes;
using DndWebApp.Api.Repositories.Items;
using DndWebApp.Api.Services.Generic;
using DndWebApp.Api.Services.Util;
namespace DndWebApp.Api.Services;

// Class main service
public partial class ClassService : IService<Class, ClassDto, ClassDto>
{
    internal IClassRepository repo;
    internal IItemRepository itemRepo;
    internal IRepository<Option> choicesRepo;

    public ClassService(IClassRepository repo, IItemRepository itemRepo, IRepository<Option> choicesRepo)
    {
        this.repo = repo;
        this.itemRepo = itemRepo;
        this.choicesRepo = choicesRepo;
    }

    public async Task<Class> CreateAsync(ClassDto dto)
    {
        ValidationUtil.ValidateRequiredString(dto.Name);
        ValidationUtil.ValidateRequiredString(dto.Description);
        ValidationUtil.ValidateRequiredString(dto.HitDie);

        Class cls = new()
        {
            Name = dto.Name,
            Description = dto.Description,
            HitDie = dto.HitDie,
            ClassLevels = []
        };

        return await repo.CreateAsync(cls);
    }

    public async Task DeleteAsync(int id)
    {
        var cls = await repo.GetByIdAsync(id) ?? throw new NullReferenceException($"Class with id {id} could not be found");
        await repo.DeleteAsync(cls);
    }

    public async Task<ICollection<Class>> GetAllAsync()
    {
        return await repo.GetAllAsync();
    }

    public async Task<Class> GetByIdAsync(int id)
    {
        return await repo.GetByIdAsync(id) ?? throw new NullReferenceException($"Class with id {id} could not be found");
    }

    public async Task UpdateAsync(ClassDto dto)
    {
        ValidationUtil.ValidateRequiredString(dto.Name);
        ValidationUtil.ValidateRequiredString(dto.Description);
        ValidationUtil.ValidateRequiredString(dto.HitDie);

        var cls = await repo.GetByIdAsync(dto.Id) ?? throw new NullReferenceException($"Class with id {dto.Id} could not be found");

        cls.Name = dto.Name;
        cls.Description = dto.Description;
        cls.HitDie = dto.HitDie;

        await repo.UpdateAsync(cls);
    }

    public async Task AddStartingEquipment(int equipmentId, int classId)
    {
        var equipment = await itemRepo.GetByIdAsync(equipmentId) ?? throw new NullReferenceException($"Item with id {equipmentId} could not be found");
        var clss = await repo.GetByIdAsync(classId) ?? throw new NullReferenceException($"Class with id {classId} could not be found");

        clss.StartingEquipment.Add(equipment);
    }

    public async Task RemoveStartingEquipment(int equipmentId, int classId)
    {
        var equipment = await itemRepo.GetByIdAsync(equipmentId) ?? throw new NullReferenceException($"Item with id {equipmentId} could not be found");
        var clss = await repo.GetByIdAsync(classId) ?? throw new NullReferenceException($"Class with id {classId} could not be found");

        clss.StartingEquipment.Remove(equipment);
    }

    public async Task AddStartingEquipmentChoice(int choiceId, int classId)
    {
        var equipmentOption = await choicesRepo.GetByIdAsync(choiceId) ?? throw new NullReferenceException($"Choice with id {choiceId} could not be found");
        if (equipmentOption is not ItemOption)
            throw new ArgumentException($"Option should be of type ItemOptions but was of type {equipmentOption.GetType()} could not be found");

        var clss = await repo.GetByIdAsync(classId) ?? throw new NullReferenceException($"Class with id {classId} could not be found");

        clss.StartingEquipmentOptions.Add((ItemOption)equipmentOption);
    }

    public ICollection<Class> SortBy(ICollection<Class> classes, bool descending = false)
    {
        return SortUtil.OrderByMany(classes, [(c => c.Name)], descending);
    }
}