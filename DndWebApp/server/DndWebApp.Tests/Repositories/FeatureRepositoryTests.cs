using Microsoft.EntityFrameworkCore;
using DndWebApp.Api.Data;
using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Repositories.Features;

public class FeatureRepositoryTests
{

    private Feature CreateTestFeature() => new Feature
    {
        Name = "Power Strike",
        Description = "Increases melee damage.",
        IsHomebrew = false
    };

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
    public async Task Feature_Update_WorksCorrectly()
    {
        // Arrange
        var options = GetInMemoryOptions("Feature_UpdateDB");
        await using var context = new AppDbContext(options);

        var feature = CreateTestFeature();
        var repo = new FeatureRepository(context);
        await repo.CreateAsync(feature);
        await context.SaveChangesAsync();

        // Act
        feature.Name = "Updated Feature";
        feature.Description = "Updated description";
        feature.IsHomebrew = true;

        await repo.UpdateAsync(feature);
        await context.SaveChangesAsync();

        // Assert
        var updated = await repo.GetWithAllDataAsync(feature.Id);
        Assert.NotNull(updated);
        Assert.Equal("Updated Feature", updated!.Name);
        Assert.Equal("Updated description", updated.Description);
        Assert.True(updated.IsHomebrew);
    }

    [Fact]
    public async Task Feature_Delete_WorksCorrectly()
    {
        // Arrange
        var options = GetInMemoryOptions("Feature_DeleteDB");
        await using var context = new AppDbContext(options);

        var feature = CreateTestFeature();
        var repo = new FeatureRepository(context);
        await repo.CreateAsync(feature);
        await context.SaveChangesAsync();

        // Act
        await repo.DeleteAsync(feature);
        await context.SaveChangesAsync();

        // Assert
        var deleted = await repo.GetWithAllDataAsync(feature.Id);
        Assert.Null(deleted);
    }


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

        var feature = CreateTestFeature();
        var trait = CreateTestTrait(species);
        var classFeature = CreateTestClassFeature(cls);
        var feat = CreateTestFeat();
        var backgroundFeature = CreateTestBackgroundFeature(bg);

        // Act
        var featureRepo = new FeatureRepository(context);
        var traitRepo = new TraitRepository(context);
        var classFeatureRepo = new ClassFeatureRepository(context);
        var featRepo = new FeatRepository(context);
        var bgFeatureRepo = new BackgroundFeatureRepository(context);

        await featureRepo.CreateAsync(feature);
        await traitRepo.CreateAsync(trait);
        await classFeatureRepo.CreateAsync(classFeature);
        await featRepo.CreateAsync(feat);
        await bgFeatureRepo.CreateAsync(backgroundFeature);
        await context.SaveChangesAsync();

        // Assert
        var fullFeature = await featureRepo.GetWithAllDataAsync(feature.Id);
        Assert.NotNull(fullFeature);
        Assert.Equal("Power Strike", fullFeature!.Name);
        Assert.NotNull(fullFeature.AbilityIncreases);
        Assert.NotNull(fullFeature.SpellsGained);
        Assert.NotNull(fullFeature.LanguageChoices);

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
    public async Task GetAllFeaturesWithAllData_ReturnsAllEntities()
    {
        // Arrange
        var options = GetInMemoryOptions("Feature_GetAllDB");
        await using var context = new AppDbContext(options);

        var feature = CreateTestFeature();
        await context.AddAsync(feature);
        await context.SaveChangesAsync();

        // Act
        var featureRepo = new FeatureRepository(context);
        var allFeatures = await featureRepo.GetAllWithAllDataAsync();

        // Assert
        Assert.NotEmpty(allFeatures);
        Assert.Contains(allFeatures, f => f.Id == feature.Id);
        Assert.All(allFeatures, f =>
        {
            Assert.NotNull(f.AbilityIncreases);
            Assert.NotNull(f.SpellsGained);
            Assert.NotNull(f.LanguageChoices);
        });
    }

    [Fact]
    public async Task Feature_PrimitiveData_Works()
    {
        // Arrange
        var options = GetInMemoryOptions("FeaturePrimitiveDB");
        await using var context = new AppDbContext(options);

        var feature = CreateTestFeature();

        // Act
        var repo = new FeatureRepository(context);
        await repo.CreateAsync(feature);
        await context.SaveChangesAsync();

        var primitive = await repo.GetPrimitiveDataAsync(feature.Id);
        var allPrimitives = await repo.GetAllPrimitiveDataAsync();

        // Assert
        Assert.NotNull(primitive);
        Assert.Equal(feature.Id, primitive!.Id);
        Assert.Equal(feature.Name, primitive.Name);
        Assert.Equal(feature.Description, primitive.Description);
        Assert.Equal(feature.IsHomebrew, primitive.IsHomebrew);

        Assert.Single(allPrimitives);
        Assert.Equal(feature.Id, allPrimitives.First().Id);
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

        var primitive = await repo.GetPrimitiveDataAsync(trait.Id);
        var allPrimitives = await repo.GetAllPrimitiveDataAsync();

        // Assert
        Assert.NotNull(primitive);
        Assert.Equal(trait.Id, primitive!.Id);
        Assert.Equal(trait.Name, primitive.Name);
        Assert.Equal(trait.Description, primitive.Description);
        Assert.Equal(trait.IsHomebrew, primitive.IsHomebrew);
        Assert.Equal(trait.RaceId, primitive.FromEntityId);

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

        var primitive = await repo.GetPrimitiveDataAsync(feature.Id);
        var allPrimitives = await repo.GetAllPrimitiveDataAsync();

        // Assert
        Assert.NotNull(primitive);
        Assert.Equal(feature.Id, primitive!.Id);
        Assert.Equal(feature.Name, primitive.Name);
        Assert.Equal(feature.Description, primitive.Description);
        Assert.Equal(feature.IsHomebrew, primitive.IsHomebrew);
        Assert.Equal(feature.ClassId, primitive.FromEntityId);

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

        var primitive = await repo.GetPrimitiveDataAsync(feature.Id);
        var allPrimitives = await repo.GetAllPrimitiveDataAsync();

        // Assert
        Assert.NotNull(primitive);
        Assert.Equal(feature.Id, primitive!.Id);
        Assert.Equal(feature.Name, primitive.Name);
        Assert.Equal(feature.Description, primitive.Description);
        Assert.Equal(feature.IsHomebrew, primitive.IsHomebrew);
        Assert.Equal(feature.BackgroundId, primitive.FromEntityId);

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

        var primitive = await repo.GetPrimitiveDataAsync(feat.Id);
        var allPrimitives = await repo.GetAllPrimitiveDataAsync();

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
