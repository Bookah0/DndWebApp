using DndWebApp.Api.Middlewares.ExceptionHandling;
using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs.RequestDtos.Character;
using DndWebApp.Api.Repositories.Interfaces;
using DndWebApp.Api.Services.Interfaces;
using static DndWebApp.Api.Services.Util.SortUtil;

namespace DndWebApp.Api.Services.Implemented.Classes;

public partial class SubclassService(ISubclassRepository repo, IClassRepository classRepo, ILogger<SubclassService> logger) : ISubclassService
{
    public async Task<Subclass> CreateAsync(ClassDto dto, int parentClassId)
    {
        var subclass = await repo.CreateAsync(new()
        {
            Name = dto.Name,
            Description = dto.Description,
            HitDie = dto.HitDie,
            ClassLevels = [],
            ParentClassId = parentClassId
        });

        return subclass;
    }

    public async Task DeleteAsync(int id)
    {
        var subclass = await repo.GetByIdAsync(id) ?? throw new NotFoundException($"Class with id {id} could not be found");
        await repo.DeleteAsync(subclass);
    }

    public async Task<ICollection<Subclass>> GetAllAsync()
    {
        var subclasses = await repo.GetAllAsync();
        return subclasses;
    }

    public async Task<Subclass> GetByIdAsync(int id)
    {
        var subclass = await repo.GetByIdAsync(id) ?? throw new NotFoundException($"Class with id {id} could not be found");
        return subclass;
    }

    public async Task UpdateAsync(int id, ClassDto dto)
    {
        var subclass = await repo.GetByIdAsync(id) ?? throw new NotFoundException($"Class with id {id} could not be found");
        
        subclass.Name = dto.Name;
        subclass.Description = dto.Description;
        subclass.HitDie = dto.HitDie;
        
        if(dto.NewParentClassId is not null)
        {
            var newParentClass = await classRepo.GetByIdAsync((int)dto.NewParentClassId) 
                ?? throw new NotFoundException($"Parent Class with id {(int)dto.NewParentClassId} could not be found");

            subclass.ParentClassId = (int)dto.NewParentClassId;
            subclass.ParentClass = newParentClass;
        }
        
        await repo.UpdateAsync(subclass);
    }

    public async Task<Subclass> GetWithLevelsAsync(int id)
    {
        return await repo.GetWithClassLevelsAsync(id) ?? throw new NotFoundException($"No subclass with id {id} can be found");
    }

    public async Task<Subclass> GetWithFeaturesAsync(int id)
    {
        return await repo.GetWithClassLevelFeaturesAsync(id) ?? throw new NotFoundException($"No subclass with id {id} can be found");
    }

    public ICollection<Subclass> SortBy(ICollection<Subclass> subclasses, bool descending = false)
    {
        return OrderByMany(subclasses, [(c => c.Name)], descending);
    }
}