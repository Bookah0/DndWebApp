using static Api.Infrastructure.Validation.ValuesValidator;
using Api.Domain.Items.DTOs;
using Api.Domain.Items.Models;
using Api.Domain.Shared.DTOs;
using Api.Domain.Shared.Enums.Items;
using Api.Domain.Users.Services;
using Api.Domain.Items.Repositories;
using Api.Infrastructure.Middleware.ExceptionHandling;
using Api.Domain.Shared.Utils;


namespace Api.Domain.Items.Services;

public class ArmorService(IArmorRepository repo, ICurrentUserService currentUserService, ILogger<ArmorService> logger) : IArmorService
{
    public async Task<Armor> CreateAsync(CreateArmorRequestDto dto)
    {
        var dtoCategory = NormalizeValue<ArmorCategory>(dto.Category);
        var dtoRarity = dto.Rarity != null ? NormalizeValue<ItemRarity>(dto.Rarity) : null;

        logger.LogInformation("Creating armor, Name: {ArmorName}", dto.Name);

        Armor armor = await repo.CreateAsync(new()
        {
            Name = dto.Name,
            Description = dto.Description ?? "",
            Weight = dto.Weight,
            Value = dto.Value,
            ArmorCategory = dtoCategory,
            BaseArmorClass = dto.BaseArmorClass,
            PlusDexMod = dto.PlusDexMod,
            StealthDisadvantage = dto.StealthDisadvantage,
            ModCap = dto.ModCap ?? 0,
            StrengthScoreRequired = dto.StrengthScoreRequired,
            Rarity = dtoRarity ?? ItemRarity.Common,
            RequiresAttunement = dto.RequiresAttunement,
            Categories = [ItemCategory.Armor],
            EquipSlot = dtoCategory.Equals(ArmorCategory.Shield) ? EquipSlot.OffHand : EquipSlot.Armor,

            CreatedAt = DateTime.UtcNow,
            CreatedBy = currentUserService.GetCurrentUserId(),
        });

        logger.LogInformation("Successfully created armor, Name: {ArmorName}, ID: {ArmorId}", armor.Name, armor.Id);
        return armor;
    }

    public async Task DeleteAsync(int id)
    {
        var armor = await repo.GetByIdAsync(id);
        logger.LogInformation("Deleting armor with ID: {ArmorId}", id);
        await repo.DeleteAsync(armor);
        logger.LogInformation("Successfully deleted armor with ID: {ArmorId}", id);
    }

    public async Task<ICollection<Armor>> GetAllAsync() => await repo.GetAllAsync();
    public async Task<Armor> GetByIdAsync(int id) => await repo.GetByIdAsync(id);

    public async Task<Armor> UpdateAsync(UpdateArmorRequestDto dto, int id)
    {
        var dtoCategory = dto.Category is not null ? NormalizeValue<ArmorCategory>(dto.Category) : null;
        var dtoRarity = dto.Rarity is not null ? NormalizeValue<ItemRarity>(dto.Rarity) : null;

        var armor = await repo.GetByIdAsync(id);
        logger.LogInformation("Updating armor, Name: {ArmorName}, ID: {ArmorId}", armor.Name, armor.Id);

        armor.Name = dto.Name ?? armor.Name;
        armor.Description = dto.Description ?? armor.Description;
        armor.Weight = dto.Weight ?? armor.Weight;
        armor.Value = dto.Value ?? armor.Value;
        armor.ArmorCategory = dtoCategory ?? armor.ArmorCategory;
        armor.BaseArmorClass = dto.BaseArmorClass ?? armor.BaseArmorClass;
        armor.PlusDexMod = dto.PlusDexMod ?? armor.PlusDexMod;
        armor.StealthDisadvantage = dto.StealthDisadvantage ?? armor.StealthDisadvantage;
        armor.ModCap = dto.ModCap ?? armor.ModCap;
        armor.StrengthScoreRequired = dto.StrengthScoreRequired ?? armor.StrengthScoreRequired;
        armor.Rarity = dtoRarity ?? armor.Rarity;
        armor.RequiresAttunement = dto.RequiresAttunement ?? armor.RequiresAttunement;
       
        armor.EquipSlot = dtoCategory is not null 
            ? dtoCategory.Equals(ArmorCategory.Shield) 
            ? EquipSlot.OffHand 
            : EquipSlot.Armor 
            : armor.EquipSlot;

        armor.IsPublic = dto.IsPublic ?? armor.IsPublic;
        armor.CloningAllowed = dto.CloningAllowed ?? armor.CloningAllowed;
        armor.UpdatedAt = DateTime.UtcNow;

        await repo.UpdateAsync(armor);
        logger.LogInformation("Successfully updated armor, Name: {ArmorName}, ID: {ArmorId}", armor.Name, armor.Id);
        return armor;
    }

    public async Task<(int, ICollection<Armor>)> GetFilteredAsync(ArmorFilterDto filter, PaginationRequestDto pagination) 
    {
        ValidateFilterAsync(filter);
        var (count, filtered) = await repo.GetFilteredAsync(filter, pagination);

        if(!filtered.HasContent() && count > 0)
            throw new ValidationException("Page does not contain any elements");

        return (count, filtered);
    }

    public void ValidateFilterAsync(ArmorFilterDto dto)
    {
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

        if (dto.MinAC is not null && dto.MaxAC is not null && dto.MinAC > dto.MaxAC)
            throw new ValidationException("Maximum AC must be greater than or equal to minimum AC");
        if (dto.MinAC is not null && dto.MinAC < 0)
            throw new ValidationException("Minimum AC must be greater than or equal to zero");
        if (dto.MaxAC is not null && dto.MaxAC < 0)
            throw new ValidationException("Maximum AC must be greater than or equal to zero");
        
        if(dto.Rarity != null)
            dto.Rarity = NormalizeValue<ItemRarity>(dto.Rarity);
        if(dto.ArmorCategory != null)
            dto.ArmorCategory = NormalizeValue<ArmorCategory>(dto.ArmorCategory);
            
        dto.Category = NormalizeValue<ItemCategory>(dto.Category);
    }
}