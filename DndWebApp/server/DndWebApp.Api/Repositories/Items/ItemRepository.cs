using DndWebApp.Api.Data;
using DndWebApp.Api.Models.Items;
using Microsoft.EntityFrameworkCore;

namespace DndWebApp.Api.Repositories.Items;

public class ItemRepository(AppDbContext context) : EfRepository<Item>(context), IItemRepository
{
    public override async Task<Item?> GetByIdAsync(int id)
    {
        return await dbSet
            .Where(i => i.Id == id && !(i is Weapon) && !(i is Armor) && !(i is Tool))
            .FirstOrDefaultAsync();
    }

    public override async Task<ICollection<Item>> GetAllAsync()
    {
        return await dbSet
            .Where(i => !(i is Weapon) && !(i is Armor) && !(i is Tool))
            .ToListAsync();
    }
}

