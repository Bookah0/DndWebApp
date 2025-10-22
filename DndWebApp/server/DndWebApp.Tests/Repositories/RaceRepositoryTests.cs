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
    private Race CreateRace(int id = 1, string name = "Elf")
    {
        return new Race
        {
            Id = id,
            Name = name,
            Speed = 30,
            Traits = []
        };
    }

    private DbContextOptions<AppDbContext> GetInMemoryOptions(string dbName)
    {
        return new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .Options;
    }

    [Fact]
    public async Task AddAndRetrieveRace_WorksCorrectly()
    {
        // Arrange
        var options = GetInMemoryOptions("Race_AddRetrieveDB");

        var elfRace = CreateRace();
        var dwarfRace = CreateRace(id: 2, name: "Dwarf");

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

        var race = CreateRace();

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

        var race = CreateRace();

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
    public async Task RetrieveSubracesAsPrimitiveDtos_ShouldHaveCorrectFieldValues()
    {
        // Arrange
        var options = GetInMemoryOptions("Subrace_AddRetrieveDB");

        var elfRace = CreateRace(id: 1, name: "Elf");
        var dwarfRace = CreateRace(id: 2, name: "Dwarf");

        elfRace.RaceDescription.GeneralDescription = "Elf description";
        dwarfRace.RaceDescription.GeneralDescription = "Dwarf description";

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

            var primitiveElf = await repo.GetPrimitiveDataAsync(1);
            var primitiveDwarf = await repo.GetPrimitiveDataAsync(2);
            var primitiveHuman = await repo.GetPrimitiveDataAsync(3);

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
        var options = GetInMemoryOptions("Subrace_AddRetrieveDB");

        var elfRace = CreateRace(id: 1, name: "Elf");
        var dwarfRace = CreateRace(id: 2, name: "Dwarf");

        var highElf = new Subrace { Id = 1, Name = "High Elf", Speed = 30, ParentRace = elfRace, ParentRaceId = 1 };
        var woodElf = new Subrace { Id = 2, Name = "Wood Elf", Speed = 30, ParentRace = elfRace, ParentRaceId = 1 };

        elfRace.Traits.Add(new Trait { Id = 1, Name = "Trait 1", Description = "Description 1", FromRace = elfRace, RaceId = 1 });
        elfRace.Traits.Add(new Trait { Id = 2, Name = "Trait 2", Description = "Description 2", FromRace = elfRace, RaceId = 1 });

        elfRace.SubRaces.Add(highElf);
        elfRace.SubRaces.Add(woodElf);

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

            var retrievedElf = await repo.GetWithAllDataAsync(1);
            var retrievedDwarf = await repo.GetWithAllDataAsync(2);

            // Elf
            Assert.NotNull(retrievedElf);
            Assert.Equal("Elf", retrievedElf!.Name);

            // Elf traits
            Assert.NotNull(retrievedElf.Traits);
            Assert.Equal(2, retrievedElf.Traits.Count);
            Assert.Contains(retrievedElf.Traits, t => t.Name == "Trait 1");
            Assert.Contains(retrievedElf.Traits, t => t.Name == "Trait 2");

            // Elf subraces
            Assert.NotNull(retrievedElf.SubRaces);
            Assert.Equal(2, retrievedElf.SubRaces.Count);
            Assert.Contains(retrievedElf.SubRaces, sr => sr.Name == "High Elf");
            Assert.Contains(retrievedElf.SubRaces, sr => sr.Name == "Wood Elf");

            // Parent race is correct in subraces
            foreach (var subrace in retrievedElf.SubRaces)
            {
                Assert.Equal(retrievedElf.Id, subrace.ParentRaceId);
                Assert.NotNull(subrace.ParentRace);
                Assert.Equal("Elf", subrace.ParentRace.Name);
            }

            // Dwarf subraces and traits should be empty
            Assert.NotNull(retrievedDwarf);
            Assert.Equal("Dwarf", retrievedDwarf!.Name);
            Assert.Empty(retrievedDwarf.Traits);
            Assert.Empty(retrievedDwarf.SubRaces);
        }
    }
}