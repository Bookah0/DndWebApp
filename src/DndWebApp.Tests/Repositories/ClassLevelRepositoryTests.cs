using static DndWebApp.Tests.Repositories.TestObjectFactory;
using DndWebApp.Api.Data;
using DndWebApp.Api.Repositories;
using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Repositories.Implemented.Classes;

namespace DndWebApp.Tests.Repositories;

public class ClassLevelRepositoryTests
{


    [Fact]
    public async Task UpdateClassLevel_WorksCorrectly()
    {
        var options = GetInMemoryOptions("ClassLevel_UpdateDB");
        int levelId;

        // Arrange
        await using (var context = new AppDbContext(options))
        {
            var classRepo = new ClassRepository(context);

            var cls = CreateTestClass();
            cls.ClassLevels.Add(CreateTestLevel(cls));

            await classRepo.CreateAsync(cls);
            levelId = cls.ClassLevels.First().Id;
        }

        // Act
        await using (var context = new AppDbContext(options))
        {
            var levelRepo = new ClassLevelRepository(context);

            var toUpdate = await levelRepo.GetWithAllDataAsync(levelId);
            Assert.NotNull(toUpdate);

            toUpdate!.NewFeatures.Add(CreateTestClassFeature(toUpdate.Id));
            toUpdate.ProficiencyBonus = 5;

            await levelRepo.UpdateAsync(toUpdate);
        }

        // Assert
        await using (var context = new AppDbContext(options))
        {
            var levelRepo = new ClassLevelRepository(context);

            var updated = await levelRepo.GetWithAllDataAsync(levelId);

            Assert.NotNull(updated);
            Assert.Equal(5, updated.ProficiencyBonus);
            Assert.NotNull(updated.NewFeatures);
            Assert.NotEmpty(updated.NewFeatures);
            Assert.Equal(2, updated.NewFeatures.Count);
        }
    }

    [Fact]
    public async Task DeleteClassLevel_ShouldDelete()
    {
        var options = GetInMemoryOptions("ClassLevel_DeleteDB");

        // Arrange
        await using (var context = new AppDbContext(options))
        {
            var classRepo = new ClassRepository(context);

            var cls = CreateTestClass();
            cls.ClassLevels.Add(CreateTestLevel(cls));
            await classRepo.CreateAsync(cls);
        };
        
        // Act
        await using (var context = new AppDbContext(options))
        {    
            var levelRepo = new ClassLevelRepository(context);

            var toDelete = await levelRepo.GetByIdAsync(10);
            await levelRepo.DeleteAsync(toDelete!);
        };


        // Assert
        await using (var context = new AppDbContext(options))
        {    
            var levelRepo = new ClassLevelRepository(context);

            Assert.Null(await levelRepo.GetByIdAsync(10));
        };
        
    }

    [Fact]
    public async Task AddAndRetrieveClass_WorksCorrectly()
    {
        var options = GetInMemoryOptions("ClassLevel_AddRetrieveDB");
        await using var context = new AppDbContext(options);
        var classRepo = new ClassRepository(context);
        var levelRepo = new ClassLevelRepository(context);

        // Arrange
        var cls = CreateTestClass();
        cls.ClassLevels.Add(CreateTestLevel(cls));
        await classRepo.CreateAsync(cls);

        // Act
        var savedLevel = await levelRepo.GetWithFeaturesAsync(10);

        // Assert
        Assert.NotNull(savedLevel);
        Assert.Equal(2, savedLevel!.Level);
        Assert.Equal(3, savedLevel.ProficiencyBonus);
        Assert.Equal(cls.Id, savedLevel.ClassId);

        Assert.NotNull(savedLevel.SpellSlots);
        Assert.Equal(1, savedLevel.SpellSlots[0]);
        Assert.Equal(0, savedLevel.SpellSlots[4]);

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
        var levelRepo = new ClassLevelRepository(context);

        // Arrange
        var cls = CreateTestClass();
        cls.ClassLevels.Add(CreateTestLevel(cls));
        await classRepo.CreateAsync(cls);

        // Act
        var savedLevels = await levelRepo.GetAllWithAllDataAsync();

        // Assert
        Assert.NotNull(savedLevels);
        Assert.NotEmpty(savedLevels);
        var savedLevel = savedLevels.First();

        Assert.NotNull(savedLevel);
        Assert.Equal(2, savedLevel!.Level);
        Assert.Equal(3, savedLevel.ProficiencyBonus);
        Assert.Equal(cls.Id, savedLevel.ClassId);

        Assert.NotNull(savedLevel.SpellSlots);
        Assert.Equal(1, savedLevel.SpellSlots[0]);
        Assert.Equal(0, savedLevel.SpellSlots[4]);

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
