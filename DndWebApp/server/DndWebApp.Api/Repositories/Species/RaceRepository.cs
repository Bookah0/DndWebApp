using DndWebApp.Api.Data;
using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.Characters.Enums;
using DndWebApp.Api.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace DndWebApp.Api.Repositories.Species;

public class RaceRepository : IRaceRepository
{
    private readonly AppDbContext context;
    private readonly IRepository<Race> baseRepo;

    public RaceRepository(AppDbContext context, IRepository<Race> baseRepo)
    {
        this.context = context;
        this.baseRepo = baseRepo;
    }

    public async Task<Race> CreateAsync(Race entity) => await baseRepo.CreateAsync(entity);
    public async Task<Race?> GetByIdAsync(int id) => await baseRepo.GetByIdAsync(id);
    public async Task<ICollection<Race>> GetAllAsync() => await baseRepo.GetAllAsync();
    public async Task UpdateAsync(Race updatedEntity) => await baseRepo.UpdateAsync(updatedEntity);
    public async Task DeleteAsync(Race entity) => await baseRepo.DeleteAsync(entity);    

    public async Task<Race?> GetWithAllDataAsync(int id)
    {
        return await context.Races
        .Include(r => r.Traits)
        .Include(r => r.SubRaces)
        .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<ICollection<Race>> GetAllWithAllDataAsync()
    {
        return await context.Races
        .Include(r => r.Traits)
        .Include(r => r.SubRaces)
        .ToListAsync();
    }
}