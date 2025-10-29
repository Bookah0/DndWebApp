using static DndWebApp.Tests.Repositories.TestObjectFactory;
using DndWebApp.Api.Data;
using DndWebApp.Api.Repositories.Classes;

namespace DndWebApp.Tests.Repositories;

public class ClassLevelRepositoryTests
{
    

    [Fact]
    public async Task UpdateClassLevel_WorksCorrectly()
    {
        var options = GetInMemoryOptions("ClassLevel_UpdateDB");
        await using var context = new AppDbContext(options);
        var classRepo = new ClassRepository(context);

        // Arrange
        var cls = CreateTestClass();
        cls.ClassLevels.Add(CreateTestLevel(cls));
        await classRepo.CreateAsync(cls);

        // Act
        var levelRepo = new ClassLevelRepository(context);
        var levelId = cls.ClassLevels.First().Id;
        var toUpdate = await levelRepo.GetWithAllDataAsync(levelId);

        toUpdate!.NewFeatures.Add(CreateTestClassFeature(toUpdate.Id));
        toUpdate.AbilityScoreBonus = 5;

        await levelRepo.UpdateAsync(toUpdate);
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
        var options = GetInMemoryOptions("ClassLevel_DeleteDB");
        await using var context = new AppDbContext(options);
        var classRepo = new ClassRepository(context);

        // Arrange
        var cls = CreateTestClass();
        cls.ClassLevels.Add(CreateTestLevel(cls));
        await classRepo.CreateAsync(cls);

        // Act
        var levelRepo = new ClassLevelRepository(context);
        var levelId = cls.ClassLevels.First().Id;
        var toDelete = await levelRepo.GetByIdAsync(levelId);
        await levelRepo.DeleteAsync(toDelete!);

        // Assert
        Assert.Null(await levelRepo.GetByIdAsync(levelId));
    }

    [Fact]
    public async Task AddAndRetrieveClass_WorksCorrectly()
    {
        var options = GetInMemoryOptions("ClassLevel_AddRetrieveDB");
        await using var context = new AppDbContext(options);
        var classRepo = new ClassRepository(context);

        // Arrange
        var cls = CreateTestClass();
        cls.ClassLevels.Add(CreateTestLevel(cls));
        await classRepo.CreateAsync(cls);

        // Act
        var levelRepo = new ClassLevelRepository(context);
        var levelId = cls.ClassLevels.First().Id;
        var savedLevel = await levelRepo.GetByIdAsync(levelId);

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
        var options = GetInMemoryOptions("ClassLevel_GetWithAllDataDB");
        await using var context = new AppDbContext(options);
        var classRepo = new ClassRepository(context);

        // Arrange
        var cls = CreateTestClass();
        cls.ClassLevels.Add(CreateTestLevel(cls));
        await classRepo.CreateAsync(cls);

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
