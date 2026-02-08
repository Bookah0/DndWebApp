using Api.Middlewares.ExceptionHandling;
using Api.Models.DTOs.RequestDtos.Inventory;
using Api.Models.Items;
using Api.Repositories.Interfaces;
using Api.Services.Constants;
using static Api.Services.Util.SortUtil;
using static Api.Services.Util.ConstantsUtil;
using Api.Models.Items.Constants;
using Api.Services.Interfaces.Items;
using Api.Services.Interfaces;

namespace Api.Services.Implemented.Items;

public class ItemService(IItemRepository repo, ICurrentUserService currentUserService, ILogger<ItemService> logger) : IItemService
{
    public async Task<Item> CreateAsync(CreateItemRequestDto dto)
    {
        if(dto.Categories == null || dto.Categories.Count < 1)
            throw new ValidationException("At least one category is required for an item.");

        var dtoCategories = ResolveOptionOrThrow(dto.Categories, ItemCategory.AllowedValues);
        var dtoRarity = ResolveOptionOrThrow(dto.Rarity, ItemRarity.AllowedValues);
        
        logger.LogInformation("Creating item, Name: {ItemName}", dto.Name);

        Item item = await repo.CreateAsync(new()
        {
            Name = dto.Name,
            Description = dto.Description,
            Categories = dtoCategories,
            Value = dto.Value,
            Rarity = dtoRarity,
            RequiresAttunement = dto.RequiresAttunement,
            Weight = dto.Weight,

            CreatedAt = DateTime.UtcNow,
            CreatedBy = currentUserService.GetCurrentUserId()
        });

        logger.LogInformation("Successfully created item, Name: {ItemName}, ID: {ItemId}", item.Name, item.Id);
        return item;
    }

    public async Task DeleteAsync(int id)
    {
        var item = await repo.GetByIdAsync(id);
        logger.LogInformation("Deleting item, Name: {ItemName}, ID: {ItemId}", item.Name, id);
        await repo.DeleteAsync(item);
        logger.LogInformation("Successfully deleted item, Name: {ItemName}, ID: {ItemId}", item.Name, id);
    }

    public async Task<ICollection<Item>> GetAllAsync()
    {
        var items = await repo.GetAllAsync();
        return items;
    }

    public async Task<Item> GetByIdAsync(int id)
    {
        var item = await repo.GetByIdAsync(id);
        return item;
    }

    public async Task<Item> UpdateAsync(UpdateItemRequestDto dto, int id)
    {
        var dtoRarity = dto.Rarity is not null ? ResolveOptionOrThrow(dto.Rarity, ItemRarity.AllowedValues) : null;

        var item = await repo.GetByIdAsync(id);
        logger.LogInformation("Updating item, Name: {ItemName}, ID: {ItemId}", item.Name, item.Id);

        item.Name = dto.Name ?? item.Name;
        item.Description = dto.Description ?? item.Description;
        item.Value = dto.Value ?? item.Value;
        item.Rarity = dtoRarity ?? item.Rarity;
        item.RequiresAttunement = dto.RequiresAttunement ?? item.RequiresAttunement;
        item.Weight = dto.Weight ?? item.Weight;
        item.Quantity = dto.Quantity ?? item.Quantity;

        item.IsPublic = dto.IsPublic ?? item.IsPublic;
        item.CloningAllowed = dto.CloningAllowed ?? item.CloningAllowed;
        item.UpdatedAt = DateTime.UtcNow;
        
        await repo.UpdateAsync(item);
        logger.LogInformation("Successfully updated item, Name: {ItemName}, ID: {ItemId}", item.Name, item.Id);
        return item;
    }

    // TODO replace with database level sorting
    public ICollection<Item> SortBy(ICollection<Item> items, string sortFilter, bool descending = false)
    {
        if(!TryResolveOption(sortFilter, SortItemOption.AllowedValues, out string? resolved))
            return items;

        return resolved switch
        {
            SortItemOption.Name => OrderByMany(items, [(i => i.Name)], descending),
            SortItemOption.Category => OrderByMany(items, [(i => i.Categories.FirstOrDefault()!), (i => i.Name)], descending),
            SortItemOption.Value => OrderByMany(items, [(i => i.Value!), (i => i.Name)], descending),
            SortItemOption.Weight => OrderByMany(items, [(i => i.Weight!), (i => i.Name)], descending),
            SortItemOption.Rarity => OrderByMany(items, [(i => i.Rarity == null), (i => i.Rarity!), (i => i.Name)], descending),
            _ => throw new ValidationException($"Invalid sort option: {sortFilter}")
        };
    }
}