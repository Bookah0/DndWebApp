using DndWebApp.Api.Data;
using DndWebApp.Api.Models.Items;
using Microsoft.EntityFrameworkCore;

namespace DndWebApp.Api.Repositories.Items;

public class ItemRepository : IItemRepository
{
    private AppDbContext context;
    private IRepository<Item> baseRepo;

    public ItemRepository(AppDbContext context, IRepository<Item> baseRepo)
    {
        this.context = context;
        this.baseRepo = baseRepo;
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

