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
        private Race CreateTestRace(string name) => new() { Name = name, Speed = 30 };

        private Subrace CreateTestSubrace(string name, Race parentRace, int parentRaceId)
        {
            return new() { Name = name, Speed = 30, ParentRace = parentRace, ParentRaceId = parentRaceId };
        }

        private Trait CreateTestTrait(string name, string description, Species fromRace, int raceId)
        {
            return new() { Name = name, Description = description, FromRace = fromRace, RaceId = raceId };
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
            var elfRace = CreateTestRace("Elf");
            var highElf = CreateTestSubrace("High Elf", elfRace, elfRace.Id);
            var woodElf = CreateTestSubrace("Wood Elf", elfRace, elfRace.Id);
            elfRace.SubRaces.Add(highElf);
            elfRace.SubRaces.Add(woodElf);

            // Act
            await using var context = new AppDbContext(options);
            context.Races.Add(elfRace);
            await context.SaveChangesAsync();

            // Assert
            var repo = new SubraceRepository(context);

            var savedHighElf = await repo.GetByIdAsync(highElf.Id);
            var savedWoodElf = await repo.GetByIdAsync(woodElf.Id);

            Assert.NotNull(savedHighElf);
            Assert.Equal("High Elf", savedHighElf!.Name);
            Assert.Equal(elfRace.Id, savedHighElf.ParentRaceId);

            Assert.NotNull(savedWoodElf);
            Assert.Equal("Wood Elf", savedWoodElf!.Name);
            Assert.Equal(elfRace.Id, savedWoodElf.ParentRaceId);

            var allSubraces = await repo.GetAllAsync();
            Assert.Equal(2, allSubraces.Count);
            Assert.Contains(allSubraces, s => s.Name == "High Elf");
            Assert.Contains(allSubraces, s => s.Name == "Wood Elf");
        }

        [Fact]
        public async Task UpdateSubrace_WorksCorrectly()
        {
            // Arrange
            var options = GetInMemoryOptions("Subrace_UpdateDB");
            var subrace = CreateTestSubrace("High Elf", null!, -1);

            await using (var context = new AppDbContext(options))
            {
                var repo = new SubraceRepository(context);
                await repo.CreateAsync(subrace);
                await context.SaveChangesAsync();
            }

            // Act
            await using (var context = new AppDbContext(options))
            {
                var repo = new SubraceRepository(context);
                var savedSubrace = await repo.GetByIdAsync(1);
                savedSubrace!.Name = "Updated Elf";

                await repo.UpdateAsync(savedSubrace);
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
            var subrace = CreateTestSubrace("High Elf", null!, -1);

            await using (var context = new AppDbContext(options))
            {
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
            var options = GetInMemoryOptions("PrimitiveSubrace_AddRetrieveDB");

            var highElf = CreateTestSubrace("High Elf", null!, 1);
            var woodElf = CreateTestSubrace("Wood Elf", null!, 2);
            highElf.RaceDescription.GeneralDescription = "High elf description";
            woodElf.RaceDescription.GeneralDescription = "Wood elf description";

            // Act
            await using (var context = new AppDbContext(options))
            {
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
                Assert.Equal(1, primitiveHighElf.ParentRaceId);
                Assert.NotNull(primitiveHighElf.GeneralDescription);
                Assert.Equal("High elf description", primitiveHighElf.GeneralDescription);

                Assert.NotNull(primitiveWoodElf);
                Assert.Equal("Wood Elf", primitiveWoodElf!.Name);
                Assert.Equal(2, primitiveWoodElf.ParentRaceId);
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
            var options = GetInMemoryOptions("Subrace_RetrieveWithTraitsDB");
            await using var context = new AppDbContext(options);

            var elfRace = CreateTestRace("Elf");
            context.Races.Add(elfRace);

            var highElf = CreateTestSubrace("High Elf", elfRace, elfRace.Id);
            var woodElf = CreateTestSubrace("Wood Elf", elfRace, elfRace.Id);
            context.SubRaces.AddRange(highElf, woodElf);

            var trait1 = CreateTestTrait("Trait 1", "Desc 1", highElf, highElf.Id);
            var trait2 = CreateTestTrait("Trait 2", "Desc 2", highElf, highElf.Id);
            var trait3 = CreateTestTrait("Trait 2", "Desc 2", woodElf, woodElf.Id);
            context.Traits.AddRange(trait1, trait2, trait3);

            await context.SaveChangesAsync();

            // Assert
            var repo = new SubraceRepository(context);
            var fetchedHighElf = await repo.GetWithAllDataAsync(highElf.Id);
            var fetchedWoodElf = await repo.GetWithAllDataAsync(woodElf.Id);

            Assert.NotNull(fetchedHighElf);
            Assert.Equal("High Elf", fetchedHighElf!.Name);
            Assert.NotNull(fetchedHighElf.Traits);
            Assert.Equal(2, fetchedHighElf.Traits.Count);

            Assert.NotNull(fetchedWoodElf);
            Assert.Equal("Wood Elf", fetchedWoodElf!.Name);
            Assert.NotNull(fetchedWoodElf.Traits);
            Assert.Single(fetchedWoodElf.Traits);

            var allSubraces = await repo.GetAllWithAllDataAsync();
            Assert.NotNull(allSubraces);
            Assert.NotEmpty(allSubraces);
        }
    }
}