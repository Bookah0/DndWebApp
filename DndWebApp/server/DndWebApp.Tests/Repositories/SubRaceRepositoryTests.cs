using static DndWebApp.Tests.Repositories.TestObjectFactory;
using DndWebApp.Api.Data;
using DndWebApp.Api.Repositories.Species;

namespace DndWebApp.Tests.Repositories;

public class SubraceRepositoryTests
{
    [Fact]
    public async Task AddAndRetrieveSubraces_WorksCorrectly()
    {
        var options = GetInMemoryOptions("Subrace_AddRetrieveDB");
        await using var context = new AppDbContext(options);
        var repo = new SubraceRepository(context);

        // Arrange
        var elfRace = CreateTestRace("Elf");
        var highElf = CreateTestSubrace("High Elf", elfRace, elfRace.Id);
        var woodElf = CreateTestSubrace("Wood Elf", elfRace, elfRace.Id);

        // Act
        elfRace.SubRaces.Add(highElf);
        elfRace.SubRaces.Add(woodElf);
        context.Races.Add(elfRace);

        var savedHighElf = await repo.GetByIdAsync(highElf.Id);
        var savedWoodElf = await repo.GetByIdAsync(woodElf.Id);

        // Assert
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
        var options = GetInMemoryOptions("Subrace_DeleteDB");
        await using var context = new AppDbContext(options);
        var repo = new SubraceRepository(context);

        // Arrange
        var subrace = CreateTestSubrace("High Elf", null!, -1);
        await repo.CreateAsync(subrace);

        // Act
        var savedSubrace = await repo.GetByIdAsync(1);
        savedSubrace!.Name = "Updated Elf";
        await repo.UpdateAsync(savedSubrace);
        var updated = await repo.GetByIdAsync(1);

        // Assert
        Assert.Equal("Updated Elf", updated!.Name);
    }

    [Fact]
    public async Task DeleteSubrace_WorksCorrectly()
    {
        var options = GetInMemoryOptions("Subrace_DeleteDB");
        await using var context = new AppDbContext(options);
        var repo = new SubraceRepository(context);

        // Arrange
        var subrace = CreateTestSubrace("High Elf", null!, -1);
        await repo.CreateAsync(subrace);

        // Act
        await repo.DeleteAsync(subrace);
        var deleted = await repo.GetByIdAsync(subrace.Id);

        // Assert
        Assert.Null(deleted);
    }

    [Fact]
    public async Task RetrieveSubracesAsPrimitiveDtos_ShouldHaveCorrectFieldValues()
    {
        var options = GetInMemoryOptions("PrimitiveSubrace_AddRetrieveDB");
        await using var context = new AppDbContext(options);
        var repo = new SubraceRepository(context);

        // Arrange
        var highElf = CreateTestSubrace("High Elf", null!, 1);
        var woodElf = CreateTestSubrace("Wood Elf", null!, 2);
        highElf.RaceDescription.GeneralDescription = "High elf description";
        woodElf.RaceDescription.GeneralDescription = "Wood elf description";

        // Act
        await repo.CreateAsync(highElf);
        await repo.CreateAsync(woodElf);

        var primitiveHighElf = await repo.GetSubraceDtoAsync(1);
        var primitiveWoodElf = await repo.GetSubraceDtoAsync(2);
        var allRacesAsPrimitive = await repo.GetAllSubraceDtosAsync();

        // Assert
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

        Assert.Equal(2, allRacesAsPrimitive.Count);
        Assert.Contains(allRacesAsPrimitive, r => r.Name == "High Elf");
        Assert.Contains(allRacesAsPrimitive, r => r.Name == "Wood Elf");
    }

    [Fact]
    public async Task AddAndRetrieveWithTraits_ShouldHaveCorrectTraits()
    {
        var options = GetInMemoryOptions("Subrace_RetrieveWithTraitsDB");
        await using var context = new AppDbContext(options);
        var repo = new SubraceRepository(context);

        // Arrange
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

        // Act
        var fetchedHighElf = await repo.GetWithAllDataAsync(highElf.Id);
        var fetchedWoodElf = await repo.GetWithAllDataAsync(woodElf.Id);
        var allSubraces = await repo.GetAllWithAllDataAsync();

        // Assert
        Assert.NotNull(fetchedHighElf);
        Assert.Equal("High Elf", fetchedHighElf!.Name);
        Assert.NotNull(fetchedHighElf.Traits);
        Assert.Equal(2, fetchedHighElf.Traits.Count);

        Assert.NotNull(fetchedWoodElf);
        Assert.Equal("Wood Elf", fetchedWoodElf!.Name);
        Assert.NotNull(fetchedWoodElf.Traits);
        Assert.Single(fetchedWoodElf.Traits);

        Assert.NotNull(allSubraces);
        Assert.NotEmpty(allSubraces);
    }
}