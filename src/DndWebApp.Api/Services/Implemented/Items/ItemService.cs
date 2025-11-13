using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs;
using DndWebApp.Api.Models.Items;
using DndWebApp.Api.Models.Items.Enums;
using DndWebApp.Api.Repositories.Interfaces;
using DndWebApp.Api.Services.Enums;
using DndWebApp.Api.Services.Util;

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
        ValidationUtil.HasContentOrThrow(dto.Name);
        ValidationUtil.HasContentOrThrow(dto.Description);
        ValidationUtil.HasContentOrThrow(dto.MainCategory);
        ValidationUtil.AboveZeroOrThrow(dto.Value);

        var dtoMainCategory = NormalizationUtil.ParseEnumOrThrow<ItemCategory>(dto.MainCategory);
        var dtoOtherCategories = NormalizationUtil.ParseEnumOrThrow<ItemCategory>(dto.OtherCategories);
        var dtoRarity = NormalizationUtil.ParseEnumOrThrow<ItemRarity>(dto.Rarity);

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
        var item = await repo.GetByIdAsync(id) ?? throw new NullReferenceException($"Item with id {id} could not be found");
        await repo.DeleteAsync(item);
    }

    public async Task<ICollection<Item>> GetMiscellaneousItemsAsync()
    {
        return await repo.GetAllAsync();
    }

    public async Task<Item> GetByIdAsync(int id)
    {
        return await repo.GetByIdAsync(id) ?? throw new NullReferenceException($"Item with id {id} could not be found");
    }

    public async Task UpdateAsync(ItemDto dto)
    {
        ValidationUtil.HasContentOrThrow(dto.Name);
        ValidationUtil.HasContentOrThrow(dto.Description);
        ValidationUtil.HasContentOrThrow(dto.MainCategory);
        ValidationUtil.AboveZeroOrThrow(dto.Value);

        var dtoMainCategory = NormalizationUtil.ParseEnumOrThrow<ItemCategory>(dto.MainCategory);
        var dtoOtherCategories = NormalizationUtil.ParseEnumOrThrow<ItemCategory>(dto.OtherCategories);
        var dtoRarity = NormalizationUtil.ParseEnumOrThrow<ItemRarity>(dto.Rarity);

        var item = await repo.GetByIdAsync(dto.Id) ?? throw new NullReferenceException($"Item with id {dto.Id} could not be found");

        item.Name = dto.Name;
        item.Description = dto.Description;
        item.Categories = [dtoMainCategory, .. dtoOtherCategories];
        item.Value = dto.Value;
        item.Rarity = dtoRarity == 0 ? item.Rarity : dtoRarity;
        item.RequiresAttunement = dto.RequiresAttunement ?? item.RequiresAttunement;
        item.IsHomebrew = dto.IsHomebrew ?? item.IsHomebrew;
        item.Weight = dto.Weight ?? item.Weight;

        await repo.UpdateAsync(item);
    }

    
    public ICollection<Item> SortBy(ICollection<Item> items, ItemSortFilter sortFilter, bool descending = false)
    {
        return sortFilter switch
        {
            ItemSortFilter.Name => SortUtil.OrderByMany(items, [(i => i.Name)], descending),
            ItemSortFilter.Category => SortUtil.OrderByMany(items, [(i => i.Categories.FirstOrDefault()), (i => i.Name)], descending),
            ItemSortFilter.Value => SortUtil.OrderByMany(items, [(i => i.Value), (i => i.Name)], descending),
            ItemSortFilter.Weight => SortUtil.OrderByMany(items, [(i => i.Weight), (i => i.Name)], descending),
            ItemSortFilter.Rarity => SortUtil.OrderByMany(items, [(i => i.Rarity == null), (i => i.Rarity!), (i => i.Name)], descending),
            _ => items,
        };
    }
}