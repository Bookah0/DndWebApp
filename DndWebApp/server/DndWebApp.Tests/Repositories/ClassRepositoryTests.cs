using DndWebApp.Api.Data;
using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.Items;
using DndWebApp.Api.Models.Items.Enums;
using DndWebApp.Api.Repositories.Classes;
using Microsoft.EntityFrameworkCore;

namespace DndWebApp.Tests.Repositories;

public class ClassRepositoryTests
{
    private Tool CreateTestTool() => new()
    {
        Name = "Thieves' Kit",
        Description = "A set of lockpicks and other tools for stealthy operations.",
        Catagories = ItemCategory.Tools,
        Activities = [],
        Properties = []
    };

    private Armor CreateTestArmor() => new()
    {
        Name = "Leather Armor",
        Description = "Light armor made from tanned leather, provides basic protection.",
        Catagories = ItemCategory.Armor,
        Category = ArmorCategory.Light,
        BaseArmorClass = 11,
        PlusDexMod = true
    };

    private Weapon CreateTestWeapon() => new()
    {
        Name = "Shortbow",
        Description = "A small bow ideal for ranged attacks.",
        Catagories = ItemCategory.Weapon,
        WeaponCategories = WeaponCategory.SimpleRanged | WeaponCategory.Shortbow,
        Properties = WeaponProperty.TwoHanded,
        DamageTypes = DamageType.Piercing,
        DamageDice = "1d6",
        Range = 80
    };

    private ItemChoice CreateTestStartingEquipmentChoice()
    {
        var option = new ItemChoice
        {
            Description = "armor or weapon",
            NumberOfChoices = 2,
            Options = []
        };

        option.Options.Add(CreateTestArmor());
        option.Options.Add(CreateTestWeapon());

        return option;
    }

    private ClassLevel CreateTestLevel(Class cls) => new ClassLevel
    {
        Level = 2,
        Class = cls,
        ClassId = cls.Id,
        AbilityScoreBonus = 1,
        ProficiencyBonus = 3
    };

    private Class CreateTestClass(string name)
    {
        var description = $"{name} description";
        var cls = new Class
        {
            Name = name,
            Description = description,
            HitDie = "1d8",
            ClassLevels = []
        };

        cls.StartingEquipment.Add(CreateTestTool());
        cls.StartingEquipmentOptions.Add(CreateTestStartingEquipmentChoice());

        cls.ClassLevels.Add(CreateTestLevel(cls));

        return cls;
    }

    private DbContextOptions<AppDbContext> GetInMemoryOptions(string dbName)
    {
        return new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .Options;
    }

    [Fact]
    public async Task UpdateClass_WorksCorrectly()
    {
        // Arrange
        var options = GetInMemoryOptions("Class_AddRetrieveDB");
        var cls = CreateTestClass("Ranger");

        await using var context = new AppDbContext(options);
        var repo = new ClassRepository(context);
        await repo.CreateAsync(cls);
        await context.SaveChangesAsync();

        // Act
        var toUpdate = await repo.GetWithAllDataAsync(cls.Id);

        toUpdate!.Name = "Barbarian";
        toUpdate.StartingEquipment.Clear();

        await repo.UpdateAsync(toUpdate);
        await context.SaveChangesAsync();

        // Assert
        var updated = await repo.GetWithAllDataAsync(toUpdate.Id);

        Assert.NotNull(updated);
        Assert.Equal("Barbarian", updated.Name);
        Assert.NotNull(updated.StartingEquipment);
        Assert.Empty(updated.StartingEquipment);
    }

    [Fact]
    public async Task DeleteClass_ShouldDelete()
    {
        var options = GetInMemoryOptions("Character_AllPrimitiveDB");

        var cls = CreateTestClass("Ranger");

        await using var context = new AppDbContext(options);
        var repo = new ClassRepository(context);
        await repo.CreateAsync(cls);
        await context.SaveChangesAsync();

        // Act
        await repo.DeleteAsync(cls);
        await context.SaveChangesAsync();
        var deleted = await repo.GetWithAllDataAsync(cls.Id);

        // Assert
        Assert.Null(deleted);
    }
    
    [Fact]
    public async Task AddAndRetrieveClass_WorksCorrectly()
    {
        // Arrange
        var options = GetInMemoryOptions("Class_AddRetrieveDB");
        var cls = CreateTestClass("Ranger");

        // Act
        await using var context = new AppDbContext(options);
        var repo = new ClassRepository(context);
        await repo.CreateAsync(cls);
        await context.SaveChangesAsync();

        var savedClass = await repo.GetByIdAsync(cls.Id);

        // Assert
        Assert.NotNull(savedClass);
        Assert.Equal("Ranger", savedClass!.Name);
        Assert.Equal("Ranger description", savedClass.Description);

        Assert.Equal("Thieves' Kit", savedClass.StartingEquipment.First().Name);
        Assert.NotEmpty(savedClass.StartingEquipmentOptions);
        Assert.NotEmpty(savedClass.ClassLevels);
        Assert.Equal(savedClass.Id, savedClass.ClassLevels.First().ClassId);
    }

    [Fact]
    public async Task GetPrimitiveDataAsync_ReturnsCorrectValues()
    {
        // Arrange
        var options = GetInMemoryOptions("Class_PrimitiveDataDB");
        var cls = CreateTestClass("Ranger");

        // Act
        await using var context = new AppDbContext(options);
        var repo = new ClassRepository(context);
        await repo.CreateAsync(cls);
        await context.SaveChangesAsync();

        var primitive = await repo.GetPrimitiveDataAsync(cls.Id);

        // Assert
        Assert.NotNull(primitive);
        Assert.Equal("Ranger", primitive!.Name);
        Assert.Equal("Ranger description", primitive.Description);
        Assert.Equal("1d8", primitive.HitDie);
        Assert.False(primitive.IsHomebrew);
    }

    [Fact]
    public async Task GetAllPrimitiveDataAsync_ReturnsAllClasss()
    {
        // Arrange
        var options = GetInMemoryOptions("Class_GetAllPrimitiveDB");
        var c1 = CreateTestClass("Ranger");
        var c2 = CreateTestClass("Rogue");

        // Act
        await using var context = new AppDbContext(options);
        var repo = new ClassRepository(context);
        await repo.CreateAsync(c1);
        await repo.CreateAsync(c2);
        await context.SaveChangesAsync();
        var allPrimitives = await repo.GetAllPrimitiveDataAsync();

        // Assert
        Assert.Equal(2, allPrimitives.Count);
        Assert.Contains(allPrimitives, b => b.Name == "Ranger");
        Assert.Contains(allPrimitives, b => b.Name == "Rogue");
    }

    [Fact]
    public async Task GetWithAllDataAsync_IncludesAllNavigationProperties()
    {
        // Arrange
        var options = GetInMemoryOptions("Class_GetWithAllDataDB");
        var cls = CreateTestClass("Acolyte");

        await using var context = new AppDbContext(options);
        var repo = new ClassRepository(context);
        await repo.CreateAsync(cls);
        await context.SaveChangesAsync();

        var fullClass = await repo.GetWithAllDataAsync(cls.Id);

        // Assert
        Assert.NotNull(fullClass);

        // Starting Items
        Assert.NotEmpty(fullClass!.StartingEquipment);
        var tool = fullClass.StartingEquipment.FirstOrDefault(i => i.Name == "Thieves' Kit");
        Assert.NotNull(tool);
        Assert.Equal(ItemCategory.Tools, tool!.Catagories);

        // Starting Item Choices
        Assert.NotEmpty(fullClass.StartingEquipmentOptions);
        var prayerChoice = fullClass.StartingEquipmentOptions
            .FirstOrDefault(o => o.Description.Contains("armor or weapon"));
        Assert.NotNull(prayerChoice);
        Assert.Equal(2, prayerChoice!.Options.Count);
        Assert.Contains(prayerChoice.Options, c => c.Name == "Leather Armor");
        Assert.Contains(prayerChoice.Options, c => c.Name == "Shortbow");

        // Levels
        Assert.NotEmpty(fullClass.ClassLevels);
        Assert.NotNull(fullClass.ClassLevels.FirstOrDefault(l => l.Level == 2));
        Assert.NotNull(fullClass.ClassLevels.FirstOrDefault(l => l.ProficiencyBonus == 3));
    }
}
