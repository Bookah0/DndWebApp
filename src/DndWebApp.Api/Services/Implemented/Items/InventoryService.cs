using DndWebApp.Api.Middlewares.ExceptionHandling;
using DndWebApp.Api.Models.DTOs.Inventory;
using DndWebApp.Api.Models.Items;
using DndWebApp.Api.Models.Items.Constants;
using DndWebApp.Api.Repositories.Interfaces;
using DndWebApp.Api.Services.Interfaces.Items;
using DndWebApp.Api.Services.Util;
using DndWebApp.Api.Services.Util.Interfaces;

namespace DndWebApp.Api.Services.Implemented.Items;
public class InventoryService(
    IInventoryRepository repo,
    IItemRepository itemRepo,
    ICharacterRepository characterRepo,
    ILogger<InventoryService> logger) : IInventoryService
{
    public async Task<Inventory> CreateAsync(CreateInventoryDto dto)
    {
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
            var item = await itemRepo.GetByIdAsync(itemId)
                ?? throw new NotFoundException($"Item with id {itemId} could not be found");
                
            inv.StoredItems.Add(item);
        }
            
        CurrencyUtil.ConvertCurrency(inv.Currency);
        return await repo.CreateAsync(inv);
    }

    public async Task AddItem(Inventory inventory, int itemId)
    {         
        var item = await itemRepo.GetByIdAsync(itemId) 
            ?? throw new NotFoundException($"Item with id {itemId} could not be found");

        inventory.StoredItems.Add(item);
        inventory.TotalWeight += item.Weight;
        await repo.UpdateAsync(inventory);
    }

    public async Task DiscardItem(Inventory inventory, int itemId)
    {
        var item = await itemRepo.GetByIdAsync(itemId) 
            ?? throw new NotFoundException($"Item with id {itemId} could not be found");
        
        if(inventory.StoredItems.FirstOrDefault(i => i.Id == itemId) is null)
             throw new NotFoundException($"Item with id {itemId} could not be found in inventory with id {inventory.Id}");

        if (inventory.StoredItems.FirstOrDefault(i => i.Id == itemId) is null)
            throw new NotFoundException($"Item with id {itemId} could not be found in inventory with id {inventory.Id}");
            
        await UnEquip(inventory, itemId);
        inventory.StoredItems.Remove(item);
        inventory.TotalWeight -= item.Weight;
        await repo.UpdateAsync(inventory);
    }


    public async Task UnEquip(Inventory inventory, int itemId)
    {
        var item = await itemRepo.GetByIdAsync(itemId)
            ?? throw new NotFoundException($"Item with id {itemId} could not be found");
            
        foreach (var equipmentSlot in inventory.EquippedItems)
        {
            if (equipmentSlot.EquipmentId == itemId)
            {
                equipmentSlot.EquipmentId = null;
                inventory.AttunedItems += item.RequiresAttunement ? 1 : 0;
                await repo.UpdateAsync(inventory);
                return;
            }
        }
        throw new NotFoundException($"Item with id {itemId} is not equipped in inventory with id {inventory.Id}");
    }

    public async Task UnEquip(Inventory inventory, string slot)
    {
        var resolvedSlot = ConstantsUtil.ResolveOptionOrThrow(slot, EquipSlot.AllowedValues, "Equipment Slot");

        foreach (var equipmentSlot in inventory.EquippedItems)
        {
            if (equipmentSlot.Slot == resolvedSlot)
            {
                if(equipmentSlot.EquipmentId is null)
                    throw new NotFoundException($"No item is equipped in slot {resolvedSlot} in inventory with id {inventory.Id}");

                var item = await itemRepo.GetByIdAsync((int)equipmentSlot.EquipmentId)
                    ?? throw new NotFoundException($"Item with id {equipmentSlot.EquipmentId} could not be found");

                equipmentSlot.EquipmentId = null;
                inventory.AttunedItems += item.RequiresAttunement ? 1 : 0;
                await repo.UpdateAsync(inventory);
                return;
            }
        }
        throw new NotFoundException($"Could not find a slot of that type in the inventory with id {inventory.Id}");
    }

    public async Task Equip(Inventory inventory, int itemId, string slot)
    {
        var item = await itemRepo.GetByIdAsync(itemId) 
            ?? throw new NotFoundException($"Item with id {itemId} could not be found");
        
        if(item is not IEquippable equippableItem)
            throw new InvalidOperationException($"Item with id {itemId} is not equippable");
        
        var resolvedSlot = ConstantsUtil.ResolveOptionOrThrow(slot, EquipSlot.AllowedValues, "Equipment Slot");

        if(equippableItem.MainSlot != resolvedSlot && equippableItem.SecondarySlot != resolvedSlot)
            throw new InvalidOperationException($"Item with id {itemId} cannot be equipped in slot {resolvedSlot}");

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
                    return;
                }
            }
        }
        if (firstSlotFound is not null)
        {
            firstSlotFound.EquipmentId = itemId;
            await repo.UpdateAsync(inventory);
            return;
        }

        throw new NotFoundException($"Could not find a slot of that type in the inventory with id {inventory.Id}");
    }

    public async Task Equip(Inventory inventory, int itemId)
    {
        var item = await itemRepo.GetByIdAsync(itemId) 
            ?? throw new NotFoundException($"Item with id {itemId} could not be found");
        
        if(item is not IEquippable equippableItem)
            throw new InvalidOperationException($"Item with id {itemId} is not equippable");
        
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
                    return;
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
            return;
        } 
        else if (firstSecondarySlotFound is not null)
        {
            firstSecondarySlotFound.EquipmentId = itemId;
            await repo.UpdateAsync(inventory);
            return;
        }

        throw new NotFoundException($"Could not find a slot of that type in the inventory with id {inventory.Id}");
    }

    public async Task<Inventory> GetByCharacterIdAsync(int characterId)
    {
        var character = await characterRepo.GetByIdAsync(characterId)
            ?? throw new NotFoundException($"Character with id {characterId} could not be found");
        
        return await repo.GetByIdAsync(character.InventoryId)
            ?? throw new NotFoundException($"Inventory for character with id {characterId} could not be found");
    }

    public async Task DeleteAsync(int id)
    {
        var inventory = await repo.GetByIdAsync(id)
            ?? throw new NotFoundException($"Inventory with id {id} could not be found");
            
        await repo.DeleteAsync(inventory);
    }
}