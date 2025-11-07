using static DndWebApp.Tests.Repositories.TestObjectFactory;
using DndWebApp.Api.Data;
using DndWebApp.Api.Models.Items.Enums;
using DndWebApp.Api.Repositories;
using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Repositories.Implemented;

namespace DndWebApp.Tests.Repositories;

public class BackgroundRepositoryTests
{
    [Fact]
    public async Task UpdateBackground_WorksCorrectly()
    {
        var options = GetInMemoryOptions("BG_UpdateDB");
        await using var context = new AppDbContext(options);
        var repo = new BackgroundRepository(context);

        // Arrange
        var bg = CreateTestBackground("Outlander");
        await repo.CreateAsync(bg);
        context.ChangeTracker.Clear();

        // Act
        var toUpdate = await repo.GetWithAllDataAsync(bg.Id);

        toUpdate!.Name = "Butler";
        toUpdate.StartingItems.Add(CreateTestItem("Golden spoon", ItemCategory.Art, description: "A golden spooon", quantity: 5));

        await repo.UpdateAsync(toUpdate);

        // Assert
        var updated = await repo.GetWithAllDataAsync(toUpdate.Id);

        Assert.NotNull(updated);
        Assert.Equal("Butler", updated.Name);
        Assert.NotNull(updated.StartingItems);
        Assert.Equal(5, updated.StartingItems.Count);
    }

    [Fact]
    public async Task DeleteBackground_ShouldDelete()
    {
        var options = GetInMemoryOptions("BG_DeleteDB");
        await using var context = new AppDbContext(options);
        var repo = new BackgroundRepository(context);

        // Arrange
        var bg = CreateTestBackground("Outlander");
        await repo.CreateAsync(bg);
        await context.SaveChangesAsync();

        // Act
        await repo.DeleteAsync(bg);
        await context.SaveChangesAsync();
        var deleted = await repo.GetWithAllDataAsync(bg.Id);

        // Assert
        Assert.Null(deleted);
    }

    [Fact]
    public async Task AddAndRetrieveBackground_WorksCorrectly()
    {
        var options = GetInMemoryOptions("BG_AddRetrieveDB");
        await using var context = new AppDbContext(options);
        var repo = new BackgroundRepository(context);

        // Arrange
        var background = CreateTestBackground("Acolyte");

        await repo.CreateAsync(background);
        await context.SaveChangesAsync();

        // Act
        var savedBackground = await repo.GetWithAllDataAsync(background.Id);

        // Assert
        Assert.NotNull(savedBackground);
        Assert.Equal("Acolyte", savedBackground!.Name);
        Assert.Equal("Acolyte description", savedBackground.Description);

        Assert.NotEmpty(savedBackground.StartingItems);
        Assert.NotEmpty(savedBackground.StartingItemsOptions);
    }

    [Fact]
    public async Task GetWithAllDataAsync_IncludesAllNavigationProperties()
    {
        var options = GetInMemoryOptions("BG_GetWithAllDataDB");
        await using var context = new AppDbContext(options);
        var repo = new BackgroundRepository(context);

        // Arrange
        var background = CreateTestBackground("Acolyte");
        background.Features.Add(CreateTestFeature());

        await repo.CreateAsync(background);
        await context.SaveChangesAsync();

        // Act
        var fullBackground = await repo.GetWithAllDataAsync(background.Id);

        // Assert
        Assert.NotNull(fullBackground);

        Assert.NotEmpty(fullBackground!.StartingItems);
        var holySymbol = fullBackground.StartingItems.FirstOrDefault(i => i.Name == "Holy Symbol");
        Assert.NotNull(holySymbol);
        Assert.Equal(1, holySymbol!.Quantity);

        var incense = fullBackground.StartingItems.FirstOrDefault(i => i.Name == "Incense Sticks");
        Assert.NotNull(incense);
        Assert.Equal(5, incense!.Quantity);

        Assert.NotEmpty(fullBackground.Features);
        var shelter = fullBackground.Features.FirstOrDefault(f => f.Name == "Shelter of the Faithful");
        Assert.NotNull(shelter);
        Assert.Equal("As an acolyte...", shelter!.Description);
    }
}
