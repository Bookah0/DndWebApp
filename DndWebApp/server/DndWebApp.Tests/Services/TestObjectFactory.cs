using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs;
using DndWebApp.Api.Models.Spells;
using DndWebApp.Api.Models.Spells.Enums;
using DndWebApp.Api.Models.World;

namespace DndWebApp.Tests.Services;

public static class TestObjectFactory
{
    internal static SpellDto CreateTestSpellDto(string name, bool isHomebrew = false, int id = 1)
    {
        return new SpellDto
        {
            Id = id,
            Name = name,
            Description = "A powerful spell",
            IsHomebrew = isHomebrew,
            ClassIds = [1],
            TargetingDto = new() { ShapeLength = "0", ShapeType = "0", ShapeWidth = "0", TargetType = "Creature", Range = "Feet", RangeValue = 10 },
            Level = 3,
            EffectsAtHigherLevels = "Extra effect",
            ReactionCondition = "",
            Duration = "Minute",
            CastingTime = "Action",
            MagicSchool = "Evocation",
            Types = ["Buff"],
            DamageRoll = "2d6",
            DamageTypes = ["Fire"],
            CastRequirementsDto = new() { Verbal = true, Somatic = true, Materials = "Bat guano", MaterialCost = 5, MaterialsConsumed = false }
        };
    }

    internal static Spell CreateTestSpell(string name, int level, SpellDuration spellDuration, int id = 1)
    {
        return new Spell
        {
            Id = id,
            Name = name,
            Description = "A powerful spell",
            Level = level,
            Duration = spellDuration,
            CastingTime = 0,
            MagicSchool = 0,
            SpellTargeting = new() { Range = SpellRange.Feet, TargetType = SpellTargetType.Creature }
        };
    }

    internal static SkillDto CreateTestSkillDto(string name, int abilityId, int id = 1, bool isHomebrew = false)
    {
        return new() { Id = id, Name = name, AbilityId = abilityId, IsHomebrew = isHomebrew };
    }

    internal static Skill CreateTestSkill(string name, int abilityId, Ability ability = null!, int id = 1)
    {
        return new() { Id = id, Name = name, AbilityId = abilityId, Ability = ability };
    }

    internal static LanguageDto CreateTestLanguageDto(string name, string family, string script, int id = 1)
    {
        return new() { Id = id, Name = name, Family = family, Script = script, IsHomebrew = false };
    }

    internal static Language CreateTestLanguage(string name, string family, string script, int id = 1)
    {
        return new() { Id = id, Name = name, Family = family, Script = script, IsHomebrew = false };
    }

    internal static AlignmentDto CreateTestAlignmentDto(string name, string abbreviation, string description, int id)
    {
        return new() { Id = id, Name = name, Description = description, Abbreviation = abbreviation };
    }

    internal static Alignment CreateTestAlignment(string name, string abbreviation, string description, int id)
    {
        return new() { Id = id, Name = name, Description = description, Abbreviation = abbreviation };
    }

    internal static AbilityDto CreateTestAbilityDto(string fullName, string shortName, string description, int id)
    {
        return new() { Id = id, FullName = fullName, ShortName = shortName, Description = description };
    }

    internal static Ability CreateTestAbility(string fullName, string shortName, string description, int id, ICollection<Skill> skills = null!)
    {
        return new() { Id = id, FullName = fullName, ShortName = shortName, Description = description, Skills = skills };
    }
}