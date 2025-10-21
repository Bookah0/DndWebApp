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

        var race = CreateRace();

        // Act
        await using (var context = new AppDbContext(options))
        {
            var repo = new RaceRepository(context);
            await repo.CreateAsync(race);
            await context.SaveChangesAsync();
        }

        // Assert
        await using (var context = new AppDbContext(options))
        {
            var repo = new RaceRepository(context);

            var savedElf = await repo.GetByIdAsync(1);

            Assert.NotNull(savedElf);
            Assert.Equal("Elf", savedElf!.Name);
        }
    }

    [Fact]
    public async Task GetAllRaces_ReturnsAllRaces()
    {
        // Arrange
        var options = GetInMemoryOptions("Race_GetAllDB");

        var elfRace = CreateRace();
        var dwarfRace = CreateRace(id: 2, name: "Dwarf");

        await using (var context = new AppDbContext(options))
        {
            var repo = new RaceRepository(context);
            await repo.CreateAsync(elfRace);
            await repo.CreateAsync(dwarfRace);
        }

        // Act & Assert
        await using (var context = new AppDbContext(options))
        {
            var repo = new RaceRepository(context);
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
        }

        // Act
        await using (var context = new AppDbContext(options))
        {
            var repo = new RaceRepository(context);
            race.Name = "Updated Elf";
            await repo.UpdateAsync(race);
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
        }

        // Act
        await using (var context = new AppDbContext(options))
        {
            var repo = new RaceRepository(context);
            await repo.DeleteAsync(race);
        }

        // Assert
        await using (var context = new AppDbContext(options))
        {
            var repo = new RaceRepository(context);
            var deleted = await repo.GetByIdAsync(race.Id);

            Assert.Null(deleted);
        }
    }
}