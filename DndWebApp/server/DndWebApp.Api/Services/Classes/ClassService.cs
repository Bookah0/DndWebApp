using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs;
using DndWebApp.Api.Repositories.Classes;
using DndWebApp.Api.Repositories.Features;
using DndWebApp.Api.Services.Generic;
using DndWebApp.Api.Services.Util;

namespace DndWebApp.Api.Services.Classes;

public partial class ClassService : IService<Class, ClassDto, ClassDto>
{
    private readonly IClassRepository repo;
    private readonly IClassLevelRepository levelRepo;
    private readonly IClassFeatureRepository featureRepo;
    private readonly ILogger<ClassService> logger;

    public ClassService(IClassRepository repo, IClassLevelRepository levelRepo, IClassFeatureRepository featureRepo, ILogger<ClassService> logger)
    {
        this.repo = repo;
        this.levelRepo = levelRepo;
        this.featureRepo = featureRepo;
        this.logger = logger;
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

    public ICollection<Class> SortBy(ICollection<Class> classes, bool descending = false)
    {
        return SortUtil.OrderByMany(classes, [(c => c.Name)], descending);
    }
}