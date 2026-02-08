using Api.Middlewares.ExceptionHandling;
using Api.Models.DTOs.RequestDtos.Inventory;
using Api.Models.Items;
using Api.Models.Items.Constants;
using Api.Repositories.Interfaces;
using Api.Services.Interfaces.Items;
using Api.Services.Util;
using Api.Services.Util.Interfaces;

namespace Api.Services.Implemented.Items;
public class InventoryService(
    IInventoryRepository repo,
    IItemRepository itemRepo,
    ICharacterRepository characterRepo,
    ILogger<InventoryService> logger) : IInventoryService
{
    public async Task<Inventory> CreateAsync(CreateInventoryDto dto)
    {
        logger.LogInformation("Creating inventory for character with ID: {CharacterId}", dto.CharacterId);

        ICollection<EquipmentSlot> equipmentSlots = [
            new EquipmentSlot(){ Slot = EquipSlot.MainHand },
            new EquipmentSlot(){ Slot = EquipSlot.OffHand },
            new EquipmentSlot(){ Slot = EquipSlot.Ranged },
            new EquipmentSlot(){ Slot = EquipSlot.Armor },
            new EquipmentSlot(){ Slot = EquipSlot.Head },
            new EquipmentSlot(){ Slot = EquipSlot.Waist },
            new EquipmentSlot(){ Slot = EquipSlot.Hands },
            new EquipmentSlot(){ Slot = EquipSlot.Feet },
            new EquipmentSlot(){ Slot = EquipSlot.ArcaneFocus },
            new EquipmentSlot(){ Slot = EquipSlot.HolySymbol },
        ];

        for (int i = 0; i < dto.RingCap; i++)
        {
            equipmentSlots.Add(new EquipmentSlot() { Slot = EquipSlot.Rings });
        }
        for (int i = 0; i < dto.NecklaceCap; i++)
        {
            equipmentSlots.Add(new EquipmentSlot() { Slot = EquipSlot.Neck });
        }
        for (int i = 0; i < dto.BackEquipmentCap; i++)
        {
            equipmentSlots.Add(new EquipmentSlot() { Slot = EquipSlot.Back });
        }

        Inventory inv = new()
        {
            CharacterId = dto.CharacterId,
            Currency = new Currency { Copper = dto.CopperCoins },
            EquippedItems = equipmentSlots,
        };

        foreach (var itemId in dto.ItemIds)
        {
            var item = await itemRepo.GetByIdAsync(itemId);
                
            inv.StoredItems.Add(item);
        }
            
        CurrencyUtil.ConvertCurrency(inv.Currency);
        logger.LogInformation("Successfully created inventory for character with ID: {CharacterId}, Inventory ID: {InventoryId}", dto.CharacterId, inv.Id);
        return await repo.CreateAsync(inv);
    }

    public async Task<Inventory> AddItem(Inventory inventory, int itemId)
    {         
        var item = await itemRepo.GetByIdAsync(itemId) ;

        logger.LogInformation("Adding item with ID: {ItemId} to inventory with ID: {InventoryId}", itemId, inventory.Id);
        inventory.StoredItems.Add(item);
        inventory.TotalWeight += item.Weight;

        await repo.UpdateAsync(inventory);
        logger.LogInformation("Added item with ID: {ItemId} to inventory with ID: {InventoryId}", itemId, inventory.Id);   
        return inventory;
    }

    public async Task DiscardItem(Inventory inventory, int itemId)
    {
        var item = await itemRepo.GetByIdAsync(itemId) ;
        
        if(inventory.StoredItems.FirstOrDefault(i => i.Id == itemId) is null)
             throw new NotFoundException($"Item with id {itemId} could not be found in inventory with id {inventory.Id}");

        if (inventory.StoredItems.FirstOrDefault(i => i.Id == itemId) is null)
            throw new NotFoundException($"Item with id {itemId} could not be found in inventory with id {inventory.Id}");
        
        logger.LogInformation("Discarding item with ID: {ItemId} from inventory with ID: {InventoryId}", itemId, inventory.Id);
        await UnEquip(inventory, itemId);
        inventory.StoredItems.Remove(item);
        inventory.TotalWeight -= item.Weight;
        await repo.UpdateAsync(inventory);
        logger.LogInformation("Discarded item with ID: {ItemId} from inventory with ID: {InventoryId}", itemId, inventory.Id);
    }


    public async Task UnEquip(Inventory inventory, int itemId)
    {
        var item = await itemRepo.GetByIdAsync(itemId);
        
        logger.LogInformation("Unequipping item with ID: {ItemId} from inventory with ID: {InventoryId}", itemId, inventory.Id);

        foreach (var equipmentSlot in inventory.EquippedItems)
        {
            if (equipmentSlot.EquipmentId == itemId)
            {
                equipmentSlot.EquipmentId = null;
                inventory.AttunedItems += item.RequiresAttunement ? 1 : 0;
                await repo.UpdateAsync(inventory);
                logger.LogInformation("Unequipped item with ID: {ItemId} from inventory with ID: {InventoryId}", itemId, inventory.Id);
                return;
            }
        }
        throw new NotFoundException($"Item with id {itemId} is not equipped in inventory with id {inventory.Id}");
    }

    public async Task UnEquip(Inventory inventory, string slot)
    {
        var resolvedSlot = ConstantsUtil.ResolveOptionOrThrow(slot, EquipSlot.AllowedValues, "Equipment Slot");
        logger.LogInformation("Unequipping item from slot: {EquipmentSlot} in inventory with ID: {InventoryId}", resolvedSlot, inventory.Id);

        foreach (var equipmentSlot in inventory.EquippedItems)
        {
            if (equipmentSlot.Slot == resolvedSlot)
            {
                if(equipmentSlot.EquipmentId is null)
                    throw new NotFoundException($"No item is equipped in slot {resolvedSlot} in inventory with id {inventory.Id}");

                var item = await itemRepo.GetByIdAsync((int)equipmentSlot.EquipmentId);

                equipmentSlot.EquipmentId = null;
                inventory.AttunedItems += item.RequiresAttunement ? 1 : 0;
                await repo.UpdateAsync(inventory);
                logger.LogInformation("Unequipped item from slot: {EquipmentSlot} in inventory with ID: {InventoryId}", resolvedSlot, inventory.Id);
                return;
            }
        }
        throw new NotFoundException($"Could not find a slot of that type in the inventory with id {inventory.Id}");
    }

    public async Task<Inventory> Equip(Inventory inventory, int itemId, string slot)
    {
        var item = await itemRepo.GetByIdAsync(itemId) ;
        
        if(item is not IEquippable equippableItem)
            throw new InvalidOperationException($"Item with id {itemId} is not equippable");
        
        var resolvedSlot = ConstantsUtil.ResolveOptionOrThrow(slot, EquipSlot.AllowedValues, "Equipment Slot");

        if(equippableItem.MainSlot != resolvedSlot && equippableItem.SecondarySlot != resolvedSlot)
            throw new InvalidOperationException($"Item with id {itemId} cannot be equipped in slot {resolvedSlot}");

        logger.LogInformation("Equipping item with ID: {ItemId} to slot: {EquipmentSlot} in inventory with ID: {InventoryId}", itemId, resolvedSlot, inventory.Id);
        EquipmentSlot? firstSlotFound = null;

        foreach (var equipmentSlot in inventory.EquippedItems)
        {
            if (equipmentSlot.Slot == resolvedSlot)
            {
                firstSlotFound = equipmentSlot;
                if (equipmentSlot.EquipmentId == null)
                {
                    equipmentSlot.EquipmentId = itemId;
                    inventory.AttunedItems -= item.RequiresAttunement ? 1 : 0;
                    await repo.UpdateAsync(inventory);
                    logger.LogInformation("Equipped item with ID: {ItemId} to slot: {EquipmentSlot} in inventory with ID: {InventoryId}", itemId, resolvedSlot, inventory.Id);
                    return inventory;
                }
            }
        }
        if (firstSlotFound is not null)
        {
            firstSlotFound.EquipmentId = itemId;
            await repo.UpdateAsync(inventory);
            logger.LogInformation("Equipped item with ID: {ItemId} to slot: {EquipmentSlot} in inventory with ID: {InventoryId}", itemId, resolvedSlot, inventory.Id);
            return inventory;
        }

        throw new NotFoundException($"Could not find a slot of that type in the inventory with id {inventory.Id}");
    }

    public async Task<Inventory> Equip(Inventory inventory, int itemId)
    {
        var item = await itemRepo.GetByIdAsync(itemId) ;
        
        if(item is not IEquippable equippableItem)
            throw new ValidationException($"Item with id {itemId} is not equippable");
        
        logger.LogInformation("Equipping item with ID: {ItemId} to slot: {EquipmentSlot} in inventory with ID: {InventoryId}", itemId, equippableItem.MainSlot, inventory.Id);
        EquipmentSlot? firstSlotFound = null;
        EquipmentSlot? firstSecondarySlotFound = null;

        foreach (var equipmentSlot in inventory.EquippedItems)
        {
            if (equipmentSlot.Slot == equippableItem.MainSlot)
            {
                firstSlotFound = equipmentSlot;
                if (equipmentSlot.EquipmentId == null)
                {
                    equipmentSlot.EquipmentId = itemId;
                    inventory.AttunedItems -= item.RequiresAttunement ? 1 : 0;
                    await repo.UpdateAsync(inventory);
                    logger.LogInformation("Equipped item with ID: {ItemId} to slot: {EquipmentSlot} in inventory with ID: {InventoryId}", itemId, equippableItem.MainSlot, inventory.Id);
                    return inventory;
                }
            }
            else if (equippableItem.SecondarySlot is not null && equipmentSlot.Slot == equippableItem.SecondarySlot)
            {
                firstSecondarySlotFound = equipmentSlot;
            }
        }
        
        if (firstSlotFound is not null)
        {
            firstSlotFound.EquipmentId = itemId;
            await repo.UpdateAsync(inventory);
            logger.LogInformation("Equipped item with ID: {ItemId} to slot: {EquipmentSlot} in inventory with ID: {InventoryId}", itemId, equippableItem.MainSlot, inventory.Id);
            return inventory;
        } 
        else if (firstSecondarySlotFound is not null)
        {
            firstSecondarySlotFound.EquipmentId = itemId;
            await repo.UpdateAsync(inventory);
            logger.LogInformation("Equipped item with ID: {ItemId} to slot: {EquipmentSlot} in inventory with ID: {InventoryId}", itemId, equippableItem.SecondarySlot, inventory.Id);
            return inventory;
        }

        throw new NotFoundException($"Could not find a slot of that type in the inventory with id {inventory.Id}");
    }

    public async Task<Inventory> GetByCharacterIdAsync(int characterId)
    {
        var character = await characterRepo.GetByIdAsync(characterId);
        return await repo.GetByIdAsync(character.InventoryId);
    }

    public async Task DeleteAsync(int id)
    {
        var inventory = await repo.GetByIdAsync(id);
        logger.LogInformation("Deleting inventory with ID: {InventoryId}", id);
        await repo.DeleteAsync(inventory);
        logger.LogInformation("Successfully deleted inventory with ID: {InventoryId}", id);
    }
}