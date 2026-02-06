using Api.Middlewares.ExceptionHandling;
using Api.Models.Items.Constants;
using Microsoft.AspNetCore.Http.HttpResults;
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

    internal static object ResolveOptionOrThrow(string? value, object weaponCategories, string v)
    {
        throw new NotImplementedException();
    }

    internal static string ConvertWeaponTypeToMainSlot (string weaponType)
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
}