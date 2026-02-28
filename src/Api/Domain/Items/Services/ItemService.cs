using static Api.Infrastructure.Validation.ValuesValidator;
using Api.Domain.Items.DTOs;
using Api.Domain.Items.Models;
using Api.Domain.Items.Repositories;
using Api.Domain.Shared.DTOs;
using Api.Domain.Shared.Utils;
using Api.Domain.Users.Services;
using Api.Infrastructure.Middleware.ExceptionHandling;
using Api.Domain.Shared.Enums.Items;

namespace Api.Domain.Items.Services;

public class ItemService(IItemRepository repo, ICurrentUserService currentUserService, ILogger<ItemService> logger) : IItemService
{
    public async Task<Item> CreateAsync(CreateItemRequestDto dto)
    {
        if(dto.Categories == null || dto.Categories.Count < 1)
            throw new ValidationException("At least one category is required for an item.");

        var dtoCategories = NormalizeValue<ItemCategory>(dto.Categories);
        var dtoRarity = NormalizeValue<ItemRarity>(dto.Rarity);
        
        logger.LogInformation("Creating item, Name: {ItemName}", dto.Name);

        Item item = await repo.CreateAsync(new()
        {
            Name = dto.Name,
            Description = dto.Description,
            Categories = dtoCategories ?? [],
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

    public async Task<ICollection<Item>> GetAllAsync()  => await repo.GetAllAsync();

    public async Task<Item> GetByIdAsync(int id) => await repo.GetByIdAsync(id);

    public async Task<Item> UpdateAsync(UpdateItemRequestDto dto, int id)
    {
        var dtoRarity = dto.Rarity is not null ? NormalizeValue<ItemRarity>(dto.Rarity) : null;

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

    public async Task<ICollection<Item>> GetAllAsync(ItemFilterDto? filter = null, PaginationRequestDto? pagination = null) 
    {
        ValidateFilterAsync(filter);
        var filtered = await repo.GetAllAsync(filter, pagination);

        if(!filtered.HasContent() && filtered.Count > 0)
            throw new ValidationException("Page does not contain any elements");

        return filtered;
    }

    public void ValidateFilterAsync(ItemFilterDto? dto)
    {
        if (dto is null) return;

        if (dto.MinValue is not null && dto.MaxValue is not null && dto.MinValue > dto.MaxValue)
            throw new ValidationException("Maximum value must be greater than or equal to minimum value");
        if (dto.MinValue is not null && dto.MinValue < 0)
            throw new ValidationException("Minimum value must be greater than or equal to zero");
        if (dto.MaxValue is not null && dto.MaxValue < 0)
            throw new ValidationException("Maximum value must be greater than or equal to zero");
        
        if (dto.MinWeight is not null && dto.MaxWeight is not null && dto.MinWeight > dto.MaxWeight)
            throw new ValidationException("Maximum weight must be greater than or equal to minimum weight");
        if (dto.MinWeight is not null && dto.MinWeight < 0)
            throw new ValidationException("Minimum weight must be greater than or equal to zero");
        if (dto.MaxWeight is not null && dto.MaxWeight < 0)
            throw new ValidationException("Maximum weight must be greater than or equal to zero");
        
        if(dto.Rarity != null)
            dto.Rarity = NormalizeValue<ItemRarity>(dto.Rarity);

        dto.ItemCategories = NormalizeValue<ItemCategory>(dto.ItemCategories);
    }
}