using Api.Middlewares.ExceptionHandling;
using Api.Models.DTOs.RequestDtos.Inventory;
using Api.Models.Items;
using Api.Repositories.Interfaces;
using Api.Services.Constants;
using static Api.Services.Util.SortUtil;
using static Api.Services.Util.ConstantsUtil;
using Api.Models.Items.Constants;
using Api.Services.Interfaces.Items;
using Api.Services.Interfaces;

namespace Api.Services.Implemented.Items;

public class ArmorService(IRepository<Armor> repo, ICurrentUserService currentUserService, ILogger<ArmorService> logger) : IArmorService
{
    public async Task<Armor> CreateAsync(CreateArmorRequestDto dto)
    {
        var dtoCategory = ResolveOptionOrThrow(dto.Category, ArmorCategory.AllowedValues, "Armor Category");
        var dtoRarity = dto.Rarity != null ? ResolveOptionOrThrow(dto.Rarity, ItemRarity.AllowedValues, "Item Rarity") : null;

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
        var dtoCategory = dto.Category is not null ? ResolveOptionOrThrow(dto.Category, ArmorCategory.AllowedValues, "Armor Category") : null;
        var dtoRarity = dto.Rarity is not null ? ResolveOptionOrThrow(dto.Rarity, ItemRarity.AllowedValues, "Item Rarity") : null;

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
        
        armor.IsPublic = dto.IsPublic ?? armor.IsPublic;
        armor.CloningAllowed = dto.CloningAllowed ?? armor.CloningAllowed;
        armor.UpdatedAt = DateTime.UtcNow;

        await repo.UpdateAsync(armor);
        logger.LogInformation("Successfully updated armor, Name: {ArmorName}, ID: {ArmorId}", armor.Name, armor.Id);
        return armor;
    }

    // TODO replace with database level sorting
    public ICollection<Armor> SortBy(ICollection<Armor> armors, string sortFilter, bool descending = false)
    {
        if(!TryResolveOption(sortFilter, SortArmorOption.AllowedValues, out string? resolved))
            return armors;

        return resolved switch
        {
            SortArmorOption.Name => OrderByMany(armors, [(i => i.Name)], descending),
            SortArmorOption.Category => OrderByMany(armors, [(i => i.ArmorCategory), (i => i.Name)], descending),
            SortArmorOption.AC => OrderByMany(armors, [(i => i.BaseArmorClass), (i => i.Name)], descending),
            SortArmorOption.Value => OrderByMany(armors, [(i => i.Value!), (i => i.Name)], descending),
            SortArmorOption.Weight => OrderByMany(armors, [(i => i.Weight!), (i => i.Name)], descending),
            SortArmorOption.Rarity => OrderByMany(armors, [(i => i.Rarity == null), (i => i.Rarity!), (i => i.Name)], descending),
            _ => throw new ValidationException($"Invalid sort option: {sortFilter}")
        };
    }
}