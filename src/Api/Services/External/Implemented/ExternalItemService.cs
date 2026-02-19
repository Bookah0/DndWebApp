namespace Api.Services.External.Implemented;

using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Api.Models.DTOs.ExternalDTOs;
using Api.Models.Items;
using Api.Repositories.Interfaces;
using Api.Services.External.Interfaces;
using Api.Services.Util;
using Api.Middlewares.ExceptionHandling;

using static Api.Validation.AllowedValues.ValuesValidator;
using Api.Validation.AllowedValues.Items;
using Api.Validation.AllowedValues;

public class ExternalItemService(IItemRepository repo, ILogger<ExternalItemService> logger) : IExternalItemService
{
    private readonly HttpClient client = new();

    public async Task FetchExternalBasicItemsAsync(CancellationToken cancellationToken = default)
    {
        var existingCount = (await repo.GetAllAsync()).Count;
        if (existingCount > 0)
        {
            logger.LogInformation("Items already exist in the database. Skipping fetch. ExistingCount: {ExistingCount}", existingCount);
            return;
        }

        logger.LogInformation("Fetching external basic items.");

        var getListResponse = await client.GetAsync("https://www.dnd5eapi.co/api/2014/equipment/", cancellationToken);
        var result = await JsonSerializer.DeserializeAsync<EIndexListDto>(getListResponse.Content.ReadAsStream(cancellationToken), cancellationToken: cancellationToken);

        if (result is null || result.Results.Count == 0)
        {
            throw new InvalidOperationException("No items found in external API.");
        }

        foreach (var item in result.Results)
        {
            var getResponse = await client.GetAsync($"https://www.dnd5eapi.co/api/2014/equipment/{item.Index}", cancellationToken);
            var stream = await getResponse.Content.ReadAsStreamAsync(cancellationToken);
            var jsonDoc = await JsonDocument.ParseAsync(stream, cancellationToken: cancellationToken);
            var category = jsonDoc.RootElement.GetProperty("equipment_category").GetProperty("index").GetString();

            switch (category)
            {
                case "armor":
                    await repo.CreateAsync(ToArmor(jsonDoc, item));
                    break;
                case "weapon":
                    await repo.CreateAsync(ToWeapon(jsonDoc, item));
                    break;
                case "tool":

                    await repo.CreateAsync(ToTool(jsonDoc, item));
                    break;
                case "waterborne-vehicles":
                case "mounts-and-vehicles":
                    await repo.CreateAsync(ToVehicle(jsonDoc, item));
                    break;
                default:
                    await repo.CreateAsync(ToItem(jsonDoc, item));
                    break;
            }
        }

        logger.LogInformation("Successfully fetched external basic items. Count: {ItemCount}", result.Results.Count);
    }

    private Armor ToArmor(JsonDocument jsonDoc, EIndexDto item)
    {
        var eArmor = jsonDoc.RootElement.Deserialize<ECreateArmorRequestDto>()
            ?? throw new InvalidOperationException($"Failed to deserialize armor: {item.Index}");

        if(eArmor.EquipmentCategory is null)
            logger.LogWarning("Item {ItemName} has null equipment category. Defaulting to Miscellaneous.", eArmor.Name);

        var itemCategory = eArmor.EquipmentCategory is null 
            ? ItemCategory.Miscellaneous 
            : NormalizeValueOrEmpty<ItemCategory>(eArmor.EquipmentCategory.Name);

        return new Armor
        {
            Name = eArmor.Name,
            Description = eArmor.Description == null ? "" : string.Join(" ", eArmor.Description),
            Categories = [itemCategory],
            Rarity = ItemRarity.Common,
            Weight = eArmor.Weight,
            Value = GetConvertedValue(eArmor.Cost.Quantity, eArmor.Cost.Unit),
            Quantity = eArmor.Cost.Quantity,
            ArmorCategory = NormalizeValueOrThrow<ArmorCategory>(eArmor.ArmorCategory),
            BaseArmorClass = eArmor.ArmorClass.BaseArmorClass,
            PlusDexMod = eArmor.ArmorClass.DexBonus,
            ModCap = eArmor.ArmorClass.MaxBonus,
            StrengthScoreRequired = eArmor.StrengthMinimum,
            StealthDisadvantage = eArmor.StealthDisadvantage,

            CreatedAt = DateTime.UtcNow,
            CreatedBy = null,
            IsHomebrew = false,
            IsPublic = true,
            CloningAllowed = true
        };
    }

    private Weapon ToWeapon(JsonDocument jsonDoc, EIndexDto item)
    {
        var eWeapon = jsonDoc.RootElement.Deserialize<ECreateWeaponRequestDto>()
            ?? throw new InvalidOperationException($"Failed to deserialize weapon: {item.Name}");

        var eDamagetype = eWeapon.Damage?.DamageType.Name
            ?? throw new InvalidOperationException($"Weapon {item.Name} missing damage object.");
        var damageType = NormalizeValueOrThrow<DamageType>(eDamagetype);

        var propertyNames = eWeapon.Properties?.Select(p => p.Name).ToList() ?? [];
        var properties = NormalizeValueOrThrow<WeaponProperty>(propertyNames);

        var category = NormalizeValueOrThrow<WeaponCategory>(eWeapon.CategoryRange);

        if(eWeapon.EquipmentCategory is null)
            logger.LogWarning("Item {ItemName} has null equipment category. Defaulting to Miscellaneous.", eWeapon.Name);

        var itemCategory = eWeapon.EquipmentCategory is null 
            ? ItemCategory.Miscellaneous 
            : NormalizeValueOrEmpty<ItemCategory>(eWeapon.EquipmentCategory.Name);
       
        var weaponType = ParseWeaponType(eWeapon);
        return new Weapon
        {
            Name = eWeapon.Name,
            Description = eWeapon.Description == null ? "" : string.Join(" ", eWeapon.Description),
            Categories = [itemCategory],
            Rarity = ItemRarity.Common,
            Weight = eWeapon.Weight,
            Value = GetConvertedValue(eWeapon.Cost.Quantity, eWeapon.Cost.Unit),
            Quantity = eWeapon.Cost.Quantity,
            WeaponCategory = category,
            WeaponType = weaponType,
            EquipSlot = GetDefaultWeaponMainSlot(weaponType),
            Properties = properties ?? [],
            DamageTypes = [damageType],
            DamageDice = eWeapon.Damage?.DamageDice ?? "",
            Range = eWeapon.Range?.Normal ?? 0,
            LongRange = eWeapon.Range?.Long ?? null,

            CreatedAt = DateTime.UtcNow,
            CreatedBy = null,
            IsHomebrew = false,
            IsPublic = true,
            CloningAllowed = true
        };
    }

    private Tool ToTool(JsonDocument jsonDoc, EIndexDto item)
    {
        var eTool = jsonDoc.RootElement.Deserialize<ECreateToolRequestDto>()
            ?? throw new InvalidOperationException($"Failed to deserialize tool: {item.Index}");

        var category = NormalizeValueOrThrow<ToolCategory>(eTool.ToolCategory);
        if(eTool.EquipmentCategory is null)
            logger.LogWarning("Item {ItemName} has null equipment category. Defaulting to Miscellaneous.", eTool.Name);

        var itemCategory = eTool.EquipmentCategory is null 
            ? ItemCategory.Miscellaneous 
            : NormalizeValueOrEmpty<ItemCategory>(eTool.EquipmentCategory.Name);


        return new Tool
        {
            Name = eTool.Name,
            Description = eTool.Description == null ? "" : string.Join(" ", eTool.Description),
            Categories = [itemCategory],
            Rarity = ItemRarity.Common,
            Weight = eTool.Weight,
            Value = GetConvertedValue(eTool.Cost.Quantity, eTool.Cost.Unit),
            Quantity = eTool.Cost.Quantity,
            ToolCategory = category,
            ToolProperties = [],

            CreatedAt = DateTime.UtcNow,
            CreatedBy = null,
            IsHomebrew = false,
            IsPublic = true,
            CloningAllowed = true
        };
    }

    private Vehicle ToVehicle(JsonDocument jsonDoc, EIndexDto item)
    {
        var eVehicle = jsonDoc.RootElement.Deserialize<EVehicleDto>()
            ?? throw new InvalidOperationException($"Failed to deserialize vehicle: {item.Index}");

        int? capacityValue = int.TryParse(eVehicle.Capacity?.Split(' ')[0], out var cap) ? cap : null;
        string? capacityUnit = eVehicle.Capacity?.Split(' ')[1] ?? null; ;
        return new Vehicle
        {
            Name = eVehicle.Name,
            Description = eVehicle.Description == null ? "" : string.Join(" ", eVehicle.Description),
            Categories = [ItemCategory.Mount, ItemCategory.Vehicle],
            Rarity = ItemRarity.Common,
            Weight = eVehicle.Weight,
            Value = GetConvertedValue(eVehicle.Cost.Quantity, eVehicle.Cost.Unit),
            Quantity = eVehicle.Cost.Quantity,
            Speed = eVehicle.Speed?.Quantity,
            SpeedUnit = eVehicle.Speed?.Unit,
            Capacity = capacityValue,
            CapacityUnit = capacityUnit,
            Landborne = eVehicle.EquipmentCategory?.Index == "mounts-and-vehicles",
            Waterborne = eVehicle.EquipmentCategory?.Index == "waterborne-vehicles",

            CreatedAt = DateTime.UtcNow,
            CreatedBy = null,
            IsHomebrew = false,
            IsPublic = true,
            CloningAllowed = true
        };
    }

    private Item ToItem(JsonDocument jsonDoc, EIndexDto item)
    {
        var eItem = jsonDoc.RootElement.Deserialize<ECreateItemRequestDto>()
            ?? throw new InvalidOperationException($"Failed to deserialize item: {item.Index}");
        if(eItem.EquipmentCategory is null)
            logger.LogWarning("Item {ItemName} has null equipment category. Defaulting to Miscellaneous.", eItem.Name);

        var itemCategory = eItem.EquipmentCategory is null 
            ? ItemCategory.Miscellaneous 
            : NormalizeValueOrEmpty<ItemCategory>(eItem.EquipmentCategory.Name);

        return new Item
        {
            Name = eItem.Name,
            Description = eItem.Description == null ? "" : string.Join(" ", eItem.Description),
            Categories = [itemCategory],
            Rarity = ItemRarity.Common,
            Weight = eItem.Weight,
            Value = GetConvertedValue(eItem.Cost.Quantity, eItem.Cost.Unit),
            Quantity = eItem.Cost.Quantity,

            CreatedAt = DateTime.UtcNow,
            CreatedBy = null,
            IsHomebrew = false,
            IsPublic = true,
            CloningAllowed = true
        };
    }

    public Task FetchExternalMagicalItemsAsync(CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Fetching external magical items.");
        throw new NotImplementedException();
    }

    private static string ParseWeaponType(ECreateWeaponRequestDto eWeapon)
    {
        if (TryNormalizeValue<WeaponType>(eWeapon.Name, out var weaponType))
            return weaponType!;

        foreach (var allowed in WeaponType.AllowedValues)
        {
            if (eWeapon.Name.Contains(allowed, StringComparison.CurrentCultureIgnoreCase))
            {
                return allowed;
            }
        }

        throw new ValidationException($"Unknown weapon type: {eWeapon.Name}");
    }

    private int GetConvertedValue(int value, string unit)
    {
        if (value <= 0)
            return 0;

        var normalizedUnit = NormalizeValueOrThrow<CurrencyUnit>(unit);

        return normalizedUnit switch
        {
            CurrencyUnit.Copper => value,
            CurrencyUnit.Silver => value * 10,
            CurrencyUnit.Electrum => value * 50,
            CurrencyUnit.Gold => value * 100,
            CurrencyUnit.Platinum => value * 1000,
            _ => throw new ValidationException($"Unknown currency unit: {unit}"),
        };
    }
}