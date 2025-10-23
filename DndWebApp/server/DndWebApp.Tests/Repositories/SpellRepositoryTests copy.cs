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
    private Spell CreateTestSpell(string name, Damage? damage = null) => new()
    {
        Name = name,
        Description = $"Description of {name}",
        Level = 1,
        Duration = "",
        CastingTime = "",
        TargetType = "",
        Damage = damage!,
        MagicSchool = MagicSchool.Evocation,
        Range = 1,
    };

    private DbContextOptions<AppDbContext> GetInMemoryOptions(string dbName) => new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .Options;

    [Fact]
    public async Task AddAndRetrieveSpells_WorksCorrectly()
    {
        // Arrange
        var options = GetInMemoryOptions("Spell_AddRetrieveDB");

        var damage = new Damage { DamageRoll = "1d4+1" };
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
}