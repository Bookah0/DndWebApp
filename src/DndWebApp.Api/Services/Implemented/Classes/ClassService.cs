using DndWebApp.Api.Middlewares.ExceptionHandling;
using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs.Character;
using DndWebApp.Api.Repositories.Interfaces;
using DndWebApp.Api.Services.Interfaces;
using static DndWebApp.Api.Services.Util.SortUtil;

namespace DndWebApp.Api.Services.Implemented.Classes;

public partial class ClassService(IClassRepository repo, ILogger<ClassService> logger) : IClassService
{
    public async Task<Class> CreateAsync(ClassDto dto)
    {
        var clss = await repo.CreateAsync(new()
        {
            Name = dto.Name,
            Description = dto.Description,
            HitDie = dto.HitDie,
            ClassLevels = []
        });

        return clss;
    }

    public async Task DeleteAsync(int id)
    {
        var clss = await repo.GetByIdAsync(id) ?? throw new NotFoundException($"Class with id {id} could not be found");
        await repo.DeleteAsync(clss);
    }

    public async Task<ICollection<Class>> GetAllAsync()
    {
        var classes = await repo.GetAllAsync();
        return classes;
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

    public async Task UpdateAsync(int id, ClassDto dto)
    {
        var clss = await repo.GetByIdAsync(id) ?? throw new NotFoundException($"Class with id {id} could not be found");

        clss.Name = dto.Name;
        clss.Description = dto.Description;
        clss.HitDie = dto.HitDie;   
        await repo.UpdateAsync(clss);
    }

    public ICollection<Class> SortBy(ICollection<Class> classes, bool descending = false)
    {
        return OrderByMany(classes, [(c => c.Name)], descending);
    }
}