using static DndWebApp.Tests.Repositories.TestObjectFactory;
using DndWebApp.Api.Data;
using DndWebApp.Api.Models.Spells;
using DndWebApp.Api.Models.Spells.Enums;
using DndWebApp.Api.Repositories.Spells;
using DndWebApp.Api.Repositories;

namespace DndWebApp.Tests.Repositories;

public class SpellRepositoryTests
{
    [Fact]
    public async Task AddAndRetrieveSpells_WorksCorrectly()
    {
        var options = GetInMemoryOptions("Spell_AddRetrieveDB");
        await using var context = new AppDbContext(options);
        var efRepo = new EfRepository<Spell>(context);
        var repo = new SpellRepository(context, efRepo);

        // Arrange
        var magicMissile = CreateTestSpell("Magic Missile");
        var fireball = CreateTestSpell("Fireball");

        // Act
        await repo.CreateAsync(magicMissile);
        await repo.CreateAsync(fireball);

        var savedMagicMissile = await repo.GetByIdAsync(magicMissile.Id);
        var allSpells = await repo.GetAllAsync();

        // Assert
        Assert.NotNull(savedMagicMissile);
        Assert.Equal("Magic Missile", savedMagicMissile!.Name);

        Assert.Equal(2, allSpells.Count);
        Assert.Contains(allSpells, s => s.Name == "Magic Missile");
        Assert.Contains(allSpells, s => s.Name == "Fireball");
    }

    [Fact]
    public async Task UpdateSpell_WorksCorrectly()
    {
        var options = GetInMemoryOptions("Spell_UpdateDB");
        await using var context = new AppDbContext(options);
        var efRepo = new EfRepository<Spell>(context);
        var repo = new SpellRepository(context, efRepo);

        // Arrange
        var spell = CreateTestSpell("Magic Missile");
        await repo.CreateAsync(spell);

        // Act
        spell = await repo.GetByIdAsync(spell.Id);
        spell!.Name = "Updated Spell";

        await repo.UpdateAsync(spell);
        var updated = await repo.GetByIdAsync(spell.Id);

        // Assert
        Assert.Equal("Updated Spell", updated!.Name);
    }

    [Fact]
    public async Task DeleteSpell_WorksCorrectly()
    {
        var options = GetInMemoryOptions("Spell_DeleteDB");
        await using var context = new AppDbContext(options);
        var efRepo = new EfRepository<Spell>(context);
        var repo = new SpellRepository(context, efRepo);

        // Arrange
        var spell = CreateTestSpell("Magic Missile");
        await repo.CreateAsync(spell);

        spell = await repo.GetByIdAsync(spell.Id);
        await repo.DeleteAsync(spell!);

        var deleted = await repo.GetByIdAsync(spell!.Id);

        // Assert
        Assert.Null(deleted);
    }

    [Fact]
    public async Task FilterAllAsync_WithMatchingFilter_ReturnsExpectedSpells()
    {
        var options = GetInMemoryOptions("Spell_FilterDB");
        var context = new AppDbContext(options);
        var efRepo = new EfRepository<Spell>(context);
        var repo = new SpellRepository(context, efRepo);

        // Arrange
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
        var filteredSpells = await repo.FilterAllAsync(filter);

        // Assert
        Assert.Single(filteredSpells);
        Assert.Equal("Fireball", filteredSpells.First().Name);
        Assert.Equal(MagicSchool.Evocation, filteredSpells.First().MagicSchool);
    }
}