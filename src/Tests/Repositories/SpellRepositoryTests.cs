using static Tests.Repositories.TestObjectFactory;
using Api.Data;
using Api.Models.Spells;
using Api.Repositories.Implemented.Spells;
using Api.Validation.AllowedValues.Spells;

namespace Tests.Repositories;

public class SpellRepositoryTests
{
    [Fact]
    public async Task AddAndRetrieveSpells_WorksCorrectly()
    {
        var options = GetInMemoryOptions("Spell_AddRetrieveDB");
        await using var context = new AppDbContext(options);
        var repo = new SpellRepository(context);

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
        var repo = new SpellRepository(context);

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
        var repo = new SpellRepository(context);

        // Arrange
        var spell = CreateTestSpell("Magic Missile");
        await repo.CreateAsync(spell);

        spell = await repo.GetByIdAsync(spell.Id);
        await repo.DeleteAsync(spell!);

        var deleted = await repo.GetByIdAsync(spell!.Id);

        // Assert
        Assert.Null(deleted);
    }
}