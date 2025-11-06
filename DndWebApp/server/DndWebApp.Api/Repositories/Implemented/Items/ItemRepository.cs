using DndWebApp.Api.Data;
using DndWebApp.Api.Models.Items;
using Microsoft.EntityFrameworkCore;

namespace DndWebApp.Api.Repositories.Implemented.Items;

public class ItemRepository : IItemRepository
{
    private readonly AppDbContext context;

    public ItemRepository(AppDbContext context)
    {
        this.context = context;
    }

    public async Task<Item> CreateAsync(Item entity) => await baseRepo.CreateAsync(entity);

    public async Task<Item?> GetByIdAsync(int id)
    {
        return await context.Items
            .Where(i => i.Id == id && !(i is Weapon) && !(i is Armor) && !(i is Tool))
            .FirstOrDefaultAsync();
    }

    public async Task<ICollection<Item>> GetAllAsync()
    {
        return await context.Items
            .Where(i => !(i is Weapon) && !(i is Armor) && !(i is Tool))
            .ToListAsync();
    }

    public async Task UpdateAsync(Item updatedEntity) => await baseRepo.UpdateAsync(updatedEntity);
    public async Task DeleteAsync(Item entity) => await baseRepo.DeleteAsync(entity);    

}

