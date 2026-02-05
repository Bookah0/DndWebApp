using DndWebApp.Api.Data;
using DndWebApp.Api.Models.Items;
using DndWebApp.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DndWebApp.Api.Repositories.Implemented.Items;

public class ItemRepository(AppDbContext context) : IItemRepository
{
    public async Task<Item> GetByIdAsync(int id) => 
        await context.Items.FirstOrDefaultAsync(i => i.Id == id) 
            ?? throw new Exception($"Item with id {id} could not be found");
    
    public async Task<Item> GetByNameAsync(string name) => 
        await context.Items.FirstOrDefaultAsync(i => i.Name == name)
            ?? throw new Exception($"Item with name {name} could not be found");
    
    public async Task<ICollection<Item>> GetAllAsync() => await context.Items.ToListAsync();

    public async Task<ICollection<Item>> GetAllMiscItemsAsync() => 
        await context.Items
            .Where(i => !(i is Weapon) && !(i is Armor) && !(i is Tool))
            .ToListAsync();
    
    public async Task<bool> ExistsAsync(int itemId) => await context.Items.AnyAsync(x => x.Id == itemId);

    public async Task<Item> CreateAsync(Item entity)
    {
        await context.Items.AddAsync(entity);
        await context.SaveChangesAsync();
        return entity;
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

