using static DndWebApp.Tests.Services.TestObjectFactory;
using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Repositories.Abilities;
using DndWebApp.Api.Services;
using Moq;
using Microsoft.Extensions.Logging.Abstractions;

namespace DndWebApp.Tests.Services;

public class AbilityServiceTests
{
    [Fact]
    public async Task AddAndRetrieveabilities_WorksCorrectly()
    {
        // Arrange
        var repo = new Mock<IAbilityRepository>();
        var service = new AbilityService(repo.Object, NullLogger<AbilityService>.Instance);

        ICollection<Ability> abilities = [];

        repo.Setup(r => r.CreateAsync(It.IsAny<Ability>()))
            .ReturnsAsync((Ability a) =>
            {
                a.Id = abilities.Count + 1;
                abilities.Add(a);
                return a;
            });

        repo.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((int id) => abilities
            .FirstOrDefault(a => a.Id == id));

        repo.Setup(r => r.GetAllAsync())
            .ReturnsAsync(() => [.. abilities]);

        // Act
        var created1 = await service.CreateAsync(CreateTestAbilityDto("Strength", "Str", "Measures bodily power and force.", 1));
        var created2 = await service.CreateAsync(CreateTestAbilityDto("Dexterity", "Dex", "Measures agility, reflexes, and balance.", 2));

        var strength = await service.GetByIdAsync(created1.Id);
        var dexterity = await service.GetByIdAsync(created2.Id);
        abilities = await service.GetAllAsync();

        // Assert
        Assert.NotNull(strength);
        Assert.Equal("Strength", strength.FullName);
        Assert.Equal("Str", strength.ShortName);
        Assert.Equal("Measures bodily power and force.", strength.Description);

        Assert.NotNull(dexterity);
        Assert.Equal("Dexterity", dexterity.FullName);
        Assert.Equal("Dex", dexterity.ShortName);
        Assert.Equal("Measures agility, reflexes, and balance.", dexterity.Description);

        // Asserts for all objects
        Assert.Contains(abilities, a => a.FullName == "Strength");
        Assert.Contains(abilities, a => a.FullName == "Dexterity");

        repo.Verify(r => r.CreateAsync(It.IsAny<Ability>()), Times.Exactly(2));
    }

    [Fact]
    public async Task AddAndRetrieveabilities_BadInputData_ShouldNotCreate()
    {
        // Arrange
        var repo = new Mock<IAbilityRepository>();
        var service = new AbilityService(repo.Object, NullLogger<AbilityService>.Instance);

        var noFullName = CreateTestAbilityDto("", "Str", "Measures bodily power and force.", 1);
        var whitespaceShortName = CreateTestAbilityDto("Strength", "     ", "Measures bodily power and force.", 1);
        var nullDescription = CreateTestAbilityDto("Strength", "Str", null!, 1);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => service.CreateAsync(noFullName));
        await Assert.ThrowsAsync<ArgumentException>(() => service.CreateAsync(whitespaceShortName));
        await Assert.ThrowsAsync<ArgumentException>(() => service.CreateAsync(nullDescription));

        repo.Verify(r => r.CreateAsync(It.IsAny<Ability>()), Times.Exactly(0));
    }

    [Fact]
    public async Task DeleteAbility_WorksCorrectly()
    {
        // Arrange
        var repo = new Mock<IAbilityRepository>();
        var service = new AbilityService(repo.Object, NullLogger<AbilityService>.Instance);

        ICollection<Ability> abilities = [CreateTestAbility("Strength", "Str", "Measures bodily power and force.", 1)];

        repo.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((int id) => abilities
            .FirstOrDefault(a => a.Id == id));

        repo.Setup(r => r.DeleteAsync(It.IsAny<Ability>()))
            .Callback((Ability a) =>
            {
                abilities.Remove(a);
            });

        // Act
        var id = abilities.First().Id;
        await service.DeleteClassLevelAsync(id);

        // Assert
        await Assert.ThrowsAsync<NullReferenceException>(() => service.DeleteClassLevelAsync(id));
        repo.Verify(r => r.DeleteAsync(It.IsAny<Ability>()), Times.Exactly(1));
    }

    [Fact]
    public async Task GetAndDeleteAbility_BadId_ShouldNotGetOrDelete()
    {
        // Arrange
        var repo = new Mock<IAbilityRepository>();
        var service = new AbilityService(repo.Object, NullLogger<AbilityService>.Instance);

        ICollection<Ability> abilities = [CreateTestAbility("Strength", "Str", "Measures bodily power and force.", 1)];
        abilities.First().Id = 1;

        repo.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((int id) => abilities
            .FirstOrDefault(a => a.Id == id));

        // Act & Assert
        await Assert.ThrowsAsync<NullReferenceException>(() => service.GetByIdAsync(-1));
        await Assert.ThrowsAsync<NullReferenceException>(() => service.DeleteClassLevelAsync(-1));

        repo.Verify(r => r.GetByIdAsync(It.IsAny<int>()), Times.Exactly(2));
        repo.Verify(r => r.DeleteAsync(It.IsAny<Ability>()), Times.Exactly(0));
    }

    [Fact]
    public async Task UpdateAbility_WorksCorrectly()
    {
        var repo = new Mock<IAbilityRepository>();
        var service = new AbilityService(repo.Object, NullLogger<AbilityService>.Instance);

        ICollection<Ability> abilities = [CreateTestAbility("Strength", "Str", "Measures bodily power and force.", 1)];
        var updateDto = CreateTestAbilityDto("Dexterity", "Dex", "Measures agility, reflexes, and balance.", 2);
        updateDto.Id = abilities.First().Id;

        repo.Setup(r => r.UpdateAsync(It.IsAny<Ability>()))
            .Callback((Ability a) =>
            {
                var ability = abilities.FirstOrDefault(align => align.Id == a.Id);
                ability!.FullName = a.FullName;
                ability.ShortName = a.ShortName;
                ability.Description = a.Description;
            });

        repo.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((int id) => abilities
            .FirstOrDefault(a => a.Id == id));

        // Act
        await service.UpdateAsync(updateDto);
        var updated = await service.GetByIdAsync(abilities.First().Id);

        // Assert
        Assert.NotNull(updated);
        Assert.Equal("Dexterity", updated.FullName);
        Assert.Equal("Dex", updated.ShortName);
        Assert.Equal("Measures agility, reflexes, and balance.", updated.Description);

        repo.Verify(r => r.GetByIdAsync(It.IsAny<int>()), Times.Exactly(2));
        repo.Verify(r => r.DeleteAsync(It.IsAny<Ability>()), Times.Exactly(0));
    }

    [Fact]
    public async Task Updateabilities_BadInputData_ShouldNotUpdate()
    {
        var repo = new Mock<IAbilityRepository>();
        var service = new AbilityService(repo.Object, NullLogger<AbilityService>.Instance);

        ICollection<Ability> abilities = [CreateTestAbility("Strength", "Str", "Measures bodily power and force.", 1)];

        var noFullName = CreateTestAbilityDto("", "Str", "Measures bodily power and force.", 1);
        var whitespaceShortName = CreateTestAbilityDto("Strength", "     ", "Measures bodily power and force.", 1);
        var nullDescription = CreateTestAbilityDto("Strength", "Str", null!, 1);

        await Assert.ThrowsAsync<ArgumentException>(() => service.UpdateAsync(noFullName));
        await Assert.ThrowsAsync<ArgumentException>(() => service.UpdateAsync(whitespaceShortName));
        await Assert.ThrowsAsync<ArgumentException>(() => service.UpdateAsync(nullDescription));

        repo.Verify(r => r.UpdateAsync(It.IsAny<Ability>()), Times.Exactly(0));
    }

    [Fact]
    public void SortBy_WorksCorrectly()
    {
        // Arrange
        var service = new AbilityService(null!, NullLogger<AbilityService>.Instance);

        ICollection<Ability> abilities =
        [
            CreateTestAbility("Strength", "Str", "Measures bodily power and force.", 1),
            CreateTestAbility("Dexterity", "Dex", "Measures agility, reflexes, and balance.", 2),
            CreateTestAbility("Wisdom", "Dex", "", 3),
            CreateTestAbility("Intelligence", "Dex", "", 4),
        ];

        // Act & Assert
        var sorted = service.SortBy(abilities);
        string[] expectedOrder = ["Strength", "Dexterity", "Intelligence", "Wisdom"];
        Assert.Equal(expectedOrder, sorted.Select(s => s.FullName));
    }
}
