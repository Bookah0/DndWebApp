using DndWebApp.Api.Middlewares.ExceptionHandling;
using DndWebApp.Api.Models.DTOs.Inventory;
using DndWebApp.Api.Models.Items;
using DndWebApp.Api.Repositories.Interfaces;
using DndWebApp.Api.Services.Constants;
using static DndWebApp.Api.Services.Util.SortUtil;
using static DndWebApp.Api.Services.Util.ConstantsUtil;
using DndWebApp.Api.Models.Items.Constants;

namespace DndWebApp.Api.Services.Implemented.Items;

public class WeaponService(IRepository<Weapon> repo, ILogger<WeaponService> logger)
{
    public async Task<Weapon> CreateAsync(WeaponDto dto)
    {
        logger.LogInformation("Creating weapon, Name: {WeaponName}", dto.Name);
        var dtoCategory = ResolveOptionOrThrow(dto.WeaponCategory, WeaponCategory.AllowedValues, "Weapon Category");
        var dtoWeaponType = ResolveOptionOrThrow(dto.WeaponType, WeaponType.AllowedValues, "Weapon Type");
        var dtoMainDamageType = ResolveOptionOrThrow(dto.MainDamageType, DamageType.AllowedValues, "Main Damage Type");
        var dtoOtherDamageTypes = ResolveOptionOrThrow(dto.OtherDamageTypes, DamageType.AllowedValues, "Other Damage Types");
        var dtoRarity = dto.Rarity is not null ? ResolveOptionOrThrow(dto.Rarity, ItemRarity.AllowedValues, "Item Rarity") : null;
        var dtoProperties = ResolveOptionOrThrow(dto.Properties, WeaponProperty.AllowedValues, "Weapon Property");

        Weapon weapon = await repo.CreateAsync(new()
        {
            Name = dto.Name,
            Description = dto.Description,
            Weight = dto.Weight,
            Value = dto.Value,
            WeaponCategory = dtoCategory,
            WeaponType = dtoWeaponType,
            Slot = ConvertWeaponTypeToMainSlot(dtoWeaponType),
            DamageDice = dto.DamageDice,
            Range = dto.Range,
            DamageTypes = [dtoMainDamageType, .. dtoOtherDamageTypes],
            Properties = dtoProperties,
            VersatileDamageDice = dto.VersitileDamageDice ?? "",
            LongRange = dto.LongRange,
            Rarity = dtoRarity,
            RequiresAttunement = dto.RequiresAttunement ?? false,
            IsHomebrew = dto.IsHomebrew ?? false,
            Categories = [ItemCategory.Weapon]
        });

        logger.LogInformation("Successfully created weapon, Name: {WeaponName}, ID: {WeaponId}", weapon.Name, weapon.Id);
        return weapon;
    }

    public async Task DeleteAsync(int id)
    {
        var weapon = await repo.GetByIdAsync(id) ?? throw new NotFoundException($"Weapon with id {id} could not be found");
        logger.LogInformation("Deleting weapon, Name: {WeaponName}, ID: {WeaponId}", weapon.Name, id);
        await repo.DeleteAsync(weapon);
        logger.LogInformation("Successfully deleted weapon, Name: {WeaponName}, ID: {WeaponId}", weapon.Name, id);
    }

    public async Task<ICollection<Weapon>> GetAllAsync()
    {
        return await repo.GetAllAsync();
    }

    public async Task<Weapon> GetByIdAsync(int id)
    {
        return await repo.GetByIdAsync(id) ?? throw new NotFoundException($"Weapon with id {id} could not be found");
    }

    public async Task UpdateAsync(int id, WeaponDto dto)
    {
        logger.LogInformation("Updating weapon, Name: {WeaponName}, ID: {WeaponId}", dto.Name, id);

        var dtoCategory = ResolveOptionOrThrow(dto.WeaponCategory, WeaponCategory.AllowedValues, "Weapon Category");
        var dtoWeaponType = ResolveOptionOrThrow(dto.WeaponType, WeaponType.AllowedValues, "Weapon Type");
        var dtoMainDamageType = ResolveOptionOrThrow(dto.MainDamageType, DamageType.AllowedValues, "Main Damage Type");
        var dtoOtherDamageTypes = ResolveOptionOrThrow(dto.OtherDamageTypes, DamageType.AllowedValues, "Other Damage Types");
        var dtoRarity = dto.Rarity is not null ? ResolveOptionOrThrow(dto.Rarity, ItemRarity.AllowedValues, "Item Rarity") : null;
        var dtoProperties = ResolveOptionOrThrow(dto.Properties, WeaponProperty.AllowedValues, "Weapon Property");

        var weapon = await repo.GetByIdAsync(id) ?? throw new NotFoundException($"Weapon with id {id} could not be found");

        weapon.Name = dto.Name;
        weapon.Description = dto.Description;
        weapon.Weight = dto.Weight;
        weapon.Value = dto.Value;
        weapon.WeaponCategory = dtoCategory;
        weapon.WeaponType = dtoWeaponType;
        weapon.DamageDice = dto.DamageDice;
        weapon.Range = dto.Range;
        weapon.DamageTypes = [dtoMainDamageType, .. dtoOtherDamageTypes];
        weapon.Properties = dtoProperties;
        weapon.VersatileDamageDice = dto.VersitileDamageDice ?? weapon.VersatileDamageDice;
        weapon.LongRange = dto.LongRange ?? weapon.LongRange;
        weapon.Rarity = dtoRarity ?? weapon.Rarity;
        weapon.RequiresAttunement = dto.RequiresAttunement ?? weapon.RequiresAttunement;
        weapon.IsHomebrew = dto.IsHomebrew ?? weapon.IsHomebrew;

        await repo.UpdateAsync(weapon);
        logger.LogInformation("Successfully updated weapon, Name: {WeaponName}, ID: {WeaponId}", weapon.Name, weapon.Id);
    }

    // TODO replace with database level sorting
    public ICollection<Weapon> SortBy(ICollection<Weapon> weapons, string sortFilter, bool descending = false)
    {
        if(!TryResolveOption(sortFilter, SortWeaponOption.AllowedValues, out string? resolved))
            return weapons;

        return resolved switch
        {
            SortWeaponOption.Name => OrderByMany(weapons, [(i => i.Name)], descending),
            SortWeaponOption.Category => OrderByMany(weapons, [(i => i.WeaponCategory), (i => i.Name)], descending),
            SortWeaponOption.Type => OrderByMany(weapons, [(i => i.WeaponType), (i => i.Name)], descending),
            SortWeaponOption.Value => OrderByMany(weapons, [(i => i.Value), (i => i.Name)], descending),
            SortWeaponOption.Weight => OrderByMany(weapons, [(i => i.Weight), (i => i.Name)], descending),
            SortWeaponOption.Rarity => OrderByMany(weapons, [(i => i.Rarity == null), (i => i.Rarity!), (i => i.Name)], descending),
            _ => throw new ArgumentOutOfRangeException(nameof(sortFilter), "Invalid sort option provided.")
        };
    }
}