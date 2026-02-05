using Api.Middlewares.ExceptionHandling;
using Api.Models.Characters;
using Api.Models.DTOs.RequestDtos.Character;
using Api.Repositories.Interfaces;
using Api.Services.Interfaces;
using static Api.Services.Util.SortUtil;

namespace Api.Services.Implemented.Classes;

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
        var clss = await repo.GetByIdAsync(id);
        logger.LogInformation("Deleting class, Name: {ClassName}, ID: {ClassId}", clss.Name, id);
        await repo.DeleteAsync(clss);
        logger.LogInformation("Successfully deleted class, Name: {ClassName}, ID: {ClassId}", clss.Name, id);
    }

    public async Task<ICollection<Class>> GetAllAsync() => await repo.GetAllAsync();
    public async Task<Class> GetWithLevelsAsync(int id) => await repo.GetWithLevelsAsync(id);
    public async Task<Class> GetWithFeaturesAsync(int id) => await repo.GetWithClassLevelFeaturesAsync(id);
    public async Task<Class> GetWithSubclassesAsync(int id) => await repo.GetWithSubclassesAsync(id);
    public async Task<Class> GetByIdAsync(int id) => await repo.GetByIdAsync(id);

    public async Task<Class> UpdateAsync(int id, ClassDto dto)
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

    public ICollection<Class> SortBy(ICollection<Class> classes, bool descending = false)
    {
        return OrderByMany(classes, [(c => c.Name)], descending);
    }
}