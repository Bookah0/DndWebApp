using Api.Middlewares.ExceptionHandling;
using Api.Models.Items.Constants;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.VisualBasic;

namespace Api.Services.Util;

public static class ConstantsUtil
{
    public static ICollection<string> ResolveOptionOrThrow(ICollection<string> inputs, IReadOnlySet<string> allowedSet, string constantsGroupName = "Constant")
    {
        ICollection<string> resolved = [];

        foreach (var input in inputs)
        {
            if (TryResolveOption(input, allowedSet, out var resolvedOption))
            {
                resolved.Add(resolvedOption!);
                continue;
            }
            throw new ValidationException($"{constantsGroupName} {input} not recognized.");
        }
        return resolved;
    }

    public static string ResolveOptionOrThrow(string input, IReadOnlySet<string> allowedSet, string constantsGroupName = "Constant")
    {
        if (TryResolveOption(input, allowedSet, out var resolved))
            return resolved!;
        
        throw new ValidationException($"{constantsGroupName} {input} not recognized.");
    }

    public static bool TryResolveOption(string input, IReadOnlySet<string> allowedSet, out string? resolved)
    {
        if (allowedSet.Contains(input))
        {
            resolved = input;
            return true;
        }

        var normalizedInput = Normalize(input);

        foreach (var allowed in allowedSet)
        {
            if (Normalize(allowed).Equals(normalizedInput))
            {
                resolved = allowed;
                return true;
            }
        }
        resolved = null;
        return false;
    }

    public static string Normalize(string str) {
        return str
            .Replace("-", "")
            .Replace("_", "")
            .Replace("'", "")
            .Replace(" ", "")
            .ToLower();
    }

    public static object ResolveOptionOrThrow(string? value, object weaponCategories, string v)
    {
        throw new NotImplementedException();
    }

    public static string GetDefaultWeaponMainSlot (string weaponType)
    {
        return weaponType switch 
        {
            // Simple Melee Weapons
            WeaponType.Club => EquipSlot.MainHand,
            WeaponType.Dagger => EquipSlot.MainHand,
            WeaponType.Greatclub => EquipSlot.TwoHand,
            WeaponType.Handaxe => EquipSlot.MainHand,
            WeaponType.Javelin => EquipSlot.MainHand,
            WeaponType.LightHammer => EquipSlot.MainHand,
            WeaponType.Mace => EquipSlot.MainHand,
            WeaponType.Quarterstaff => EquipSlot.TwoHand,
            WeaponType.Sickle => EquipSlot.MainHand,
            WeaponType.Spear => EquipSlot.TwoHand,
            
            // Simple Ranged Weapons
            WeaponType.LightCrossbow => EquipSlot.Ranged,
            WeaponType.Dart => EquipSlot.Ranged,
            WeaponType.Shortbow => EquipSlot.Ranged,
            WeaponType.Sling => EquipSlot.Ranged,
            
            // Martial Melee Weapons
            WeaponType.Battleaxe => EquipSlot.TwoHand,
            WeaponType.Flail => EquipSlot.MainHand,
            WeaponType.Glaive => EquipSlot.TwoHand,
            WeaponType.Greataxe => EquipSlot.TwoHand,
            WeaponType.Greatsword => EquipSlot.TwoHand,
            WeaponType.Halberd => EquipSlot.TwoHand,
            WeaponType.Lance => EquipSlot.TwoHand,
            WeaponType.Longsword => EquipSlot.TwoHand,
            WeaponType.Maul => EquipSlot.TwoHand,
            WeaponType.Morningstar => EquipSlot.MainHand,
            WeaponType.Pike => EquipSlot.TwoHand,
            WeaponType.Rapier => EquipSlot.MainHand,
            WeaponType.Scimitar => EquipSlot.MainHand,
            WeaponType.Shortsword => EquipSlot.MainHand,
            WeaponType.Trident => EquipSlot.TwoHand,
            WeaponType.WarPick => EquipSlot.MainHand,
            WeaponType.Warhammer => EquipSlot.TwoHand,
            WeaponType.Whip => EquipSlot.MainHand,
            
            // Martial Ranged Weapons
            WeaponType.Blowgun => EquipSlot.Ranged,
            WeaponType.HandCrossbow => EquipSlot.Ranged,
            WeaponType.HeavyCrossbow => EquipSlot.Ranged,
            WeaponType.Longbow => EquipSlot.Ranged,
            WeaponType.Net => EquipSlot.Ranged,
            
            _ => throw new ValidationException($"Unknown weapon type: {weaponType}")
        };
    }

    public static ICollection<string> GetDefaultWeaponDamageTypes(string weaponType)
    {
        return weaponType switch
        {
            // Simple Melee Weapons
            WeaponType.Club => [DamageType.Bludgeoning],
            WeaponType.Dagger => [DamageType.Piercing],
            WeaponType.Greatclub => [DamageType.Bludgeoning],
            WeaponType.Handaxe => [DamageType.Slashing],
            WeaponType.Javelin => [DamageType.Piercing],
            WeaponType.LightHammer => [DamageType.Bludgeoning],
            WeaponType.Mace => [DamageType.Bludgeoning],
            WeaponType.Quarterstaff => [DamageType.Bludgeoning],
            WeaponType.Sickle => [DamageType.Slashing],
            WeaponType.Spear => [DamageType.Piercing],
            
            // Simple Ranged Weapons
            WeaponType.LightCrossbow => [DamageType.Piercing],
            WeaponType.Dart => [DamageType.Piercing],
            WeaponType.Shortbow => [DamageType.Piercing],
            WeaponType.Sling => [DamageType.Bludgeoning],
            
            // Martial Melee Weapons
            WeaponType.Battleaxe => [DamageType.Slashing],
            WeaponType.Flail => [DamageType.Bludgeoning],
            WeaponType.Glaive => [DamageType.Slashing],
            WeaponType.Greataxe => [DamageType.Slashing],
            WeaponType.Greatsword => [DamageType.Slashing, DamageType.Piercing],
            WeaponType.Halberd => [DamageType.Slashing],
            WeaponType.Lance => [DamageType.Piercing],
            WeaponType.Longsword => [DamageType.Slashing, DamageType.Piercing],
            WeaponType.Maul => [DamageType.Bludgeoning],
            WeaponType.Morningstar => [DamageType.Bludgeoning],
            WeaponType.Pike => [DamageType.Piercing],
            WeaponType.Rapier => [DamageType.Piercing],
            WeaponType.Scimitar => [DamageType.Slashing],
            WeaponType.Shortsword => [DamageType.Slashing, DamageType.Piercing],
            WeaponType.Trident => [DamageType.Piercing],
            WeaponType.WarPick => [DamageType.Piercing],
            WeaponType.Warhammer => [DamageType.Bludgeoning],
            WeaponType.Whip => [DamageType.Slashing],
            
            // Martial Ranged Weapons
            WeaponType.Blowgun => [DamageType.Piercing],
            WeaponType.HandCrossbow => [DamageType.Piercing],
            WeaponType.HeavyCrossbow => [DamageType.Piercing],
            WeaponType.Longbow => [DamageType.Piercing],
            WeaponType.Net => [DamageType.Piercing],
            
            _ => throw new ValidationException($"Unknown weapon type: {weaponType}")
        };
    }
}