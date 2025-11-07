using static DndWebApp.Tests.Repositories.TestObjectFactory;
using DndWebApp.Api.Data;
using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.Features;
using DndWebApp.Api.Repositories;
using DndWebApp.Api.Repositories.Implemented.Features;

namespace DndWebApp.Tests.Repositories;

public class FeatureRepositoryTests
{
    [Fact]
    public async Task AddAndRetrieveFeatures_WorksCorrectly()
    {
        var options = GetInMemoryOptions("Feature_AddRetrieveDB");
        await using var context = new AppDbContext(options);

        var traitRepo = new TraitRepository(context);
        var classFeatureRepo = new ClassFeatureRepository(context);
        var featRepo = new FeatRepository(context);
        var bgFeatureRepo = new BackgroundFeatureRepository(context);

        // Arrange
        var species = CreateTestRace("Elf");
        var cls = CreateTestClass();
        var classLevel = CreateTestLevel(cls);
        var bg = CreateTestBackground("Acholyte");

        await context.AddRangeAsync(classLevel, species, cls, bg);
        await context.SaveChangesAsync();

        var trait = CreateTestTrait("Trait", "decsription", species, species.Id);
        var classFeature = CreateTestClassFeature(classLevel.Id);
        var feat = CreateTestFeat();
        var backgroundFeature = CreateTestFeature(bg: bg, bgId: bg.Id);

        // Act
        await traitRepo.CreateAsync(trait);
        await classFeatureRepo.CreateAsync(classFeature);
        await featRepo.CreateAsync(feat);
        await bgFeatureRepo.CreateAsync(backgroundFeature);

        // Assert
        var fullTrait = await traitRepo.GetWithAllDataAsync(trait.Id);
        Assert.NotNull(fullTrait);
        Assert.Equal(species.Id, fullTrait!.FromRace.Id);
        Assert.NotNull(fullTrait.AbilityIncreases);

        var fullClassFeat = await classFeatureRepo.GetWithAllDataAsync(classFeature.Id);
        Assert.NotNull(fullClassFeat);
        Assert.Equal(cls.ClassLevels.First().Id, fullClassFeat!.ClassLevelId);
        Assert.NotNull(fullClassFeat.AbilityIncreases);

        var fullFeat = await featRepo.GetWithAllDataAsync(feat.Id);
        Assert.NotNull(fullFeat);
        Assert.NotNull(fullFeat.AbilityIncreases);

        var fullBgFeat = await bgFeatureRepo.GetWithAllDataAsync(backgroundFeature.Id);
        Assert.NotNull(fullBgFeat);
        Assert.Equal(bg.Id, fullBgFeat.Background!.Id);
        Assert.NotNull(fullBgFeat.AbilityIncreases);
    }
}
