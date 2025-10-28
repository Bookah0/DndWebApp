using DndWebApp.Api.Data;
using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.Features;
using DndWebApp.Api.Models.Items;
using DndWebApp.Api.Models.Items.Enums;
using DndWebApp.Api.Repositories.Backgrounds;
using Microsoft.EntityFrameworkCore;

namespace DndWebApp.Tests.Repositories;

public class BackgroundRepositoryTests
{
    private Item CreateTestItem(string name, string description, ItemCategory category, int quantity)
    {
        return new Item { Name = name, Description = description, Categories = [category], Quantity = quantity };
    }

    private BackgroundFeature CreateTestFeature(string name = "Shelter of the Faithful", string description = "As an acolyte, you command the respect of those who share your faith, and you can perform the religious ceremonies of your deity.", Background bg = null!, int bgId = -1)
    {
        return new BackgroundFeature { Name = name, Description = description, Background = bg, BackgroundId = bgId};
    }

    private Item CreateTestStartingItem(string name = "Holy Symbol", ItemCategory category = ItemCategory.Utility, int quantity = 1)
    {
        return CreateTestItem(name, $"{name} description", category, quantity);
    }

    private ItemChoice CreateTestStartingItemChoice()
    {
        var option = new ItemChoice
        {
            Description = "a prayer book or prayer wheel",
            NumberOfChoices = 2,
            Options = []
        };

        option.Options.Add(CreateTestStartingItem("Prayer Book", ItemCategory.None));
        option.Options.Add(CreateTestStartingItem("Prayer Wheel", ItemCategory.None));

        return option;
    }

    private Background CreateTestBackground(string name)
    {
        var description = $"{name} description";
        var background = new Background
        {
            Name = name,
            Description = description,
            StartingCurrency = new() { Gold = 15 }
        };

        background.StartingItems.Add(CreateTestStartingItem("Holy Symbol", ItemCategory.Utility));
        background.StartingItems.Add(CreateTestStartingItem("Incense Sticks", ItemCategory.None, 5));
        background.StartingItems.Add(CreateTestStartingItem("Vestments", ItemCategory.None));
        background.StartingItems.Add(CreateTestStartingItem("Common Clothes", ItemCategory.None));
        background.StartingItemsOptions.Add(CreateTestStartingItemChoice());

        background.Features.Add(CreateTestFeature());

        return background;
    }


    private DbContextOptions<AppDbContext> GetInMemoryOptions(string dbName)
    {
        return new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .Options;
    }

[Fact]
    public async Task UpdateBackground_WorksCorrectly()
    {
        // Arrange
        var options = GetInMemoryOptions("BG_UpdateDB");
        var bg = CreateTestBackground("Outlander");

        await using var context = new AppDbContext(options);
        var repo = new BackgroundRepository(context);
        await repo.CreateAsync(bg);
        await context.SaveChangesAsync();

        // Act
        var toUpdate = await repo.GetWithAllDataAsync(bg.Id);

        toUpdate!.Name = "Butler";
        toUpdate.StartingItems.Add(CreateTestItem("Golden spoon", "A golden spooon", ItemCategory.Art, 5));

        await repo.UpdateAsync(toUpdate);
        await context.SaveChangesAsync();

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
            var primitive = await repo.GetPrimitiveDataAsync(background.Id);

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
            var allPrimitives = await repo.GetAllPrimitiveDataAsync();

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
            Assert.Equal("As an acolyte, you command the respect of those who share your faith, and you can perform the religious ceremonies of your deity.", shelter!.Description);
        }
    }
}
