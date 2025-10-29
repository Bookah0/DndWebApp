using static DndWebApp.Tests.Repositories.TestObjectFactory;
using DndWebApp.Api.Data;
using DndWebApp.Api.Models.Items.Enums;
using DndWebApp.Api.Repositories.Classes;

namespace DndWebApp.Tests.Repositories;

public class ClassRepositoryTests
{
    [Fact]
    public async Task UpdateClass_WorksCorrectly()
    {
        var options = GetInMemoryOptions("Class_AddRetrieveDB");
        await using var context = new AppDbContext(options);
        var repo = new ClassRepository(context);
        
        // Arrange
        var cls = CreateTestClass();
        await repo.CreateAsync(cls);
        context.ChangeTracker.Clear();

        // Act
        var toUpdate = await repo.GetWithAllDataAsync(cls.Id);
        toUpdate!.Name = "Barbarian";
        toUpdate.StartingEquipment.Clear();

        await repo.UpdateAsync(toUpdate);

        // Assert
        var updated = await repo.GetWithAllDataAsync(toUpdate.Id);

        Assert.NotNull(updated);
        Assert.Equal("Barbarian", updated.Name);
        Assert.NotNull(updated.StartingEquipment);
        Assert.Empty(updated.StartingEquipment);
    }

    [Fact]
    public async Task DeleteClass_ShouldDelete()
    {
        var options = GetInMemoryOptions("Class_DeleteDB");
        await using var context = new AppDbContext(options);
        var repo = new ClassRepository(context);

        // Arrange
        var cls = CreateTestClass();
        await repo.CreateAsync(cls);

        // Act
        await repo.DeleteAsync(cls);
        var deleted = await repo.GetWithAllDataAsync(cls.Id);

        // Assert
        Assert.Null(deleted);
    }
    
    [Fact]
    public async Task AddAndRetrieveClass_WorksCorrectly()
    {
        var options = GetInMemoryOptions("Class_AddRetrieveDB");
        await using var context = new AppDbContext(options);
        var repo = new ClassRepository(context);
        
        // Arrange
        var cls = CreateTestClass();
        cls.StartingEquipment.Add(CreateTestItem("Thieves' Kit", ItemCategory.Tools));
        cls.StartingEquipmentOptions.Add(CreateTestStartingEquipmentChoice());
        cls.ClassLevels.Add(CreateTestLevel(cls));

        // Act
        await repo.CreateAsync(cls);
        var savedClass = await repo.GetByIdAsync(cls.Id);

        // Assert
        Assert.NotNull(savedClass);
        Assert.Equal("Ranger", savedClass!.Name);
        Assert.Equal("Description", savedClass.Description);

        Assert.Equal("Thieves' Kit", savedClass.StartingEquipment.First().Name);
        Assert.NotEmpty(savedClass.StartingEquipmentOptions);
        Assert.NotEmpty(savedClass.ClassLevels);
        Assert.Equal(savedClass.Id, savedClass.ClassLevels.First().ClassId);
    }

    [Fact]
    public async Task GetDtoDataAsync_ReturnsCorrectValues()
    {
        var options = GetInMemoryOptions("Class_DtoDataDB");
        await using var context = new AppDbContext(options);
        var repo = new ClassRepository(context);

        // Arrange
        var cls = CreateTestClass();

        // Act
        await repo.CreateAsync(cls);
        var dto = await repo.GetDtoAsync(cls.Id);

        // Assert
        Assert.NotNull(dto);
        Assert.Equal("Ranger", dto!.Name);
        Assert.Equal("Description", dto.Description);
        Assert.Equal("1d8", dto.HitDie);
        Assert.False(dto.IsHomebrew);
    }

    [Fact]
    public async Task GetAllDtoDataAsync_ReturnsAllClasss()
    {
        var options = GetInMemoryOptions("Class_GetAllDtoDB");
        await using var context = new AppDbContext(options);
        var repo = new ClassRepository(context);

        // Arrange
        var c1 = CreateTestClass();
        var c2 = CreateTestClass(name: "Rogue");

        // Act
        await repo.CreateAsync(c1);
        await repo.CreateAsync(c2);
        var allDtos = await repo.GetAllDtosAsync();

        // Assert
        Assert.Equal(2, allDtos.Count);
        Assert.Contains(allDtos, b => b.Name == "Ranger");
        Assert.Contains(allDtos, b => b.Name == "Rogue");
    }

    [Fact]
    public async Task GetWithAllDataAsync_IncludesAllNavigationProperties()
    {
        var options = GetInMemoryOptions("Class_GetWithAllDataDB");
        await using var context = new AppDbContext(options);
        var repo = new ClassRepository(context);
        
        // Arrange
        var cls = CreateTestClass();
        cls.StartingEquipment.Add(CreateTestItem("Thieves' Kit", ItemCategory.Tools));
        cls.StartingEquipmentOptions.Add(CreateTestStartingEquipmentChoice());
        cls.ClassLevels.Add(CreateTestLevel(cls));

        await repo.CreateAsync(cls);

        // Act
        var fullClass = await repo.GetWithAllDataAsync(cls.Id);
 
        var tool = fullClass!.StartingEquipment.FirstOrDefault(i => i.Name == "Thieves' Kit");
        var prayerChoice = fullClass.StartingEquipmentOptions.FirstOrDefault(o => o.Description.Contains("armor or weapon"));

        // Assert
        Assert.NotNull(fullClass);
        Assert.NotEmpty(fullClass!.StartingEquipment);

        Assert.NotNull(tool);
        Assert.Equal(ItemCategory.Tools, tool!.Categories.First());

        Assert.NotEmpty(fullClass.StartingEquipmentOptions);
        Assert.NotNull(prayerChoice);
        Assert.Equal(2, prayerChoice!.Options.Count);
        Assert.Contains(prayerChoice.Options, c => c.Name == "Leather Armor");
        Assert.Contains(prayerChoice.Options, c => c.Name == "Shortbow");

        Assert.NotEmpty(fullClass.ClassLevels);
        Assert.NotNull(fullClass.ClassLevels.FirstOrDefault(l => l.Level == 2));
        Assert.NotNull(fullClass.ClassLevels.FirstOrDefault(l => l.ProficiencyBonus == 3));
    }
}
