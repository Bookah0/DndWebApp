using DndWebApp.Api.Data;
using DndWebApp.Api.Models.Items;
using Microsoft.EntityFrameworkCore;

namespace DndWebApp.Api.Repositories.Items;

public class InventoryRepository : IInventoryRepository
{
    private readonly AppDbContext context;
    private readonly IRepository<Inventory> baseRepo;

    public InventoryRepository(AppDbContext context, IRepository<Inventory> baseRepo)
    {
        this.context = context;
        this.baseRepo = baseRepo;
    }

    public async Task<Inventory> CreateAsync(Inventory entity) => await baseRepo.CreateAsync(entity);
    public async Task<Inventory?> GetByIdAsync(int id) => await baseRepo.GetByIdAsync(id);
    public async Task<ICollection<Inventory>> GetAllAsync() => await baseRepo.GetAllAsync();
    public async Task UpdateAsync(Inventory updatedEntity) => await baseRepo.UpdateAsync(updatedEntity);
    public async Task DeleteAsync(Inventory entity) => await baseRepo.DeleteAsync(entity);

    public async Task<Inventory?> GetWithCurrencyAsync(int id)
    {
        return await context.Inventories
            .Include(i => i.Currency)
            .FirstOrDefaultAsync(x => x.Id == id);
    }   
    
    public async Task<Inventory?> GetWithEquippedItemsAsync(int id)
    {
        return await context.Inventories
        .Include(i => i.EquippedItems)
        .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Inventory?> GetWithStoredItemsAsync(int id)
    {
        return await context.Inventories
        .Include(i => i.StoredItems)
        .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Inventory?> GetWithAllDataAsync(int id)
    {
        return await context.Inventories
        .Include(i => i.Currency)
        .Include(r => r.EquippedItems)
        .Include(i => i.StoredItems)
        .FirstOrDefaultAsync(x => x.Id == id);
    }
}