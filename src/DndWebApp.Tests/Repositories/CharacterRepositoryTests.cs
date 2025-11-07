using static DndWebApp.Tests.Repositories.TestObjectFactory;
using DndWebApp.Api.Data;
using DndWebApp.Api.Models.Characters.Enums;
using DndWebApp.Api.Repositories;
using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Repositories.Implemented;

namespace DndWebApp.Tests.Repositories;

public class CharacterRepositoryTests
{
    [Fact]
    public async Task UpdateCharacter_ChangesPersist()
    {
        var options = GetInMemoryOptions("Character_UpdateDB");
        await using var context = new AppDbContext(options);
        var repo = new CharacterRepository(context);

        // Arrange
        var character = CreateTestCharacter();
        await repo.CreateAsync(character);

        // Act
        character.Level += 1;
        character.WeaponCategoryProficiencies.Add(new() { WeaponCategory = WeaponCategory.MartialMelee, FeatureId = character.Background!.Id });
        character.Languages.Clear();

        await repo.UpdateAsync(character);
        var updated = await repo.GetWithAllDataAsync(character.Id);

        // Assert
        Assert.Equal(6, updated!.Level);
        Assert.Contains(updated!.WeaponCategoryProficiencies, p => p.WeaponCategory == WeaponCategory.MartialMelee);
        Assert.Empty(updated!.Languages);
    }

    [Fact]
    public async Task DeleteCharacter_ShouldDelete()
    {
        var options = GetInMemoryOptions("Character_DeleteDB");
        await using var context = new AppDbContext(options);
        var repo = new CharacterRepository(context);

        // Arrange
        var character = CreateTestCharacter();
        await repo.CreateAsync(character);

        // Act
        await repo.DeleteAsync(character);
        var deleted = await repo.GetByIdAsync(character.Id);

        // Assert
        Assert.Null(deleted);
    }

    [Fact]
    public async Task GetWithAllDataAsync_ReturnsFullCharacterGraph()
    {
        var options = GetInMemoryOptions("Character_FullDataDB");
        await using var context = new AppDbContext(options);
        var repo = new CharacterRepository(context);

        // Arrange
        var character = CreateTestCharacter();
        await repo.CreateAsync(character);

        // Act
        var result = await repo.GetWithAllDataAsync(character.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Arannis", result!.Name);
        Assert.Equal("Elf", result.Race!.Name);
        Assert.Equal("Ranger", result.Class!.Name);
        Assert.Equal(14, result.CombatStats!.ArmorClass);
        Assert.NotEmpty(result.SkillProficiencies);
        Assert.Contains(result.SkillProficiencies, s => s.SkillType == SkillType.Athletics);
        Assert.NotEmpty(result.Languages);
    }

    [Fact]
    public async Task GetCharacterDescriptionAsync_ReturnsExpectedDetails()
    {
        var options = GetInMemoryOptions("Character_DescriptionDB");
        await using var context = new AppDbContext(options);
        var repo = new CharacterRepository(context);

        // Arrange
        var character = CreateTestCharacter();
        character.CharacterDescription.Age = 40;
        character.CharacterDescription.Hair = "Brown";
        character.CharacterDescription.Skin = "Hazel";

        await repo.CreateAsync(character);
        await context.SaveChangesAsync();

        // Act
        var desc = await repo.GetCharacterDescriptionAsync(character.Id);

        // Assert
        Assert.NotNull(desc);
        Assert.Equal(40, desc!.Age);
        Assert.Equal("Brown", desc.Hair);
        Assert.Equal("Hazel", desc.Skin);
    }
}