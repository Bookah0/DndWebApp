namespace DndWebApp.Api.Services.External.Implemented;

using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using DndWebApp.Api.Models.DTOs.ExternalDTOs;
using DndWebApp.Api.Models.Items;
using DndWebApp.Api.Repositories.Interfaces;
using DndWebApp.Api.Services.External.Interfaces;
using DndWebApp.Api.Services.Util;
using static DndWebApp.Api.Services.Util.NormalizationUtil;
using static DndWebApp.Api.Services.Util.ConstantsUtil;
using DndWebApp.Api.Models.Items.Constants;
using Microsoft.AspNetCore.Http.HttpResults;
using DndWebApp.Api.Middlewares.ExceptionHandling;

public class ExternalItemService : IExternalItemService
{
    private readonly IItemRepository repo;
    private readonly HttpClient client = new();

    public ExternalItemService(IItemRepository repo)
    {
        this.repo = repo;
    }

    public async Task FetchExternalBasicItemsAsync(CancellationToken cancellationToken = default)
    {
        if ((await repo.GetAllAsync()).Count > 0)
        {
            throw new InvalidOperationException("Items already exist in the database. Skipping fetch.");
        }

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
                    await repo.CreateAsync(ToVehicle(jsonDoc, item, category));
                    break;
                default:
                    await repo.CreateAsync(ToItem(jsonDoc, item));
                    break;
            }
        }
    }

    private Armor ToArmor(JsonDocument jsonDoc, EIndexDto item)
    {
        var eArmor = jsonDoc.RootElement.Deserialize<EArmorDto>()
            ?? throw new InvalidOperationException($"Failed to deserialize armor: {item.Index}");
        var itemCategory = ResolveOptionOrThrow(eArmor.EquipmentCategory.Name, ItemCategory.AllowedValues, "Item Category");

        return new Armor
        {
            Name = eArmor.Name,
            Description = eArmor.Description == null ? "" : string.Join(" ", eArmor.Description),
            Categories = [itemCategory],
            Rarity = ItemRarity.Common,
            Weight = eArmor.Weight,
            Value = GetConvertedValue(eArmor.Cost.Quantity, eArmor.Cost.Unit),
            Quantity = eArmor.Cost.Quantity,
            ArmorCategory = ResolveOptionOrThrow(eArmor.ArmorCategory, ArmorCategory.AllowedValues, "Armor Category"),
            BaseArmorClass = eArmor.ArmorClass.BaseArmorClass,
            PlusDexMod = eArmor.ArmorClass.DexBonus,
            ModCap = eArmor.ArmorClass.MaxBonus,
            StrengthScoreRequired = eArmor.StrengthMinimum,
            StealthDisadvantage = eArmor.StealthDisadvantage,
        };
    }

    private Weapon ToWeapon(JsonDocument jsonDoc, EIndexDto item)
    {
        var eWeapon = jsonDoc.RootElement.Deserialize<EWeaponDto>()
            ?? throw new InvalidOperationException($"Failed to deserialize weapon: {item.Name}");
        
        var eDamagetype = eWeapon.Damage?.DamageType.Name 
            ?? throw new InvalidOperationException($"Weapon {item.Name} missing damage object.");
        var damageType = ResolveOptionOrThrow(eDamagetype, DamageType.AllowedValues, "Damage Type");

        var propertyNames = eWeapon.Properties?.Select(p => p.Name).ToList() ?? [];
        var properties = ResolveOptionOrThrow(propertyNames, WeaponProperty.AllowedValues, "Weapon Property");
        
        var category = ResolveOptionOrThrow(eWeapon.CategoryRange, WeaponCategory.AllowedValues, "Weapon Category");
        var itemCategory = ResolveOptionOrThrow(eWeapon.EquipmentCategory.Name, ItemCategory.AllowedValues, "Item Category");

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
            WeaponType = ParseWeaponType(eWeapon),
            Properties = properties,
            DamageTypes = [damageType],
            DamageDice = eWeapon.Damage?.DamageDice ?? "",
            Range = eWeapon.Range?.Normal ?? 0,
            LongRange = eWeapon.Range?.Long ?? null,
        };
    }

    private Tool ToTool(JsonDocument jsonDoc, EIndexDto item)
    {
        var eTool = jsonDoc.RootElement.Deserialize<EToolDto>()
            ?? throw new InvalidOperationException($"Failed to deserialize tool: {item.Index}");

        var category = ResolveOptionOrThrow(eTool.ToolCategory, ToolCategory.AllowedValues, "Tool Category");
        var itemCategory = ResolveOptionOrThrow(eTool.EquipmentCategory.Name, ItemCategory.AllowedValues, "Item Category");

        return new Tool
        {
            Name = eTool.Name,
            Description = eTool.Description == null ? "" : string.Join(" ", eTool.Description),
            Categories = [itemCategory],
            Rarity = ItemRarity.Common,
            Weight = eTool.Weight,
            Value = GetConvertedValue(eTool.Cost.Quantity, eTool.Cost.Unit),
            Quantity = eTool.Cost.Quantity,
            ToolType = category,
            Properties = []
        };
    }

    private Vehicle ToVehicle(JsonDocument jsonDoc, EIndexDto item, string category)
    {
        var eVehicle = jsonDoc.RootElement.Deserialize<EVehicleDto>()
            ?? throw new InvalidOperationException($"Failed to deserialize vehicle: {item.Index}");

        int? capacityValue = int.TryParse(eVehicle.Capacity?.Split(' ')[0], out var cap) ? cap : null;
        string? capacityUnit = eVehicle.Capacity?.Split(' ')[1] ?? null;
        
        var itemCategory = ResolveOptionOrThrow(eVehicle.EquipmentCategory.Name, ItemCategory.AllowedValues, "Item Category");

        return new Vehicle
        {
            Name = eVehicle.Name,
            Description = eVehicle.Description == null ? "" : string.Join(" ", eVehicle.Description),
            Categories = [itemCategory],
            Rarity = ItemRarity.Common,
            Weight = eVehicle.Weight,
            Value = GetConvertedValue(eVehicle.Cost.Quantity, eVehicle.Cost.Unit),
            Quantity = eVehicle.Cost.Quantity,
            Speed = eVehicle.Speed?.Quantity,
            SpeedUnit = eVehicle.Speed?.Unit,
            Capacity = capacityValue,
            CapacityUnit = capacityUnit,
            Landborne = eVehicle.EquipmentCategory.Index == "mounts-and-vehicles",
            Waterborne = eVehicle.EquipmentCategory.Index == "waterborne-vehicles",
        };
    }

    private Item ToItem(JsonDocument jsonDoc, EIndexDto item)
    {
        var eItem = jsonDoc.RootElement.Deserialize<EItemDto>()
            ?? throw new InvalidOperationException($"Failed to deserialize item: {item.Index}");

        var itemCategory = ResolveOptionOrThrow(eItem.EquipmentCategory.Name, ItemCategory.AllowedValues, "Item Category");

        return new Item
        {
            Name = eItem.Name,
            Description = eItem.Description == null ? "" : string.Join(" ", eItem.Description),
            Categories = [itemCategory],
            Rarity = ItemRarity.Common,
            Weight = eItem.Weight,
            Value = GetConvertedValue(eItem.Cost.Quantity, eItem.Cost.Unit),
            Quantity = eItem.Cost.Quantity
        };
    }

    public Task FetchExternalMagicalItemsAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    private string ParseWeaponType(EWeaponDto eWeapon)
    {
        if (TryResolveOption(eWeapon.Name, WeaponType.AllowedValues, out var weaponType))
            return weaponType!;

        foreach (var allowed in WeaponType.AllowedValues)
        {
            if (eWeapon.Name.Contains(allowed, StringComparison.CurrentCultureIgnoreCase))
            {
                return allowed;
            }
        }

        throw new NotFoundException($"Unknown weapon type: {eWeapon.Name}");
    }

    private int GetConvertedValue(int value, string unit)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(value);
        ValidationUtil.HasContentOrThrow(unit);

        return unit switch
        {
            "cp" => value,
            "sp" => value * 10,
            "ep" => value * 50,
            "gp" => value * 100,
            "pp" => value * 1000,
            _ => throw new ArgumentOutOfRangeException($"Unknown currency unit: {unit}"),
        };
    }
}