using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs;
using DndWebApp.Api.Repositories.Interfaces;
using DndWebApp.Api.Services.Interfaces;
using DndWebApp.Api.Services.Util;

namespace DndWebApp.Api.Services.Implemented.Classes;

public partial class SubclassService : ISubclassService
{
    private readonly ISubclassRepository repo;
    private readonly ILogger<SubclassService> logger;

    public SubclassService(ISubclassRepository repo, ILogger<SubclassService> logger)
    {
        this.repo = repo;
        this.logger = logger;
    }

    public async Task<Subclass> CreateAsync(ClassDto dto, int parentClassId)
    {
        ValidationUtil.HasContentOrThrow(dto.Name);
        ValidationUtil.HasContentOrThrow(dto.Description);
        ValidationUtil.HasContentOrThrow(dto.HitDie);

        Subclass cls = new()
        {
            Name = dto.Name,
            Description = dto.Description,
            HitDie = dto.HitDie,
            ClassLevels = [],
            ParentClassId = parentClassId
        };

        return await repo.CreateAsync(cls);
    }

    public async Task DeleteAsync(int id)
    {
        var cls = await repo.GetByIdAsync(id) ?? throw new NullReferenceException($"Class with id {id} could not be found");
        await repo.DeleteAsync(cls);
    }

    public async Task<ICollection<Subclass>> GetAllAsync()
    {
        return await repo.GetMiscellaneousItemsAsync();
    }

    public async Task<Subclass> GetByIdAsync(int id)
    {
        return await repo.GetByIdAsync(id) ?? throw new NullReferenceException($"Class with id {id} could not be found");
    }

    public async Task UpdateAsync(ClassDto dto, int? newParentClassId)
    {
        ValidationUtil.HasContentOrThrow(dto.Name);
        ValidationUtil.HasContentOrThrow(dto.Description);
        ValidationUtil.HasContentOrThrow(dto.HitDie);

        var cls = await repo.GetByIdAsync(dto.Id) ?? throw new NullReferenceException($"Class with id {dto.Id} could not be found");

        cls.Name = dto.Name;
        cls.Description = dto.Description;
        cls.HitDie = dto.HitDie;
        cls.ParentClassId = newParentClassId ?? cls.ParentClassId;

        await repo.UpdateAsync(cls);
    }

    public async Task<ICollection<ClassLevel>> GetAllLevelsAsync(int classId)
    {
            var classWithLevels = await repo.GetByIdAsync(classId) ?? throw new NullReferenceException($"No subclass with id {classId} can be found");
            return classWithLevels.ClassLevels;
    }

    public ICollection<Class> SortBy(ICollection<Class> classes, bool descending = false)
    {
        return SortUtil.OrderByMany(classes, [(c => c.Name)], descending);
    }
}