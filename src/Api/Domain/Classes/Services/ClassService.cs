using Api.Domain.Classes.DTOs;
using Api.Domain.Classes.Models;
using Api.Domain.Classes.Repositories;
using Api.Domain.Items.Repositories;
using Api.Domain.Shared.Utils;
using Api.Domain.Users.Services;
using Api.Infrastructure.Middleware.ExceptionHandling;

namespace Api.Domain.Classes.Services;

public partial class BaseClassService(
    IBaseClassRepository repo, 
    IClassLevelRepository classLevelRepo, 
    IItemRepository itemRepo, 
    ICurrentUserService currentUserService, 
    ILogger<BaseClassService> logger) : IBaseClassService
{
    public async Task<BaseClass> CreateAsync(CreateClassRequestDto dto)
    {
        logger.LogInformation("Creating class, Name: {ClassName}", dto.Name);

        var clss = await repo.CreateAsync(new()
        {
            Name = dto.Name,
            Description = dto.Description,
            HitDie = dto.HitDie,
            SpellcastingAbilityId = dto.SpellcastingAbilityId,
            ClassLevels = [],
            Subclasses = [],
            StartingEquipment = [],
            StartingEquipmentChoices = [],
            CreatedAt = DateTime.UtcNow,
            CreatedBy = currentUserService.GetCurrentUserId(),
        });

        logger.LogInformation("Successfully created class, Name: {ClassName}, ID: {ClassId}", clss.Name, clss.Id);
        return clss;
    }

    public async Task DeleteAsync(int id)
    {
        var clss = await repo.GetByIdAsync(id);
        logger.LogInformation("Deleting class, Name: {ClassName}, ID: {ClassId}", clss.Name, id);
        await repo.DeleteAsync(clss);
        logger.LogInformation("Successfully deleted class, Name: {ClassName}, ID: {ClassId}", clss.Name, id);
    }

    public async Task<ICollection<BaseClass>> GetAllAsync() => await repo.GetAllAsync();
    public async Task<ICollection<BaseClass>> GetAllWithLevelFeaturesAsync() => await repo.GetAllWithLevelFeaturesAsync();
    public async Task<BaseClass> GetWithStartingEquipmentAsync(int id) => await repo.GetWithStartingEquipmentAsync(id);
    public async Task<BaseClass> GetWithLevelsAsync(int id) => await repo.GetWithLevelsAsync(id);
    public async Task<BaseClass> GetWithFeaturesAsync(int id) => await repo.GetWithLevelFeaturesAsync(id);
    public async Task<BaseClass> GetWithSubclassesAsync(int id) => await repo.GetWithSubclassesAsync(id);
    public async Task<BaseClass> GetByIdAsync(int id) => await repo.GetByIdAsync(id);

    public async Task<BaseClass> UpdateAsync(int id, UpdateClassRequestDto dto)
    {
        var clss = await repo.GetByIdAsync(id);
        logger.LogInformation("Updating class, Name: {ClassName}, ID: {ClassId}", clss.Name, id);

        clss.Name = dto.Name ?? clss.Name;
        clss.Description = dto.Description ?? clss.Description;
        clss.HitDie = dto.HitDie ?? clss.HitDie;   
        clss.SpellcastingAbilityId = dto.SpellcastingAbilityId ?? clss.SpellcastingAbilityId;
        clss.IsPublic = dto.IsPublic ?? clss.IsPublic;
        clss.CloningAllowed = dto.CloningAllowed ?? clss.CloningAllowed;
        clss.UpdatedAt = DateTime.UtcNow;

        await repo.UpdateAsync(clss);
        logger.LogInformation("Successfully updated class, Name: {ClassName}, ID: {ClassId}", clss.Name, id);
        return clss;
    }

    public async Task<BaseClass> AddClassLevel(int id, int classLevelId)
    {
        var clss = await repo.GetByIdAsync(id);
        var classLevel = await classLevelRepo.GetByIdAsync(classLevelId);

        if(classLevel.ClassId != clss.Id)
            throw new ValidationException($"Class level with id '{classLevelId}' does not belong to class with id '{id}'.");

        logger.LogInformation("Adding class level to class, ClassName: {ClassName}, ClassId: {ClassId}, ClassLevelId: {ClassLevelId}", clss.Name, id, classLevelId);
        clss.ClassLevels.Add(classLevel);
        await repo.UpdateAsync(clss);
        logger.LogInformation("Successfully added class level to class, ClassName: {ClassName}, ClassId: {ClassId}, ClassLevelId: {ClassLevelId}", clss.Name, id, classLevelId);
        return clss;
    }

    public async Task<BaseClass> RemoveClassLevel(int id, int classLevelId)
    {
        var clss = await repo.GetByIdAsync(id);
        var classLevel = await classLevelRepo.GetByIdAsync(classLevelId);

        if(classLevel.ClassId != clss.Id)
            throw new ValidationException($"Class level with id '{classLevelId}' does not belong to class with id '{id}'.");

        logger.LogInformation("Removing class level from class, ClassName: {ClassName}, ClassId: {ClassId}, ClassLevelId: {ClassLevelId}", clss.Name, id, classLevelId);
        clss.ClassLevels.Remove(classLevel);
        await repo.UpdateAsync(clss);
        logger.LogInformation("Successfully removed class level from class, ClassName: {ClassName}, ClassId: {ClassId}, ClassLevelId: {ClassLevelId}", clss.Name, id, classLevelId);
        return clss;
    }

    public async Task<BaseClass> AddStartingEquipment(int id, int equipmentId)
    {
        var clss = await repo.GetByIdAsync(id);
        var equipment = await itemRepo.GetByIdAsync(equipmentId);

        logger.LogInformation("Adding starting equipment to class, ClassName: {ClassName}, ClassId: {ClassId}, EquipmentName: {EquipmentName}, EquipmentId: {EquipmentId}", clss.Name, id, equipment.Name, equipmentId);
        clss.StartingEquipment.Add(equipment);
        await repo.UpdateAsync(clss);
        logger.LogInformation("Successfully added starting equipment to class, ClassName: {ClassName}, ClassId: {ClassId}, EquipmentName: {EquipmentName}, EquipmentId: {EquipmentId}", clss.Name, id, equipment.Name, equipmentId);
        return clss;
    }

    public async Task<BaseClass> RemoveStartingEquipment(int id, int equipmentId)
    {
        var clss = await repo.GetByIdAsync(id);
        var equipment = await itemRepo.GetByIdAsync(equipmentId);

        logger.LogInformation("Removing starting equipment from class, ClassName: {ClassName}, ClassId: {ClassId}, EquipmentName: {EquipmentName}, EquipmentId: {EquipmentId}", clss.Name, id, equipment.Name, equipmentId);
        clss.StartingEquipment.Remove(equipment);
        await repo.UpdateAsync(clss);
        logger.LogInformation("Successfully removed starting equipment from class, ClassName: {ClassName}, ClassId: {ClassId}, EquipmentName: {EquipmentName}, EquipmentId: {EquipmentId}", clss.Name, id, equipment.Name, equipmentId);
        return clss;
    }

    public ICollection<BaseClass> SortBy(ICollection<BaseClass> classes, bool descending = false)
    {
        return QueryUtil.OrderByMany(classes, [(c => c.Name)], descending);
    }
}