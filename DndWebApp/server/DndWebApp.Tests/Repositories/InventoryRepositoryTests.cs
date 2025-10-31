using static DndWebApp.Tests.Repositories.TestObjectFactory;
using DndWebApp.Api.Data;
using DndWebApp.Api.Models.Items.Enums;
using DndWebApp.Api.Repositories.Items;
using DndWebApp.Api.Models.Items;
using DndWebApp.Api.Repositories;

namespace DndWebApp.Tests.Repositories;

public class InventoryRepositoryTests
{
    [Fact]
    public async Task AddAndRetrieveItems_WorksCorrectly()
    {
        var options = GetInMemoryOptions("Inventory_AddRetrieveDB");
        await using var context = new AppDbContext(options);
        var efRepo = new EfRepository<Inventory>(context);
        var invRepo = new InventoryRepository(context, efRepo);

        // Arrange
        var inv = CreateTestInventory();

        // Act
        await invRepo.CreateAsync(inv);

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
        Assert.Equal(ItemCategory.Tools, withStorage.Gear.First().Categories.First());
        Assert.Equal("Shortbow", withStorage.Equipment.First().Name);
        Assert.Equal(ItemCategory.Tools, withAll.Gear.First().Categories.First());
        Assert.Equal("Shortbow", withAll.Equipment.First().Name);
    }
}
