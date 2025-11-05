using DndWebApp.Api.Models.DTOs;
using DndWebApp.Api.Models.Items;
using DndWebApp.Api.Models.Items.Enums;
using DndWebApp.Api.Repositories.Items;
using DndWebApp.Api.Services.Generic;
namespace DndWebApp.Api.Services.Items;

public class InventoryService : IService<Inventory, CreateInventoryDto, UpdateInventoryDto>
{
    private readonly IInventoryRepository repo;
    private readonly IItemRepository itemRepo;
    private readonly ILogger<InventoryService> logger;

    public InventoryService(IInventoryRepository repo, IItemRepository itemRepo, ILogger<InventoryService> logger)
    {
        this.repo = repo;
        this.itemRepo = itemRepo;
        this.logger = logger;
    }

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

        foreach (var itemId in dto.itemIds)
        {
            var item = await itemRepo.GetByIdAsync(itemId)
                ?? throw new NullReferenceException($"Item with id {dto.Id} could not be found");
                
            inv.StoredItems.Add(item);
        }
            
        ConvertCurrency(inv.Currency);
        return await repo.CreateAsync(inv);
    }

    public async Task AddItem(int invId, int itemId)
    {
        var inv = await repo.GetByIdAsync(invId)
            ?? throw new NullReferenceException($"Inventory with id {invId} could not be found");
            
        var item = await itemRepo.GetByIdAsync(itemId) 
            ?? throw new NullReferenceException($"Item with id {itemId} could not be found");

        inv.StoredItems.Add(item);
        inv.TotalWeight += item.Weight;
        await repo.UpdateAsync(inv);
    }

    public async Task DiscardItem(int invId, int itemId)
    {
        var inv = await repo.GetByIdAsync(invId)
            ?? throw new NullReferenceException($"Inventory with id {invId} could not be found");
        
        var item = await itemRepo.GetByIdAsync(itemId) 
            ?? throw new NullReferenceException($"Item with id {itemId} could not be found");
        
        if(inv.StoredItems.FirstOrDefault(i => i.Id == itemId) is null)
             throw new NullReferenceException($"Item with id {itemId} could not be found in inventory with id {invId}");

        if (inv.StoredItems.FirstOrDefault(i => i.Id == itemId) is null)
            throw new NullReferenceException($"Item with id {itemId} could not be found in inventory with id {invId}");

        await UnEquip(invId, itemId);
        inv.StoredItems.Remove(item);
        inv.TotalWeight -= item.Weight;
        await repo.UpdateAsync(inv);
    }


    public async Task UnEquip(int invId, int itemId)
    {
        var inv = await repo.GetByIdAsync(invId)
            ?? throw new NullReferenceException($"Inventorywith id {invId} could not be found");

        var item = await itemRepo.GetByIdAsync(itemId)
            ?? throw new NullReferenceException($"Item with id {itemId} could not be found");
            
        foreach (var equipmentSlot in inv.EquippedItems)
        {
            if (equipmentSlot.EquipmentId == itemId)
            {
                equipmentSlot.EquipmentId = null;
                inv.AttunedItems += item.RequiresAttunement ? 1 : 0;
                await repo.UpdateAsync(inv);
                return;
            }
        }
        throw new NullReferenceException($"Item with id {itemId} is not equipped in inventory with id {invId}");
    }

    public async Task Equip(int invId, int itemId, EquipSlot slot)
    {
        var inv = await repo.GetByIdAsync(invId)
            ?? throw new NullReferenceException($"Inventory with id {itemId} could not be found");
        
        var item = await itemRepo.GetByIdAsync(itemId) 
            ?? throw new NullReferenceException($"Item with id {itemId} could not be found");

        EquipmentSlot? firstSlotFound = null;

        foreach (var equipmentSlot in inv.EquippedItems)
        {
            if (equipmentSlot.Slot == slot)
            {
                firstSlotFound = equipmentSlot;
                if (equipmentSlot.EquipmentId == null)
                {
                    equipmentSlot.EquipmentId = itemId;
                    inv.AttunedItems -= item.RequiresAttunement ? 1 : 0;
                    await repo.UpdateAsync(inv);
                    return;
                }
            }
        }
        if (firstSlotFound is not null)
        {
            firstSlotFound.EquipmentId = itemId;
            await repo.UpdateAsync(inv);
            return;
        }

        throw new NullReferenceException($"Could not find a slot of that type in the inventory with id {invId}");
    }

    public async Task<ICollection<Inventory>> GetAllAsync()
    {
        return await repo.GetAllAsync();
    }

    public async Task<Inventory> GetByIdAsync(int id)
    {
        return await repo.GetByIdAsync(id) 
            ?? throw new NullReferenceException($"Inventory with id {id} could not be found");
    }

    public async Task DeleteAsync(int id)
    {
        var inv = await repo.GetByIdAsync(id)
            ?? throw new NullReferenceException($"Inventory with id {id} could not be found");
            
        await repo.DeleteAsync(inv);
    }
    
    public void ConvertCurrency(Currency currency)
    {
        var valueInBrass = currency.Brass + (currency.Copper * 10) + (currency.Silver * 100) + (currency.Gold * 1000) + (currency.Electrum * 10000);

        currency.Electrum = valueInBrass / 10000;
        currency.Gold = valueInBrass % 10000 / 1000;
        currency.Silver = valueInBrass % 1000 / 100;
        currency.Copper = valueInBrass % 100 / 10;
        currency.Brass = valueInBrass % 10;
    }
}