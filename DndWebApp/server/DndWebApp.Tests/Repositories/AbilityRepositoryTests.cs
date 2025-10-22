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

        var str = CreateAbility(1, "Strength", "Str", "Measures bodily power and force.");
        var dex = CreateAbility(2, "Dexterity", "Dex", "Measures agility, reflexes, and balance.");

        // Act
        await using (var context = new AppDbContext(options))
        {
            var repo = new AbilityRepository(context);
            await repo.CreateAsync(str);
            await repo.CreateAsync(dex);
            await context.SaveChangesAsync();
        }

        // Assert
        await using (var context = new AppDbContext(options))
        {
            var repo = new AbilityRepository(context);
            var savedStr = await repo.GetByIdAsync(1);

            Assert.NotNull(savedStr);
            Assert.Equal("Strength", savedStr!.FullName);
            Assert.Equal("Str", savedStr.ShortName);

            var savedAbilities = await repo.GetAllAsync();
            Assert.NotNull(savedAbilities);
            Assert.Equal(2, savedAbilities.Count);       
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

    [Fact]
    public async Task RetrieveAbilitiesAsPrimitiveDtos_ShouldHaveCorrectFieldValues()
    {
        // Arrange
        var options = GetInMemoryOptions("Ability_AddRetrieveDB");

        var dex = CreateAbility(1, "Dexterity", "Dex", "Measures agility, reflexes, and balance.");
        var con = CreateAbility(2, "Constitution", "Con", "Measures health, stamina, and vital force.");

        // Act
        await using (var context = new AppDbContext(options))
        {
            var repo = new AbilityRepository(context);
            await repo.CreateAsync(dex);
            await repo.CreateAsync(con);
            await context.SaveChangesAsync();
        }

        // Assert
        await using (var context = new AppDbContext(options))
        {
            var repo = new AbilityRepository(context);
            var savedAbility = await repo.GetPrimitiveDataAsync(1);

            Assert.NotNull(savedAbility);
            Assert.Equal("Dexterity", savedAbility!.FullName);
            Assert.Equal("Dex", savedAbility.ShortName);
            Assert.Equal("Measures agility, reflexes, and balance.", savedAbility.Description);

            var savedAbilities = await repo.GetAllPrimitiveDataAsync();
            Assert.NotNull(savedAbilities);
            Assert.Equal("Dexterity", savedAbility!.FullName);
            Assert.Equal("Dex", savedAbility.ShortName);
            Assert.Equal("Measures agility, reflexes, and balance.", savedAbility.Description);
            Assert.Contains(savedAbilities, a => a.FullName == "Dexterity");
            Assert.Contains(savedAbilities, a => a.ShortName == "Dex");
            Assert.Contains(savedAbilities, a => a.Description == "Measures agility, reflexes, and balance.");
            Assert.Contains(savedAbilities, a => a.FullName == "Constitution");
            Assert.Contains(savedAbilities, a => a.ShortName == "Con");
            Assert.Contains(savedAbilities, a => a.Description == "Measures health, stamina, and vital force.");
        }
    }

    [Fact]
    public async Task RetrieveWithSkills_ShouldHaveCorrectSkills()
    {
        // Arrange
        var options = GetInMemoryOptions("Ability_GetAllDB");
        
        var sleight = CreateSkill(1, "Sleight of Hand", 1);
        var stealth = CreateSkill(2, "Stealth", 1);
                                
        var dex = CreateAbility(1, "Dexterity", "Dex", "Measures agility, reflexes, and balance.", [sleight, stealth]);
        var con = CreateAbility(2, "Constitution", "Con", "Measures health, stamina, and vital force.");

        // Act
        await using (var context = new AppDbContext(options))
        {
            var repo = new AbilityRepository(context);
            await repo.CreateAsync(dex);
            await repo.CreateAsync(con);

            await context.SaveChangesAsync();
        }

        // Assert
        await using (var context = new AppDbContext(options))
        {
            var repo = new AbilityRepository(context);

            var fetchedDex = await repo.GetWithSkillsAsync(1);
            var fetchedCon = await repo.GetWithSkillsAsync(2);

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
