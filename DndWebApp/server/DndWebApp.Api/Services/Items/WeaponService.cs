using DndWebApp.Api.Models.DTOs;
using DndWebApp.Api.Models.Items;
using DndWebApp.Api.Models.Items.Enums;
using DndWebApp.Api.Repositories;
using DndWebApp.Api.Services.Generic;
using DndWebApp.Api.Services.Util;

namespace DndWebApp.Api.Services.Items;

public class WeaponService : IService<Weapon, WeaponDto, WeaponDto>
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
        ValidationUtil.NotNullOrWhiteSpace(dto.Name);
        ValidationUtil.NotNullOrWhiteSpace(dto.Description);
        ValidationUtil.NotNullOrWhiteSpace(dto.DamageDice);
        ValidationUtil.NotNullAboveZero(dto.Weight);
        ValidationUtil.NotNullAboveZero(dto.Value);
        ValidationUtil.NotNullAboveZero(dto.Range);

        var dtoCategory = ValidationUtil.ParseEnumOrThrow<WeaponCategory>(dto.WeaponCategory);
        var dtoWeaponType = ValidationUtil.ParseEnumOrThrow<WeaponType>(dto.WeaponType);
        var dtoMainDamageType = ValidationUtil.ParseEnumOrThrow<DamageType>(dto.MainDamageType);
        var dtoOtherDamageTypes = ValidationUtil.ParseEnumOrThrow<DamageType>(dto.OtherDamageTypes);
        var dtoRarity = ValidationUtil.ParseEnumOrThrow<ItemRarity>(dto.Rarity);
        var dtoProperties = ValidationUtil.ParseEnumOrThrow<WeaponProperty>(dto.Properties);

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
        ValidationUtil.NotNullOrWhiteSpace(dto.Name);
        ValidationUtil.NotNullOrWhiteSpace(dto.Description);
        ValidationUtil.NotNullOrWhiteSpace(dto.DamageDice);
        ValidationUtil.NotNullAboveZero(dto.Weight);
        ValidationUtil.NotNullAboveZero(dto.Weight);
        ValidationUtil.NotNullAboveZero(dto.Range);

        var dtoCategory = ValidationUtil.ParseEnumOrThrow<WeaponCategory>(dto.WeaponCategory);
        var dtoWeaponType = ValidationUtil.ParseEnumOrThrow<WeaponType>(dto.WeaponType);
        var dtoMainDamageType = ValidationUtil.ParseEnumOrThrow<DamageType>(dto.MainDamageType);
        var dtoOtherDamageTypes = ValidationUtil.ParseEnumOrThrow<DamageType>(dto.OtherDamageTypes);
        var dtoRarity = ValidationUtil.ParseEnumOrThrow<ItemRarity>(dto.Rarity);
        var dtoProperties = ValidationUtil.ParseEnumOrThrow<WeaponProperty>(dto.Properties);

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

    public enum WeaponSortingFilter { Name, Category, Type, Value, Weight, Rarity }
    public ICollection<Weapon> SortBy(ICollection<Weapon> weapons, WeaponSortingFilter sortFilter, bool descending = false)
    {
        return sortFilter switch
        {
            WeaponSortingFilter.Name => SortUtil.OrderByMany(weapons, [(i => i.Name)], descending),
            WeaponSortingFilter.Category => SortUtil.OrderByMany(weapons, [(i => i.WeaponCategory), (i => i.Name)], descending),
            WeaponSortingFilter.Type => SortUtil.OrderByMany(weapons, [(i => i.WeaponType), (i => i.Name)], descending),
            WeaponSortingFilter.Value => SortUtil.OrderByMany(weapons, [(i => i.Value), (i => i.Name)], descending),
            WeaponSortingFilter.Weight => SortUtil.OrderByMany(weapons, [(i => i.Weight), (i => i.Name)], descending),
            WeaponSortingFilter.Rarity => SortUtil.OrderByMany(weapons, [(i => i.Rarity == null), (i => i.Rarity!), (i => i.Name)], descending),
            _ => weapons,
        };
    }
}