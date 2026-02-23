using static Api.Domain.Shared.Utils.QueryUtil;
using Api.Domain.Items.Models;
using Api.Infrastructure.Data;
using Api.Domain.Items.DTOs;
using Api.Domain.Shared.DTOs;
using Api.Domain.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using Api.Infrastructure.Validation;
using Api.Infrastructure.Middleware.ExceptionHandling;
using Api.Domain.Shared.Utils;

namespace Api.Domain.Items.Repositories;

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

    public async Task<Item> UpdateAsync(Item updatedEntity)
    {
        context.Items.Update(updatedEntity);
        await context.SaveChangesAsync();
        return updatedEntity;
    }

    public async Task<(int, ICollection<Item>)> GetFilteredAsync(ItemFilterDto filter, PaginationRequestDto pagination)
    {      
        var normalizedSortBy = ValuesValidator.NormalizeValueOrThrow<SortItemOption>(filter.SortBy ?? SortItemOption.Default);

        var query = context.Items
            .AsQueryable()
            .WhereIf(filter.CreatedBy, i => i.CreatedBy == filter.CreatedBy)
            .WhereIf(filter.Name, i => i.Name.Contains(filter.Name!))
            .WhereIf(filter.Category, i => i.Categories.Any(c => filter.Category!.Contains(c)))
            .WhereIf(filter.Rarity, i => i.Rarity.Contains(filter.Rarity!)) 
            .WhereIf(filter.RequiresAttunement, i => i.RequiresAttunement == filter.RequiresAttunement)
            
            .WhereIf(filter.MinWeight, i => i.Weight >= filter.MinWeight)
            .WhereIf(filter.MaxWeight, i => i.Weight <= filter.MaxWeight)
            .WhereIf(filter.MinValue, i => i.Value >= filter.MinValue)
            .WhereIf(filter.MaxValue, i => i.Value <= filter.MaxValue)

            .WhereIf(filter.IsHomebrew, i => i.IsHomebrew == filter.IsHomebrew)
            .WhereIf(filter.CloningAllowed, i => i.CloningAllowed == filter.CloningAllowed)
            .SortBy(normalizedSortBy, sortSelectorsMap, SortItemOption.Default, filter.SortDescending); 

        var itemCount = await query.CountAsync();
        var filteredItems = await query
            .Skip((pagination.Page - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToListAsync();

        return (itemCount, filteredItems);
    }
    
    private readonly Dictionary<string, IEnumerable<Func<Item, object>>> sortSelectorsMap = new()
    {
        { SortItemOption.Name, [(s => s.Name)] },
        { SortItemOption.Category, [(s => s.Categories.FirstOrDefault()!), (s => s.Name)] },
        { SortItemOption.Value, [(s => s.Value!), (s => s.Name)] },
        { SortItemOption.Weight, [(s => s.Weight!), (s => s.Name)] },
        { SortItemOption.Rarity, [(s => s.Rarity == null), (s => s.Rarity!), (s => s.Name)] },
    };
}

