using System.ComponentModel;
using DndWebApp.Api.Data;
using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs;
using DndWebApp.Api.Models.World;
using DndWebApp.Api.Repositories;
using DndWebApp.Api.Repositories.Abilities;
using DndWebApp.Api.Repositories.Skills;
using DndWebApp.Api.Services;
using Microsoft.EntityFrameworkCore;
using Xunit.Sdk;

namespace DndWebApp.Tests.Services;

public class SkillServiceTests
{
    private Ability CreateAbility(string fullName, string shortName, int sortWeight = 0)
    {
        return new() { FullName = fullName, ShortName = shortName, Description = "Desc..", Skills = [], SortWeight = sortWeight };
    }

    private SkillDto CreateSkillDto(string name, int abilityId, int id = 1)
    {
        return new() { Id = id, Name = name, AbilityId = abilityId, IsHomebrew = false };
    }
    private DbContextOptions<AppDbContext> GetInMemoryOptions(string dbName)
    {
        return new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
            .Options;
    }

    [Fact]
    public async Task AddAndRetrieveSkills_WorksCorrectly()
    {
        var options = GetInMemoryOptions("Skill_AddRetrieveDB");

        using var context = new AppDbContext(options);
        var repo = new SkillRepository(context);
        var abilityRepo = new AbilityRepository(context);
        var service = new SkillService(repo, abilityRepo, context);

        var ability = CreateAbility("Luck", "LK");
        await abilityRepo.CreateAsync(ability);
        await context.SaveChangesAsync();

        var fishingDto = CreateSkillDto("Fishing", ability.Id);
        var engineeringDto = CreateSkillDto("Engineering", ability.Id);

        var created1 = await service.CreateAsync(fishingDto);
        var created2 = await service.CreateAsync(engineeringDto);

        var fishing = await service.GetByIdAsync(created1.Id);
        var engineering = await service.GetByIdAsync(created2.Id);

        Assert.NotNull(fishing);
        Assert.NotNull(engineering);
        Assert.Equal("Fishing", fishing.Name);
        Assert.Equal("Engineering", engineering.Name);

        var allSkills = await service.GetAllAsync();
        Assert.Contains(allSkills, a => a.Name == "Fishing");
        Assert.Contains(allSkills, a => a.Name == "Engineering");
    }

    [Fact]
    public async Task AddAndRetrieveSkills_BadInputData_ShouldNotCreate()
    {
        var options = GetInMemoryOptions("Skill_BadAddRetrieveDB");

        using var context = new AppDbContext(options);
        var repo = new SkillRepository(context);
        var abilityRepo = new AbilityRepository(context);
        var service = new SkillService(repo, abilityRepo, context);

        var ability = CreateAbility("Luck", "LK");
        await abilityRepo.CreateAsync(ability);
        await context.SaveChangesAsync();

        var badAbilityId = CreateSkillDto("Fishing", -1);
        var noName = CreateSkillDto("", ability.Id);
        var whiteSpace = CreateSkillDto("  ", ability.Id);

        await Assert.ThrowsAsync<NullReferenceException>(() => service.CreateAsync(badAbilityId));
        await Assert.ThrowsAsync<ArgumentException>(() => service.CreateAsync(noName));
        await Assert.ThrowsAsync<ArgumentException>(() => service.CreateAsync(whiteSpace));
    }

    [Fact]
    public async Task DeleteSkill_WorksCorrectly()
    {
        var options = GetInMemoryOptions("Skill_RemoveDB");
        using var context = new AppDbContext(options);
        var repo = new SkillRepository(context);
        var abilityRepo = new AbilityRepository(context);
        var service = new SkillService(repo, abilityRepo, context);

        var ability = CreateAbility("Luck", "LK");
        await abilityRepo.CreateAsync(ability);
        await context.SaveChangesAsync();

        var fishingDto = CreateSkillDto("Fishing", ability.Id);
        var created1 = await service.CreateAsync(fishingDto);

        await service.DeleteAsync(created1.Id);
        await Assert.ThrowsAsync<NullReferenceException>(() => service.GetByIdAsync(created1.Id));
    }

    [Fact]
    public async Task DeleteSkill_BadId_ShouldNotDelete()
    {
        var options = GetInMemoryOptions("Skill_BadRemoveDB");
        using var context = new AppDbContext(options);
        var repo = new SkillRepository(context);
        var abilityRepo = new AbilityRepository(context);
        var service = new SkillService(repo, abilityRepo, context);

        var ability = CreateAbility("Luck", "LK");
        await abilityRepo.CreateAsync(ability);
        await context.SaveChangesAsync();

        var fishingDto = CreateSkillDto("Fishing", ability.Id);
        var created1 = await service.CreateAsync(fishingDto);

        await service.DeleteAsync(created1.Id);
        await Assert.ThrowsAsync<NullReferenceException>(() => service.DeleteAsync(created1.Id));
    }

    [Fact]
    public async Task UpdateSkill_WorksCorrectly()
    {
        var options = GetInMemoryOptions("Skill_UpdateDB");

        using var context = new AppDbContext(options);
        var repo = new SkillRepository(context);
        var abilityRepo = new AbilityRepository(context);
        var service = new SkillService(repo, abilityRepo, context);

        var luckAbility = CreateAbility("Luck", "LK");
        var pilotingAbility = CreateAbility("Piloting", "PT");
        await abilityRepo.CreateAsync(luckAbility);
        await abilityRepo.CreateAsync(pilotingAbility);
        await context.SaveChangesAsync();

        var fishingDto = CreateSkillDto("Fishing", luckAbility.Id);
        var fishingSkill = await service.CreateAsync(fishingDto);

        fishingDto.Name = "Driving";
        fishingDto.AbilityId = pilotingAbility.Id;
        fishingDto.Id = fishingSkill.Id;
        await service.UpdateAsync(fishingDto);
        var drivingSkill = await service.GetByIdAsync(fishingSkill.Id);

        Assert.NotNull(drivingSkill);
        Assert.NotNull(drivingSkill.Ability);
        Assert.Equal("Driving", drivingSkill.Name);
        Assert.Equal("Piloting", drivingSkill.Ability.FullName);
    }

    [Fact]
    public async Task UpdateSkills_BadInputData_ShouldNotUpdate()
    {
        var options = GetInMemoryOptions("Skill_BadUpdateDB");

        using var context = new AppDbContext(options);
        var repo = new SkillRepository(context);
        var abilityRepo = new AbilityRepository(context);
        var service = new SkillService(repo, abilityRepo, context);

        var luckAbility = CreateAbility("Luck", "LK");
        await abilityRepo.CreateAsync(luckAbility);
        await context.SaveChangesAsync();

        var fishingDto = CreateSkillDto("Fishing", luckAbility.Id);
        var fishingSkill = await service.CreateAsync(fishingDto);

        var badId = CreateSkillDto("Fishing", luckAbility.Id, id: -1);
        var noName = CreateSkillDto("", luckAbility.Id, id: fishingSkill.Id);
        var whitespaceName = CreateSkillDto("   ", luckAbility.Id, id: fishingSkill.Id);
        var badAbilityId = CreateSkillDto("Fishing", -1, id: fishingSkill.Id);

        await Assert.ThrowsAsync<NullReferenceException>(() => service.UpdateAsync(badId));
        await Assert.ThrowsAsync<ArgumentException>(() => service.UpdateAsync(noName));
        await Assert.ThrowsAsync<ArgumentException>(() => service.UpdateAsync(whitespaceName));
        await Assert.ThrowsAsync<NullReferenceException>(() => service.UpdateAsync(badAbilityId));
    }

    [Fact]
    public async Task SortBy_WorksCorrectly()
    {
        var options = GetInMemoryOptions("Skill_SortDB");

        using var context = new AppDbContext(options);
        var repo = new SkillRepository(context);
        var abilityRepo = new AbilityRepository(context);
        var service = new SkillService(repo, abilityRepo, context);

        var intel = CreateAbility("Intelligence", "INT", sortWeight: 4);
        var str = CreateAbility("Strength", "STR", sortWeight: 1);
        var wis = CreateAbility("Wisdom", "WIS", sortWeight: 5);
        await abilityRepo.CreateAsync(intel);
        await abilityRepo.CreateAsync(str);
        await abilityRepo.CreateAsync(wis);
        await context.SaveChangesAsync();

        var arcanaDto = CreateSkillDto("Arcana", intel.Id);
        var historyDto = CreateSkillDto("History", intel.Id);
        var natureDto = CreateSkillDto("Nature", intel.Id);
        var religionDto = CreateSkillDto("Religion", intel.Id);
        var medicineDto = CreateSkillDto("Medicine", wis.Id);
        var insightDto = CreateSkillDto("Insight", wis.Id);
        var athleticsDto = CreateSkillDto("Athletics", str.Id);

        var arcana = await service.CreateAsync(arcanaDto);
        var history = await service.CreateAsync(historyDto);
        var nature = await service.CreateAsync(natureDto);
        var religion = await service.CreateAsync(religionDto);
        var medicine = await service.CreateAsync(medicineDto);
        var insight = await service.CreateAsync(insightDto);
        var athletics = await service.CreateAsync(athleticsDto);
        await context.SaveChangesAsync();

        // Act & Assert
        var allSkills = await service.GetAllAsync();

        allSkills = service.SortBy(allSkills, SkillService.SkillSorting.Name);
        Assert.NotNull(allSkills);
        string[] expectedOrder =
        [
            "Arcana",
            "Athletics",
            "History",
            "Insight",
            "Medicine",
            "Nature",
            "Religion",
        ];
        string[] actualOrder = [.. allSkills.Select(s => s.Name)];
        Assert.Equal(expectedOrder, actualOrder);

        allSkills = await service.GetAllWithAbilityAsync();
        allSkills = service.SortBy(allSkills, SkillService.SkillSorting.Ability);
        expectedOrder =
        [
            "Athletics",
            "Arcana",
            "History",
            "Nature",
            "Religion",
            "Insight",
            "Medicine"
        ];
        actualOrder = [.. allSkills.Select(s => s.Name)];
        Assert.Equal(expectedOrder, actualOrder);

        allSkills = service.SortBy(allSkills, SkillService.SkillSorting.Ability, true);
        expectedOrder =
        [
            "Medicine",
            "Insight",
            "Religion",
            "Nature",
            "History",
            "Arcana",
            "Athletics"
        ];
        actualOrder = [.. allSkills.Select(s => s.Name)];
        Assert.Equal(expectedOrder, actualOrder);
    }
}
