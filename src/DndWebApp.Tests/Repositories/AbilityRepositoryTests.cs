using static DndWebApp.Tests.Repositories.TestObjectFactory;
using DndWebApp.Api.Data;
using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Repositories.Implemented;

namespace DndWebApp.Tests.Repositories;

public class AbilityRepositoryTests
{
    [Fact]
    public async Task AddAndRetrieveAbility_WorksCorrectly()
    {
        var options = GetInMemoryOptions("Ability_AddRetrieveDB");
        await using var context = new AppDbContext(options);
        var repo = new AbilityRepository(context);

        // Arrange
        var str = CreateTestAbility("Strength", "Str", "Measures bodily power and force.");
        var dex = CreateTestAbility("Dexterity", "Dex", "Measures agility, reflexes, and balance.");

        await repo.CreateAsync(str);
        await repo.CreateAsync(dex);

        // Act
        var savedStr = await repo.GetByIdAsync(str.Id);
        var allAbilities = await repo.GetMiscellaneousItemsAsync();

        // Assert
        Assert.NotNull(savedStr);
        Assert.Equal("Strength", savedStr!.FullName);
        Assert.Equal("Str", savedStr.ShortName);

        Assert.Equal(2, allAbilities.Count);
        Assert.Contains(allAbilities, a => a.FullName == "Strength");
        Assert.Contains(allAbilities, a => a.FullName == "Dexterity");
    }

    [Fact]
    public async Task UpdateAbility_WorksCorrectly()
    {
        var options = GetInMemoryOptions("Ability_UpdateDB");
        await using var context = new AppDbContext(options);
        var repo = new AbilityRepository(context);

        // Arrange
        var ability = CreateTestAbility("Strength", "Str", "Measures bodily power and force.");
        await repo.CreateAsync(ability);
        await context.SaveChangesAsync();


        // Act
        ability.FullName = "Updated Strength";
        await repo.UpdateAsync(ability);
        await context.SaveChangesAsync();

        var updated = await repo.GetByIdAsync(ability.Id);

        // Assert
        Assert.Equal("Updated Strength", updated!.FullName);
    }

    [Fact]
    public async Task DeleteAbility_WorksCorrectly()
    {
        var options = GetInMemoryOptions("Ability_DeleteDB");
        await using var context = new AppDbContext(options);
        var repo = new AbilityRepository(context);

        // Arrange
        var ability = CreateTestAbility("Strength", "Str", "Measures bodily power and force.");
        await repo.CreateAsync(ability);
        await context.SaveChangesAsync();

        // Act
        await repo.DeleteAsync(ability);
        await context.SaveChangesAsync();

        var deleted = await repo.GetByIdAsync(ability.Id);

        // Assert
        Assert.Null(deleted);
    }

    [Fact]
    public async Task RetrieveWithSkills_ShouldHaveCorrectSkills()
    {
        var options = GetInMemoryOptions("AbilityWithSkill_GetAllDB");
        await using var context = new AppDbContext(options);
        var repo = new AbilityRepository(context);

        // Arrange
        var sleightOfHand = CreateSkill("Sleight of Hand", 0);
        var stealth = CreateSkill("Stealth", 0);
        var dex = CreateTestAbility("Dexterity", "Dex", "Measures agility, reflexes, and balance.", [sleightOfHand, stealth]);
        var con = CreateTestAbility("Constitution", "Con", "Measures health, stamina, and vital force.");

        await repo.CreateAsync(dex);
        await repo.CreateAsync(con);

        // Act
        var fetchedDex = await repo.GetWithSkillsAsync(dex.Id);
        var fetchedCon = await repo.GetWithSkillsAsync(con.Id);

        // Assert
        Assert.NotNull(fetchedDex);
        Assert.NotNull(fetchedDex.Skills);
        Assert.Equal(2, fetchedDex.Skills.Count);
        Assert.Contains(fetchedDex.Skills, s => s.Name == "Sleight of Hand");
        Assert.Contains(fetchedDex.Skills, s => s.Name == "Stealth");
        Assert.All(fetchedDex.Skills, s => Assert.Equal(fetchedDex.Id, s.AbilityId));

        Assert.NotNull(fetchedCon);
        Assert.NotNull(fetchedCon.Skills);
        Assert.Empty(fetchedCon.Skills);

        var allAbilities = await repo.GetAllWithSkillsAsync();
        Assert.NotNull(allAbilities);
        Assert.NotEmpty(allAbilities);
        Assert.NotNull(allAbilities.First().Skills);
        Assert.True(allAbilities.First().Skills.Count == 0 || allAbilities.First().Skills.Count == 2);
    }
}
