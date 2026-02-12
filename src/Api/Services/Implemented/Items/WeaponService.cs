using Api.Middlewares.ExceptionHandling;
using Api.Models.DTOs.RequestDtos.Inventory;
using Api.Models.Items;
using Api.Repositories.Interfaces;
using static Api.Services.Util.SortUtil;
using static Api.Validation.AllowedValues.ValuesValidator;
using Api.Services.Interfaces.Items;
using Api.Services.Interfaces;
using Api.Services.Util;
using Api.Validation.AllowedValues.Items;
using Api.Validation.AllowedValues;

namespace Api.Services.Implemented.Items;

public class WeaponService(IRepository<Weapon> repo, ICurrentUserService currentUserService, ILogger<WeaponService> logger) : IWeaponService
{
    public async Task<Weapon> CreateAsync(CreateWeaponRequestDto dto)
    {
        logger.LogInformation("Creating weapon, Name: {WeaponName}", dto.Name);
        var dtoCategory = ResolveValueOrThrow<WeaponCategory>(dto.WeaponCategory);
        var dtoWeaponType = ResolveValueOrThrow<WeaponType>(dto.WeaponType);
        var dtoRarity = dto.Rarity is not null ? ResolveValueOrThrow<ItemRarity>(dto.Rarity) : null;
        var dtoProperties = ResolveValueOrThrow<WeaponProperty>(dto.Properties);

        Weapon weapon = await repo.CreateAsync(new()
        {
            Name = dto.Name,
            Description = dto.Description,
            Weight = dto.Weight ?? 0,
            Value = dto.Value ?? 0,
            WeaponCategory = dtoCategory,
            WeaponType = dtoWeaponType,
            Slot = GetDefaultWeaponMainSlot(dtoWeaponType),
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
                ? ResolveValueOrThrow<DamageType>(dto.DamageTypes)!
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

        var dtoCategory = dto.WeaponCategory is not null ? ResolveValueOrThrow<WeaponCategory>(dto.WeaponCategory) : null;
        var dtoWeaponType = dto.WeaponType is not null ? ResolveValueOrThrow<WeaponType>(dto.WeaponType) : null;
        var dtoRarity = dto.Rarity is not null ? ResolveValueOrThrow<ItemRarity>(dto.Rarity) : null;
        var dtoSlot = dto.Slot is not null ? ResolveValueOrThrow<EquipSlot>(dto.Slot) : null;

        weapon.WeaponCategory = dtoCategory ?? weapon.WeaponCategory;
        weapon.WeaponType = dtoWeaponType ?? weapon.WeaponType;
        weapon.Rarity = dtoRarity ?? weapon.Rarity;
        weapon.Slot = dtoSlot ?? weapon.Slot;
        
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

    // TODO replace with database level sorting
    public ICollection<Weapon> SortBy(ICollection<Weapon> weapons, string sortFilter, bool descending = false)
    {
        if(!TryResolveValue<SortWeaponOption>(sortFilter, out string? resolved))
            return weapons;

        return resolved switch
        {
            SortWeaponOption.Name => OrderByMany(weapons, [(i => i.Name)], descending),
            SortWeaponOption.Category => OrderByMany(weapons, [(i => i.WeaponCategory), (i => i.Name)], descending),
            SortWeaponOption.Type => OrderByMany(weapons, [(i => i.WeaponType), (i => i.Name)], descending),
            SortWeaponOption.Value => OrderByMany(weapons, [(i => i.Value!), (i => i.Name)], descending),
            SortWeaponOption.Weight => OrderByMany(weapons, [(i => i.Weight!), (i => i.Name)], descending),
            SortWeaponOption.Rarity => OrderByMany(weapons, [(i => i.Rarity == null), (i => i.Rarity!), (i => i.Name)], descending),
            _ => throw new ValidationException($"Invalid sort option: {sortFilter}")
        };
    }
}