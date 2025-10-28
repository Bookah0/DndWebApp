using DndWebApp.Api.Data;
using DndWebApp.Api.Models.Spells;
using DndWebApp.Api.Models.Spells.Enums;
using DndWebApp.Api.Repositories.Spells;
using Microsoft.EntityFrameworkCore;

namespace DndWebApp.Tests.Repositories;

public class SpellRepositoryTests
{
    private Spell CreateTestSpell(string name, SpellDamage? damage = null) => new()
    {
        Name = name,
        Description = $"Description of {name}",
        Level = 1,
        Duration = 0,
        CastingTime = 0,
        SpellTargeting = new(){ TargetType = SpellTargetType.Creature, Range = SpellRange.Feet, RangeValue = 20 },
        SpellDamage = damage!,
        MagicSchool = MagicSchool.Evocation,
    };

    private DbContextOptions<AppDbContext> GetInMemoryOptions(string dbName) => new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .Options;

    [Fact]
    public async Task AddAndRetrieveSpells_WorksCorrectly()
    {
        // Arrange
        var options = GetInMemoryOptions("Spell_AddRetrieveDB");

        var damage = new SpellDamage { DamageRoll = "1d4+1" };
        var magicMissile = CreateTestSpell("Magic Missile", damage);
        var fireball = CreateTestSpell("Fireball");
        int magicMissileId;

        // Act
        await using (var context = new AppDbContext(options))
        {
            var repo = new SpellRepository(context);
            await repo.CreateAsync(magicMissile);
            await repo.CreateAsync(fireball);
            await context.SaveChangesAsync();

            magicMissileId = magicMissile.Id;
        }

        // Assert
        await using (var context = new AppDbContext(options))
        {
            var repo = new SpellRepository(context);

            var savedMagicMissile = await repo.GetByIdAsync(magicMissileId);
            Assert.NotNull(savedMagicMissile);
            Assert.Equal("Magic Missile", savedMagicMissile!.Name);
            Assert.NotNull(savedMagicMissile.SpellDamage);
            Assert.Equal("1d4+1", savedMagicMissile.SpellDamage.DamageRoll);

            var allSpells = await repo.GetAllAsync();
            Assert.Equal(2, allSpells.Count);
            Assert.Contains(allSpells, s => s.Name == "Magic Missile");
            Assert.Contains(allSpells, s => s.Name == "Fireball");
        }
    }

    [Fact]
    public async Task GetAllSpells_ReturnsAllSpells()
    {
        // Arrange
        var options = GetInMemoryOptions("Spell_GetAllDB");

        await using (var context = new AppDbContext(options))
        {
            var repo = new SpellRepository(context);
            await repo.CreateAsync(CreateTestSpell("Magic Missile"));
            await repo.CreateAsync(CreateTestSpell("Fireball"));
            await context.SaveChangesAsync();
        }

        // Act & Assert
        await using (var context = new AppDbContext(options))
        {
            var repo = new SpellRepository(context);
            var all = await repo.GetAllAsync();

            Assert.Equal(2, all.Count);
            Assert.Contains(all, s => s.Name == "Magic Missile");
            Assert.Contains(all, s => s.Name == "Fireball");
        }
    }

    [Fact]
    public async Task UpdateSpell_WorksCorrectly()
    {
        // Arrange
        var options = GetInMemoryOptions("Spell_UpdateDB");
        int spellId;

        await using (var context = new AppDbContext(options))
        {
            var repo = new SpellRepository(context);
            var spell = CreateTestSpell("Magic Missile");
            await repo.CreateAsync(spell);
            await context.SaveChangesAsync();
            spellId = spell.Id;
        }

        // Act
        await using (var context = new AppDbContext(options))
        {
            var repo = new SpellRepository(context);
            var spell = await repo.GetByIdAsync(spellId);
            spell!.Name = "Updated Spell";
            await repo.UpdateAsync(spell);
            await context.SaveChangesAsync();
        }

        // Assert
        await using (var context = new AppDbContext(options))
        {
            var repo = new SpellRepository(context);
            var updated = await repo.GetByIdAsync(spellId);
            Assert.Equal("Updated Spell", updated!.Name);
        }
    }

    [Fact]
    public async Task DeleteSpell_WorksCorrectly()
    {
        // Arrange
        var options = GetInMemoryOptions("Spell_DeleteDB");
        int spellId;

        await using (var context = new AppDbContext(options))
        {
            var repo = new SpellRepository(context);
            var spell = CreateTestSpell("Magic Missile");
            await repo.CreateAsync(spell);
            await context.SaveChangesAsync();
            spellId = spell.Id;
        }

        // Act
        await using (var context = new AppDbContext(options))
        {
            var repo = new SpellRepository(context);
            var spell = await repo.GetByIdAsync(spellId);
            await repo.DeleteAsync(spell!);
            await context.SaveChangesAsync();
        }

        // Assert
        await using (var context = new AppDbContext(options))
        {
            var repo = new SpellRepository(context);
            var deleted = await repo.GetByIdAsync(spellId);
            Assert.Null(deleted);
        }
    }

    [Fact]
    public async Task FilterAllAsync_WithMatchingFilter_ReturnsExpectedSpells()
    {
        // Arrange
        var options = GetInMemoryOptions("Spell_FilterDB");
        var context = new AppDbContext(options);

        var spells = new List<Spell>
        {
            CreateTestSpell("Fireball"),
            CreateTestSpell("Frostbite"),
            CreateTestSpell("Magic Missile")
        };

        await context.Spells.AddRangeAsync(spells);
        await context.SaveChangesAsync();

        var filter = new SpellFilter
        {
            Name = "Fire",
            MinLevel = 1,
            MaxLevel = 3,
            MagicSchools = [MagicSchool.Evocation],
            IsHomebrew = false,
            ClassIds = null,
            Durations = null,
            CastingTimes = null,
            SpellTypes = null,
            TargetType = null,
            Range = null,
            DamageTypes = null,
        };

        // Act
        var repo = new SpellRepository(context);
        var result = await repo.FilterAllAsync(filter);

        // Assert
        Assert.Single(result);
        var spell = result.First();
        Assert.Equal("Fireball", spell.Name);
        Assert.Equal(MagicSchool.Evocation, spell.MagicSchool);
    }
}