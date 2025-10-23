using DndWebApp.Api.Data;
using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.Items;
using DndWebApp.Api.Models.Items.Enums;
using DndWebApp.Api.Models.World;
using DndWebApp.Api.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DndWebApp.Tests.Repositories;

public class BackgroundRepositoryTests
{
    private Item CreateTestItem(string name, string description, ItemCategory category, int quantity)
    {
        return new Item { Name = name, Description = description, Catagories = category, Quantity = quantity };
    }

    private Feature CreateTestFeature(string name = "Shelter of the Faithful", string description = "As an acolyte, you command the respect of those who share your faith, and you can perform the religious ceremonies of your deity.")
    {
        return new Feature { Name = name, Description = description };
    }

    private Item CreateTestStartingItem(string name = "Holy Symbol", ItemCategory categories = ItemCategory.Utility, int quantity = 1)
    {
        return CreateTestItem(name, $"{name} description", categories, quantity);
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
    public async Task AddAndRetrieveBackground_WorksCorrectly()
    {
        // Arrange
        var options = GetInMemoryOptions("Background_AddRetrieveDB");
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
        var options = GetInMemoryOptions("Background_PrimitiveDataDB");
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
        var options = GetInMemoryOptions("Background_GetAllPrimitiveDB");
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
        var options = GetInMemoryOptions("Background_GetWithAllDataDB");
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
