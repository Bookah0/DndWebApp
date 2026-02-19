using Api.Middlewares.ExceptionHandling;
using Api.Models.DTOs.RequestDtos.Inventory;
using Api.Models.Items;
using Api.Repositories.Interfaces;
using static Api.Services.Util.QueryUtil;
using static Api.Validation.AllowedValues.ValuesValidator;
using Api.Services.Interfaces.Items;
using Api.Services.Interfaces;
using Api.Services.Util;
using Api.Validation.AllowedValues.Items;
using Api.Validation.AllowedValues;
using Api.Models.DTOs.Items;
using Api.Models.DTOs.ResponseDtos;

namespace Api.Services.Implemented.Items;

public class WeaponService(IWeaponRepository repo, ICurrentUserService currentUserService, ILogger<WeaponService> logger) : IWeaponService
{
    public async Task<Weapon> CreateAsync(CreateWeaponRequestDto dto)
    {
        logger.LogInformation("Creating weapon, Name: {WeaponName}", dto.Name);
        var dtoCategory = NormalizeValueOrThrow<WeaponCategory>(dto.WeaponCategory);
        var dtoWeaponType = NormalizeValueOrThrow<WeaponType>(dto.WeaponType);
        var dtoRarity = dto.Rarity is not null ? NormalizeValueOrThrow<ItemRarity>(dto.Rarity) : null;
        var dtoProperties = NormalizeValueOrThrow<WeaponProperty>(dto.Properties);

        Weapon weapon = await repo.CreateAsync(new()
        {
            Name = dto.Name,
            Description = dto.Description,
            Weight = dto.Weight ?? 0,
            Value = dto.Value ?? 0,
            WeaponCategory = dtoCategory,
            WeaponType = dtoWeaponType,
            EquipSlot = GetDefaultWeaponMainSlot(dtoWeaponType),
            DamageDice = dto.DamageDice,
            Range = dto.Range,
            Properties = dtoProperties ?? [],
            VersatileDamageDice = dto.VersitileDamageDice,
            LongRange = dto.LongRange,
            Rarity = dtoRarity ?? ItemRarity.Common,
            RequiresAttunement = dto.RequiresAttunement,
            Categories = [ItemCategory.Weapon],

            CreatedAt = DateTime.UtcNow,
            CreatedBy = currentUserService.GetCurrentUserId(),

            DamageTypes = dto.DamageTypes.HasContent()
                ? NormalizeValueOrThrow<DamageType>(dto.DamageTypes)!
                : GetDefaultWeaponDamageTypes(dto.WeaponType)
        });

        logger.LogInformation("Successfully created weapon, Name: {WeaponName}, ID: {WeaponId}", weapon.Name, weapon.Id);
        return weapon;
    }

    public async Task DeleteAsync(int id)
    {
        var weapon = await repo.GetByIdAsync(id);
        logger.LogInformation("Deleting weapon, Name: {WeaponName}, ID: {WeaponId}", weapon.Name, id);
        await repo.DeleteAsync(weapon);
        logger.LogInformation("Successfully deleted weapon, Name: {WeaponName}, ID: {WeaponId}", weapon.Name, id);
    }

    public async Task<ICollection<Weapon>> GetAllAsync() =>await repo.GetAllAsync();
    public async Task<Weapon> GetByIdAsync(int id) => await repo.GetByIdAsync(id);

    public async Task<Weapon> UpdateAsync(UpdateWeaponRequestDto dto, int id)
    {
        logger.LogInformation("Updating weapon, Name: {WeaponName}, ID: {WeaponId}", dto.Name, id);
        var weapon = await repo.GetByIdAsync(id);

        var dtoCategory = dto.WeaponCategory is not null ? NormalizeValueOrThrow<WeaponCategory>(dto.WeaponCategory) : null;
        var dtoWeaponType = dto.WeaponType is not null ? NormalizeValueOrThrow<WeaponType>(dto.WeaponType) : null;
        var dtoRarity = dto.Rarity is not null ? NormalizeValueOrThrow<ItemRarity>(dto.Rarity) : null;
        var dtoSlot = dto.Slot is not null ? NormalizeValueOrThrow<EquipSlot>(dto.Slot) : null;

        weapon.WeaponCategory = dtoCategory ?? weapon.WeaponCategory;
        weapon.WeaponType = dtoWeaponType ?? weapon.WeaponType;
        weapon.Rarity = dtoRarity ?? weapon.Rarity;
        weapon.EquipSlot = dtoSlot ?? weapon.EquipSlot;
        
        weapon.Name = dto.Name ?? weapon.Name;
        weapon.Description = dto.Description ?? weapon.Description;
        weapon.Weight = dto.Weight ?? weapon.Weight;
        weapon.Value = dto.Value ?? weapon.Value;
        weapon.DamageDice = dto.DamageDice ?? weapon.DamageDice;
        weapon.Range = dto.Range ?? weapon.Range;
        weapon.VersatileDamageDice = dto.VersitileDamageDice ?? weapon.VersatileDamageDice;
        weapon.LongRange = dto.LongRange ?? weapon.LongRange;
        weapon.RequiresAttunement = dto.RequiresAttunement ?? weapon.RequiresAttunement;

        weapon.IsPublic = dto.IsPublic ?? weapon.IsPublic;
        weapon.CloningAllowed = dto.CloningAllowed ?? weapon.CloningAllowed;
        weapon.UpdatedAt = DateTime.UtcNow;

        await repo.UpdateAsync(weapon);
        logger.LogInformation("Successfully updated weapon, Name: {WeaponName}, ID: {WeaponId}", weapon.Name, weapon.Id);
        return weapon;
    }

    public async Task<(int, ICollection<Weapon>)> GetFilteredAsync(WeaponFilterDto filter, PaginationRequestDto pagination) 
    {
        ValidateFilterAsync(filter);
        var (count, filtered) = await repo.GetFilteredAsync(filter, pagination);

        if(!filtered.HasContent() && count > 0)
            throw new ValidationException("Page does not contain any elements");

        return (count, filtered);
    }

    public void ValidateFilterAsync(WeaponFilterDto dto)
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
        
        if (dto.Name is not null)
            dto.Name = NormalizationUtil.NormalizeWhiteSpace(dto.Name);
        if(dto.Rarity != null)
            dto.Rarity = NormalizeValueOrThrow<ItemRarity>(dto.Rarity);
        if(dto.WeaponCategory != null)
            dto.WeaponCategory = NormalizeValueOrThrow<WeaponCategory>(dto.WeaponCategory);
        if(dto.WeaponType != null)
            dto.WeaponType = NormalizeValueOrThrow<WeaponType>(dto.WeaponType);
        if(dto.Slot != null)
            dto.Slot = NormalizeValueOrThrow<EquipSlot>(dto.Slot);
            
        dto.Category = NormalizeValueOrThrow<ItemCategory>(dto.Category);
    }
}