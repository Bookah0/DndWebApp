using DndWebApp.Api.Data;
using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.Characters.Enums;
using DndWebApp.Api.Models.Items;
using DndWebApp.Api.Models.Items.Enums;
using DndWebApp.Api.Models.World;
using DndWebApp.Api.Models.World.Enums;
using DndWebApp.Api.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DndWebApp.Tests.Repositories;

public class CharacterRepositoryTests
{
    private DbContextOptions<AppDbContext> GetInMemoryOptions(string dbName)
    {
        return new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .Options;
    }
    private Class CreateTestClass() => new Class
    {
        Name = "Ranger",
        Description = "Likes bears",
        HitDie = "1d8",
        ClassLevels = []
    };

    private Ability CreateTestAbility()
    {
        return new Ability() { FullName = "Strength", ShortName = "Str", Description = "Description", Skills = [] };
    }

    private Character CreateTestCharacter()
    {
        var str = CreateTestAbility();
        var background = new Background { Name = "Outlander", Description = "You grew up in the wilds, far from civilization", StartingCurrency = new() };
        var cls = CreateTestClass();

        return new Character
        {
            Name = "Arannis",
            Level = 5,
            Race = new Race { Name = "Elf", Speed = 30 },
            Class = cls,
            ClassId = cls.Id,
            Background = background,
            AbilityScores = [new AbilityValue() { Ability = str, AbilityId = str.Id, Value = 10 }],
            CombatStats = new CombatStats
            {
                ArmorClass = 14,
                Initiative = 3,
                Speed = 30,
                CurrentHitDice = 1,
                CurrentHP = 9,
                MaxHP = 10,
                MaxHitDice = 1
            },
            CurrentSpellSlots = new CurrentSpellSlots
            {
                Lvl1 = 4,
                Lvl2 = 2
            },
            CharacterBuildData = new CharacterBuilding
            {
                Eyes = "Brown"
            },
            SkillProficiencies = [new SkillProficiency() { SkillType = SkillType.Athletics, CharacterFeatureId = background.Id, HasExpertise = false }],
            Languages = [new() { LanguageType = LanguageType.Primordial, CharacterFeatureId = background.Id }],
            ToolProficiencies = [new() { ToolType = ToolCategory.HerbalismKit, CharacterFeatureId = background.Id }],
            WeaponProficiencies = [new() { WeaponTypes = WeaponCategory.MartialRanged, CharacterFeatureId = background.Id }]
        };
    }

    [Fact]
    public async Task UpdateCharacter_ChangesPersist()
    {
        // Arrange
        var options = GetInMemoryOptions("Character_UpdateDB");
        var character = CreateTestCharacter();

        await using var context = new AppDbContext(options);
        var repo = new CharacterRepository(context);
        await repo.CreateAsync(character);
        await context.SaveChangesAsync();

        // Act
        var toUpdate = await repo.GetWithAllDataAsync(character.Id);
        toUpdate!.Level += 1;
        toUpdate.WeaponProficiencies.Add(new() { WeaponTypes = WeaponCategory.MartialMelee, CharacterFeatureId = character.Background.Id });
        toUpdate.Languages.Clear();

        await repo.UpdateAsync(toUpdate);
        await context.SaveChangesAsync();

        // Assert
        var updated = await repo.GetWithAllDataAsync(toUpdate.Id);
        Assert.Equal(6, updated!.Level);
        Assert.Contains(updated!.WeaponProficiencies, p => p.WeaponTypes == WeaponCategory.MartialMelee);
        Assert.Empty(updated!.Languages);
    }
    
    [Fact]
    public async Task DeleteCharacter_ShouldDelete()
    {
        // Arrange
        var options = GetInMemoryOptions("Character_DeleteDB");
        var character = CreateTestCharacter();

        await using var context = new AppDbContext(options);
        var repo = new CharacterRepository(context);
        await repo.CreateAsync(character);
        await context.SaveChangesAsync();

        // Act
        await repo.DeleteAsync(character);
        await context.SaveChangesAsync();
        var deleted = await repo.GetWithAllDataAsync(character.Id);

        // Assert
        Assert.Null(deleted);
    }

    [Fact]
    public async Task GetWithAllDataAsync_ReturnsFullCharacterGraph()
    {
        // Arrange
        var options = GetInMemoryOptions("Character_FullDataDB");
        var character = CreateTestCharacter();

        await using var context = new AppDbContext(options);
        var repo = new CharacterRepository(context);
        await repo.CreateAsync(character);
        await context.SaveChangesAsync();

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
        // Arrange
        var options = GetInMemoryOptions("Character_PrimitiveDB");
        var character = CreateTestCharacter();

        await using var context = new AppDbContext(options);
        var repo = new CharacterRepository(context);
        await repo.CreateAsync(character);
        await context.SaveChangesAsync();

        // Act
        var dto = await repo.GetPrimitiveDataAsync(character.Id);

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
        var allCharacters = await repo.GetAllPrimitiveDataAsync();

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