using Api.Controllers.Features;
using Api.Middlewares.ExceptionHandling;
using Api.Models.Characters;
using Api.Models.DTOs.RequestDtos.Character;
using Api.Repositories.Interfaces;
using Api.Services.Interfaces;
using static Api.Services.Util.SortUtil;

namespace Api.Services.Implemented.Classes;

public partial class SubclassService(
    ISubclassRepository repo, 
    IBaseClassRepository classRepo, 
    ICurrentUserService currentUserService, 
    ILogger<SubclassService> logger) : ISubclassService
{
    public async Task<Subclass> CreateAsync(CreateSubclassRequestDto dto, int parentClassId)
    {
        logger.LogInformation("Creating subclass, Name: {SubclassName}, ParentClassId: {ParentClassId}", dto.Name, parentClassId);

        var subclass = await repo.CreateAsync(new()
        {
            Name = dto.Name,
            Description = dto.Description,
            HitDie = dto.HitDie,
            ClassLevels = [],
            ParentClassId = parentClassId,

            CreatedAt = DateTime.UtcNow,
            CreatedBy = currentUserService.GetCurrentUserId(),
        });

        logger.LogInformation("Successfully created subclass, Name: {SubclassName}, ID: {SubclassId}", subclass.Name, subclass.Id);
        return subclass;
    }

    public async Task DeleteAsync(int id)
    {
        var subclass = await repo.GetByIdAsync(id);
        logger.LogInformation("Deleting subclass with Name: {SubclassName}, ID: {SubclassId}", subclass.Name, id);
        await repo.DeleteAsync(subclass);
        logger.LogInformation("Successfully deleted subclass, Name: {SubclassName}, ID: {SubclassId}", subclass.Name, id);
    }

    public async Task<ICollection<Subclass>> GetAllAsync()
    {
        var subclasses = await repo.GetAllAsync();
        return subclasses;
    }

    public async Task<Subclass> GetByIdAsync(int id) => await repo.GetByIdAsync(id);

    public async Task<Subclass> UpdateAsync(int id, UpdateSubclassRequestDto dto)
    {
        var subclass = await repo.GetByIdAsync(id);
        logger.LogInformation("Updating subclass, Name: {SubclassName}, ID: {SubclassId}", subclass.Name, id);

        subclass.Name = dto.Name ?? subclass.Name;
        subclass.Description = dto.Description ?? subclass.Description;
        subclass.HitDie = dto.HitDie ?? subclass.HitDie;
        subclass.IsPublic = dto.IsPublic ?? subclass.IsPublic;
        subclass.CloningAllowed = dto.CloningAllowed ?? subclass.CloningAllowed;
        subclass.UpdatedAt = DateTime.UtcNow;
        
        if(dto.NewParentClassId is not null && dto.NewParentClassId != subclass.ParentClassId)
        {
            var newParentClass = await classRepo.GetByIdAsync((int)dto.NewParentClassId);
            subclass.ParentClassId = (int)dto.NewParentClassId;
            subclass.ParentClass = newParentClass;
        }
        
        await repo.UpdateAsync(subclass);
        logger.LogInformation("Successfully updated subclass, Name: {SubclassName}, ID: {SubclassId}", subclass.Name, id);
        return subclass;
    }

    public async Task<Subclass> GetWithLevelsAsync(int id) => await repo.GetWithClassLevelsAsync(id);
    public async Task<Subclass> GetWithFeaturesAsync(int id) => await repo.GetWithClassLevelFeaturesAsync(id);
    

    public ICollection<Subclass> SortBy(ICollection<Subclass> subclasses, bool descending = false)
    {
        return OrderByMany(subclasses, [(c => c.Name)], descending);
    }
}