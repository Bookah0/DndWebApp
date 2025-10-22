using DndWebApp.Api.Data;
using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DndWebApp.Tests.Repositories;

public class AbilityRepositoryTests
{
    private Skill CreateSkill(string name, int abilityId)
    {
        return new() { Name = name, AbilityId = abilityId };
    }

    private Ability CreateAbility(string fullName, string shortName, string description, List<Skill>? skills = null)
    {
        return new() { FullName = fullName, ShortName = shortName, Description = description, Skills = skills ?? [] };
    }

    private DbContextOptions<AppDbContext> GetInMemoryOptions(string dbName) =>
        new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .Options;

    [Fact]
    public async Task AddAndRetrieveAbility_WorksCorrectly()
    {
        var options = GetInMemoryOptions("Ability_AddRetrieveDB");
        var str = CreateAbility("Strength", "Str", "Measures bodily power and force.");
        var dex = CreateAbility("Dexterity", "Dex", "Measures agility, reflexes, and balance.");

        await using (var context = new AppDbContext(options))
        {
            var repo = new AbilityRepository(context);
            await repo.CreateAsync(str);
            await repo.CreateAsync(dex);
            await context.SaveChangesAsync();
        }

        await using (var context = new AppDbContext(options))
        {
            var repo = new AbilityRepository(context);
            var savedStr = await repo.GetByIdAsync(str.Id);
            Assert.NotNull(savedStr);
            Assert.Equal("Strength", savedStr!.FullName);
            Assert.Equal("Str", savedStr.ShortName);

            var allAbilities = await repo.GetAllAsync();
            Assert.Equal(2, allAbilities.Count);
            Assert.Contains(allAbilities, a => a.FullName == "Strength");
            Assert.Contains(allAbilities, a => a.FullName == "Dexterity");
        }
    }

    [Fact]
    public async Task UpdateAbility_WorksCorrectly()
    {
        var options = GetInMemoryOptions("Ability_UpdateDB");
        var ability = CreateAbility("Strength", "Str", "Measures bodily power and force.");

        await using (var context = new AppDbContext(options))
        {
            var repo = new AbilityRepository(context);
            await repo.CreateAsync(ability);
            await context.SaveChangesAsync();
        }

        await using (var context = new AppDbContext(options))
        {
            var repo = new AbilityRepository(context);
            ability.FullName = "Updated Strength";
            await repo.UpdateAsync(ability);
            await context.SaveChangesAsync();
        }

        await using (var context = new AppDbContext(options))
        {
            var repo = new AbilityRepository(context);
            var updated = await repo.GetByIdAsync(ability.Id);
            Assert.Equal("Updated Strength", updated!.FullName);
        }
    }

    [Fact]
    public async Task DeleteAbility_WorksCorrectly()
    {
        var options = GetInMemoryOptions("Ability_DeleteDB");
        var ability = CreateAbility("Strength", "Str", "Measures bodily power and force.");

        await using (var context = new AppDbContext(options))
        {
            var repo = new AbilityRepository(context);
            await repo.CreateAsync(ability);
            await context.SaveChangesAsync();
        }

        await using (var context = new AppDbContext(options))
        {
            var repo = new AbilityRepository(context);
            await repo.DeleteAsync(ability);
            await context.SaveChangesAsync();
        }

        await using (var context = new AppDbContext(options))
        {
            var repo = new AbilityRepository(context);
            var deleted = await repo.GetByIdAsync(ability.Id);
            Assert.Null(deleted);
        }
    }

    [Fact]
    public async Task RetrieveAbilitiesAsPrimitiveDtos_ShouldHaveCorrectFieldValues()
    {
        var options = GetInMemoryOptions("PrimitiveAbility_AddRetrieveDB");
        var dex = CreateAbility("Dexterity", "Dex", "Measures agility, reflexes, and balance.");
        var con = CreateAbility("Constitution", "Con", "Measures health, stamina, and vital force.");

        await using (var context = new AppDbContext(options))
        {
            var repo = new AbilityRepository(context);
            await repo.CreateAsync(dex);
            await repo.CreateAsync(con);
            await context.SaveChangesAsync();
        }

        await using (var context = new AppDbContext(options))
        {
            var repo = new AbilityRepository(context);
            var savedDex = await repo.GetPrimitiveDataAsync(dex.Id);
            var savedCon = await repo.GetPrimitiveDataAsync(con.Id);

            Assert.NotNull(savedDex);
            Assert.Equal("Dexterity", savedDex!.FullName);
            Assert.Equal("Dex", savedDex.ShortName);

            Assert.NotNull(savedCon);
            Assert.Equal("Constitution", savedCon!.FullName);
            Assert.Equal("Con", savedCon.ShortName);

            var allAbilities = await repo.GetAllPrimitiveDataAsync();
            Assert.Equal(2, allAbilities.Count);
            Assert.Contains(allAbilities, a => a.FullName == "Dexterity");
            Assert.Contains(allAbilities, a => a.FullName == "Constitution");
        }
    }

    [Fact]
    public async Task RetrieveWithSkills_ShouldHaveCorrectSkills()
    {
        var options = GetInMemoryOptions("AbilityWithSkill_GetAllDB");

        var sleightOfHand = CreateSkill("Sleight of Hand", 0);
        var stealth = CreateSkill("Stealth", 0);

        var dex = CreateAbility("Dexterity", "Dex", "Measures agility, reflexes, and balance.", [sleightOfHand, stealth]);
        var con = CreateAbility("Constitution", "Con", "Measures health, stamina, and vital force.");

        await using (var context = new AppDbContext(options))
        {
            var repo = new AbilityRepository(context);
            await repo.CreateAsync(dex);
            await repo.CreateAsync(con);
            await context.SaveChangesAsync();
        }

        await using (var context = new AppDbContext(options))
        {
            var repo = new AbilityRepository(context);
            var fetchedDex = await repo.GetWithSkillsAsync(dex.Id);
            var fetchedCon = await repo.GetWithSkillsAsync(con.Id);

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
}
