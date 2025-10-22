using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DndWebApp.Api.Data;
using DndWebApp.Api.Models.Items.Enums;
using DndWebApp.Api.Models.Spells;
using DndWebApp.Api.Models.Spells.Enums;
using DndWebApp.Api.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DndWebApp.Tests.Repositories;

public class SpellRepositoryTests
{
    private Spell CreateSpell(int id, string name, Damage? damage = null)
    {
        return new Spell
        {
            Id = id,
            Name = name,
            Description = $"Description of {name}",
            Level = id,
            Duration = "Instantaneous",
            CastingTime = "1 action",
            TargetType = "",
            Damage = damage,
            MagicSchool = MagicSchool.Evocation,
            Range = 100,
        };
    }

    private DbContextOptions<AppDbContext> GetInMemoryOptions(string dbName)
    {
        return new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .Options;
    }

    [Fact]
    public async Task AddAndRetrieveSpells_WorksCorrectly()
    {
        // Arrange
        var options = GetInMemoryOptions("Spell_AddRetrieveDB");

        var damage = new Damage { DamageRoll = "1d4+1" };
        var magicMissile = CreateSpell(1, "Magic Missile", damage: damage);
        var fireball = CreateSpell(2, "Fireball");

        // Act
        await using (var context = new AppDbContext(options))
        {
            var repo = new SpellRepository(context);
            await repo.CreateAsync(magicMissile);
            await repo.CreateAsync(fireball);
            await context.SaveChangesAsync();
        }

        // Assert
        await using (var context = new AppDbContext(options))
        {
            var repo = new SpellRepository(context);

            var savedMagicMissile = await repo.GetByIdAsync(1);

            Assert.NotNull(savedMagicMissile);
            Assert.Equal("Magic Missile", savedMagicMissile!.Name);
            Assert.NotNull(savedMagicMissile.Damage);
            Assert.Equal("1d4+1", savedMagicMissile.Damage.DamageRoll);

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

        var spell1 = CreateSpell(1, "Magic Missile");
        var spell2 = CreateSpell(2, "Fireball");

        await using (var context = new AppDbContext(options))
        {
            var repo = new SpellRepository(context);
            await repo.CreateAsync(spell1);
            await repo.CreateAsync(spell2);
            await context.SaveChangesAsync();
        }

        // Act & Assert
        await using (var context = new AppDbContext(options))
        {
            var repo = new SpellRepository(context);

        }
    }

    [Fact]
    public async Task UpdateSpell_WorksCorrectly()
    {
        // Arrange
        var options = GetInMemoryOptions("Spell_UpdateDB");

        var spell = CreateSpell(1, "Magic Missile");

        await using (var context = new AppDbContext(options))
        {
            var repo = new SpellRepository(context);
            await repo.CreateAsync(spell);
            await context.SaveChangesAsync();
        }

        // Act
        await using (var context = new AppDbContext(options))
        {
            var repo = new SpellRepository(context);
            spell.Name = "Updated Spell";
            await repo.UpdateAsync(spell);
            await context.SaveChangesAsync();
        }

        // Assert
        await using (var context = new AppDbContext(options))
        {
            var repo = new SpellRepository(context);
            var updated = await repo.GetByIdAsync(1);

            Assert.Equal("Updated Spell", updated!.Name);
        }
    }

    [Fact]
    public async Task DeleteSpell_WorksCorrectly()
    {
        // Arrange
        var options = GetInMemoryOptions("Spell_DeleteDB");

        var spell = CreateSpell(1, "Magic Missile");

        await using (var context = new AppDbContext(options))
        {
            var repo = new SpellRepository(context);
            await repo.CreateAsync(spell);
            await context.SaveChangesAsync();
        }

        // Act
        await using (var context = new AppDbContext(options))
        {
            var repo = new SpellRepository(context);
            await repo.DeleteAsync(spell);
            await context.SaveChangesAsync();
        }

        // Assert
        await using (var context = new AppDbContext(options))
        {
            var repo = new SpellRepository(context);
            var deleted = await repo.GetByIdAsync(spell.Id);

            Assert.Null(deleted);
        }
    }
}