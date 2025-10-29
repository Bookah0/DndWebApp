using static DndWebApp.Tests.Repositories.TestObjectFactory;
using DndWebApp.Api.Data;
using DndWebApp.Api.Models.Items.Enums;
using DndWebApp.Api.Repositories.Backgrounds;

namespace DndWebApp.Tests.Repositories;

public class BackgroundRepositoryTests
{
    [Fact]
    public async Task UpdateBackground_WorksCorrectly()
    {
        // Arrange
        var options = GetInMemoryOptions("BG_UpdateDB");
        var bg = CreateTestBackground("Outlander");

        await using var context = new AppDbContext(options);
        var repo = new BackgroundRepository(context);
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

        var bg = CreateTestBackground("Outlander");

        await using var context = new AppDbContext(options);
        var repo = new BackgroundRepository(context);
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
        // Arrange
        var options = GetInMemoryOptions("BG_AddRetrieveDB");
        var background = CreateTestBackground("Acolyte");

        await using (var context = new AppDbContext(options))
        {
            var repo = new BackgroundRepository(context);
            await repo.CreateAsync(background);
            await context.SaveChangesAsync();
        }

        // Act
        await using (var context = new AppDbContext(options))
        {
            var repo = new BackgroundRepository(context);
            var savedBackground = await repo.GetByIdAsync(background.Id);

            // Assert
            Assert.NotNull(savedBackground);
            Assert.Equal("Acolyte", savedBackground!.Name);
            Assert.Equal("Acolyte description", savedBackground.Description);

            Assert.Empty(savedBackground.StartingItems);        // Default value
            Assert.Empty(savedBackground.StartingItemsOptions); // Default value
        }
    }

    [Fact]
    public async Task GetPrimitiveDataAsync_ReturnsCorrectValues()
    {
        // Arrange
        var options = GetInMemoryOptions("BG_PrimitiveDataDB");
        var background = CreateTestBackground("Acolyte");

        await using (var context = new AppDbContext(options))
        {
            var repo = new BackgroundRepository(context);
            await repo.CreateAsync(background);
            await context.SaveChangesAsync();
        }

        // Act
        await using (var context = new AppDbContext(options))
        {
            var repo = new BackgroundRepository(context);
            var primitive = await repo.GetDtoAsync(background.Id);

            // Assert
            Assert.NotNull(primitive);
            Assert.Equal("Acolyte", primitive!.Name);
            Assert.Equal("Acolyte description", primitive.Description);
            Assert.False(primitive.IsHomebrew);
        }
    }

    [Fact]
    public async Task GetAllPrimitiveDataAsync_ReturnsAllBackgrounds()
    {
        // Arrange
        var options = GetInMemoryOptions("BG_GetAllPrimitiveDB");
        var bg1 = CreateTestBackground("Acolyte");
        var bg2 = CreateTestBackground("Soldier");

        await using (var context = new AppDbContext(options))
        {
            var repo = new BackgroundRepository(context);
            await repo.CreateAsync(bg1);
            await repo.CreateAsync(bg2);
            await context.SaveChangesAsync();
        }

        // Act
        await using (var context = new AppDbContext(options))
        {
            var repo = new BackgroundRepository(context);
            var allPrimitives = await repo.GetAllDtosAsync();

            // Assert
            Assert.Equal(2, allPrimitives.Count);
            Assert.Contains(allPrimitives, b => b.Name == "Acolyte");
            Assert.Contains(allPrimitives, b => b.Name == "Soldier");
        }
    }

    [Fact]
    public async Task GetWithAllDataAsync_IncludesAllNavigationProperties()
    {
        // Arrange
        var options = GetInMemoryOptions("BG_GetWithAllDataDB");
        var background = CreateTestBackground("Acolyte");
        background.Features.Add(CreateTestFeature());

        await using (var context = new AppDbContext(options))
        {
            var repo = new BackgroundRepository(context);
            await repo.CreateAsync(background);
            await context.SaveChangesAsync();
        }

        // Act
        await using (var context = new AppDbContext(options))
        {
            var repo = new BackgroundRepository(context);
            var fullBackground = await repo.GetWithAllDataAsync(background.Id);

            // Assert
            Assert.NotNull(fullBackground);

            // Starting Items
            Assert.NotEmpty(fullBackground!.StartingItems);
            var holySymbol = fullBackground.StartingItems.FirstOrDefault(i => i.Name == "Holy Symbol");
            Assert.NotNull(holySymbol);
            Assert.Equal(1, holySymbol!.Quantity);

            var incense = fullBackground.StartingItems.FirstOrDefault(i => i.Name == "Incense Sticks");
            Assert.NotNull(incense);
            Assert.Equal(5, incense!.Quantity);

            // Starting Item Choices
            Assert.NotEmpty(fullBackground.StartingItemsOptions);
            var prayerChoice = fullBackground.StartingItemsOptions
                .FirstOrDefault(o => o.Description.Contains("prayer book or prayer wheel"));
            Assert.NotNull(prayerChoice);
            Assert.Equal(2, prayerChoice!.Options.Count);
            Assert.Contains(prayerChoice.Options, c => c.Name == "Prayer Book");
            Assert.Contains(prayerChoice.Options, c => c.Name == "Prayer Wheel");

            // Features
            Assert.NotEmpty(fullBackground.Features);
            var shelter = fullBackground.Features.FirstOrDefault(f => f.Name == "Shelter of the Faithful");
            Assert.NotNull(shelter);
            Assert.Equal("As an acolyte...", shelter!.Description);
        }
    }
}
