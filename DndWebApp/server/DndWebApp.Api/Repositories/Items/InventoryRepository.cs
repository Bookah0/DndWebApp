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
    
    public async Task<Inventory?> GetWithEquippedItemsAsync(int id)
    {
        return await context.Inventories
        .Include(i => i.Currency)
        .Include(r => r.EquippedMainHand)
        .Include(r => r.EquippedOffHand)
        .Include(r => r.EquippedRanged)
        .Include(r => r.EquippedArmor)
        .Include(r => r.EquippedHead)
        .Include(r => r.EquippedWaist)
        .Include(r => r.EquippedHands)
        .Include(r => r.EquippedFeet)
        .Include(i => i.EquippedArcaneFocus)
        .Include(i => i.EquippedHolySymbol)
        .AsSplitQuery()
        .Include(i => i.EquippedOnBack)
        .Include(i => i.EquippedNecklaces)
        .Include(i => i.EquippedRings)
        .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Inventory?> GetWithStoredItemsAsync(int id)
    {
        return await context.Inventories
        .Include(i => i.Currency)
        .AsSplitQuery()
        .Include(i => i.Treasures)
        .Include(i => i.Equipment)
        .Include(i => i.Consumables)
        .Include(i => i.Gear)
        .Include(i => i.Misc)
        .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Inventory?> GetWithAllDataAsync(int id)
    {
        return await context.Inventories
        .Include(i => i.Currency)
        .Include(r => r.EquippedMainHand)
        .Include(r => r.EquippedOffHand)
        .Include(r => r.EquippedRanged)
        .Include(r => r.EquippedArmor)
        .Include(r => r.EquippedHead)
        .Include(r => r.EquippedWaist)
        .Include(r => r.EquippedHands)
        .Include(r => r.EquippedFeet)
        .Include(i => i.EquippedArcaneFocus)
        .Include(i => i.EquippedHolySymbol)
        .AsSplitQuery()
        .Include(i => i.Treasures)
        .Include(i => i.Equipment)
        .Include(i => i.Consumables)
        .Include(i => i.Gear)
        .Include(i => i.Misc)
        .Include(i => i.EquippedOnBack)
        .Include(i => i.EquippedNecklaces)
        .Include(i => i.EquippedRings)
        .FirstOrDefaultAsync(x => x.Id == id);
    }
}