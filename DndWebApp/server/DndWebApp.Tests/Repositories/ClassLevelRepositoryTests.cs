using DndWebApp.Api.Data;
using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.Items;
using DndWebApp.Api.Models.Items.Enums;
using DndWebApp.Api.Models.World;
using DndWebApp.Api.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DndWebApp.Tests.Repositories;

public class ClassLevelRepositoryTests
{
    public SpellSlotsAtLevel CreateTestSpellSlotsAtLevel => new() { CantripsKnown = 1, SpellsKnown = 3, Lvl1 = 2 };

    public List<ClassSpecificSlot> CreateClassSpecificSlots => [new ClassSpecificSlot { Name = "Bardic inspiration", Quantity = 2 }];

    private ClassFeature CreateTestClassFeature(Class cls) => new()
    {
        Name = "Spellcasting",
        Description = "Gain spellcasting abilities.",
        Class = cls,
        ClassId = cls.Id,
        LevelWhenGained = 2
    };

    private ClassLevel CreateTestLevel(Class cls) => new()
    {
        Level = 2,
        Class = cls,
        ClassId = cls.Id,
        AbilityScoreBonus = 1,
        ProficiencyBonus = 3,
        ClassSpecificSlotsAtLevel = CreateClassSpecificSlots,
        SpellSlotsAtLevel = CreateTestSpellSlotsAtLevel,
        NewFeatures = [CreateTestClassFeature(cls)]
    };

    private Class CreateTestClass()
    {
        var cls = new Class
        {
            Name = "Ranger",
            Description = "Description",
            HitDie = "1d8",
            ClassLevels = []
        };
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
    public async Task UpdateClassLevel_WorksCorrectly()
    {
        // Arrange
        var options = GetInMemoryOptions("ClassLevel_UpdateDB");
        var cls = CreateTestClass();

        await using var context = new AppDbContext(options);
        var classRepo = new ClassRepository(context);
        await classRepo.CreateAsync(cls);
        await context.SaveChangesAsync();

        // Act
        var levelRepo = new ClassLevelRepository(context);
        var toUpdate = await levelRepo.GetWithAllDataAsync(cls.Id);

        toUpdate!.NewFeatures.Add(CreateTestClassFeature(cls));
        toUpdate.AbilityScoreBonus = 5;

        await levelRepo.UpdateAsync(toUpdate);
        await context.SaveChangesAsync();

        // Assert
        var updated = await levelRepo.GetWithAllDataAsync(toUpdate.Id);

        // Assert
        Assert.NotNull(updated);
        Assert.Equal(5, updated.AbilityScoreBonus);
        Assert.NotNull(updated.NewFeatures);
        Assert.NotEmpty(updated.NewFeatures);
        Assert.Equal(2, updated.NewFeatures.Count);
    }
    
    [Fact]
    public async Task DeleteClassLevel_ShouldDelete()
    {
        // Arrange
        var options = GetInMemoryOptions("ClassLevel_DeleteDB");
        var cls = CreateTestClass();

        await using var context = new AppDbContext(options);
        var classRepo = new ClassRepository(context);
        await classRepo.CreateAsync(cls);
        await context.SaveChangesAsync();

        // Act
        var levelRepo = new ClassLevelRepository(context);
        var toDelete = await levelRepo.GetByIdAsync(cls.Id);

        await levelRepo.DeleteAsync(toDelete);
        await context.SaveChangesAsync();
        var deleted = await levelRepo.GetByIdAsync(cls.Id);

        // Assert
        Assert.Null(deleted);
    }

    [Fact]
    public async Task AddAndRetrieveClass_WorksCorrectly()
    {
        // Arrange
        var options = GetInMemoryOptions("ClassLevel_AddRetrieveDB");
        var cls = CreateTestClass();

        await using var context = new AppDbContext(options);
        var classRepo = new ClassRepository(context);
        await classRepo.CreateAsync(cls);
        await context.SaveChangesAsync();

        // Act
        var levelRepo = new ClassLevelRepository(context);
        var savedLevel = await levelRepo.GetByIdAsync(cls.Id);

        // Assert
        Assert.NotNull(savedLevel);
        Assert.Equal(2, savedLevel!.Level);
        Assert.Equal(1, savedLevel.AbilityScoreBonus);
        Assert.Equal(3, savedLevel.ProficiencyBonus);
        Assert.Equal(cls.Id, savedLevel.ClassId);

        Assert.NotNull(savedLevel.SpellSlotsAtLevel);
        Assert.Equal(2, savedLevel.SpellSlotsAtLevel.Lvl1);
        Assert.Equal(3, savedLevel.SpellSlotsAtLevel.SpellsKnown);
        Assert.Equal(1, savedLevel.SpellSlotsAtLevel.CantripsKnown);

        Assert.NotNull(savedLevel.ClassSpecificSlotsAtLevel);
        Assert.NotEmpty(savedLevel.ClassSpecificSlotsAtLevel);
        Assert.Equal("Bardic inspiration", savedLevel.ClassSpecificSlotsAtLevel.First().Name);
        Assert.Equal(2, savedLevel.ClassSpecificSlotsAtLevel.First().Quantity);

        Assert.NotNull(savedLevel.NewFeatures);
        Assert.NotEmpty(savedLevel.NewFeatures);
        Assert.Equal("Spellcasting", savedLevel.NewFeatures.First().Name);
        Assert.Equal("Gain spellcasting abilities.", savedLevel.NewFeatures.First().Description);
    }

    [Fact]
    public async Task GetWithAllDataAsync_IncludesAllNavigationProperties()
    {
        // Arrange
        var options = GetInMemoryOptions("ClassLevel_GetWithAllDataDB");
        var cls = CreateTestClass();

        await using var context = new AppDbContext(options);
        var classRepo = new ClassRepository(context);
        await classRepo.CreateAsync(cls);
        await context.SaveChangesAsync();

        // Act
        var levelRepo = new ClassLevelRepository(context);
        var savedLevels = await levelRepo.GetAllWithAllDataAsync();

        // Assert
        Assert.NotNull(savedLevels);
        Assert.NotEmpty(savedLevels);
        var savedLevel = savedLevels.First();

        Assert.NotNull(savedLevel);
        Assert.Equal(2, savedLevel!.Level);
        Assert.Equal(1, savedLevel.AbilityScoreBonus);
        Assert.Equal(3, savedLevel.ProficiencyBonus);
        Assert.Equal(cls.Id, savedLevel.ClassId);

        Assert.NotNull(savedLevel.SpellSlotsAtLevel);
        Assert.Equal(2, savedLevel.SpellSlotsAtLevel.Lvl1);
        Assert.Equal(3, savedLevel.SpellSlotsAtLevel.SpellsKnown);
        Assert.Equal(1, savedLevel.SpellSlotsAtLevel.CantripsKnown);

        Assert.NotNull(savedLevel.ClassSpecificSlotsAtLevel);
        Assert.NotEmpty(savedLevel.ClassSpecificSlotsAtLevel);
        Assert.Equal("Bardic inspiration", savedLevel.ClassSpecificSlotsAtLevel.First().Name);
        Assert.Equal(2, savedLevel.ClassSpecificSlotsAtLevel.First().Quantity);

        Assert.NotNull(savedLevel.NewFeatures);
        Assert.NotEmpty(savedLevel.NewFeatures);
        Assert.Equal("Spellcasting", savedLevel.NewFeatures.First().Name);
        Assert.Equal("Gain spellcasting abilities.", savedLevel.NewFeatures.First().Description);
    }
}
