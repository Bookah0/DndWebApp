using DndWebApp.Api.Data;
using DndWebApp.Api.Models.Items;
using DndWebApp.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DndWebApp.Api.Repositories.Implemented.Items;

public class ItemRepository : IItemRepository
{
    private readonly AppDbContext context;

    public ItemRepository(AppDbContext context)
    {
        this.context = context;
    }

    public async Task<Item> CreateAsync(Item entity)
    {
        await context.Items.AddAsync(entity);
        await context.SaveChangesAsync();
        return entity;
    }
    public async Task<Item?> GetByIdAsync(int id) => await context.Items.FirstOrDefaultAsync(i => i.Id == id);
    public async Task<Item?> GetByNameAsync(string name) => await context.Items.FirstOrDefaultAsync(i => i.Name == name);
    public async Task<ICollection<Item>> GetAllAsync() => await context.Items.ToListAsync();

    public async Task<ICollection<Item>> GetAllMiscItemsAsync()
    {
        return await context.Items
            .Where(i => !(i is Weapon) && !(i is Armor) && !(i is Tool))
            .ToListAsync();
    }

    public async Task DeleteAsync(Item entity)
    {
        context.Items.Remove(entity);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Item updatedEntity)
    {
        context.Items.Update(updatedEntity);
        await context.SaveChangesAsync();
    }
}

