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
        private Race CreateRace(string name = "Elf")
        {
            return new Race
            {
                Name = name,
                Speed = 30,
                Traits = []
            };
        }

        private Subrace CreateSubrace(Race parentRace = null!, string name = "High Elf", int parentRaceId = 1)
        {
            return new Subrace
            {
                Name = name,
                ParentRaceId = parentRaceId,
                ParentRace = parentRace,
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
        public async Task AddAndRetrieveSubraces_WorksCorrectly()
        {
            // Arrange
            var options = GetInMemoryOptions("Subrace_AddRetrieveDB");

            var parentRace = CreateRace();
            var highElf = CreateSubrace(parentRace, name: "High Elf");
            var woodElf = CreateSubrace(parentRace, name: "Wood Elf");

            int parentRaceId = -1;
            int highElfId = -1;
            int woodElfId = -1;

            // Act
            await using (var context = new AppDbContext(options))
            {
                context.Races.Add(parentRace);
                await context.SaveChangesAsync();

                var repo = new SubraceRepository(context);
                parentRaceId = parentRace.Id;
                highElf.ParentRaceId = parentRace.Id;
                woodElf.ParentRaceId = parentRace.Id;

                await repo.CreateAsync(highElf);
                await repo.CreateAsync(woodElf);
                await context.SaveChangesAsync();

                highElfId = highElf.Id;
                woodElfId = woodElf.Id;
            }

            // Assert
            await using (var context = new AppDbContext(options))
            {
                var repo = new SubraceRepository(context);

                var savedHighElf = await repo.GetByIdAsync(highElfId);
                var savedWoodElf = await repo.GetByIdAsync(woodElfId);

                Assert.NotNull(savedHighElf);
                Assert.Equal("High Elf", savedHighElf!.Name);
                Assert.Equal(parentRaceId, savedHighElf.ParentRaceId);
                //Assert.NotNull(savedHighElf.ParentRace);
                //Assert.Equal("Elf", savedHighElf.ParentRace!.Name);

                Assert.NotNull(savedWoodElf);
                Assert.Equal("Wood Elf", savedWoodElf!.Name);
                Assert.Equal(parentRaceId, savedWoodElf.ParentRaceId);

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

            var subrace = CreateSubrace(name: "High Elf");

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

            var elfRace = CreateRace(name: "Elf");
            var subrace = new Subrace { Name = "High Elf", Speed = 30, ParentRaceId = elfRace.Id, ParentRace = null! };

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

            var highElf = CreateSubrace(name: "High Elf");
            var woodElf = CreateSubrace(name: "Wood Elf");
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
                Assert.Equal(1, primitiveWoodElf.ParentRaceId);
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

            int highElfId;
            int woodElfId;
            int elfRaceId;

            await using (var context = new AppDbContext(options))
            {
                var elfRace = new Race { Name = "Elf", Speed = 30 };

                context.Races.Add(elfRace);
                await context.SaveChangesAsync();
                elfRaceId = elfRace.Id;
            }

            await using (var context = new AppDbContext(options))
            {
                var elfRace = await context.Races.FindAsync(elfRaceId);
                var highElf = new Subrace { Name = "High Elf", Speed = 30, ParentRace = elfRace!, ParentRaceId = elfRace!.Id };
                var woodElf = new Subrace { Name = "Wood Elf", Speed = 30, ParentRace = elfRace, ParentRaceId = elfRace!.Id };

                context.SubRaces.AddRange(highElf, woodElf);
                await context.SaveChangesAsync();

                highElfId = highElf.Id;
                woodElfId = woodElf.Id;
            }

            await using (var context = new AppDbContext(options))
            {
                var highElf = await context.SubRaces.FindAsync(highElfId);
                var woodElf = await context.SubRaces.FindAsync(woodElfId);

                var trait1 = new Trait { Name = "Trait 1", Description = "Description 1", FromRace = highElf!, RaceId = highElf!.Id };
                var trait2 = new Trait { Name = "Trait 2", Description = "Description 2", FromRace = highElf!, RaceId = highElf!.Id };
                var trait3 = new Trait { Name = "Trait 3", Description = "Description 3", FromRace = woodElf!, RaceId = woodElf!.Id };

                context.Traits.AddRange(trait1, trait2, trait3);
                await context.SaveChangesAsync();
            }

            // Assert
            await using (var context = new AppDbContext(options))
            {
                var repo = new SubraceRepository(context);

                var fetchedHighElf = await repo.GetWithAllDataAsync(highElfId);
                var fetchedWoodElf = await repo.GetWithAllDataAsync(woodElfId);

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
}