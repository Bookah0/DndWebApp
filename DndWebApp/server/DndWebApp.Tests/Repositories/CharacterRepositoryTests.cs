using static DndWebApp.Tests.Repositories.TestObjectFactory;
using DndWebApp.Api.Data;
using DndWebApp.Api.Models.Characters.Enums;
using DndWebApp.Api.Repositories.Characters;

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
        character.WeaponProficiencies.Add(new() { WeaponTypes = WeaponCategory.MartialMelee, CharacterFeatureId = character.Background.Id });
        character.Languages.Clear();
        
        await repo.UpdateAsync(character);
        var updated = await repo.GetWithAllDataAsync(character.Id);

        // Assert
        Assert.Equal(6, updated!.Level);
        Assert.Contains(updated!.WeaponProficiencies, p => p.WeaponTypes == WeaponCategory.MartialMelee);
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
        var deleted = await repo.GetWithAllDataAsync(character.Id);

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
    public async Task GetPrimitiveDataAsync_ReturnsMinimalData()
    {
        var options = GetInMemoryOptions("Character_PrimitiveDB");
        await using var context = new AppDbContext(options);
        var repo = new CharacterRepository(context);

        // Arrange
        var character = CreateTestCharacter();
        await repo.CreateAsync(character);

        // Act
        var dto = await repo.GetDtoAsync(character.Id);

        // Assert
        Assert.NotNull(dto);
        Assert.Equal("Arannis", dto!.Name);
        Assert.Equal(9, dto.CurrentHP);
        Assert.Equal(5, dto.Level);
    }

    [Fact]
    public async Task GetAllPrimitiveDataAsync_ReturnsAllCharacters()
    {
        // Arrange
        var options = GetInMemoryOptions("Character_AllPrimitiveDB");

        var c1 = CreateTestCharacter();
        var c2 = CreateTestCharacter();
        c2.Name = "Lyra";

        await using var context = new AppDbContext(options);
        var repo = new CharacterRepository(context);
        await repo.CreateAsync(c1);
        await repo.CreateAsync(c2);
        await context.SaveChangesAsync();

        // Act
        var allCharacters = await repo.GetAllDtosAsync();

        // Assert
        Assert.Equal(2, allCharacters.Count);
        Assert.Contains(allCharacters, c => c.Name == "Arannis");
        Assert.Contains(allCharacters, c => c.Name == "Lyra");
    }

    [Fact]
    public async Task GetCharacterDescriptionAsync_ReturnsExpectedDetails()
    {
        var options = GetInMemoryOptions("Character_DescriptionDB");
        var character = CreateTestCharacter();
        character.CharacterBuildData.Age = 40;
        character.CharacterBuildData.Hair = "Brown";
        character.CharacterBuildData.Skin = "Hazel";

        await using var context = new AppDbContext(options);
        var repo = new CharacterRepository(context);
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

[Fact]
public async Task GetCurrentSpellSlotsAsync_ReturnsSpellSlots()
{
    var options = GetInMemoryOptions("Character_SpellSlotsDB");
    var character = CreateTestCharacter();

    await using var context = new AppDbContext(options);
    var repo = new CharacterRepository(context);
    await repo.CreateAsync(character);
    await context.SaveChangesAsync();

    // Act
    var slots = await repo.GetCurrentSpellSlotsAsync(character.Id);

    // Assert
    Assert.NotNull(slots);
    Assert.Equal(4, slots!.Lvl1);
    Assert.Equal(2, slots.Lvl2);
}

}