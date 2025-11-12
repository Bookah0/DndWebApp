using DndWebApp.Api.Models.DTOs;
using DndWebApp.Api.Models.Items;
using DndWebApp.Api.Models.Items.Enums;
using DndWebApp.Api.Repositories;
using DndWebApp.Api.Repositories.Interfaces;
using DndWebApp.Api.Services.Enums;
using DndWebApp.Api.Services.Util;

namespace DndWebApp.Api.Services.Implemented.Items;

public class ArmorService
{
    private readonly IRepository<Armor> repo;
    private readonly ILogger<ArmorService> logger;

    public ArmorService(IRepository<Armor> repo, ILogger<ArmorService> logger)
    {
        this.repo = repo;
        this.logger = logger;
    }

    public async Task<Armor> CreateAsync(ArmorDto dto)
    {
        ValidationUtil.HasContentOrThrow(dto.Name);
        ValidationUtil.HasContentOrThrow(dto.Description);
        ValidationUtil.AboveZeroOrThrow(dto.Weight);
        ValidationUtil.AboveZeroOrThrow(dto.Value);
        ValidationUtil.AboveZeroOrThrow(dto.BaseArmorClass);

        var dtoCategory = ValidationUtil.ParseEnumOrThrow<ArmorCategory>(dto.Category);
        var dtoRarity = ValidationUtil.ParseEnumOrThrow<ItemRarity>(dto.Rarity);

        Armor armor = new()
        {
            Name = dto.Name,
            Description = dto.Description,
            Weight = dto.Weight,
            Value = dto.Value,
            Category = dtoCategory,
            BaseArmorClass = dto.BaseArmorClass,
            PlusDexMod = dto.PlusDexMod,
            StealthDisadvantage = dto.StealthDisadvantage ?? false,
            ModCap = dto.ModCap ?? 0,
            StrengthScoreRequired = dto.StrengthScoreRequired ?? 0,
            Rarity = dtoRarity,
            RequiresAttunement = dto.RequiresAttunement ?? false,
            IsHomebrew = dto.IsHomebrew ?? false,
            Categories = [ItemCategory.Armor]
        };

        return await repo.CreateAsync(armor);
    }

    public async Task DeleteAsync(int id)
    {
        var armor = await repo.GetByIdAsync(id) ?? throw new NullReferenceException($"Armor with id {id} could not be found");
        await repo.DeleteAsync(armor);
    }

    public async Task<ICollection<Armor>> GetAllAsync()
    {
        return await repo.GetMiscellaneousItemsAsync();
    }

    public async Task<Armor> GetByIdAsync(int id)
    {
        return await repo.GetByIdAsync(id) ?? throw new NullReferenceException($"Armor with id {id} could not be found");
    }

    public async Task UpdateAsync(ArmorDto dto)
    {
        ValidationUtil.HasContentOrThrow(dto.Name);
        ValidationUtil.HasContentOrThrow(dto.Description);
        ValidationUtil.AboveZeroOrThrow(dto.Weight);
        ValidationUtil.AboveZeroOrThrow(dto.Value);
        ValidationUtil.AboveZeroOrThrow(dto.BaseArmorClass);

        var dtoCategory = ValidationUtil.ParseEnumOrThrow<ArmorCategory>(dto.Category);
        var dtoRarity = ValidationUtil.ParseEnumOrThrow<ItemRarity>(dto.Rarity);

        var armor = await repo.GetByIdAsync(dto.Id) ?? throw new NullReferenceException($"Armor with id {dto.Id} could not be found");

        armor.Name = dto.Name;
        armor.Description = dto.Description;
        armor.Weight = dto.Weight;
        armor.Value = dto.Value;
        armor.Category = dtoCategory;
        armor.BaseArmorClass = dto.BaseArmorClass;
        armor.PlusDexMod = dto.PlusDexMod;
        armor.StealthDisadvantage = dto.StealthDisadvantage ?? armor.StealthDisadvantage;
        armor.ModCap = dto.ModCap ?? armor.ModCap;
        armor.StrengthScoreRequired = dto.StrengthScoreRequired ?? armor.StrengthScoreRequired;
        armor.Rarity = dtoRarity == 0 ? armor.Rarity : dtoRarity;
        armor.RequiresAttunement = dto.RequiresAttunement ?? armor.RequiresAttunement;
        armor.IsHomebrew = dto.IsHomebrew ?? armor.IsHomebrew;

        await repo.UpdateAsync(armor);
    }

    public ICollection<Armor> SortBy(ICollection<Armor> armors, ArmorSortFilter sortFilter, bool descending = false)
    {
        return sortFilter switch
        {
            ArmorSortFilter.Name => SortUtil.OrderByMany(armors, [(i => i.Name)], descending),
            ArmorSortFilter.Category => SortUtil.OrderByMany(armors, [(i => i.Category), (i => i.Name)], descending),
            ArmorSortFilter.AC => SortUtil.OrderByMany(armors, [(i => i.BaseArmorClass), (i => i.Name)], descending),
            ArmorSortFilter.Value => SortUtil.OrderByMany(armors, [(i => i.Value), (i => i.Name)], descending),
            ArmorSortFilter.Weight => SortUtil.OrderByMany(armors, [(i => i.Weight), (i => i.Name)], descending),
            ArmorSortFilter.Rarity => SortUtil.OrderByMany(armors, [(i => i.Rarity == null), (i => i.Rarity!), (i => i.Name)], descending),
            _ => armors,
        };
    }
}