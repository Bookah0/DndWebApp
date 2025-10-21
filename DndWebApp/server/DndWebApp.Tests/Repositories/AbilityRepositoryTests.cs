using DndWebApp.Api.Data;
using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DndWebApp.Tests.Repositories;

public class AbilityRepositoryTests
{
    private Skill CreateSkill(int id, string name, int abilityId)
    {
        return new Skill
        {
            Id = id,
            Name = name,
            AbilityId = abilityId
        };
    }

    private Ability CreateAbility(int id, string fullName, string shortName, string description, List<Skill>? skills = null)
    {
        return new Ability
        {
            Id = id,
            FullName = fullName,
            ShortName = shortName,
            Description = description,
            Skills = skills ?? []
        };
    }

    private DbContextOptions<AppDbContext> GetInMemoryOptions(string dbName)
    {
        return new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .Options;
    }

    [Fact]
    public async Task AddAndRetrieveAbility_WorksCorrectly()
    {
        // Arrange
        var options = GetInMemoryOptions("Ability_AddRetrieveDB");

        var athletics = CreateSkill(1, "Athletics", 1);
        var ability = CreateAbility(1, "Strength", "Str", "Measures bodily power and force.", [athletics]);

        // Act
        await using (var context = new AppDbContext(options))
        {
            var repo = new AbilityRepository(context);
            await repo.CreateAsync(ability);
            await context.SaveChangesAsync();
        }

        // Assert
        await using (var context = new AppDbContext(options))
        {
            var repo = new AbilityRepository(context);
            var savedAbility = await repo.GetByIdAsync(1);

            Assert.NotNull(savedAbility);
            Assert.Equal("Strength", savedAbility!.FullName);
            Assert.Equal("Str", savedAbility.ShortName);
            Assert.Single(savedAbility.Skills);
            Assert.Equal("Athletics", savedAbility.Skills.First().Name);
            Assert.Equal(savedAbility.Id, savedAbility.Skills.First().AbilityId);
        }
    }

    [Fact]
    public async Task GetAllAbilities_ReturnsAllAbilities()
    {
        // Arrange
        var options = GetInMemoryOptions("Ability_GetAllDB");

        var abilities = new List<Ability>
            {
                CreateAbility(1, "Strength", "Str", "Measures bodily power and force."),
                CreateAbility(2, "Dexterity", "Dex", "Measures agility, reflexes, and balance."),
                CreateAbility(3, "Constitution", "Con", "Measures health, stamina, and vital force."),
                CreateAbility(4, "Intelligence", "Int", "Measures reasoning and memory."),
                CreateAbility(5, "Wisdom", "Wis", "Measures perception and insight."),
                CreateAbility(6, "Charisma", "Cha", "Measures force of personality, persuasiveness, and leadership.")
            };

        // Act
        await using (var context = new AppDbContext(options))
        {
            var repo = new AbilityRepository(context);
            foreach (var ability in abilities)
            {
                await repo.CreateAsync(ability);
            }
            await context.SaveChangesAsync();
        }

        // Assert
        await using (var context = new AppDbContext(options))
        {
            var repo = new AbilityRepository(context);
            var savedAbilities = await repo.GetAllAsync();

            Assert.NotNull(savedAbilities);
            Assert.Equal(6, savedAbilities.Count);
        }
    }

    [Fact]
    public async Task UpdateAbility_WorksCorrectly()
    {
        // Arrange
        var options = GetInMemoryOptions("Ability_UpdateDB");

        var ability = CreateAbility(1, "Strength", "Str", "Measures bodily power and force.");

        await using (var context = new AppDbContext(options))
        {
            var repo = new AbilityRepository(context);
            await repo.CreateAsync(ability);
            await context.SaveChangesAsync();
        }

        // Act
        await using (var context = new AppDbContext(options))
        {
            var repo = new AbilityRepository(context);
            ability.FullName = "Updated Strength";
            await repo.UpdateAsync(ability);
            await context.SaveChangesAsync();
        }

        // Assert
        await using (var context = new AppDbContext(options))
        {
            var repo = new AbilityRepository(context);
            var updated = await repo.GetByIdAsync(1);

            Assert.Equal("Updated Strength", updated!.FullName);
        }
    }

    [Fact]
    public async Task DeleteAbility_WorksCorrectly()
    {
        // Arrange
        var options = GetInMemoryOptions("Ability_DeleteDB");

        var ability = CreateAbility(1, "Strength", "Str", "Measures bodily power and force.");

        await using (var context = new AppDbContext(options))
        {
            var repo = new AbilityRepository(context);
            await repo.CreateAsync(ability);
            await context.SaveChangesAsync();
        }

        // Act
        await using (var context = new AppDbContext(options))
        {
            var repo = new AbilityRepository(context);
            await repo.DeleteAsync(ability);
            await context.SaveChangesAsync();
        }

        // Assert
        await using (var context = new AppDbContext(options))
        {
            var repo = new AbilityRepository(context);
            var deleted = await repo.GetByIdAsync(ability.Id);
            Assert.Null(deleted);
        }
    }
}
