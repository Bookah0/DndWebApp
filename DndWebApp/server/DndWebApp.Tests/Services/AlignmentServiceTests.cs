using System.ComponentModel;
using DndWebApp.Api.Data;
using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs;
using DndWebApp.Api.Models.World;
using DndWebApp.Api.Repositories;
using DndWebApp.Api.Repositories.Abilities;
using DndWebApp.Api.Services;
using Microsoft.EntityFrameworkCore;
using Xunit.Sdk;

namespace DndWebApp.Tests.Services;

public class AbilityServiceTests
{
    private AlignmentDto CreateAlignmentDto(string name, string abbreviation, string description)
    {
        return new() { Name = name, Description = description, Abbreviation = abbreviation };
    }

    private DbContextOptions<AppDbContext> GetInMemoryOptions(string dbName)
    {
        return new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
            .Options;
    }

    [Fact]
    public async Task AddAndRetrieveAlignments_WorksCorrectly()
    {
        var options = GetInMemoryOptions("Alignment_AddRetrieveDB");
        var lawfulGoodDto = CreateAlignmentDto("Lawful good", "LG", "A lawful good character typically acts with compassion and always with honor and a sense of duty.");
        var chaoticEvilDto = CreateAlignmentDto("Chaotic evil", "CE", "A chaotic evil character tends to have no respect for rules.");

        using var context = new AppDbContext(options);
        var repo = new EfRepository<Alignment>(context);
        var service = new AlignmentService(repo, context);

        var created1 = await service.CreateAsync(lawfulGoodDto);
        var created2 = await service.CreateAsync(chaoticEvilDto);

        var lawfulGood = await service.GetByIdAsync(created1.Id);
        var chaoticEvil = await service.GetByIdAsync(created2.Id);

        Assert.NotNull(lawfulGood);
        Assert.NotNull(chaoticEvil);
        Assert.Equal("Lawful good", lawfulGood.Name);
        Assert.Equal("CE", chaoticEvil.Abbreviation);
        Assert.Equal("A lawful good character typically acts with compassion and always with honor and a sense of duty.", lawfulGood.Description);

        var allAlignments = await service.GetAllAsync();
        Assert.Contains(allAlignments, a => a.Name == "Chaotic evil");
        Assert.Contains(allAlignments, a => a.Abbreviation == "LG");
    }

    [Fact]
    public async Task AddAndRetrieveAlignments_BadInputData_ShouldNotCreate()
    {
        var options = GetInMemoryOptions("Alignment_AddRetrieveDB");
        var noName = CreateAlignmentDto("", "LG", "A lawful good character typically acts with compassion and always with honor and a sense of duty.");
        var whitespaceAberration = CreateAlignmentDto("Lawful good", "   ", "A lawful good character typically acts with compassion and always with honor and a sense of duty.");
        var nullDescription = CreateAlignmentDto("Lawful good", "LG", null!);

        using var context = new AppDbContext(options);
        var repo = new EfRepository<Alignment>(context);
        var service = new AlignmentService(repo, context);

        await Assert.ThrowsAsync<ArgumentException>(() => service.CreateAsync(noName));
        await Assert.ThrowsAsync<ArgumentException>(() => service.CreateAsync(whitespaceAberration));
        await Assert.ThrowsAsync<ArgumentException>(() => service.CreateAsync(nullDescription));
    }

    [Fact]
    public async Task RemoveAlignment_WorksCorrectly()
    {
        var options = GetInMemoryOptions("Alignment_UpdateDB");
        var chaoticEvilDto = CreateAlignmentDto("Chaotic evil", "CE", "A chaotic evil character tends to have no respect for rules.");

        using var context = new AppDbContext(options);
        var repo = new EfRepository<Alignment>(context);
        var service = new AlignmentService(repo, context);

        var created = await service.CreateAsync(chaoticEvilDto);
        var chaoticEvil = await service.GetByIdAsync(created.Id);

        await service.DeleteAsync(created.Id);

        await Assert.ThrowsAsync<NullReferenceException>(() => service.GetByIdAsync(created.Id));
    }

    [Fact]
    public async Task UpdateAlignment_WorksCorrectly()
    {
        var options = GetInMemoryOptions("Alignment_UpdateDB");
        var chaoticEvilDto = CreateAlignmentDto("Chaotic evil", "CE", "A chaotic evil character tends to have no respect for rules.");

        using var context = new AppDbContext(options);
        var repo = new EfRepository<Alignment>(context);
        var service = new AlignmentService(repo, context);

        var created = await service.CreateAsync(chaoticEvilDto);

        chaoticEvilDto.Name = "Chaotic Kindness.";
        chaoticEvilDto.Description = "A chaotic kind character tends to promote kindness, mercy, and benevolence.";
        chaoticEvilDto.Id = created.Id;
        await service.UpdateAsync(chaoticEvilDto);

        var chaoticKindness = await service.GetByIdAsync(created.Id);

        Assert.NotNull(chaoticKindness);
        Assert.Equal("Chaotic Kindness.", chaoticKindness.Name);
        Assert.Equal("CE", chaoticKindness.Abbreviation);
        Assert.Equal("A chaotic kind character tends to promote kindness, mercy, and benevolence.", chaoticKindness.Description);
    }

    [Fact]
    public async Task UpdateAlignments_BadInputData_ShouldNotUpdate()
    {
        var options = GetInMemoryOptions("Alignment_BadUpdateDB");
        var chaoticEvilDto = CreateAlignmentDto("Chaotic evil", "CE", "A chaotic evil character tends to have no respect for rules.");
        var noName = CreateAlignmentDto("", "LG", "A lawful good character typically acts with compassion and always with honor and a sense of duty.");
        var whitespaceAberration = CreateAlignmentDto("Lawful good", "   ", "A lawful good character typically acts with compassion and always with honor and a sense of duty.");
        var nullDescription = CreateAlignmentDto("Lawful good", "LG", null!);

        using var context = new AppDbContext(options);
        var repo = new EfRepository<Alignment>(context);
        var service = new AlignmentService(repo, context);

        var created = await service.CreateAsync(chaoticEvilDto);
        var chaoticEvil = await service.GetByIdAsync(created.Id);

        await Assert.ThrowsAsync<ArgumentException>(() => service.UpdateAsync(noName));
        await Assert.ThrowsAsync<ArgumentException>(() => service.UpdateAsync(whitespaceAberration));
        await Assert.ThrowsAsync<ArgumentException>(() => service.UpdateAsync(nullDescription));

        Assert.Equal("Chaotic evil", chaoticEvil.Name);
        Assert.Equal("CE", chaoticEvil.Abbreviation);
        Assert.Equal("A chaotic evil character tends to have no respect for rules.", chaoticEvil.Description);
    }
}
