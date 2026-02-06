using Api.Middlewares.ExceptionHandling;
using Api.Models.Characters;
using Api.Models.DTOs.RequestDtos.Character;
using Api.Repositories.Interfaces;
using Api.Services.Interfaces;
using static Api.Services.Util.SortUtil;

namespace Api.Services.Implemented.Classes;

public partial class ClassService(IClassRepository repo, ICurrentUserService currentUserService, ILogger<ClassService> logger) : IClassService
{
    public async Task<BaseClass> CreateAsync(ClassDto dto)
    {
        logger.LogInformation("Creating class, Name: {ClassName}", dto.Name);

        var clss = await repo.CreateAsync(new()
        {
            Name = dto.Name,
            Description = dto.Description,
            HitDie = dto.HitDie,
            ClassLevels = [],

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
    public async Task<BaseClass> GetWithLevelsAsync(int id) => await repo.GetWithLevelsAsync(id);
    public async Task<BaseClass> GetWithFeaturesAsync(int id) => await repo.GetWithClassLevelFeaturesAsync(id);
    public async Task<BaseClass> GetWithSubclassesAsync(int id) => await repo.GetWithSubclassesAsync(id);
    public async Task<BaseClass> GetByIdAsync(int id) => await repo.GetByIdAsync(id);

    public async Task<BaseClass> UpdateAsync(int id, ClassDto dto)
    {
        var clss = await repo.GetByIdAsync(id);
        logger.LogInformation("Updating class, Name: {ClassName}, ID: {ClassId}", clss.Name, id);

        clss.Name = dto.Name;
        clss.Description = dto.Description;
        clss.HitDie = dto.HitDie;   
        await repo.UpdateAsync(clss);
        logger.LogInformation("Successfully updated class, Name: {ClassName}, ID: {ClassId}", clss.Name, id);
        return clss;
    }

    public ICollection<BaseClass> SortBy(ICollection<BaseClass> classes, bool descending = false)
    {
        return OrderByMany(classes, [(c => c.Name)], descending);
    }
}