using DndWebApp.Api.Data;
using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.Items;
using DndWebApp.Api.Models.Items.Enums;
using DndWebApp.Api.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DndWebApp.Tests.Repositories;

public class ItemRepositoryTests
{
    private Item CreateTestItem() => new Item
    {
        Name = "Spoon",
        Description = "A simple metal spoon, useful for eating or mixing potions.",
        Catagories = ItemCategory.AdventuringGear
    };

    private Armor CreateTestArmor() => new Armor
    {
        Name = "Leather Armor",
        Description = "Light armor made from tanned leather, provides basic protection.",
        Catagories = ItemCategory.Armor,
        Category = ArmorCategory.Light,
        BaseArmorClass = 11,
        PlusDexMod = true
    };

    private Weapon CreateTestWeapon() => new Weapon
    {
        Name = "Shortbow",
        Description = "A small bow ideal for ranged attacks.",
        Catagories = ItemCategory.Weapon,
        WeaponCategories = WeaponCategory.SimpleRanged | WeaponCategory.Shortbow,
        Properties = WeaponProperty.TwoHanded,
        DamageTypes = DamageType.Piercing,
        DamageDice = "1d6",
        Range = 80
    };

    private Tool CreateTestTool() => new Tool
    {
        Name = "Thieves' Kit",
        Description = "A set of lockpicks and other tools for stealthy operations.",
        Catagories = ItemCategory.Tools,
        Activities = [],
        Properties = []
    };

    private DbContextOptions<AppDbContext> GetInMemoryOptions(string dbName) => new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
            .Options;

    [Fact]
    public async Task AddAndRetrieveItems_WorksCorrectly()
    {
        // Arrange
        var options = GetInMemoryOptions("Race_AddRetrieveDB");
        await using var context = new AppDbContext(options);

        var item = CreateTestItem();
        var armor = CreateTestArmor();
        var weapon = CreateTestWeapon();
        var tool = CreateTestTool();

        // Act
        var itemRepo = new ItemRepository(context);
        var armorRepo = new ArmorRepository(context);
        var weaponRepo = new WeaponRepository(context);
        var toolRepo = new ToolRepository(context);

        await itemRepo.CreateAsync(item);
        await armorRepo.CreateAsync(armor);
        await weaponRepo.CreateAsync(weapon);
        await toolRepo.CreateAsync(tool);
        await context.SaveChangesAsync();

        // Assert
        var savedItem = await itemRepo.GetByIdAsync(item.Id);
        var savedArmor = await armorRepo.GetByIdAsync(armor.Id);
        var savedWeapon = await weaponRepo.GetByIdAsync(weapon.Id);
        var savedTool = await toolRepo.GetByIdAsync(tool.Id);

        // Item
        Assert.NotNull(savedItem);
        Assert.Equal("Spoon", savedItem!.Name);
        Assert.Equal("A simple metal spoon, useful for eating or mixing potions.", savedItem.Description);
        Assert.Equal(ItemCategory.AdventuringGear, savedItem.Catagories);

        // Armor
        Assert.NotNull(savedArmor);
        Assert.Equal("Leather Armor", savedArmor!.Name);
        Assert.Equal("Light armor made from tanned leather, provides basic protection.", savedArmor.Description);
        Assert.Equal(ItemCategory.Armor, savedArmor.Catagories);
        Assert.Equal(ArmorCategory.Light, savedArmor.Category);
        Assert.Equal(11, savedArmor.BaseArmorClass);
        Assert.True(savedArmor.PlusDexMod);

        // Weapon
        Assert.NotNull(savedWeapon);
        Assert.Equal("Shortbow", savedWeapon!.Name);
        Assert.Equal("A small bow ideal for ranged attacks.", savedWeapon.Description);
        Assert.Equal(ItemCategory.Weapon, savedWeapon.Catagories);
        Assert.Equal(WeaponCategory.SimpleRanged | WeaponCategory.Shortbow, savedWeapon.WeaponCategories);
        Assert.Equal(WeaponProperty.TwoHanded, savedWeapon.Properties);
        Assert.Equal(DamageType.Piercing, savedWeapon.DamageTypes);
        Assert.Equal("1d6", savedWeapon.DamageDice);
        Assert.Equal(80, savedWeapon.Range);

        // Tool
        Assert.NotNull(savedTool);
        Assert.Equal("Thieves' Kit", savedTool!.Name);
        Assert.Equal("A set of lockpicks and other tools for stealthy operations.", savedTool.Description);
        Assert.Equal(ItemCategory.Tools, savedTool.Catagories);
    }

    [Fact]
    public async Task GetAllItems_WorksCorrectly()
    {
        // Arrange
        var options = GetInMemoryOptions("Items_GetAllDB");
        await using var context = new AppDbContext(options);

        var item = CreateTestItem();
        var armor = CreateTestArmor();
        var weapon = CreateTestWeapon();
        var tool = CreateTestTool();

        var itemRepo = new ItemRepository(context);
        var armorRepo = new ArmorRepository(context);
        var weaponRepo = new WeaponRepository(context);
        var toolRepo = new ToolRepository(context);

        await itemRepo.CreateAsync(item);
        await armorRepo.CreateAsync(armor);
        await weaponRepo.CreateAsync(weapon);
        await toolRepo.CreateAsync(tool);
        await context.SaveChangesAsync();

        // Act
        var allItems = await itemRepo.GetAllAsync();
        var allArmor = await armorRepo.GetAllAsync();
        var allWeapons = await weaponRepo.GetAllAsync();
        var allTools = await toolRepo.GetAllAsync();

        // Assert
        Assert.NotNull(allItems);
        Assert.Single(allItems);
        Assert.Equal("Spoon", allItems.First().Name);

        Assert.NotNull(allArmor);
        Assert.Single(allArmor);
        Assert.Equal("Leather Armor", allArmor.First().Name);

        Assert.NotNull(allWeapons);
        Assert.Single(allWeapons);
        Assert.Equal("Shortbow", allWeapons.First().Name);

        Assert.NotNull(allTools);
        Assert.Single(allTools);
        Assert.Equal("Thieves' Kit", allTools.First().Name);
    }

    [Fact]
    public async Task UpdateItems_WorksCorrectly()
    {
        // Arrange
        var options = GetInMemoryOptions("Items_UpdateDB");
        await using var context = new AppDbContext(options);

        var item = CreateTestItem();
        var armor = CreateTestArmor();
        var weapon = CreateTestWeapon();
        var tool = CreateTestTool();

        var itemRepo = new ItemRepository(context);
        var armorRepo = new ArmorRepository(context);
        var weaponRepo = new WeaponRepository(context);
        var toolRepo = new ToolRepository(context);

        await itemRepo.CreateAsync(item);
        await armorRepo.CreateAsync(armor);
        await weaponRepo.CreateAsync(weapon);
        await toolRepo.CreateAsync(tool);
        await context.SaveChangesAsync();

        // Act
        item.Name = "Golden Spoon";
        item.Description = "A fancy golden spoon.";

        armor.BaseArmorClass = 12;
        armor.PlusDexMod = false;

        weapon.DamageDice = "1d8";
        weapon.Range = 100;

        tool.Description = "Updated set of lockpicks and stealth tools.";

        await itemRepo.UpdateAsync(item);
        await armorRepo.UpdateAsync(armor);
        await weaponRepo.UpdateAsync(weapon);
        await toolRepo.UpdateAsync(tool);
        await context.SaveChangesAsync();

        // Assert
        var savedItem = await itemRepo.GetByIdAsync(item.Id);
        var savedArmor = await armorRepo.GetByIdAsync(armor.Id);
        var savedWeapon = await weaponRepo.GetByIdAsync(weapon.Id);
        var savedTool = await toolRepo.GetByIdAsync(tool.Id);

        // Item
        Assert.NotNull(savedItem);
        Assert.Equal("Golden Spoon", savedItem!.Name);
        Assert.Equal("A fancy golden spoon.", savedItem.Description);

        // Armor
        Assert.NotNull(savedArmor);
        Assert.Equal(12, savedArmor!.BaseArmorClass);
        Assert.False(savedArmor.PlusDexMod);

        // Weapon
        Assert.NotNull(savedWeapon);
        Assert.Equal("1d8", savedWeapon!.DamageDice);
        Assert.Equal(100, savedWeapon.Range);

        // Tool
        Assert.NotNull(savedTool);
        Assert.Equal("Updated set of lockpicks and stealth tools.", savedTool!.Description);
    }

    [Fact]
    public async Task DeleteItems_WorksCorrectly()
    {
        // Arrange
        var options = GetInMemoryOptions("Items_DeleteDB");
        await using var context = new AppDbContext(options);

        var item = CreateTestItem();
        var armor = CreateTestArmor();
        var weapon = CreateTestWeapon();
        var tool = CreateTestTool();

        var itemRepo = new ItemRepository(context);
        var armorRepo = new ArmorRepository(context);
        var weaponRepo = new WeaponRepository(context);
        var toolRepo = new ToolRepository(context);

        await itemRepo.CreateAsync(item);
        await armorRepo.CreateAsync(armor);
        await weaponRepo.CreateAsync(weapon);
        await toolRepo.CreateAsync(tool);
        await context.SaveChangesAsync();

        // Act
        await itemRepo.DeleteAsync(item);
        await armorRepo.DeleteAsync(armor);
        await weaponRepo.DeleteAsync(weapon);
        await toolRepo.DeleteAsync(tool);
        await context.SaveChangesAsync();

        // Assert
        var deletedItem = await itemRepo.GetByIdAsync(item.Id);
        var deletedArmor = await armorRepo.GetByIdAsync(armor.Id);
        var deletedWeapon = await weaponRepo.GetByIdAsync(weapon.Id);
        var deletedTool = await toolRepo.GetByIdAsync(tool.Id);

        Assert.Null(deletedItem);
        Assert.Null(deletedArmor);
        Assert.Null(deletedWeapon);
        Assert.Null(deletedTool);
    }

    [Fact]
    public async Task RetrieveToolWithAllData_ShouldHaveCorrectActivitiesAndProperties()
    {
        // Arrange
        var options = GetInMemoryOptions("GetAllWithCollections_AddRetrieveDB");

        var tool = CreateTestTool();
        var activity1 = new ToolActivity { Title = "Pick a lock", DC = "Varies" };
        var activity2 = new ToolActivity { Title = "Disable a trap", DC = "Varies" };
        tool.Activities.Add(activity1);
        tool.Activities.Add(activity2);
        var property1 = new ToolProperty { Title = "Components", Description = "Thieves' tools include a small file, a set of lock picks, a small mirror mounted on a metal handle, a set of narrow-bladed scissors, and a pair of pliers" };
        var property2 = new ToolProperty { Title = "History", Description = "Your knowledge of traps grants you insight when answering questions about locations that are renowned for their traps." };
        tool.Properties.Add(property1);
        tool.Properties.Add(property2);

        // Act
        await using var context = new AppDbContext(options);

        var repo = new ToolRepository(context);
        await repo.CreateAsync(tool);
        await context.SaveChangesAsync();

        // Assert
        var retrievedTool = await repo.GetWithAllDataAsync(tool.Id);

        Assert.NotNull(retrievedTool);
        Assert.Equal("Thieves' Kit", retrievedTool!.Name);

        Assert.NotNull(retrievedTool.Activities);
        Assert.Equal(2, retrievedTool.Activities.Count);
        Assert.Contains(retrievedTool.Activities, a => a.Title == "Pick a lock");
        Assert.Contains(retrievedTool.Activities, a => a.Title == "Disable a trap");

        Assert.NotNull(retrievedTool.Properties);
        Assert.Equal(2, retrievedTool.Properties.Count);
        Assert.Contains(retrievedTool.Properties, a => a.Title == "Components");
        Assert.Contains(retrievedTool.Properties, a => a.Description == "Your knowledge of traps grants you insight when answering questions about locations that are renowned for their traps.");
    }
}
