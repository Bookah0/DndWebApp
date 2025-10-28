using System.ComponentModel;
using System.Data.Common;
using DndWebApp.Api.Data;
using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs;
using DndWebApp.Api.Models.World;
using DndWebApp.Api.Repositories;
using DndWebApp.Api.Repositories.Abilities;
using DndWebApp.Api.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit.Sdk;

namespace DndWebApp.Tests.Services;

public class LanguageServiceTests
{
    internal static LanguageDto CreateTestLanguageDto(string name, string family, string script, int id = 1)
    {
        return new() { Id = id, Name = name, Family = family, Script = script, IsHomebrew = false };
    }

    internal static Language CreateTestLanguage(string name, string family, string script, int id = 1)
    {
        return new() { Id = id, Name = name, Family = family, Script = script, IsHomebrew = false };
    }

    [Fact]
    public async Task AddAndRetrieveLanguages_WorksCorrectly()
    {
        // Arrange
        var repo = new Mock<IRepository<Language>>();
        var service = new LanguageService(repo.Object);

        List<Language> languages = [];

        repo.Setup(r => r.CreateAsync(It.IsAny<Language>()))
            .ReturnsAsync((Language l) =>
            {
                l.Id = languages.Count+1;
                languages.Add(l);
                return l;
            });

        repo.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((int id) => languages
            .FirstOrDefault(l => l.Id == id));

        repo.Setup(r => r.GetAllAsync())
            .ReturnsAsync(() => [.. languages]);

        // Act
        var created1 = await service.CreateAsync(CreateTestLanguageDto("Auran", "Primordial", "Dwarvish"));
        var created2 = await service.CreateAsync(CreateTestLanguageDto("Dethek", "Dwarvish", "Dwarvish"));

        var auran = await service.GetByIdAsync(created1.Id);
        var dethek = await service.GetByIdAsync(created2.Id);
        var allLanguages = await service.GetAllAsync();

        // Assert
        Assert.NotNull(auran);
        Assert.NotNull(dethek);
        Assert.Equal("Auran", auran.Name);
        Assert.Equal("Primordial", auran.Family);
        Assert.Equal("Dwarvish", dethek.Script);

        Assert.Contains(allLanguages, l => l.Name == "Dethek");
        Assert.Contains(allLanguages, l => l.Family == "Dwarvish");
        Assert.Contains(allLanguages, l => l.Script == "Dwarvish");

        repo.Verify(r => r.CreateAsync(It.IsAny<Language>()), Times.Exactly(2));
    }

    [Fact]
    public async Task AddAndRetrieveLanguages_BadInputData_ShouldNotCreate()
    {
        // Arrange
        var repo = new Mock<IRepository<Language>>();
        var service = new LanguageService(repo.Object);

        var noName = CreateTestLanguageDto("", "Primordial", "Dwarvish");
        var whitespaceFamily = CreateTestLanguageDto("Auran", "   ", "Dwarvish");
        var nullScript = CreateTestLanguageDto("Auran", "Primordial", null!);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => service.CreateAsync(noName));
        await Assert.ThrowsAsync<ArgumentException>(() => service.CreateAsync(whitespaceFamily));
        await Assert.ThrowsAsync<ArgumentException>(() => service.CreateAsync(nullScript));
        repo.Verify(r => r.CreateAsync(It.IsAny<Language>()), Times.Exactly(0));
    }

    [Fact]
    public async Task DeleteLanguage_WorksCorrectly()
    {
        // Arrange
        var repo = new Mock<IRepository<Language>>();
        var service = new LanguageService(repo.Object);

        List<Language> languages = [CreateTestLanguage("Auran", "Primordial", "Dwarvish")];

        repo.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((int id) => languages
            .FirstOrDefault(l => l.Id == id));

        repo.Setup(r => r.DeleteAsync(It.IsAny<Language>()))
            .Callback((Language l) =>
            {
                languages.Remove(l);
            });

        // Act
        var id = languages.First().Id;
        await service.DeleteAsync(id);

        // Assert
        await Assert.ThrowsAsync<NullReferenceException>(() => service.DeleteAsync(id));
        repo.Verify(r => r.DeleteAsync(It.IsAny<Language>()), Times.Exactly(1));
    }

        [Fact]
    public async Task GetAndDeleteSkill_BadId_ShouldNotGetOrDelete()
    {
        // Arrange
        var repo = new Mock<IRepository<Language>>();
        var service = new LanguageService(repo.Object);

        List<Language> languages = [CreateTestLanguage("Auran", "Primordial", "Dwarvish")];

        repo.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((int id) => languages
            .FirstOrDefault(l => l.Id == id));

        // Act & Assert
        await Assert.ThrowsAsync<NullReferenceException>(() => service.GetByIdAsync(-1));
        await Assert.ThrowsAsync<NullReferenceException>(() => service.DeleteAsync(-1));

        repo.Verify(r => r.GetByIdAsync(It.IsAny<int>()), Times.Exactly(2));
        repo.Verify(r => r.DeleteAsync(It.IsAny<Language>()), Times.Exactly(0));
    }
    
    [Fact]
    public async Task UpdateLanguage_WorksCorrectly()
    {
        var repo = new Mock<IRepository<Language>>();
        var service = new LanguageService(repo.Object);
        
        List<Language> languages = [CreateTestLanguage("Auran", "Primordial", "Dwarvish")];
        var updateDto = CreateTestLanguageDto("Auran", "Elvish", "Espruar");

        repo.Setup(r => r.UpdateAsync(It.IsAny<Language>()))
            .Callback(() =>
            {
                var language = languages.First();
                language!.Name = updateDto.Name;
                language.Family = updateDto.Family;
                language.Script = updateDto.Script;
            });

        repo.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((int id) => languages
            .FirstOrDefault(l => l.Id == id));

        // Act
        await service.UpdateAsync(updateDto);
        var updated = await service.GetByIdAsync(1);

        // Assert
        Assert.NotNull(updated);
        Assert.Equal("Auran", updated.Name);
        Assert.Equal("Elvish", updated.Family);
        Assert.Equal("Espruar", updated.Script);
    }

    [Fact]
    public async Task UpdateLanguages_BadInputData_ShouldNotUpdate()
    {
        var repo = new Mock<IRepository<Language>>();
        var service = new LanguageService(repo.Object);

        List<Language> languages = [CreateTestLanguage("Auran", "Primordial", "Dwarvish")];
        var auranDto = CreateTestLanguageDto("Auran", "Primordial", "Dwarvish");
        var noName = CreateTestLanguageDto("", "Primordial", "Dwarvish");
        var whitespaceFamily = CreateTestLanguageDto("Auran", "   ", "Dwarvish");
        var nullScript = CreateTestLanguageDto("Auran", "Primordial", null!);

        repo.Setup(r => r.UpdateAsync(It.IsAny<Language>()))
            .Callback((Language l) =>
            {
                var language = languages.FirstOrDefault(lang => lang.Id == l.Id);
                language!.Name = l.Name;
                language.Family = l.Family;
                language.Script = l.Script;
            });

        repo.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((int id) => languages
            .FirstOrDefault(a => a.Id == id));

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => service.UpdateAsync(noName));
        await Assert.ThrowsAsync<ArgumentException>(() => service.UpdateAsync(whitespaceFamily));
        await Assert.ThrowsAsync<ArgumentException>(() => service.UpdateAsync(nullScript));

        var auran = await service.GetByIdAsync(1);
        Assert.Equal("Auran", auran.Name);
        Assert.Equal("Primordial", auran.Family);
        Assert.Equal("Dwarvish", auran.Script);
    }

    [Fact]
    public void SortBy_WorksCorrectly()
    {
        // Arrange
        var service = new LanguageService(null!);

        List<Language> languages =
        [
            CreateTestLanguage("Auran", "Primordial", "Dwarvish"),
            CreateTestLanguage("Dethek", "Dwarvish", "Dwarvish"),
            CreateTestLanguage("Elvish", "Elven", "Espruar"),
        ];

        // Act & Assert
        var sorted = service.SortBy(languages, LanguageService.LanguageSorting.Name);
        string[] expectedOrder =["Auran","Dethek","Elvish",];
        Assert.Equal(expectedOrder, sorted.Select(s => s.Name));

        sorted = service.SortBy(languages, LanguageService.LanguageSorting.Family, true);
        expectedOrder =["Primordial","Elven","Dwarvish",];
        Assert.Equal(expectedOrder, sorted.Select(s => s.Family));

        sorted = service.SortBy(languages, LanguageService.LanguageSorting.Script);
        expectedOrder =["Auran","Dethek","Elvish"];
        Assert.Equal(expectedOrder, sorted.Select(s => s.Name));
    }
}
