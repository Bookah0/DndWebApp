using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DndWebApp.Api.Data;
using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace YourNamespace.Tests
{
    public class SubraceRepositoryTests
    {
        // Helper: create a Race in memory
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

        // Helper: create a Subrace in memory
        private Subrace CreateSubrace(Race parentRace, int id = 1, string name = "High Elf")
        {
            return new Subrace
            {
                Id = id,
                Name = name,
                ParentRaceId = parentRace.Id,
                ParentRace = parentRace,
                Speed = parentRace.Speed,
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
        public async Task AddAndRetrieveSubraces_WorksCorrectly()
        {
            // Arrange
            var options = GetInMemoryOptions("Subrace_AddRetrieveDB");

            var parentRace = CreateRace();
            var highElf = CreateSubrace(parentRace, id: 1, name: "High Elf");
            var woodElf = CreateSubrace(parentRace, id: 2, name: "Wood Elf");

            // Act
            await using (var context = new AppDbContext(options))
            {
                context.Races.Add(parentRace);
                await context.SaveChangesAsync();

                var repo = new SubraceRepository(context);
                await repo.CreateAsync(highElf);
                await repo.CreateAsync(woodElf);
                await context.SaveChangesAsync();
            }

            // Assert
            await using (var context = new AppDbContext(options))
            {
                var repo = new SubraceRepository(context);

                var savedHighElf = await repo.GetByIdAsync(1);
                var savedWoodElf = await repo.GetByIdAsync(2);

                Assert.NotNull(savedHighElf);
                Assert.Equal("High Elf", savedHighElf!.Name);
                Assert.Equal(parentRace.Id, savedHighElf.ParentRaceId);
                Assert.NotNull(savedHighElf.ParentRace);
                Assert.Equal("Elf", savedHighElf.ParentRace!.Name);

                Assert.NotNull(savedWoodElf);
                Assert.Equal("Wood Elf", savedWoodElf!.Name);
                Assert.Equal(parentRace.Id, savedWoodElf.ParentRaceId);

                var allSubraces = await repo.GetAllAsync();
                Assert.Equal(2, allSubraces.Count);
            }
        }

        [Fact]
        public async Task GetAllSubraces_ReturnsAllSubraces()
        {
            // Arrange
            var options = GetInMemoryOptions("Subrace_GetAllDB");

            var parentRace = CreateRace();
            var highElf = CreateSubrace(parentRace, id: 1, name: "High Elf");
            var woodElf = CreateSubrace(parentRace, id: 2, name: "Wood Elf");

            await using (var context = new AppDbContext(options))
            {
                context.Races.Add(parentRace);
                await context.SaveChangesAsync();

                var repo = new SubraceRepository(context);
                await repo.CreateAsync(highElf);
                await repo.CreateAsync(woodElf);
            }

            // Act & Assert
            await using (var context = new AppDbContext(options))
            {
                var repo = new SubraceRepository(context);
                var allSubraces = await repo.GetAllAsync();

                Assert.Equal(2, allSubraces.Count);
                Assert.Contains(allSubraces, s => s.Name == "High Elf");
                Assert.Contains(allSubraces, s => s.Name == "Wood Elf");
            }
        }

        [Fact]
        public async Task UpdateSubrace_WorksCorrectly()
        {
            // Arrange
            var options = GetInMemoryOptions("Subrace_UpdateDB");

            var parentRace = CreateRace();
            var subrace = CreateSubrace(parentRace, id: 1, name: "High Elf");

            await using (var context = new AppDbContext(options))
            {
                context.Races.Add(parentRace);
                await context.SaveChangesAsync();

                var repo = new SubraceRepository(context);
                await repo.CreateAsync(subrace);
            }

            // Act
            await using (var context = new AppDbContext(options))
            {
                var repo = new SubraceRepository(context);
                subrace.Name = "Updated Elf";
                await repo.UpdateAsync(subrace);
            }

            // Assert
            await using (var context = new AppDbContext(options))
            {
                var repo = new SubraceRepository(context);
                var updated = await repo.GetByIdAsync(1);

                Assert.Equal("Updated Elf", updated!.Name);
            }
        }

        [Fact]
        public async Task DeleteSubrace_WorksCorrectly()
        {
            // Arrange
            var options = GetInMemoryOptions("Subrace_DeleteDB");

            var parentRace = CreateRace();
            var subrace = CreateSubrace(parentRace, id: 1, name: "High Elf");
            
            await using (var context = new AppDbContext(options))
            {
                context.Races.Add(parentRace);
                await context.SaveChangesAsync();

                var repo = new SubraceRepository(context);
                await repo.CreateAsync(subrace);
            }

            // Act
            await using (var context = new AppDbContext(options))
            {
                var repo = new SubraceRepository(context);
                await repo.DeleteAsync(subrace);
            }

            // Assert
            await using (var context = new AppDbContext(options))
            {
                var repo = new SubraceRepository(context);
                var deleted = await repo.GetByIdAsync(subrace.Id);

                Assert.Null(deleted);
            }
        }
    }
}