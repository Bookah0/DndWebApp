using DndWebApp.Api.Data;
using DndWebApp.Api.Models.Items;
using Microsoft.EntityFrameworkCore;

namespace DndWebApp.Api.Repositories.Items;

public class InventoryRepository(AppDbContext context) : EfRepository<Inventory>(context), IInventoryRepository
{
    public async Task<Inventory?> GetWithEquippedItemsAsync(int id)
    {
        return await dbSet
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
        return await dbSet
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
        return await dbSet
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