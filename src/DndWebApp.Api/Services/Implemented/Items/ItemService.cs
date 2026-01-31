using DndWebApp.Api.Middlewares.ExceptionHandling;
using DndWebApp.Api.Models.DTOs.Inventory;
using DndWebApp.Api.Models.Items;
using DndWebApp.Api.Repositories.Interfaces;
using DndWebApp.Api.Services.Constants;
using static DndWebApp.Api.Services.Util.SortUtil;
using static DndWebApp.Api.Services.Util.ConstantsUtil;
using DndWebApp.Api.Models.Items.Constants;

namespace DndWebApp.Api.Services.Implemented.Items;

public class ItemService
{
    private readonly IItemRepository repo;
    private readonly ILogger<ItemService> logger;

    public ItemService(IItemRepository repo, ILogger<ItemService> logger)
    {
        this.repo = repo;
        this.logger = logger;
    }

    public async Task<Item> CreateAsync(ItemDto dto)
    {
        var dtoMainCategory =  ResolveOptionOrThrow(dto.MainCategory, ItemCategory.AllowedValues, "Item Category");
        var dtoOtherCategories = ResolveOptionOrThrow(dto.OtherCategories, ItemCategory.AllowedValues, "Item Category");
        var dtoRarity = dto.Rarity is not null ? ResolveOptionOrThrow(dto.Rarity, ItemRarity.AllowedValues, "Item Rarity") : null;

        Item item = new()
        {
            Name = dto.Name,
            Description = dto.Description,
            Categories = [dtoMainCategory, .. dtoOtherCategories],
            Value = dto.Value,
            Rarity = dtoRarity,
            RequiresAttunement = dto.RequiresAttunement ?? false,
            IsHomebrew = dto.IsHomebrew ?? false,
            Weight = dto.Weight ?? 0,
        };

        return await repo.CreateAsync(item);
    }

    public async Task DeleteAsync(int id)
    {
        var item = await repo.GetByIdAsync(id) ?? throw new NotFoundException($"Item with id {id} could not be found");
        await repo.DeleteAsync(item);
    }

    public async Task<ICollection<Item>> GetMiscellaneousItemsAsync()
    {
        return await repo.GetAllAsync();
    }

    public async Task<Item> GetByIdAsync(int id)
    {
        return await repo.GetByIdAsync(id) ?? throw new NotFoundException($"Item with id {id} could not be found");
    }

    public async Task UpdateAsync(ItemDto dto)
    {
        var dtoMainCategory =  ResolveOptionOrThrow(dto.MainCategory, ItemCategory.AllowedValues, "Item Category");
        var dtoOtherCategories = ResolveOptionOrThrow(dto.OtherCategories, ItemCategory.AllowedValues, "Item Category");
        var dtoRarity = dto.Rarity is not null ? ResolveOptionOrThrow(dto.Rarity, ItemRarity.AllowedValues, "Item Rarity") : null;

        var item = await repo.GetByIdAsync(dto.Id) ?? throw new NotFoundException($"Item with id {dto.Id} could not be found");

        item.Name = dto.Name;
        item.Description = dto.Description;
        item.Categories = [dtoMainCategory, .. dtoOtherCategories];
        item.Value = dto.Value;
        item.Rarity = dtoRarity ?? item.Rarity;
        item.RequiresAttunement = dto.RequiresAttunement ?? item.RequiresAttunement;
        item.IsHomebrew = dto.IsHomebrew ?? item.IsHomebrew;
        item.Weight = dto.Weight ?? item.Weight;

        await repo.UpdateAsync(item);
    }

    public ICollection<Item> SortBy(ICollection<Item> items, string sortFilter, bool descending = false)
    {
        if(!TryResolveOption(sortFilter, SortItemOption.AllowedValues, out string? resolved))
            return items;

        return resolved switch
        {
            SortItemOption.Name => OrderByMany(items, [(i => i.Name)], descending),
            SortItemOption.Category => OrderByMany(items, [(i => i.Categories.FirstOrDefault()!), (i => i.Name)], descending),
            SortItemOption.Value => OrderByMany(items, [(i => i.Value), (i => i.Name)], descending),
            SortItemOption.Weight => OrderByMany(items, [(i => i.Weight), (i => i.Name)], descending),
            SortItemOption.Rarity => OrderByMany(items, [(i => i.Rarity == null), (i => i.Rarity!), (i => i.Name)], descending),
            _ => throw new ArgumentOutOfRangeException(nameof(sortFilter), "Invalid sort option provided.")
        };
    }
}