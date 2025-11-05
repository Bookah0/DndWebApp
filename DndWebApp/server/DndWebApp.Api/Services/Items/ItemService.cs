using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs;
using DndWebApp.Api.Models.Items;
using DndWebApp.Api.Models.Items.Enums;
using DndWebApp.Api.Repositories.Classes;
using DndWebApp.Api.Repositories.Features;
using DndWebApp.Api.Repositories.Items;
using DndWebApp.Api.Services.Generic;
using DndWebApp.Api.Services.Util;

namespace DndWebApp.Api.Services.Items;

public class ItemService : IService<Item, ItemDto>
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

        var dtoMainCategory = ValidationUtil.ParseEnumOrThrow<ItemCategory>(dto.MainCategory);
        var dtoOtherCategories = ValidationUtil.ParseEnumOrThrow<ItemCategory>(dto.OtherCategories);
        var dtoRarity = ValidationUtil.ParseEnumOrThrow<ItemRarity>(dto.Rarity);

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

    public async Task<ICollection<Item>> GetAllAsync()
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

        var dtoMainCategory = ValidationUtil.ParseEnumOrThrow<ItemCategory>(dto.MainCategory);
        var dtoOtherCategories = ValidationUtil.ParseEnumOrThrow<ItemCategory>(dto.OtherCategories);
        var dtoRarity = ValidationUtil.ParseEnumOrThrow<ItemRarity>(dto.Rarity);

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

    public enum ItemSortingFilter { Name, Category, Value, Weight, Rarity }
    public ICollection<Item> SortBy(ICollection<Item> items, ItemSortingFilter sortFilter, bool descending = false)
    {
        return sortFilter switch
        {
            ItemSortingFilter.Name => SortUtil.OrderByMany(items, [(i => i.Name)], descending),
            ItemSortingFilter.Category => SortUtil.OrderByMany(items, [(i => i.Categories.FirstOrDefault()), (i => i.Name)], descending),
            ItemSortingFilter.Value => SortUtil.OrderByMany(items, [(i => i.Value), (i => i.Name)], descending),
            ItemSortingFilter.Weight => SortUtil.OrderByMany(items, [(i => i.Weight), (i => i.Name)], descending),
            ItemSortingFilter.Rarity => SortUtil.OrderByMany(items, [(i => i.Rarity == null), (i => i.Rarity!), (i => i.Name)], descending),
            _ => items,
        };
    }
}