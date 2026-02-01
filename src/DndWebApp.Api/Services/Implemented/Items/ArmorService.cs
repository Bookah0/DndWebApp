using DndWebApp.Api.Middlewares.ExceptionHandling;
using DndWebApp.Api.Models.DTOs.Inventory;
using DndWebApp.Api.Models.Items;
using DndWebApp.Api.Repositories.Interfaces;
using DndWebApp.Api.Services.Constants;
using static DndWebApp.Api.Services.Util.SortUtil;
using static DndWebApp.Api.Services.Util.ConstantsUtil;
using DndWebApp.Api.Models.Items.Constants;
using DndWebApp.Api.Services.Interfaces.Items;

namespace DndWebApp.Api.Services.Implemented.Items;

public class ArmorService(IRepository<Armor> repo, ILogger<ArmorService> logger) : IArmorService
{
    public async Task<Armor> CreateAsync(ArmorDto dto)
    {
        var dtoCategory = ResolveOptionOrThrow(dto.Category, ArmorCategory.AllowedValues, "Armor Category");
        var dtoRarity = dto.Rarity != null ? ResolveOptionOrThrow(dto.Rarity, ItemRarity.AllowedValues, "Item Rarity") : null;

        Armor armor = await repo.CreateAsync(new()
        {
            Name = dto.Name,
            Description = dto.Description,
            Weight = dto.Weight,
            Value = dto.Value,
            ArmorCategory = dtoCategory,
            BaseArmorClass = dto.BaseArmorClass,
            PlusDexMod = dto.PlusDexMod,
            StealthDisadvantage = dto.StealthDisadvantage ?? false,
            ModCap = dto.ModCap ?? 0,
            StrengthScoreRequired = dto.StrengthScoreRequired ?? 0,
            Rarity = dtoRarity,
            RequiresAttunement = dto.RequiresAttunement ?? false,
            IsHomebrew = dto.IsHomebrew ?? false,
            Categories = [ItemCategory.Armor]
        });

        return armor;
    }

    public async Task DeleteAsync(int id)
    {
        var armor = await repo.GetByIdAsync(id) ?? throw new NotFoundException($"Armor with id {id} could not be found");
        await repo.DeleteAsync(armor);
    }

    public async Task<ICollection<Armor>> GetAllAsync()
    {
        var armors = await repo.GetAllAsync();
        return armors;
    }

    public async Task<Armor> GetByIdAsync(int id)
    {
        var armor = await repo.GetByIdAsync(id) ?? throw new NotFoundException($"Armor with id {id} could not be found");
        return armor;
    }

    public async Task UpdateAsync(ArmorDto dto, int id)
    {
        var dtoCategory = ResolveOptionOrThrow(dto.Category, ArmorCategory.AllowedValues, "Armor Category");
        var dtoRarity = dto.Rarity != null ? ResolveOptionOrThrow(dto.Rarity, ItemRarity.AllowedValues, "Item Rarity") : null;

        var armor = await repo.GetByIdAsync(id) ?? throw new NotFoundException($"Armor with id {id} could not be found");

        armor.Name = dto.Name;
        armor.Description = dto.Description;
        armor.Weight = dto.Weight;
        armor.Value = dto.Value;
        armor.ArmorCategory = dtoCategory;
        armor.BaseArmorClass = dto.BaseArmorClass;
        armor.PlusDexMod = dto.PlusDexMod;
        armor.StealthDisadvantage = dto.StealthDisadvantage ?? armor.StealthDisadvantage;
        armor.ModCap = dto.ModCap ?? armor.ModCap;
        armor.StrengthScoreRequired = dto.StrengthScoreRequired ?? armor.StrengthScoreRequired;
        armor.Rarity = dtoRarity ?? armor.Rarity;
        armor.RequiresAttunement = dto.RequiresAttunement ?? armor.RequiresAttunement;
        armor.IsHomebrew = dto.IsHomebrew ?? armor.IsHomebrew;

        await repo.UpdateAsync(armor);
    }

    public ICollection<Armor> SortBy(ICollection<Armor> armors, string sortFilter, bool descending = false)
    {
        if(!TryResolveOption(sortFilter, SortArmorOption.AllowedValues, out string? resolved))
            return armors;

        return resolved switch
        {
            SortArmorOption.Name => OrderByMany(armors, [(i => i.Name)], descending),
            SortArmorOption.Category => OrderByMany(armors, [(i => i.ArmorCategory), (i => i.Name)], descending),
            SortArmorOption.AC => OrderByMany(armors, [(i => i.BaseArmorClass), (i => i.Name)], descending),
            SortArmorOption.Value => OrderByMany(armors, [(i => i.Value), (i => i.Name)], descending),
            SortArmorOption.Weight => OrderByMany(armors, [(i => i.Weight), (i => i.Name)], descending),
            SortArmorOption.Rarity => OrderByMany(armors, [(i => i.Rarity == null), (i => i.Rarity!), (i => i.Name)], descending),
            _ => throw new ArgumentOutOfRangeException(nameof(sortFilter), "Invalid sort option provided.")
        };
    }
}