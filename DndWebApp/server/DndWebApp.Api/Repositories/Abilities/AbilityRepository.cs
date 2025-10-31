using DndWebApp.Api.Data;
using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace DndWebApp.Api.Repositories.Abilities;

public class AbilityRepository : IAbilityRepository
{
    private AppDbContext context;
    private IRepository<Ability> baseRepo;

    public AbilityRepository(AppDbContext context, IRepository<Ability> baseRepo)
    {
        this.context = context;
        this.baseRepo = baseRepo;
    }

    public async Task<Ability> CreateAsync(Ability entity) => await baseRepo.CreateAsync(entity);
    public async Task<Ability?> GetByIdAsync(int id) => await baseRepo.GetByIdAsync(id);
    public async Task<ICollection<Ability>> GetAllAsync() => await baseRepo.GetAllAsync();
    public async Task UpdateAsync(Ability updatedEntity) => await baseRepo.UpdateAsync(updatedEntity);
    public async Task DeleteAsync(Ability entity) => await baseRepo.DeleteAsync(entity);

    public async Task<AbilityDto?> GetDtoAsync(int id)
    {
        return await context.AbilityScores
            .AsNoTracking()
            .Select(a => new AbilityDto
            {
                Id = a.Id,
                FullName = a.FullName,
                ShortName = a.ShortName,
                Description = a.Description
            })
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Ability?> GetWithSkillsAsync(int id)
    {
        return await context.AbilityScores
            .Include(a => a.Skills)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<ICollection<AbilityDto>> GetAllDtosAsync()
    {
        return await context.AbilityScores
            .AsNoTracking()
            .Select(a => new AbilityDto
            {
                Id = a.Id,
                FullName = a.FullName,
                ShortName = a.ShortName,
                Description = a.Description
            })
            .ToListAsync();
    }

    public async Task<ICollection<Ability>> GetAllWithSkillsAsync()
    {
        return await context.AbilityScores
            .Include(a => a.Skills)
            .ToListAsync();
    }

}