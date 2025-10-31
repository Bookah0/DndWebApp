using static DndWebApp.Tests.Repositories.TestObjectFactory;
using DndWebApp.Api.Data;
using DndWebApp.Api.Repositories.Features;
using DndWebApp.Api.Repositories.Backgrounds;
using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.Features;
using DndWebApp.Api.Repositories;

namespace DndWebApp.Tests.Repositories;

public class FeatureRepositoryTests
{
    [Fact]
    public async Task AddAndRetrieveFeatures_WorksCorrectly()
    {
        var options = GetInMemoryOptions("Feature_AddRetrieveDB");
        await using var context = new AppDbContext(options);
        var baseTraitRepo = new EfRepository<Trait>(context);
        var baseClassFeatureRepo = new EfRepository<ClassFeature>(context);
        var baseBgFeatureRepo = new EfRepository<BackgroundFeature>(context);
        var baseFeatRepo = new EfRepository<Feat>(context);

        var traitRepo = new TraitRepository(context, baseTraitRepo);
        var classFeatureRepo = new ClassFeatureRepository(context, baseClassFeatureRepo);
        var featRepo = new FeatRepository(context, baseFeatRepo);
        var bgFeatureRepo = new BackgroundFeatureRepository(context, baseBgFeatureRepo);
        
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

    [Fact]
    public async Task Trait_dtoData_Works()
    {
        var options = GetInMemoryOptions("TraitdtoDB");
        await using var context = new AppDbContext(options);
        var baseTraitRepo = new EfRepository<Trait>(context);
        var repo = new TraitRepository(context, baseTraitRepo);

        // Arrange
        var race = CreateTestRace("Elf");
        var trait = CreateTestTrait("", "", race, 1);
        race.Id = 1;

        // Act
        await repo.CreateAsync(trait);

        var dto = await repo.GetDtoAsync(trait.Id);
        var alldtos = await repo.GetAllDtosAsync();

        // Assert
        Assert.NotNull(dto);
        Assert.Equal(trait.Id, dto!.Id);
        Assert.Equal(trait.Name, dto.Name);
        Assert.Equal(trait.Description, dto.Description);
        Assert.Equal(trait.IsHomebrew, dto.IsHomebrew);
        Assert.Equal(trait.RaceId, dto.RaceId);

        Assert.Single(alldtos);
        Assert.Equal(trait.Id, alldtos.First().Id);
    }

    [Fact]
    public async Task ClassFeature_dtoData_Works()
    {
        var options = GetInMemoryOptions("ClassFeaturedtoDB");
        await using var context = new AppDbContext(options);
        var baseClassFeatureRepo = new EfRepository<ClassFeature>(context);
        var repo = new ClassFeatureRepository(context, baseClassFeatureRepo);

        // Arrange
        var clss = CreateTestClass();
        var classLevel = CreateTestLevel(clss);
        var feature = CreateTestClassFeature(classLevel.Id);

        // Act
        await repo.CreateAsync(feature);

        var dto = await repo.GetDtoAsync(feature.Id);
        var alldtos = await repo.GetAllDtosAsync();

        // Assert
        Assert.NotNull(dto);
        Assert.Equal(feature.Id, dto!.Id);
        Assert.Equal(feature.Name, dto.Name);
        Assert.Equal(feature.Description, dto.Description);
        Assert.Equal(feature.IsHomebrew, dto.IsHomebrew);
        Assert.Equal(feature.ClassLevelId, dto.ClassLevelId);

        Assert.Single(alldtos);
        Assert.Equal(feature.Id, alldtos.First().Id);
    }


    [Fact]
    public async Task BackgroundFeature_dtoData_Works()
    {
        var options = GetInMemoryOptions("BackgroundFeaturedtoDB");
        await using var context = new AppDbContext(options);
        var baseBgFeatureRepo = new EfRepository<BackgroundFeature>(context);
        var baseBgRepo = new EfRepository<Background>(context);
        var repo = new BackgroundFeatureRepository(context, baseBgFeatureRepo);
        var bgrepo = new BackgroundRepository(context, baseBgRepo);

        // Arrange
        var background = CreateTestBackground("Acholyte");
        var feature = CreateTestFeature(bg: background, bgId: background.Id);
        
        background.Features.Add(feature);
        context.Backgrounds.Add(background);
        await context.SaveChangesAsync();

        var dto = await repo.GetDtoAsync(background.Features.First().Id);
        var alldtos = await repo.GetAllDtosAsync();

        // Assert
        Assert.NotNull(dto);
        Assert.Equal(feature.Name, dto.Name);
        Assert.Equal(feature.Description, dto.Description);
        Assert.Equal(feature.IsHomebrew, dto.IsHomebrew);

        Assert.Single(alldtos);
    }

    [Fact]
    public async Task Feat_dtoData_Works()
    {
        var options = GetInMemoryOptions("FeatdtoDB");
        await using var context = new AppDbContext(options);
        var baseFeatRepo = new EfRepository<Feat>(context);
        var repo = new FeatRepository(context, baseFeatRepo);

        // Arrange
        var feat = CreateTestFeat();

        // Act
        await repo.CreateAsync(feat);
        await context.SaveChangesAsync();

        var dto = await repo.GetDtoAsync(feat.Id);
        var alldtos = await repo.GetDtosAsync();

        // Assert
        Assert.NotNull(dto);
        Assert.Equal(feat.Id, dto!.Id);
        Assert.Equal(feat.Name, dto.Name);
        Assert.Equal(feat.Description, dto.Description);
        Assert.Equal(feat.IsHomebrew, dto.IsHomebrew);
        Assert.Equal(feat.Prerequisite, dto.Prerequisite);

        Assert.Single(alldtos);
        Assert.Equal(feat.Id, alldtos.First().Id);
    }
}
