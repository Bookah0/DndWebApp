using Api.Data;
using Api.Models.Characters;
using Api.Models.Features;
using Api.Models.Items;
using Api.Models.Items.Constants;
using Api.Models.Spells;
using Api.Models.Spells.Constants;
using Microsoft.EntityFrameworkCore;

namespace Tests.Repositories;

public static class TestObjectFactory
{
    internal static DbContextOptions<AppDbContext> GetInMemoryOptions(string dbName)
    {
        return new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
            .Options;
    }

    internal static Ability CreateTestAbility(string fullName, string shortName, string description = "", List<Skill>? skills = null)
    {
        return new() { FullName = fullName, ShortName = shortName, Description = description, Skills = skills ?? [] };
    }

    internal static BackgroundFeature CreateTestFeature(string name = "Shelter of the Faithful", string description = "As an acolyte...", Background? bg = null, int bgId = 1)
    {
        return new BackgroundFeature { Name = name, Description = description, Background = bg, BackgroundId = bgId };
    }

    internal static Item CreateTestItem(string name, string category, int quantity = 1, string description = "")
    {
        return new Item { Name = name, Description = description, Categories = [category], Quantity = quantity };
    }

    internal static Background CreateTestBackground(string name)
    {
        var description = $"{name} description";
        var background = new Background
        {
            Name = name,
            Description = description,
            StartingCurrency = new() { Gold = 15 }
        };

        background.StartingItems.Add(CreateTestItem("Holy Symbol", ItemCategory.Utility));
        background.StartingItems.Add(CreateTestItem("Incense Sticks", ItemCategory.None, 5));
        background.StartingItems.Add(CreateTestItem("Vestments", ItemCategory.None));
        background.StartingItems.Add(CreateTestItem("Common Clothes", ItemCategory.None));

        background.StartingItemsOptions.Add(new() { Description = "desc", ItemOptionIds = [1, 2] });
        return background;
    }

    internal static Character CreateTestCharacter()
    {
        var str = CreateTestAbility("Strength", "Str");
        var background = new Background { Name = "Outlander", Description = "You grew up in the wilds, far from civilization", StartingCurrency = new() };
        var cls = CreateTestClass();

        return new Character
        {
            Name = "Arannis",
            Level = 5,
            TimeCreated = DateTime.UtcNow,
            Race = new Race { Name = "Elf", Speed = 30 },
            Subrace = new Subrace { Name = "HighElf", ParentRace = null!, ParentRaceId = -1, Speed = 30 },
            Class = cls,
            ClassId = cls.Id,
            Background = background,
            BackgroundId = background.Id,
            Inventory = new Inventory { Currency = new(), Id = 10, EquippedItems = [] },
            InventoryId = 10,
            AbilityScores = [new AbilityValue() { Ability = str, AbilityId = str.Id, Value = 10 }],
            CombatStats = new CombatStats
            {
                ArmorClass = 14,
                Initiative = 3,
                Speed = 30,
                CurrentHitDice = 1,
                CurrentHP = 9,
                MaxHP = 10,
                MaxHitDice = 1
            },
            CurrentSpellSlots = [4, 2, 0, 0, 0, 0, 0, 0, 0],
            CharacterDescription = new CharacterDescription
            {
                Eyes = "Brown"
            },
            SkillProficiencies = [new SkillProficiency() { SkillId = 12, FeatureId = background.Id, HasExpertise = false }],
            Languages = [new() { LanguageId = 1, FeatureId = background.Id }],
            ToolProficiencies = [new() { ToolType = ToolCategory.HerbalismKit, FeatureId = background.Id }],
            WeaponCategoryProficiencies = [new() { WeaponCategory = WeaponCategory.MartialRanged, FeatureId = background.Id }]
        };
    }

    internal static int[] CreateTestSpellSlotsAtLevel() { return [1, 0, 0, 0, 0, 0, 0, 0, 0]; }
    internal static List<ClassSpecificSlot> CreateClassSpecificSlots() { return [new ClassSpecificSlot { Name = "Bardic inspiration", Quantity = 2 }]; }
    internal static ClassFeature CreateTestClassFeature(int classLevelId)
    {
        return new()
        {
            Name = "Spellcasting",
            Description = "Gain spellcasting abilities.",
            LevelId = classLevelId,
            ClassId = 1
        };
    }

    internal static ClassLevel CreateTestLevel(Class cls)
    {
        var classLvl = new ClassLevel()
        {
            Id = 10,
            Level = 2,
            Class = cls,
            ClassId = cls.Id,
            ProficiencyBonus = 3,
            ClassSpecificSlotsAtLevel = CreateClassSpecificSlots(),
            SpellSlots = CreateTestSpellSlotsAtLevel(),
            NewFeatures = [CreateTestClassFeature(10)]
        };
        return classLvl;
    }

    internal static Class CreateTestClass(string name = "Ranger")
    {
        var cls = new Class
        {
            Name = name,
            Description = "Description",
            HitDie = 8,
            ClassLevels = []
        };
        return cls;
    }

    internal static Feat CreateTestFeat()
    {
        return new()
        {
            Name = "Sharpshooter",
            Description = "Improve ranged attacks.",
            Prerequisite = "Dex 13"
        };
    }

    internal static Armor CreateTestArmor() => new()
    {
        Name = "Leather Armor",
        Description = "Light armor made from tanned leather, provides basic protection.",
        Categories = [ItemCategory.Armor],
        ArmorCategory = ArmorCategory.Light,
        BaseArmorClass = 11,
        PlusDexMod = true
    };

    internal static Weapon CreateTestWeapon() => new()
    {
        Name = "Shortbow",
        Description = "A small bow ideal for ranged attacks.",
        Categories = [ItemCategory.Weapon],
        WeaponCategory = WeaponCategory.SimpleRanged,
        WeaponType = WeaponType.Shortbow,
        Slot = EquipSlot.TwoHand,
        Properties = [WeaponProperty.TwoHanded],
        DamageTypes = [DamageType.Piercing],
        DamageDice = "1d6",
        Range = 80
    };

    internal static Inventory CreateTestInventory()
    {
        var inv = new Inventory
        {
            Currency = new(),
            StoredItems = [CreateTestArmor(), CreateTestWeapon(), CreateTestTool()]
        };

        return inv;
    }

    internal static Item CreateTestItem() => new()
    {
        Name = "Spoon",
        Description = "A simple metal spoon, useful for eating or mixing potions.",
        Categories = [ItemCategory.AdventuringGear]
    };

    internal static Tool CreateTestTool() => new()
    {
        Name = "Thieves' Kit",
        Description = "A set of lockpicks and other tools for stealthy operations.",
        Categories = [ItemCategory.Tools],
        ToolType = ToolCategory.ThievesTools,
        Activities = [],
        Properties = []
    };

    internal static Spell CreateTestSpell(string name) => new()
    {
        Name = name,
        Description = $"Description of {name}",
        Level = 1,
        Duration = SpellDuration.Instantaneous,
        CastingTime = CastingTime.Action,
        SpellTargeting = new() { TargetType = SpellTargetType.Creature, Range = SpellRange.Feet, RangeValue = 20 },
        MagicSchool = MagicSchool.Evocation,
    };

    internal static Race CreateTestRace(string name) => new() { Name = name, Speed = 30 };

    internal static Subrace CreateTestSubrace(string name, Race parentRace, int parentRaceId)
    {
        return new() { Name = name, Speed = 30, ParentRace = parentRace, ParentRaceId = parentRaceId };
    }

    internal static Trait CreateTestTrait(string name, string description, Species fromRace, int raceId)
    {
        return new() { Name = name, Description = description, FromRace = fromRace, RaceId = raceId };
    }

    internal static Skill CreateSkill(string name, int abilityId)
    {
        return new() { Name = name, AbilityId = abilityId };
    }
}