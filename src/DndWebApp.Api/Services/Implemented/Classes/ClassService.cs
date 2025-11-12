using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs;
using DndWebApp.Api.Repositories.Interfaces;
using DndWebApp.Api.Services.Interfaces;
using DndWebApp.Api.Services.Util;

namespace DndWebApp.Api.Services.Implemented.Classes;

public partial class ClassService : IClassService
{
    private readonly IClassRepository repo;
    private readonly ILogger<ClassService> logger;

    public ClassService(IClassRepository repo, ILogger<ClassService> logger)
    {
        this.repo = repo;
        this.logger = logger;
    }

    public async Task<Class> CreateAsync(ClassDto dto)
    {
        ValidationUtil.HasContentOrThrow(dto.Name);
        ValidationUtil.HasContentOrThrow(dto.Description);
        ValidationUtil.HasContentOrThrow(dto.HitDie);

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
        return await repo.GetMiscellaneousItemsAsync();
    }

    public async Task<ICollection<ClassLevel>> GetAllLevelsAsync(int classId)
    {
        var classWithLevels = await repo.GetByIdAsync(classId) ?? throw new NullReferenceException($"No subclass with id {classId} can be found");
        return classWithLevels.ClassLevels;
    }
    
    public async Task<Class> GetByIdAsync(int id)
    {
        return await repo.GetByIdAsync(id) ?? throw new NullReferenceException($"Class with id {id} could not be found");
    }

    public async Task UpdateAsync(ClassDto dto)
    {
        ValidationUtil.HasContentOrThrow(dto.Name);
        ValidationUtil.HasContentOrThrow(dto.Description);
        ValidationUtil.HasContentOrThrow(dto.HitDie);

        var cls = await repo.GetByIdAsync(dto.Id) ?? throw new NullReferenceException($"Class with id {dto.Id} could not be found");

        cls.Name = dto.Name;
        cls.Description = dto.Description;
        cls.HitDie = dto.HitDie;

        await repo.UpdateAsync(cls);
    }

    public ICollection<Class> SortBy(ICollection<Class> classes, bool descending = false)
    {
        return SortUtil.OrderByMany(classes, [(c => c.Name)], descending);
    }
}