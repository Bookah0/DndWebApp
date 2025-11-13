using DndWebApp.Api.Models.DTOs;
using DndWebApp.Api.Models.Items;
using DndWebApp.Api.Models.Items.Enums;
using DndWebApp.Api.Repositories;
using DndWebApp.Api.Repositories.Interfaces;
using DndWebApp.Api.Services.Enums;
using DndWebApp.Api.Services.Util;

namespace DndWebApp.Api.Services.Implemented.Items;

public class WeaponService
{
    private readonly IRepository<Weapon> repo;
    private readonly ILogger<WeaponService> logger;

    public WeaponService(IRepository<Weapon> repo, ILogger<WeaponService> logger)
    {
        this.repo = repo;
        this.logger = logger;
    }

    public async Task<Weapon> CreateAsync(WeaponDto dto)
    {
        ValidationUtil.HasContentOrThrow(dto.Name);
        ValidationUtil.HasContentOrThrow(dto.Description);
        ValidationUtil.HasContentOrThrow(dto.DamageDice);
        ValidationUtil.AboveZeroOrThrow(dto.Weight);
        ValidationUtil.AboveZeroOrThrow(dto.Value);
        ValidationUtil.AboveZeroOrThrow(dto.Range);

        var dtoCategory = NormalizationUtil.ParseEnumOrThrow<WeaponCategory>(dto.WeaponCategory);
        var dtoWeaponType = NormalizationUtil.ParseEnumOrThrow<WeaponType>(dto.WeaponType);
        var dtoMainDamageType = NormalizationUtil.ParseEnumOrThrow<DamageType>(dto.MainDamageType);
        var dtoOtherDamageTypes = NormalizationUtil.ParseEnumOrThrow<DamageType>(dto.OtherDamageTypes);
        var dtoRarity = NormalizationUtil.ParseEnumOrThrow<ItemRarity>(dto.Rarity);
        var dtoProperties = NormalizationUtil.ParseEnumOrThrow<WeaponProperty>(dto.Properties);

        Weapon weapon = new()
        {
            Name = dto.Name,
            Description = dto.Description,
            Weight = dto.Weight,
            Value = dto.Value,
            WeaponCategory = dtoCategory,
            WeaponType = dtoWeaponType,
            DamageDice = dto.DamageDice,
            Range = dto.Range,
            DamageTypes = [dtoMainDamageType, .. dtoOtherDamageTypes],
            Properties = dtoProperties,
            VersitileDamageDice = dto.VersitileDamageDice ?? "",
            LongRange = dto.LongRange,
            Rarity = dtoRarity,
            RequiresAttunement = dto.RequiresAttunement ?? false,
            IsHomebrew = dto.IsHomebrew ?? false,
            Categories = [ItemCategory.Weapon]
        };

        return await repo.CreateAsync(weapon);
    }

    public async Task DeleteAsync(int id)
    {
        var weapon = await repo.GetByIdAsync(id) ?? throw new NullReferenceException($"Weapon with id {id} could not be found");
        await repo.DeleteAsync(weapon);
    }

    public async Task<ICollection<Weapon>> GetAllAsync()
    {
        return await repo.GetAllAsync();
    }

    public async Task<Weapon> GetByIdAsync(int id)
    {
        return await repo.GetByIdAsync(id) ?? throw new NullReferenceException($"Weapon with id {id} could not be found");
    }

    public async Task UpdateAsync(WeaponDto dto)
    {
        ValidationUtil.HasContentOrThrow(dto.Name);
        ValidationUtil.HasContentOrThrow(dto.Description);
        ValidationUtil.HasContentOrThrow(dto.DamageDice);
        ValidationUtil.AboveZeroOrThrow(dto.Weight);
        ValidationUtil.AboveZeroOrThrow(dto.Weight);
        ValidationUtil.AboveZeroOrThrow(dto.Range);

        var dtoCategory = NormalizationUtil.ParseEnumOrThrow<WeaponCategory>(dto.WeaponCategory);
        var dtoWeaponType = NormalizationUtil.ParseEnumOrThrow<WeaponType>(dto.WeaponType);
        var dtoMainDamageType = NormalizationUtil.ParseEnumOrThrow<DamageType>(dto.MainDamageType);
        var dtoOtherDamageTypes = NormalizationUtil.ParseEnumOrThrow<DamageType>(dto.OtherDamageTypes);
        var dtoRarity = NormalizationUtil.ParseEnumOrThrow<ItemRarity>(dto.Rarity);
        var dtoProperties = NormalizationUtil.ParseEnumOrThrow<WeaponProperty>(dto.Properties);

        var weapon = await repo.GetByIdAsync(dto.Id) ?? throw new NullReferenceException($"Weapon with id {dto.Id} could not be found");

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
        weapon.VersitileDamageDice = dto.VersitileDamageDice ?? weapon.VersitileDamageDice;
        weapon.LongRange = dto.LongRange ?? weapon.LongRange;
        weapon.Rarity = dtoRarity == 0 ? weapon.Rarity : dtoRarity;
        weapon.RequiresAttunement = dto.RequiresAttunement ?? weapon.RequiresAttunement;
        weapon.IsHomebrew = dto.IsHomebrew ?? weapon.IsHomebrew;

        await repo.UpdateAsync(weapon);
    }

    
    public ICollection<Weapon> SortBy(ICollection<Weapon> weapons, WeaponSortFilter sortFilter, bool descending = false)
    {
        return sortFilter switch
        {
            WeaponSortFilter.Name => SortUtil.OrderByMany(weapons, [(i => i.Name)], descending),
            WeaponSortFilter.Category => SortUtil.OrderByMany(weapons, [(i => i.WeaponCategory), (i => i.Name)], descending),
            WeaponSortFilter.Type => SortUtil.OrderByMany(weapons, [(i => i.WeaponType), (i => i.Name)], descending),
            WeaponSortFilter.Value => SortUtil.OrderByMany(weapons, [(i => i.Value), (i => i.Name)], descending),
            WeaponSortFilter.Weight => SortUtil.OrderByMany(weapons, [(i => i.Weight), (i => i.Name)], descending),
            WeaponSortFilter.Rarity => SortUtil.OrderByMany(weapons, [(i => i.Rarity == null), (i => i.Rarity!), (i => i.Name)], descending),
            _ => weapons,
        };
    }
}