using DndWebApp.Api.Middlewares.ExceptionHandling;
using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs.Character;
using DndWebApp.Api.Repositories.Interfaces;
using DndWebApp.Api.Services.Interfaces;
using static DndWebApp.Api.Services.Util.SortUtil;

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
        var cls = await repo.GetByIdAsync(id) ?? throw new NotFoundException($"Class with id {id} could not be found");
        await repo.DeleteAsync(cls);
    }

    public async Task<ICollection<Class>> GetAllAsync()
    {
        return await repo.GetAllAsync();
    }

    public async Task<ICollection<ClassLevel>> GetAllLevelsAsync(int classId)
    {
        var classWithLevels = await repo.GetByIdAsync(classId) ?? throw new NotFoundException($"No subclass with id {classId} can be found");
        return classWithLevels.ClassLevels;
    }
    
    public async Task<Class> GetByIdAsync(int id)
    {
        return await repo.GetByIdAsync(id) ?? throw new NotFoundException($"Class with id {id} could not be found");
    }

    public async Task UpdateAsync(ClassDto dto)
    {
        var cls = await repo.GetByIdAsync(dto.Id) ?? throw new NotFoundException($"Class with id {dto.Id} could not be found");

        cls.Name = dto.Name;
        cls.Description = dto.Description;
        cls.HitDie = dto.HitDie;

        await repo.UpdateAsync(cls);
    }

    public ICollection<Class> SortBy(ICollection<Class> classes, bool descending = false)
    {
        return OrderByMany(classes, [(c => c.Name)], descending);
    }
}