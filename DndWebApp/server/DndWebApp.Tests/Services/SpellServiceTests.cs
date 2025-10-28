using DndWebApp.Api.Data;
using DndWebApp.Api.Models.DTOs;
using DndWebApp.Api.Models.Items.Enums;
using DndWebApp.Api.Models.Spells.Enums;
using DndWebApp.Api.Repositories.Classes;
using DndWebApp.Api.Repositories.Spells;
using DndWebApp.Api.Services;
using Microsoft.EntityFrameworkCore;

namespace DndWebApp.Tests.Services;

public class SpellServiceTests
{
    private SpellDto CreateSpellDto(string name, int id = 0)
    {
        return new SpellDto
        {
            Id = id,
            Name = name,
            Description = "A powerful spell",
            IsHomebrew = false,
            ClassIds = [1],
            ShapeLength = "0",
            ShapeType = "0",
            ShapeWidth = "0",
            Level = 3,
            EffectsAtHigherLevels = "Extra effect",
            ReactionCondition = "",
            Duration = "Minute1",
            CastingTime = "Action",
            MagicSchool = "Evocation",
            TargetType = "Creature",
            Range = "Feet",
            RangeValue = 10,
            Types = ["Buff"],
            DamageRoll = "2d6",
            DamageTypes = ["Fire"],
            Verbal = true,
            Somatic = true,
            Materials = "Bat guano",
            MaterialCost = 5,
            MaterialsConsumed = false
        };
    }

    private DbContextOptions<AppDbContext> GetInMemoryOptions(string dbName)
    {
        return new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
            .Options;
    }

    [Fact]
    public async Task AddAndRetrieveSpells_WorksCorrectly()
    {
        // Arrange
        var options = GetInMemoryOptions("SpellService_AddRetrieveDB");
        using var context = new AppDbContext(options);
        var repo = new SpellRepository(context);
        var classRepo = new ClassRepository(context);
        var service = new SpellService(repo, classRepo, context);

        var fireballDto = CreateSpellDto("Fireball");
        var lightningDto = CreateSpellDto("Lightning Bolt");

        // Act
        var createdFireball = await service.CreateAsync(fireballDto);
        var createdLightning = await service.CreateAsync(lightningDto);

        var retrievedFireball = await service.GetByIdAsync(createdFireball.Id);
        var retrievedLightning = await service.GetByIdAsync(createdLightning.Id);
        
        // Assert
        Assert.NotNull(retrievedFireball);
        Assert.Equal("Fireball", retrievedFireball.Name);
        Assert.Equal(3, retrievedFireball.Level);
        Assert.Equal(MagicSchool.Evocation, retrievedFireball.MagicSchool);

        Assert.NotNull(retrievedLightning);
        Assert.Equal("Lightning Bolt", retrievedLightning.Name);

        Assert.NotNull(retrievedFireball.SpellTargeting);
        Assert.Equal(SpellTargetType.Creature, retrievedFireball.SpellTargeting.TargetType);
        Assert.Equal(SpellRange.Feet, retrievedFireball.SpellTargeting.Range);
        Assert.Equal(10, retrievedFireball.SpellTargeting.RangeValue);

        Assert.NotNull(retrievedFireball.SpellDamage);
        Assert.Equal("2d6", retrievedFireball.SpellDamage.DamageRoll);
        Assert.Equal(DamageType.Fire, retrievedFireball.SpellDamage.DamageTypes);

        Assert.NotNull(retrievedFireball.CastingRequirements);
        Assert.True(retrievedFireball.CastingRequirements.Verbal);
        Assert.True(retrievedFireball.CastingRequirements.Somatic);
        Assert.Equal("Bat guano", retrievedFireball.CastingRequirements.Materials);
        Assert.Equal(5, retrievedFireball.CastingRequirements.MaterialCost);
        Assert.False(retrievedFireball.CastingRequirements.MaterialsConsumed);

        var allSpells = await service.GetAllAsync();
        Assert.Contains(allSpells, s => s.Name == "Fireball");
        Assert.Contains(allSpells, s => s.Name == "Lightning Bolt");
    }

    [Fact]
    public async Task UpdateSpell_WorksCorrectly()
    {
        // Arrange
        var options = GetInMemoryOptions("SpellService_UpdateDB");
        using var context = new AppDbContext(options);
        var repo = new SpellRepository(context);
        var classRepo = new ClassRepository(context);
        var service = new SpellService(repo, classRepo, context);

        var fireballDto = CreateSpellDto("Fireball");
        var createdFireball = await service.CreateAsync(fireballDto);

        // Act
        fireballDto.Id = createdFireball.Id;
        fireballDto.Name = "Mega Fireball";
        fireballDto.Level = 5;
        fireballDto.RangeValue = 20;
        fireballDto.DamageRoll = "4d6";
        fireballDto.Materials = "Dragon scale";

        await service.UpdateAsync(fireballDto);

        var updatedFireball = await service.GetByIdAsync(createdFireball.Id);

        // Assert
        Assert.NotNull(updatedFireball);
        Assert.Equal("Mega Fireball", updatedFireball.Name);
        Assert.Equal(5, updatedFireball.Level);

        Assert.NotNull(updatedFireball.SpellTargeting);
        Assert.Equal(20, updatedFireball.SpellTargeting.RangeValue);

        Assert.NotNull(updatedFireball.SpellDamage);
        Assert.Equal("4d6", updatedFireball.SpellDamage.DamageRoll);

        Assert.NotNull(updatedFireball.CastingRequirements);
        Assert.Equal("Dragon scale", updatedFireball.CastingRequirements.Materials);
    }

    [Fact]
    public async Task DeleteSpell_WorksCorrectly()
    {
        // Arrange
        var options = GetInMemoryOptions("SpellService_DeleteDB");
        using var context = new AppDbContext(options);
        var repo = new SpellRepository(context);
        var classRepo = new ClassRepository(context);
        var service = new SpellService(repo, classRepo, context);

        var lightningDto = CreateSpellDto("Lightning Bolt");
        var createdLightning = await service.CreateAsync(lightningDto);

        // Act
        await service.DeleteAsync(createdLightning.Id);

        // Assert
        await Assert.ThrowsAsync<NullReferenceException>(() => service.GetByIdAsync(createdLightning.Id));
        await Assert.ThrowsAsync<NullReferenceException>(() => service.DeleteAsync(createdLightning.Id));
    }

    [Fact]
    public async Task SortBy_WorksCorrectly()
    {
        // Arrange
        var options = GetInMemoryOptions("SpellService_DeleteDB");
        using var context = new AppDbContext(options);
        var repo = new SpellRepository(context);
        var classRepo = new ClassRepository(context);
        var service = new SpellService(repo, classRepo, context);

        var lightningDto = CreateSpellDto("Lightning Bolt");
        var flameDto = CreateSpellDto("Flame Bolt");
        var iceDto = CreateSpellDto("Ice Bolt");
        var rockDto = CreateSpellDto("Rock Bolt");
        
        lightningDto.Level = 1;
        flameDto.Level = 2;
        iceDto.Level = 1;
        rockDto.Level = 2;
        
        lightningDto.Duration = "Instantaneous";
        flameDto.Duration = "Minute";
        iceDto.Duration = "Minute";
        rockDto.Duration = "Minute";
        lightningDto.DurationValue = 0;
        flameDto.DurationValue = 1;
        iceDto.DurationValue = 1;
        rockDto.DurationValue = 10;

        var createdLightning = await service.CreateAsync(lightningDto);
        var createdFlame = await service.CreateAsync(flameDto);
        var createdIce = await service.CreateAsync(iceDto);
        var createdRock = await service.CreateAsync(rockDto);

        // Act & Assert
        var allSpells = await service.GetAllAsync();
        allSpells = service.SortBy(allSpells, SpellService.SpellSorting.Name);

        Assert.NotNull(allSpells);
        Assert.Equal("Flame Bolt", allSpells.First().Name);
        Assert.Equal("Rock Bolt", allSpells.Last().Name);
        
        allSpells = service.SortBy(allSpells, SpellService.SpellSorting.Name, true);
        Assert.NotNull(allSpells);
        Assert.Equal("Flame Bolt", allSpells.Last().Name);
        Assert.Equal("Rock Bolt", allSpells.First().Name);

        allSpells = service.SortBy(allSpells, SpellService.SpellSorting.Level);
        Assert.NotNull(allSpells);
        Assert.Equal("Ice Bolt", allSpells.First().Name);
        Assert.Equal("Rock Bolt", allSpells.Last().Name);

        allSpells = service.SortBy(allSpells, SpellService.SpellSorting.Duration);
        Assert.NotNull(allSpells);
        Assert.Equal("Lightning Bolt", allSpells.First().Name);
        Assert.Equal("Flame Bolt", allSpells.ElementAt(1).Name);
        Assert.Equal("Ice Bolt", allSpells.ElementAt(2).Name);
        Assert.Equal("Rock Bolt", allSpells.Last().Name);
    }
}
