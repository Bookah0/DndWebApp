using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DndWebApp.Api.Data;
using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DndWebApp.Tests.Repositories;

public class RaceRepositoryTests
{
    private Race CreateTestRace(string name)
    {
        return new() { Name = name, Speed = 30 };
    }

    private Subrace CreateTestSubrace(string name, Race parentRace, int parentRaceId)
    {
        return new() { Name = name, Speed = 30, ParentRace = parentRace, ParentRaceId = parentRaceId };
    }

    private Trait CreateTestTrait(string name, string description, Species fromRace, int raceId )
    {
        return new() { Name = name, Description = description, FromRace = fromRace, RaceId = raceId };
    }

    private DbContextOptions<AppDbContext> GetInMemoryOptions(string dbName)
    {
        return new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
            .Options;
    }

    [Fact]
    public async Task AddAndRetrieveRace_WorksCorrectly()
    {
        // Arrange
        var options = GetInMemoryOptions("Race_AddRetrieveDB");

        var elfRace = CreateTestRace("Elf");
        var dwarfRace = CreateTestRace("Dwarf");

        // Act
        await using (var context = new AppDbContext(options))
        {
            var repo = new RaceRepository(context);
            await repo.CreateAsync(elfRace);
            await repo.CreateAsync(dwarfRace);
            await context.SaveChangesAsync();
        }

        // Assert
        await using (var context = new AppDbContext(options))
        {
            var repo = new RaceRepository(context);

            var savedElf = await repo.GetByIdAsync(1);

            Assert.NotNull(savedElf);
            Assert.Equal("Elf", savedElf!.Name);

            var allRaces = await repo.GetAllAsync();
            Assert.Equal(2, allRaces.Count);
            Assert.Contains(allRaces, r => r.Name == "Elf");
            Assert.Contains(allRaces, r => r.Name == "Dwarf");
        }
    }

    [Fact]
    public async Task UpdateRace_WorksCorrectly()
    {
        // Arrange
        var options = GetInMemoryOptions("Race_UpdateDB");

        var race = CreateTestRace("Elf");

        await using (var context = new AppDbContext(options))
        {
            var repo = new RaceRepository(context);
            await repo.CreateAsync(race);
            await context.SaveChangesAsync();
        }

        // Act
        await using (var context = new AppDbContext(options))
        {
            var repo = new RaceRepository(context);
            race.Name = "Updated Elf";
            await repo.UpdateAsync(race);
            await context.SaveChangesAsync();
        }

        // Assert
        await using (var context = new AppDbContext(options))
        {
            var repo = new RaceRepository(context);
            var updated = await repo.GetByIdAsync(1);

            Assert.Equal("Updated Elf", updated!.Name);
        }
    }

    [Fact]
    public async Task DeleteRace_WorksCorrectly()
    {
        // Arrange
        var options = GetInMemoryOptions("Race_DeleteDB");

        var race = CreateTestRace("Elf");

        await using (var context = new AppDbContext(options))
        {
            var repo = new RaceRepository(context);
            await repo.CreateAsync(race);
            await context.SaveChangesAsync();
        }

        // Act
        await using (var context = new AppDbContext(options))
        {
            var repo = new RaceRepository(context);
            await repo.DeleteAsync(race);
            await context.SaveChangesAsync();
        }

        // Assert
        await using (var context = new AppDbContext(options))
        {
            var repo = new RaceRepository(context);
            var deleted = await repo.GetByIdAsync(race.Id);

            Assert.Null(deleted);
        }
    }

    [Fact]
    public async Task RetrieveRacesAsPrimitiveDtos_ShouldHaveCorrectFieldValues()
    {
        // Arrange
        var options = GetInMemoryOptions("PrimitiveRace_AddRetrieveDB");

        var elfRace = CreateTestRace("Elf");
        var dwarfRace = CreateTestRace("Dwarf");
        int elfId;
        int dwarfId;

        elfRace.RaceDescription.GeneralDescription = "Elf description";
        dwarfRace.RaceDescription.GeneralDescription = "Dwarf description";

        // Act
        await using (var context = new AppDbContext(options))
        {
            var repo = new RaceRepository(context);
            await repo.CreateAsync(elfRace);
            await repo.CreateAsync(dwarfRace);
            await context.SaveChangesAsync();

            elfId = elfRace.Id;
            dwarfId = dwarfRace.Id;
        }

        // Assert
        await using (var context = new AppDbContext(options))
        {
            var repo = new RaceRepository(context);

            var primitiveElf = await repo.GetPrimitiveDataAsync(elfId);
            var primitiveDwarf = await repo.GetPrimitiveDataAsync(dwarfId);

            Assert.NotNull(primitiveElf);
            Assert.Equal("Elf", primitiveElf!.Name);
            Assert.NotNull(primitiveElf.GeneralDescription);
            Assert.Equal("Elf description", primitiveElf.GeneralDescription);

            Assert.NotNull(primitiveDwarf);
            Assert.Equal("Dwarf", primitiveDwarf!.Name);
            Assert.NotNull(primitiveDwarf.GeneralDescription);
            Assert.Equal("Dwarf description", primitiveDwarf.GeneralDescription);

            var allRacesAsPrimitive = await repo.GetAllPrimitiveDataAsync();
            Assert.Equal(2, allRacesAsPrimitive.Count);
            Assert.Contains(allRacesAsPrimitive, r => r.Name == "Dwarf");
            Assert.Contains(allRacesAsPrimitive, r => r.Name == "Elf");
        }
    }

    [Fact]
    public async Task RetrieveWithTraitAndSubraces_ShouldHaveCorrectTrait()
    {
        // Arrange
        var options = GetInMemoryOptions("GetAllWithCollections_AddRetrieveDB");

        var elfRace = CreateTestRace("Elf");
        var dwarfRace = CreateTestRace("Dwarf");

        var highElf = CreateTestSubrace("High Elf", elfRace, elfRace.Id);
        var woodElf = CreateTestSubrace("Wood Elf", elfRace, elfRace.Id);
        elfRace.SubRaces.Add(highElf);
        elfRace.SubRaces.Add(woodElf);

        var trait1 = CreateTestTrait("Trait 1", "Desc 1", elfRace, elfRace.Id);
        var trait2 = CreateTestTrait("Trait 2", "Desc 2", elfRace, elfRace.Id);
        elfRace.Traits.Add(trait1);
        elfRace.Traits.Add(trait2);

        // Act
        await using var context = new AppDbContext(options);

        context.Races.Add(elfRace);
        context.Races.Add(dwarfRace);
        await context.SaveChangesAsync();

        // Assert
        var repo = new RaceRepository(context);
        var retrievedElf = await repo.GetWithAllDataAsync(elfRace.Id);
        var retrievedDwarf = await repo.GetWithAllDataAsync(dwarfRace.Id);

        Assert.NotNull(retrievedElf);
        Assert.Equal("Elf", retrievedElf!.Name);

        Assert.NotNull(retrievedElf.Traits);
        Assert.Equal(2, retrievedElf.Traits.Count);
        Assert.Contains(retrievedElf.Traits, t => t.Name == "Trait 1");
        Assert.Contains(retrievedElf.Traits, t => t.Name == "Trait 2");

        Assert.NotNull(retrievedElf.SubRaces);
        Assert.Equal(2, retrievedElf.SubRaces.Count);
        Assert.Contains(retrievedElf.SubRaces, sr => sr.Name == "High Elf");
        Assert.Contains(retrievedElf.SubRaces, sr => sr.Name == "Wood Elf");

        foreach (var subrace in retrievedElf.SubRaces)
        {
            Assert.Equal(retrievedElf.Id, subrace.ParentRaceId);
            Assert.NotNull(subrace.ParentRace);
            Assert.Equal("Elf", subrace.ParentRace.Name);
        }

        Assert.NotNull(retrievedDwarf);
        Assert.Equal("Dwarf", retrievedDwarf!.Name);
        Assert.Empty(retrievedDwarf.Traits);
        Assert.Empty(retrievedDwarf.SubRaces);
    }
}