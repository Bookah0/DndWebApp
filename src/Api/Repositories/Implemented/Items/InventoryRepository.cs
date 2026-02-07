using Api.Data;
using Api.Models.Items;
using Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories.Implemented.Items;

public class InventoryRepository(AppDbContext context) : IInventoryRepository
{
    public async Task<Inventory> GetByIdAsync(int id) => 
        await context.Inventories.FirstOrDefaultAsync(i => i.Id == id)
            ?? throw new Exception($"Inventory with id {id} could not be found");

    public async Task<Inventory> GetWithCurrencyAsync(int id) =>
        await context.Inventories
            .Include(i => i.Currency)
            .FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new Exception($"Inventory with id {id} could not be found");
    
    public async Task<Inventory> GetWithEquippedItemsAsync(int id) =>
        await context.Inventories
            .Include(i => i.EquippedItems)
            .FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new Exception($"Inventory with id {id} could not be found");

    public async Task<Inventory> GetWithStoredItemsAsync(int id) =>
        await context.Inventories
            .Include(i => i.StoredItems)
            .FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new Exception($"Inventory with id {id} could not be found");

    public async Task<Inventory> GetWithAllDataAsync(int id) =>
        await context.Inventories
            .Include(i => i.Currency)
            .Include(r => r.EquippedItems)
            .Include(i => i.StoredItems)
            .FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new Exception($"Inventory with id {id} could not be found");

    public async Task<ICollection<Inventory>> GetAllAsync() => await context.Inventories.ToListAsync();
    
    public async Task<Inventory> CreateAsync(Inventory entity)
    {
        await context.Inventories.AddAsync(entity);
        await context.SaveChangesAsync();
        return entity;
    }
    public async Task DeleteAsync(Inventory entity)
    {
        context.Inventories.Remove(entity);
        await context.SaveChangesAsync();
    }

    public async Task<Inventory> UpdateAsync(Inventory updatedEntity)
    {
        context.Inventories.Update(updatedEntity);
        await context.SaveChangesAsync();
        return updatedEntity;
    }
}