using DndWebApp.Api.Middlewares.ExceptionHandling;
using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs.RequestDtos.Character;
using DndWebApp.Api.Repositories.Interfaces;
using DndWebApp.Api.Services.Interfaces;
using static DndWebApp.Api.Services.Util.SortUtil;

namespace DndWebApp.Api.Services.Implemented.Classes;

public partial class ClassService(IClassRepository repo, ILogger<ClassService> logger) : IClassService
{
    public async Task<Class> CreateAsync(ClassDto dto)
    {
        logger.LogInformation("Creating class, Name: {ClassName}", dto.Name);

        var clss = await repo.CreateAsync(new()
        {
            Name = dto.Name,
            Description = dto.Description,
            HitDie = dto.HitDie,
            ClassLevels = []
        });

        logger.LogInformation("Successfully created class, Name: {ClassName}, ID: {ClassId}", clss.Name, clss.Id);
        return clss;
    }

    public async Task DeleteAsync(int id)
    {
        var clss = await repo.GetByIdAsync(id) ?? throw new NotFoundException($"Class with id {id} could not be found");
        logger.LogInformation("Deleting class, Name: {ClassName}, ID: {ClassId}", clss.Name, id);
        await repo.DeleteAsync(clss);
        logger.LogInformation("Successfully deleted class, Name: {ClassName}, ID: {ClassId}", clss.Name, id);
    }

    public async Task<ICollection<Class>> GetAllAsync()
    {
        return await repo.GetAllAsync();
    }

    public async Task<Class> GetWithLevelsAsync(int id)
    {
        return await repo.GetWithLevelsAsync(id) ?? throw new NotFoundException($"No class with id {id} can be found");
    }

    public async Task<Class> GetWithFeaturesAsync(int id)
    {
        return await repo.GetWithClassLevelFeaturesAsync(id) ?? throw new NotFoundException($"No class with id {id} can be found");
    }

    public async Task<Class> GetWithSubclassesAsync(int id)
    {
        return await repo.GetWithSubclassesAsync(id) ?? throw new NotFoundException($"Class with id {id} could not be found");
    }
    
    public async Task<Class> GetByIdAsync(int id)
    {
        return await repo.GetByIdAsync(id) ?? throw new NotFoundException($"Class with id {id} could not be found");
    }

    public async Task<Class> UpdateAsync(int id, ClassDto dto)
    {
        var clss = await repo.GetByIdAsync(id) ?? throw new NotFoundException($"Class with id {id} could not be found");
        logger.LogInformation("Updating class, Name: {ClassName}, ID: {ClassId}", clss.Name, id);

        clss.Name = dto.Name;
        clss.Description = dto.Description;
        clss.HitDie = dto.HitDie;   
        await repo.UpdateAsync(clss);
        logger.LogInformation("Successfully updated class, Name: {ClassName}, ID: {ClassId}", clss.Name, id);
        return clss;
    }

    public ICollection<Class> SortBy(ICollection<Class> classes, bool descending = false)
    {
        return OrderByMany(classes, [(c => c.Name)], descending);
    }
}