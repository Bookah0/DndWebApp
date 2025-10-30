using static DndWebApp.Tests.Services.TestObjectFactory;
using DndWebApp.Api.Models.Items.Enums;
using DndWebApp.Api.Models.Spells;
using DndWebApp.Api.Models.Spells.Enums;
using DndWebApp.Api.Repositories.Classes;
using DndWebApp.Api.Repositories.Spells;
using DndWebApp.Api.Services.Spells;

using Moq;

namespace DndWebApp.Tests.Services;

public class SpellServiceTests
{
    [Fact]
    public async Task AddAndRetrieveSpells_WorksCorrectly()
    {
        // Arrange
        var repo = new Mock<ISpellRepository>();
        var classRepo = new Mock<IClassRepository>();
        var service = new SpellService(repo.Object, classRepo.Object);

        var fireballDto = CreateTestSpellDto("Fireball");
        var lightningDto = CreateTestSpellDto("Lightning Bolt");
        
        List<Spell> spells = [];

        repo.Setup(r => r.CreateAsync(It.IsAny<Spell>()))
            .ReturnsAsync((Spell s) =>
            {
                s.Id = spells.Count+1;
                spells.Add(s);
                return s;
            });

        repo.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((int id) => spells
            .FirstOrDefault(s => s.Id == id));

        repo.Setup(r => r.GetAllAsync())
            .ReturnsAsync(() => [.. spells]);

        // Act
        var createdFireball = await service.CreateAsync(fireballDto);
        var createdLightning = await service.CreateAsync(lightningDto);

        var retrievedFireball = await service.GetByIdAsync(createdFireball.Id);
        var retrievedLightning = await service.GetByIdAsync(createdLightning.Id);

        // Assert
        Assert.NotNull(retrievedFireball);
        Assert.Equal("Fireball", retrievedFireball.Name);
        Assert.Equal(3, retrievedFireball.Level);
        Assert.Equal(MagicSchool.Evocation, retrievedFireball.MagicSchool);

        Assert.NotNull(retrievedLightning);
        Console.WriteLine("Id: " + retrievedLightning.Id);
        Assert.Equal("Lightning Bolt", retrievedLightning.Name);

        Assert.NotNull(retrievedFireball.SpellTargeting);
        Assert.Equal(SpellTargetType.Creature, retrievedFireball.SpellTargeting.TargetType);
        Assert.Equal(10, retrievedFireball.SpellTargeting.RangeValue);

        Assert.Equal("2d6", retrievedFireball.DamageRoll);
        Assert.Equal(DamageType.Fire, retrievedFireball.DamageTypes.First());

        var allSpells = await service.GetAllAsync();
        Assert.Contains(allSpells, s => s.Name == "Fireball");
        Assert.Contains(allSpells, s => s.Name == "Lightning Bolt");

        repo.Verify(r => r.CreateAsync(It.IsAny<Spell>()), Times.Exactly(2));
    }

    [Fact]
    public async Task UpdateSpell_WorksCorrectly()
    {
        // Arrange
        var repo = new Mock<ISpellRepository>();
        var classRepo = new Mock<IClassRepository>();
        var service = new SpellService(repo.Object, classRepo.Object);

        List<Spell> spells = [CreateTestSpell("Fireball", 0, 0, 0)];

        repo.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((int id) => spells
            .FirstOrDefault(s => s.Id == id));

        repo.Setup(r => r.UpdateAsync(It.IsAny<Spell>()))
            .Callback((Spell s) =>
            {
                var spell = spells.FirstOrDefault(sk => sk.Id == s.Id);
                s!.IsHomebrew = s.IsHomebrew;
                s.Name = s.Name;
            });

        var fireballDto = CreateTestSpellDto("Mega Fireball", true);
        fireballDto.Level = 5;
        fireballDto.RangeValue = 20;
        fireballDto.DamageRoll = "4d6";
        fireballDto.Materials = "Dragon scale";
        fireballDto.Id = spells.First().Id;

        // Act
        await service.UpdateAsync(fireballDto);
        var updatedFireball = await service.GetByIdAsync(spells.First().Id);

        // Assert
        Assert.NotNull(updatedFireball);
        Assert.Equal("Mega Fireball", updatedFireball.Name);
        Assert.Equal(5, updatedFireball.Level);

        Assert.NotNull(updatedFireball.SpellTargeting);
        Assert.Equal(20, updatedFireball.SpellTargeting.RangeValue);
        Assert.Equal("4d6", updatedFireball.DamageRoll);

        Assert.NotNull(updatedFireball.CastingRequirements);
        Assert.Equal("Dragon scale", updatedFireball.CastingRequirements.Materials);

        repo.Verify(r => r.UpdateAsync(It.IsAny<Spell>()), Times.Exactly(1));
    }

    [Fact]
    public async Task DeleteSpell_WorksCorrectly()
    {
        // Arrange
        var repo = new Mock<ISpellRepository>();
        var classRepo = new Mock<IClassRepository>();
        var service = new SpellService(repo.Object, classRepo.Object);

        List<Spell> spells = [CreateTestSpell("Lightning Bolt", 0, 0, 0)];

        repo.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((int id) => spells
            .FirstOrDefault(s => s.Id == id));

        repo.Setup(r => r.DeleteAsync(It.IsAny<Spell>()))
            .Callback((Spell s) =>
            {
                spells.Remove(s);
            });

        // Act
        await service.DeleteAsync(spells.First().Id);

        // Assert
        await Assert.ThrowsAsync<NullReferenceException>(() => service.DeleteAsync(1));
        repo.Verify(r => r.DeleteAsync(It.IsAny<Spell>()), Times.Exactly(1));
    }

    [Fact]
    public async Task GetAndDeleteSkill_BadId_ShouldNotGetOrDelete()
    {
        // Arrange
        var repo = new Mock<ISpellRepository>();
        var classRepo = new Mock<IClassRepository>();
        var service = new SpellService(repo.Object, classRepo.Object);

        List<Spell> spells = [CreateTestSpell("Lightning Bolt", 0, 0, 0)];

        repo.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((int id) => spells
            .FirstOrDefault(s => s.Id == id));

        // Act & Assert
        await Assert.ThrowsAsync<NullReferenceException>(() => service.GetByIdAsync(-1));
        await Assert.ThrowsAsync<NullReferenceException>(() => service.DeleteAsync(-1));

        repo.Verify(r => r.GetByIdAsync(It.IsAny<int>()), Times.Exactly(2));
        repo.Verify(r => r.DeleteAsync(It.IsAny<Spell>()), Times.Exactly(0));
    }

    [Fact]
    public void SortBy_WorksCorrectly()
    {
        // Arrange
        var service = new SpellService(null!, null!);

        ICollection<Spell> spells =
        [
            CreateTestSpell("Lightning Bolt", 1, SpellDuration.Instantaneous, 0),
            CreateTestSpell("Flame Bolt", 2, SpellDuration.Minute, 1),
            CreateTestSpell("Ice Bolt", 1, SpellDuration.Minute, 1),
            CreateTestSpell("Rock Bolt", 2, SpellDuration.Minute, 10),
        ];

        // Act & Assert
        var sorted = service.SortBy(spells, SpellService.SpellSortFilter.Name);
        string[] expectedOrder = ["Flame Bolt", "Ice Bolt", "Lightning Bolt", "Rock Bolt"];
        Assert.Equal(expectedOrder, sorted.Select(s => s.Name));

        sorted = service.SortBy(spells, SpellService.SpellSortFilter.Name, true);
        expectedOrder = ["Rock Bolt", "Lightning Bolt", "Ice Bolt", "Flame Bolt"];
        Assert.Equal(expectedOrder, sorted.Select(s => s.Name));

        sorted = service.SortBy(spells, SpellService.SpellSortFilter.Level);
        expectedOrder = ["Ice Bolt", "Lightning Bolt", "Flame Bolt", "Rock Bolt"];
        Assert.Equal(expectedOrder, sorted.Select(s => s.Name));

        sorted = service.SortBy(spells, SpellService.SpellSortFilter.Duration);
        expectedOrder = ["Lightning Bolt", "Flame Bolt", "Ice Bolt", "Rock Bolt"];
        Assert.Equal(expectedOrder, sorted.Select(s => s.Name));
    }
}
