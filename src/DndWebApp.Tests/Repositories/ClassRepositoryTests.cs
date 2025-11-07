using static DndWebApp.Tests.Repositories.TestObjectFactory;
using DndWebApp.Api.Data;
using DndWebApp.Api.Models.Items.Enums;
using DndWebApp.Api.Repositories;
using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Repositories.Implemented.Classes;

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
        cls.ClassLevels.Add(CreateTestLevel(cls));

        // Act
        await repo.CreateAsync(cls);
        var savedClass = await repo.GetWithAllDataAsync(cls.Id);

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
    public async Task GetWithAllDataAsync_IncludesAllNavigationProperties()
    {
        var options = GetInMemoryOptions("Class_GetWithAllDataDB");
        await using var context = new AppDbContext(options);
        var repo = new ClassRepository(context);

        // Arrange
        var cls = CreateTestClass();
        cls.StartingEquipment.Add(CreateTestItem("Thieves' Kit", ItemCategory.Tools));
        cls.ClassLevels.Add(CreateTestLevel(cls));

        await repo.CreateAsync(cls);

        // Act
        var fullClass = await repo.GetWithAllDataAsync(cls.Id);

        var tool = fullClass!.StartingEquipment.FirstOrDefault(i => i.Name == "Thieves' Kit");

        // Assert
        Assert.NotNull(fullClass);
        Assert.NotEmpty(fullClass!.StartingEquipment);

        Assert.NotNull(tool);
        Assert.Equal(ItemCategory.Tools, tool!.Categories.First());

        Assert.NotEmpty(fullClass.ClassLevels);
        Assert.NotNull(fullClass.ClassLevels.FirstOrDefault(l => l.Level == 2));
        Assert.NotNull(fullClass.ClassLevels.FirstOrDefault(l => l.ProficiencyBonus == 3));
    }
}
