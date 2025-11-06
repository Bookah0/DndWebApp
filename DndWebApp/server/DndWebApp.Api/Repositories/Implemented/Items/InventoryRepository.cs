using DndWebApp.Api.Data;
using DndWebApp.Api.Models.Items;
using Microsoft.EntityFrameworkCore;

namespace DndWebApp.Api.Repositories.Implemented.Items;

public class InventoryRepository : IInventoryRepository
{
    private readonly AppDbContext context;

    public InventoryRepository(AppDbContext context)
    {
        this.context = context;
    }

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