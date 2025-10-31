using static DndWebApp.Tests.Services.TestObjectFactory;
using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Repositories.Abilities;
using DndWebApp.Api.Repositories.Skills;
using DndWebApp.Api.Services;
using Moq;
using Microsoft.Extensions.Logging.Abstractions;

namespace DndWebApp.Tests.Services;

public class SkillServiceTests
{
    [Fact]
    public async Task AddAndRetrieveSkills_WorksCorrectly()
    {
        // Arrange
        var repo = new Mock<ISkillRepository>();
        var abilityRepo = new Mock<IAbilityRepository>();
        var service = new SkillService(repo.Object, abilityRepo.Object, NullLogger<SkillService>.Instance);

        List<Skill> skills = [];

        repo.Setup(r => r.CreateAsync(It.IsAny<Skill>()))
            .ReturnsAsync((Skill s) =>
            {
                s.Id = skills.Count+1;
                skills.Add(s);
                return s;
            });

        repo.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((int id) => skills
            .FirstOrDefault(s => s.Id == id));
        
        abilityRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((int id) => new Ability{ Id = id, FullName = "Luck", ShortName = "LK", Description = "", Skills = [] });

        repo.Setup(r => r.GetAllAsync())
            .ReturnsAsync(() => [.. skills]);

        // Act
        var created1 = await service.CreateAsync(CreateTestSkillDto("Fishing", 1));
        var created2 = await service.CreateAsync(CreateTestSkillDto("Engineering", 1));

        var fishing = await service.GetByIdAsync(created1.Id);
        var engineering = await service.GetByIdAsync(created2.Id);

        // Assert
        Assert.NotNull(fishing);
        Assert.NotNull(engineering);
        Assert.Equal("Fishing", fishing.Name);
        Assert.Equal("Engineering", engineering.Name);

        var allSkills = await service.GetAllAsync();
        Assert.Contains(allSkills, a => a.Name == "Fishing");
        Assert.Contains(allSkills, a => a.Name == "Engineering");

        repo.Verify(r => r.CreateAsync(It.IsAny<Skill>()), Times.Exactly(2));
    }

    [Fact]
    public async Task AddAndRetrieveSkills_BadInputData_ShouldNotCreate()
    {
        // Arrange
        var repo = new Mock<ISkillRepository>();
        var abilityRepo = new Mock<IAbilityRepository>();
        var service = new SkillService(repo.Object, abilityRepo.Object, NullLogger<SkillService>.Instance);

        var badAbilityId = CreateTestSkillDto("Fishing", -1);
        var noName = CreateTestSkillDto("", 1);
        var whiteSpace = CreateTestSkillDto("  ", 1);

        // Act & Assert
        await Assert.ThrowsAsync<NullReferenceException>(() => service.CreateAsync(badAbilityId));
        await Assert.ThrowsAsync<ArgumentException>(() => service.CreateAsync(noName));
        await Assert.ThrowsAsync<ArgumentException>(() => service.CreateAsync(whiteSpace));
        repo.Verify(r => r.CreateAsync(It.IsAny<Skill>()), Times.Exactly(0));
    }

    [Fact]
    public async Task DeleteSkill_WorksCorrectly()
    {
        // Arrange
        var repo = new Mock<ISkillRepository>();
        var abilityRepo = new Mock<IAbilityRepository>();
        var service = new SkillService(repo.Object, abilityRepo.Object, NullLogger<SkillService>.Instance);

        List<Skill> skills = [CreateTestSkill("Fishing", 1)];

        repo.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((int id) => skills
            .FirstOrDefault(s => s.Id == id));

        repo.Setup(r => r.DeleteAsync(It.IsAny<Skill>()))
            .Callback((Skill s) =>
            {
                skills.Remove(s);
            });

        abilityRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((int id) => new Ability { Id = id, FullName = "Luck", ShortName = "LK", Description = "", Skills = [] });
            
        // Act & Assert
        await service.DeleteAsync(1);
        await Assert.ThrowsAsync<NullReferenceException>(() => service.DeleteAsync(1));
        repo.Verify(r => r.DeleteAsync(It.IsAny<Skill>()), Times.Exactly(1));
    }

    [Fact]
    public async Task GetAndDeleteSkill_BadId_ShouldNotGetOrDelete()
    {
        // Arrange
        var repo = new Mock<ISkillRepository>();
        var abilityRepo = new Mock<IAbilityRepository>();
        var service = new SkillService(repo.Object, abilityRepo.Object, NullLogger<SkillService>.Instance);

        List<Skill> skills = [CreateTestSkill("Fishing", 1)];

        repo.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((int id) => skills
            .FirstOrDefault(s => s.Id == id));

        abilityRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((int id) => new Ability { Id = id, FullName = "Luck", ShortName = "LK", Description = "", Skills = [] });
            
        // Act & Assert
        await Assert.ThrowsAsync<NullReferenceException>(() => service.GetByIdAsync(-1));
        await Assert.ThrowsAsync<NullReferenceException>(() => service.DeleteAsync(-1));

        repo.Verify(r => r.GetByIdAsync(It.IsAny<int>()), Times.Exactly(2));
        repo.Verify(r => r.DeleteAsync(It.IsAny<Skill>()), Times.Exactly(0));
    }

    [Fact]
    public async Task UpdateSkill_WorksCorrectly()
    {
        // Arrange
        var repo = new Mock<ISkillRepository>();
        var abilityRepo = new Mock<IAbilityRepository>();
        var service = new SkillService(repo.Object, abilityRepo.Object, NullLogger<SkillService>.Instance);
        
        List<Skill> skills = [CreateTestSkill("Fishing", 1)];

        repo.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((int id) => skills
            .FirstOrDefault(s => s.Id == id));

        repo.Setup(r => r.UpdateAsync(It.IsAny<Skill>()))
            .Callback((Skill s) =>
            {
                var skill = skills.FirstOrDefault(sk => sk.Id == s.Id);
                skill!.IsHomebrew = s.IsHomebrew;
                skill.Name = s.Name;
            });

        abilityRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((int id) => new Ability{ Id = id, FullName = "Luck", ShortName = "LK", Description = "", Skills = [] });

        var fishingDto = CreateTestSkillDto("Driving", 0, id: 1, isHomebrew: true);
        
        // Act
        await service.UpdateAsync(fishingDto);
        var drivingSkill = await service.GetByIdAsync(1);

        // Assert
        Assert.NotNull(drivingSkill);
        Assert.Equal("Driving", drivingSkill.Name);
        Assert.True(drivingSkill.IsHomebrew);

        repo.Verify(r => r.UpdateAsync(It.IsAny<Skill>()), Times.Exactly(1));
        repo.Verify(r => r.GetByIdAsync(It.IsAny<int>()), Times.Exactly(2));
    }

    [Fact]
    public async Task UpdateSkills_BadInputData_ShouldNotUpdate()
    {
        // Assert
        var repo = new Mock<ISkillRepository>();
        var abilityRepo = new Mock<IAbilityRepository>();
        var service = new SkillService(repo.Object, abilityRepo.Object, NullLogger<SkillService>.Instance);
        
        List<Skill> skills = [CreateTestSkill("Fishing", 1)];

        repo.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((int id) => skills
            .FirstOrDefault(s => s.Id == id));

        repo.Setup(r => r.DeleteAsync(It.IsAny<Skill>()))
            .Callback((Skill s) =>
            {
                skills.Remove(s);
            });

        repo.Setup(r => r.UpdateAsync(It.IsAny<Skill>()))
            .Callback((Skill s) =>
            {
                var skill = skills.FirstOrDefault(sk => sk.Id == s.Id);
                skill!.IsHomebrew = s.IsHomebrew;
                skill.Name = s.Name;
                skill.AbilityId = s.AbilityId;
            });

        abilityRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((int id) => new Ability{ Id = id, FullName = "Luck", ShortName = "LK", Description = "", Skills = [] });

        var badId = CreateTestSkillDto("Fishing", 2, id: -1);
        var noName = CreateTestSkillDto("", 2, id: 1);
        var whitespaceName = CreateTestSkillDto("   ", 2, id: 1);
        var badAbilityId = CreateTestSkillDto("Fishing", -1, id: 1);

        // Act & Assert
        await Assert.ThrowsAsync<NullReferenceException>(() => service.UpdateAsync(badId));
        await Assert.ThrowsAsync<ArgumentException>(() => service.UpdateAsync(noName));
        await Assert.ThrowsAsync<ArgumentException>(() => service.UpdateAsync(whitespaceName));
        await Assert.ThrowsAsync<ArgumentException>(() => service.UpdateAsync(badAbilityId));
    }

    [Fact]
    public void SortBy_WorksCorrectly()
    {
        // Assert
        var service = new SkillService(null!, null!, NullLogger<SkillService>.Instance);

        var intel = CreateTestAbility("Intelligence", "INT", "Desc..", id: 1);
        var str = CreateTestAbility("Strength", "STR", "Desc..", id: 2);
        var wis = CreateTestAbility("Wisdom", "WIS", "Desc..", id: 3);

        List<Skill> skills =
        [
            CreateTestSkill("Arcana", 1, ability: intel),
            CreateTestSkill("History", 1, ability: intel),
            CreateTestSkill("Nature", 1, ability: intel),
            CreateTestSkill("Religion", 1, ability: intel),
            CreateTestSkill("Medicine", 3, ability: wis),
            CreateTestSkill("Insight", 3, ability: wis),
            CreateTestSkill("Athletics", 2, ability: str),
        ];

        // Act & Assert
        var sorted = service.SortBy(skills, SkillService.SkillSorting.Name);
        string[] expectedOrder = ["Arcana", "Athletics", "History", "Insight", "Medicine", "Nature", "Religion"];
        Assert.Equal(expectedOrder, sorted.Select(s => s.Name));

        sorted = service.SortBy(skills, SkillService.SkillSorting.Ability);
        expectedOrder = ["Athletics", "Arcana", "History", "Nature", "Religion", "Insight", "Medicine"];
        Assert.Equal(expectedOrder, sorted.Select(s => s.Name));

        sorted = service.SortBy(skills, SkillService.SkillSorting.Ability, true);
        expectedOrder = ["Medicine", "Insight", "Religion", "Nature", "History", "Arcana", "Athletics"];
        Assert.Equal(expectedOrder, sorted.Select(s => s.Name));
    }
}
