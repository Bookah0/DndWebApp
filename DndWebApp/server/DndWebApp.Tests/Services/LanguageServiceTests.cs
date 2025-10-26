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

public class LanguageServiceTests
{
    private LanguageDto CreateLanguageDto(string name, string family, string script)
    {
        return new() { Name = name, Family = family, Script = script, IsHomebrew = false };
    }

    private DbContextOptions<AppDbContext> GetInMemoryOptions(string dbName)
    {
        return new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
            .Options;
    }

    [Fact]
    public async Task AddAndRetrieveLanguages_WorksCorrectly()
    {
        var options = GetInMemoryOptions("Language_AddRetrieveDB");
        var auranDto = CreateLanguageDto("Auran", "Primordial", "Dwarvish");
        var dethekDto = CreateLanguageDto("Dethek", "Dwarvish", "Dwarvish");

        using var context = new AppDbContext(options);
        var repo = new EfRepository<Language>(context);
        var service = new LanguageService(repo, context);

        var created1 = await service.CreateAsync(auranDto);
        var created2 = await service.CreateAsync(dethekDto);

        var auran = await service.GetByIdAsync(created1.Id);
        var dethek = await service.GetByIdAsync(created2.Id);

        Assert.NotNull(auran);
        Assert.NotNull(dethek);
        Assert.Equal("Auran", auran.Name);
        Assert.Equal("Primordial", auran.Family);
        Assert.Equal("Dwarvish", dethek.Script);

        var allLanguages = await service.GetAllAsync();
        Assert.Contains(allLanguages, a => a.Name == "Dethek");
        Assert.Contains(allLanguages, a => a.Family == "Dwarvish");
        Assert.Contains(allLanguages, a => a.Script == "Dwarvish");
    }

    [Fact]
    public async Task AddAndRetrieveLanguages_BadInputData_ShouldNotCreate()
    {
        var options = GetInMemoryOptions("Language_AddRetrieveDB");
        var noName = CreateLanguageDto("", "Primordial", "Dwarvish");
        var whitespaceFamily = CreateLanguageDto("Auran", "   ", "Dwarvish");
        var nullScript = CreateLanguageDto("Auran", "Primordial", null!);

        using var context = new AppDbContext(options);
        var repo = new EfRepository<Language>(context);
        var service = new LanguageService(repo, context);

        await Assert.ThrowsAsync<ArgumentException>(() => service.CreateAsync(noName));
        await Assert.ThrowsAsync<ArgumentException>(() => service.CreateAsync(whitespaceFamily));
        await Assert.ThrowsAsync<ArgumentException>(() => service.CreateAsync(nullScript));
    }

    [Fact]
    public async Task DeleteLanguage_WorksCorrectly()
    {
        var options = GetInMemoryOptions("Language_DeleteDB");
        var auranDto = CreateLanguageDto("Auran", "Primordial", "Dwarvish");

        using var context = new AppDbContext(options);
        var repo = new EfRepository<Language>(context);
        var service = new LanguageService(repo, context);

        var created = await service.CreateAsync(auranDto);
        var auran = await service.GetByIdAsync(created.Id);

        await service.DeleteAsync(auran.Id);

        await Assert.ThrowsAsync<NullReferenceException>(() => service.GetByIdAsync(auran.Id));
    }

    [Fact]
    public async Task UpdateLanguage_WorksCorrectly()
    {
        var options = GetInMemoryOptions("Language_UpdateDB");
        var auranDto = CreateLanguageDto("Auran", "Primordial", "Dwarvish");

        using var context = new AppDbContext(options);
        var repo = new EfRepository<Language>(context);
        var service = new LanguageService(repo, context);

        var created = await service.CreateAsync(auranDto);

        auranDto.Family = "Elvish";
        auranDto.Script = "Espruar";
        auranDto.Id = created.Id;
        await service.UpdateAsync(auranDto);

        var auran = await service.GetByIdAsync(created.Id);

        Assert.NotNull(auran);
        Assert.Equal("Auran", auran.Name);
        Assert.Equal("Elvish", auran.Family);
        Assert.Equal("Espruar", auran.Script);
    }

    [Fact]
    public async Task UpdateLanguages_BadInputData_ShouldNotUpdate()
    {
        var options = GetInMemoryOptions("Language_BadUpdateDB");
        var auranDto = CreateLanguageDto("Auran", "Primordial", "Dwarvish");
        var noName = CreateLanguageDto("", "Primordial", "Dwarvish");
        var whitespaceFamily = CreateLanguageDto("Auran", "   ", "Dwarvish");
        var nullScript = CreateLanguageDto("Auran", "Primordial", null!);

        using var context = new AppDbContext(options);
        var repo = new EfRepository<Language>(context);
        var service = new LanguageService(repo, context);

        var created = await service.CreateAsync(auranDto);
        var auran = await service.GetByIdAsync(created.Id);

        await Assert.ThrowsAsync<ArgumentException>(() => service.UpdateAsync(noName));
        await Assert.ThrowsAsync<ArgumentException>(() => service.UpdateAsync(whitespaceFamily));
        await Assert.ThrowsAsync<ArgumentException>(() => service.UpdateAsync(nullScript));

        Assert.Equal("Auran", auran.Name);
        Assert.Equal("Primordial", auran.Family);
        Assert.Equal("Dwarvish", auran.Script);
    }
}
