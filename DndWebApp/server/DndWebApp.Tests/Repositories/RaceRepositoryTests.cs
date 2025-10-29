using static DndWebApp.Tests.Repositories.TestObjectFactory;
using DndWebApp.Api.Data;
using DndWebApp.Api.Repositories.Species;

namespace DndWebApp.Tests.Repositories;

public class RaceRepositoryTests
{
    [Fact]
    public async Task AddAndRetrieveRace_WorksCorrectly()
    {
        var options = GetInMemoryOptions("Race_AddRetrieveDB");
        await using var context = new AppDbContext(options);
        var repo = new RaceRepository(context);

        // Arrange
        var elfRace = CreateTestRace("Elf");
        var dwarfRace = CreateTestRace("Dwarf");

        // Act
        await repo.CreateAsync(elfRace);
        await repo.CreateAsync(dwarfRace);

        var savedElf = await repo.GetByIdAsync(1);

        // Assert
        Assert.NotNull(savedElf);
        Assert.Equal("Elf", savedElf!.Name);

        var allRaces = await repo.GetAllAsync();
        Assert.Equal(2, allRaces.Count);
        Assert.Contains(allRaces, r => r.Name == "Elf");
        Assert.Contains(allRaces, r => r.Name == "Dwarf");
    }

    [Fact]
    public async Task UpdateRace_WorksCorrectly()
    {
        var options = GetInMemoryOptions("Race_UpdateDB");
        await using var context = new AppDbContext(options);
        var repo = new RaceRepository(context);

        // Arrange
        var race = CreateTestRace("Elf");
        await repo.CreateAsync(race);

        // Act
        race.Name = "Updated Elf";
        await repo.UpdateAsync(race);
        var updated = await repo.GetByIdAsync(1);

        // Assert
        Assert.Equal("Updated Elf", updated!.Name);

    }

    [Fact]
    public async Task DeleteRace_WorksCorrectly()
    {
        var options = GetInMemoryOptions("Race_DeleteDB");
        await using var context = new AppDbContext(options);
        var repo = new RaceRepository(context);

        // Arrange
        var race = CreateTestRace("Elf");
        await repo.CreateAsync(race);

        // Act
        await repo.DeleteAsync(race);
        var deleted = await repo.GetByIdAsync(race.Id);

        // Assert
        Assert.Null(deleted);
    }

    [Fact]
    public async Task RetrieveRacesAsPrimitiveDtos_ShouldHaveCorrectFieldValues()
    {
        var options = GetInMemoryOptions("PrimitiveRace_AddRetrieveDB");
        await using var context = new AppDbContext(options);
        var repo = new RaceRepository(context);

        // Arrange
        var elfRace = CreateTestRace("Elf");
        var dwarfRace = CreateTestRace("Dwarf");

        elfRace.RaceDescription.GeneralDescription = "Elf description";
        dwarfRace.RaceDescription.GeneralDescription = "Dwarf description";

        // Act
        var createdElf = await repo.CreateAsync(elfRace);
        var createdDwarf = await repo.CreateAsync(dwarfRace);

        var primitiveElf = await repo.GetRaceDtoAsync(createdElf.Id);
        var primitiveDwarf = await repo.GetRaceDtoAsync(createdDwarf.Id);

        // Assert
        Assert.NotNull(primitiveElf);
        Assert.Equal("Elf", primitiveElf!.Name);
        Assert.NotNull(primitiveElf.GeneralDescription);
        Assert.Equal("Elf description", primitiveElf.GeneralDescription);

        Assert.NotNull(primitiveDwarf);
        Assert.Equal("Dwarf", primitiveDwarf!.Name);
        Assert.NotNull(primitiveDwarf.GeneralDescription);
        Assert.Equal("Dwarf description", primitiveDwarf.GeneralDescription);

        var allRacesAsPrimitive = await repo.GetAllRaceDtosAsync();
        Assert.Equal(2, allRacesAsPrimitive.Count);
        Assert.Contains(allRacesAsPrimitive, r => r.Name == "Dwarf");
        Assert.Contains(allRacesAsPrimitive, r => r.Name == "Elf");
    }

    [Fact]
    public async Task RetrieveWithTraitAndSubraces_ShouldHaveCorrectTrait()
    {
        var options = GetInMemoryOptions("GetAllWithCollections_AddRetrieveDB");
        await using var context = new AppDbContext(options);
        var repo = new RaceRepository(context);

        // Arrange
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
        context.Races.Add(elfRace);
        context.Races.Add(dwarfRace);
        await context.SaveChangesAsync();

        var retrievedElf = await repo.GetWithAllDataAsync(elfRace.Id);
        var retrievedDwarf = await repo.GetWithAllDataAsync(dwarfRace.Id);

        // Assert
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