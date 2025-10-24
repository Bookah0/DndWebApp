using DndWebApp.Api.Data;
using DndWebApp.Api.Models.Items;
using DndWebApp.Api.Models.Items.Enums;
using DndWebApp.Api.Repositories.Items;
using Microsoft.EntityFrameworkCore;

namespace DndWebApp.Tests.Repositories;

public class InventoryRepositoryTests
{
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

    private Inventory CreateTestInventory(){
        var inv = new Inventory { Currency = new() };
        inv.EquippedArmor = CreateTestArmor();
        inv.Equipment.Add(CreateTestWeapon());
        inv.Gear.Add(CreateTestTool());
        return inv;
    }

    private DbContextOptions<AppDbContext> GetInMemoryOptions(string dbName) => new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
            .Options;

    [Fact]
    public async Task AddAndRetrieveItems_WorksCorrectly()
    {
        // Arrange
        var options = GetInMemoryOptions("Inventory_AddRetrieveDB");
        await using var context = new AppDbContext(options);

        var inv = CreateTestInventory();

        // Act
        var invRepo = new InventoryRepository(context);
        await invRepo.CreateAsync(inv);
        await context.SaveChangesAsync();

        var savedInventory = await invRepo.GetByIdAsync(inv.Id);
        var withEquipped = await invRepo.GetWithEquippedItemsAsync(inv.Id);
        var withStorage = await invRepo.GetWithStoredItemsAsync(inv.Id);
        var withAll = await invRepo.GetWithAllDataAsync(inv.Id);

        // Assert
        Assert.NotNull(savedInventory);
        Assert.NotNull(withEquipped);
        Assert.NotNull(withStorage);
        Assert.NotNull(withAll);

        Assert.Equal("Leather Armor", withEquipped.EquippedArmor!.Name);
        Assert.Equal("Leather Armor", withAll.EquippedArmor!.Name);
        Assert.Null(withStorage.EquippedArmor);

        Assert.Empty(withEquipped.Gear);
        Assert.Empty(withEquipped.Equipment);
        Assert.Equal(ItemCategory.Tools, withStorage.Gear.First().Catagories);
        Assert.Equal("Shortbow", withStorage.Equipment.First().Name);
        Assert.Equal(ItemCategory.Tools, withAll.Gear.First().Catagories);
        Assert.Equal("Shortbow", withAll.Equipment.First().Name);
    }
}
