using Microsoft.EntityFrameworkCore;
using DndWebApp.Api.Data;
using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Repositories.Features;
using DndWebApp.Api.Models.Features;

namespace DndWebApp.Tests.Repositories;

public class FeatureRepositoryTests
{
    private Trait CreateTestTrait(Species species) => new Trait
    {
        Name = "Darkvision",
        Description = "See in the dark.",
        FromRace = species,
        RaceId = species.Id
    };

    private ClassFeature CreateTestClassFeature(Class cls) => new ClassFeature
    {
        Name = "Spellcasting",
        Description = "Gain spellcasting abilities.",
        Class = cls,
        ClassId = cls.Id,
        LevelWhenGained = 1
    };

    private Feat CreateTestFeat() => new Feat
    {
        Name = "Sharpshooter",
        Description = "Improve ranged attacks.",
        Prerequisite = "Dex 13"
    };

    private BackgroundFeature CreateTestBackgroundFeature(Background bg) => new BackgroundFeature
    {
        Name = "Shelter of the Faithful",
        Description = "Command respect in your community.",
        Background = bg,
        BackgroundId = bg.Id
    };

    private Class CreateTestClass() => new Class
    {
        Name = "Shelter of the Faithful",
        Description = "Command respect in your community.",
        HitDie = "",
        ClassLevels = [],
        SpellLevel = 2,
        Info = [],
        SpellcastingAbilityId = -1
    };

    private Race CreateTestRace(string name = "Elf") => new() { Name = name, Speed = 30 };

    private Background CreateTestBackground()
    {
        var background = new Background
        {
            Name = "Acholyte",
            Description = "Acholyte description",
            StartingCurrency = new() { Gold = 15 }
        };
        return background;
    }

    private DbContextOptions<AppDbContext> GetInMemoryOptions(string dbName) => new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
            .Options;

    [Fact]
    public async Task AddAndRetrieveFeatures_WorksCorrectly()
    {
        // Arrange
        var options = GetInMemoryOptions("Feature_AddRetrieveDB");
        await using var context = new AppDbContext(options);

        var species = CreateTestRace();
        var cls = CreateTestClass();
        var bg = CreateTestBackground();

        await context.AddRangeAsync(species, cls, bg);
        await context.SaveChangesAsync();

        var trait = CreateTestTrait(species);
        var classFeature = CreateTestClassFeature(cls);
        var feat = CreateTestFeat();
        var backgroundFeature = CreateTestBackgroundFeature(bg);

        // Act
        var traitRepo = new TraitRepository(context);
        var classFeatureRepo = new ClassFeatureRepository(context);
        var featRepo = new FeatRepository(context);
        var bgFeatureRepo = new BackgroundFeatureRepository(context);

        await traitRepo.CreateAsync(trait);
        await classFeatureRepo.CreateAsync(classFeature);
        await featRepo.CreateAsync(feat);
        await bgFeatureRepo.CreateAsync(backgroundFeature);
        await context.SaveChangesAsync();

        // Assert
        var fullTrait = await traitRepo.GetWithAllDataAsync(trait.Id);
        Assert.NotNull(fullTrait);
        Assert.Equal(species.Id, fullTrait!.FromRace.Id);
        Assert.NotNull(fullTrait.AbilityIncreases);

        var fullClassFeat = await classFeatureRepo.GetWithAllDataAsync(classFeature.Id);
        Assert.NotNull(fullClassFeat);
        Assert.Equal(cls.Id, fullClassFeat!.Class.Id);
        Assert.NotNull(fullClassFeat.AbilityIncreases);

        var fullFeat = await featRepo.GetWithAllDataAsync(feat.Id);
        Assert.NotNull(fullFeat);
        Assert.NotNull(fullFeat.AbilityIncreases);

        var fullBgFeat = await bgFeatureRepo.GetWithAllDataAsync(backgroundFeature.Id);
        Assert.NotNull(fullBgFeat);
        Assert.Equal(bg.Id, fullBgFeat!.Background.Id);
        Assert.NotNull(fullBgFeat.AbilityIncreases);
    }

    [Fact]
    public async Task Trait_PrimitiveData_Works()
    {
        // Arrange
        var options = GetInMemoryOptions("TraitPrimitiveDB");
        await using var context = new AppDbContext(options);

        var race = CreateTestRace();
        var trait = CreateTestTrait(race);
        var repo = new TraitRepository(context);

        // Act
        await repo.CreateAsync(trait);
        await context.SaveChangesAsync();

        var primitive = await repo.GetTraitDtoAsync(trait.Id);
        var allPrimitives = await repo.GetAllTraitDtosAsync();

        // Assert
        Assert.NotNull(primitive);
        Assert.Equal(trait.Id, primitive!.Id);
        Assert.Equal(trait.Name, primitive.Name);
        Assert.Equal(trait.Description, primitive.Description);
        Assert.Equal(trait.IsHomebrew, primitive.IsHomebrew);
        Assert.Equal(trait.RaceId, primitive.FromId);

        Assert.Single(allPrimitives);
        Assert.Equal(trait.Id, allPrimitives.First().Id);
    }

    [Fact]
    public async Task ClassFeature_PrimitiveData_Works()
    {
        // Arrange
        var options = GetInMemoryOptions("ClassFeaturePrimitiveDB");
        await using var context = new AppDbContext(options);

        var clss = CreateTestClass();
        var feature = CreateTestClassFeature(clss);

        // Act
        var repo = new ClassFeatureRepository(context);
        await repo.CreateAsync(feature);
        await context.SaveChangesAsync();

        var primitive = await repo.GetClassFeatureDtoAsync(feature.Id);
        var allPrimitives = await repo.GetAllClassFeatureDtosAsync();

        // Assert
        Assert.NotNull(primitive);
        Assert.Equal(feature.Id, primitive!.Id);
        Assert.Equal(feature.Name, primitive.Name);
        Assert.Equal(feature.Description, primitive.Description);
        Assert.Equal(feature.IsHomebrew, primitive.IsHomebrew);
        Assert.Equal(feature.ClassId, primitive.FromId);

        Assert.Single(allPrimitives);
        Assert.Equal(feature.Id, allPrimitives.First().Id);
    }


    [Fact]
    public async Task BackgroundFeature_PrimitiveData_Works()
    {
        // Arrange
        var options = GetInMemoryOptions("BackgroundFeaturePrimitiveDB");
        await using var context = new AppDbContext(options);

        var background = CreateTestBackground();
        var feature = CreateTestBackgroundFeature(background);

        // Act
        var repo = new BackgroundFeatureRepository(context);
        await repo.CreateAsync(feature);
        await context.SaveChangesAsync();

        var primitive = await repo.GetBackgroundDtoAsync(feature.Id);
        var allPrimitives = await repo.GetAllBackgroundDtosAsync();

        // Assert
        Assert.NotNull(primitive);
        Assert.Equal(feature.Id, primitive!.Id);
        Assert.Equal(feature.Name, primitive.Name);
        Assert.Equal(feature.Description, primitive.Description);
        Assert.Equal(feature.IsHomebrew, primitive.IsHomebrew);
        Assert.Equal(feature.BackgroundId, primitive.FromId);

        Assert.Single(allPrimitives);
        Assert.Equal(feature.Id, allPrimitives.First().Id);
    }

    [Fact]
    public async Task Feat_PrimitiveData_Works()
    {
        // Arrange
        var options = GetInMemoryOptions("FeatPrimitiveDB");
        await using var context = new AppDbContext(options);

        var feat = CreateTestFeat();

        // Act
        var repo = new FeatRepository(context);
        await repo.CreateAsync(feat);
        await context.SaveChangesAsync();

        var primitive = await repo.GetFeatDtoAsync(feat.Id);
        var allPrimitives = await repo.GetAllFeatDtosAsync();

        // Assert
        Assert.NotNull(primitive);
        Assert.Equal(feat.Id, primitive!.Id);
        Assert.Equal(feat.Name, primitive.Name);
        Assert.Equal(feat.Description, primitive.Description);
        Assert.Equal(feat.IsHomebrew, primitive.IsHomebrew);
        Assert.Equal(feat.Prerequisite, primitive.Prerequisite);

        Assert.Single(allPrimitives);
        Assert.Equal(feat.Id, allPrimitives.First().Id);
    }
}
