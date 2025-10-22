using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DndWebApp.Api.Data;
using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs;
using DndWebApp.Api.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace YourNamespace.Tests
{
    public class SubraceRepositoryTests
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
                await context.SaveChangesAsync();
            }

            // Act
            await using (var context = new AppDbContext(options))
            {
                var repo = new SubraceRepository(context);
                subrace.Name = "Updated Elf";
                await repo.UpdateAsync(subrace);
                await context.SaveChangesAsync();
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
                await context.SaveChangesAsync();
            }

            // Act
            await using (var context = new AppDbContext(options))
            {
                var repo = new SubraceRepository(context);
                await repo.DeleteAsync(subrace);
                await context.SaveChangesAsync();
            }

            // Assert
            await using (var context = new AppDbContext(options))
            {
                var repo = new SubraceRepository(context);
                var deleted = await repo.GetByIdAsync(subrace.Id);

                Assert.Null(deleted);
            }
        }

        [Fact]
        public async Task RetrieveSubracesAsPrimitiveDtos_ShouldHaveCorrectFieldValues()
        {
            // Arrange
            var options = GetInMemoryOptions("Subrace_AddRetrieveDB");

            var parentRace = CreateRace();
            var highElf = CreateSubrace(parentRace, id: 1, name: "High Elf");
            var woodElf = CreateSubrace(parentRace, id: 2, name: "Wood Elf");
            parentRace.RaceDescription.GeneralDescription = "Parent description";
            highElf.RaceDescription.GeneralDescription = "High elf description";
            woodElf.RaceDescription.GeneralDescription = "Wood elf description";

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

                var primitiveHighElf = await repo.GetPrimitiveDataAsync(1);
                var primitiveWoodElf = await repo.GetPrimitiveDataAsync(2);

                Assert.NotNull(primitiveHighElf);
                Assert.Equal("High Elf", primitiveHighElf!.Name);
                Assert.Equal(parentRace.Id, primitiveHighElf.ParentRaceId);
                Assert.NotNull(primitiveHighElf.GeneralDescription);
                Assert.Equal("High elf description", primitiveHighElf.GeneralDescription);

                Assert.NotNull(primitiveWoodElf);
                Assert.Equal("Wood Elf", primitiveWoodElf!.Name);
                Assert.Equal(parentRace.Id, primitiveWoodElf.ParentRaceId);
                Assert.NotNull(primitiveWoodElf.GeneralDescription);
                Assert.Equal("Wood elf description", primitiveWoodElf.GeneralDescription);

                var allRacesAsPrimitive = await repo.GetAllPrimitiveDataAsync();
                Assert.Equal(2, allRacesAsPrimitive.Count);
                Assert.Contains(allRacesAsPrimitive, r => r.Name == "High Elf");
                Assert.Contains(allRacesAsPrimitive, r => r.Name == "Wood Elf");
            }
        }

        [Fact]
        public async Task AddAndRetrieveWithTraits_ShouldHaveCorrectTraits()
        {
            // Arrange
            var options = GetInMemoryOptions("Subrace_AddRetrieveDB");

            var parentRace = CreateRace();
            var highElf = CreateSubrace(parentRace, id: 1, name: "High Elf");
            var woodElf = CreateSubrace(parentRace, id: 2, name: "Wood Elf");

            var trait1 = new Trait { Id = 1, Name = "Trait 1", Description = "Description 1", FromRace = null, RaceId = 1 };
            var trait2 = new Trait { Id = 2, Name = "Trait 2", Description = "Description 2", FromRace = null, RaceId = 1 };
            var trait3 = new Trait { Id = 3, Name = "Trait 3", Description = "Description 3", FromRace = null, RaceId = 2 };

            highElf.Traits.Add(trait1);
            highElf.Traits.Add(trait2);
            woodElf.Traits.Add(trait3);

            // Act
            await using (var context = new AppDbContext(options))
            {
                var repo = new SubraceRepository(context);

                context.Traits.Add(trait1);
                context.Traits.Add(trait2);

                await repo.CreateAsync(highElf);
                await repo.CreateAsync(woodElf);

                await context.SaveChangesAsync();
            }

            // Assert
            await using (var context = new AppDbContext(options))
            {
                var repo = new SubraceRepository(context);

                var fetchedHighElf = await repo.GetWithAllDataAsync(1);
                var fetchedWoodElf = await repo.GetWithAllDataAsync(2);

                Assert.NotNull(fetchedHighElf);
                Assert.NotNull(fetchedHighElf.Traits);
                Assert.Equal(2, fetchedHighElf.Traits.Count);

                Assert.NotNull(fetchedWoodElf);
                Assert.NotNull(fetchedWoodElf.Traits);
                Assert.Single(fetchedWoodElf.Traits);

                var allSubraces = await repo.GetAllWithAllDataAsync();
                Assert.NotNull(allSubraces);
                Assert.NotEmpty(allSubraces);
                Assert.NotNull(allSubraces.First().Traits);
                Assert.NotEmpty(allSubraces.First().Traits);
            }
        }

    }
}