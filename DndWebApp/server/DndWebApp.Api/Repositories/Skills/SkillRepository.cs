using DndWebApp.Api.Data;
using DndWebApp.Api.Models.Characters;
using Microsoft.EntityFrameworkCore;

namespace DndWebApp.Api.Repositories.Skills;

public class SkillRepository : ISkillRepository
{
    private readonly AppDbContext context;
    private readonly IRepository<Skill> baseRepo;

    public SkillRepository(AppDbContext context, IRepository<Skill> baseRepo)
    {
        this.context = context;
        this.baseRepo = baseRepo;
    }

    public async Task<Skill> CreateAsync(Skill entity) => await baseRepo.CreateAsync(entity);
    public async Task<Skill?> GetByIdAsync(int id) => await baseRepo.GetByIdAsync(id);
    public async Task<ICollection<Skill>> GetAllAsync() => await baseRepo.GetAllAsync();
    public async Task UpdateAsync(Skill updatedEntity) => await baseRepo.UpdateAsync(updatedEntity);
    public async Task DeleteAsync(Skill entity) => await baseRepo.DeleteAsync(entity);    

    public async Task<Skill?> GetWithAbilityAsync(int id)
    {
        return await context.Skills.Include(s => s.Ability).FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<ICollection<Skill>> GetAllWithAbilityAsync()
    {
        return await context.Skills.Include(s => s.Ability).ToListAsync();
    }
}