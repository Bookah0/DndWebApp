using Api.Middlewares.ExceptionHandling;
using Api.Models.Characters;
using Api.Models.DTOs.RequestDtos.Inventory;
using Api.Models.DTOs.ResponseDtos;
using Api.Models.Items;
using Api.Repositories.Interfaces;
using Api.Services.Interfaces.Items;
using Api.Services.Util;
using Api.Services.Util.Interfaces;
using Api.Validation.AllowedValues.Items;
using AutoMapper;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Api.Validation.AllowedValues.ValuesValidator;

namespace Api.Services.Implemented.Items;

public class InventoryService(
    IItemRepository itemRepo,
    IWeaponRepository weaponRepo,
    IArmorRepository armorRepo,
    ICharacterRepository characterRepo,
    IMapper mapper,
    ILogger<InventoryService> logger) : IInventoryService
{
    public async Task<Inventory> CreateAsync(CreateInventoryDto dto)
    {
        logger.LogInformation("Creating inventory for character");

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
            Currency = dto.Currency,
            EquippedItems = equipmentSlots,
        };

        foreach (var itemId in dto.ItemIds)
        {
            if (!await itemRepo.ExistsAsync(itemId))
                throw new NotFoundException($"Item with id {itemId} could not be found");

            inv.StoredItems.Add(new InventoryItem()
            {
                ItemId = itemId,
                Quantity = 1,
            });
        }

        CurrencyUtil.ConvertCurrency(inv.Currency);
        logger.LogInformation("Successfully created inventory for character");
        return inv;
    }

    public async Task<Inventory> AddItemAsync(int characterId, int itemId, int quantity = 1) => await AddItemAsync(await characterRepo.GetByIdAsync(characterId), itemId, quantity);

    public async Task<Inventory> AddItemAsync(Character character, int itemId, int quantity = 1)
    {
        var item = await itemRepo.GetByIdAsync(itemId);

        logger.LogInformation("Adding item with ID: {ItemId} to {CharacterName}'s inventory", itemId, character.Name);

        var invItem = character.Inventory.StoredItems.FirstOrDefault(i => i.ItemId == itemId);

        if (invItem is not null)
        {
            invItem.Quantity += quantity;
        }
        else
        {
            invItem = new InventoryItem()
            {
                ItemId = itemId,
                Quantity = quantity,
            };
            character.Inventory.StoredItems.Add(invItem);
        }

        character.Inventory.TotalWeight += item.Weight * quantity ?? 0;
        await characterRepo.UpdateAsync(character);
        logger.LogInformation("Added item with ID: {ItemId} to {CharacterName}'s inventory", invItem.ItemId, character.Name);
        return character.Inventory;
    }

    public async Task<Inventory> DiscardItemAsync(int characterId, int itemId, int quantity = 1) => await DiscardItemAsync(await characterRepo.GetByIdAsync(characterId), itemId, quantity);

    public async Task<Inventory> DiscardItemAsync(Character character, int itemId, int quantity = 1)
    {
        var item = await itemRepo.GetByIdAsync(itemId);

        var invItem = character.Inventory.StoredItems.FirstOrDefault(i => i.ItemId == itemId)
            ?? throw new NotFoundException($"Item with id {itemId} could not be found in {character.Name}'s inventory");

        logger.LogInformation("Discarding item with ID: {ItemId} in {CharacterName}'s inventory", itemId, character.Name);

        if (quantity > invItem.Quantity)
        {
            var equippedItem = character.Inventory.EquippedItems.FirstOrDefault(e => e.EquipmentId == itemId);
            
            if(equippedItem is not null)
            {
                equippedItem.EquipmentId = null;
                character.Inventory.AttunedItems += item.RequiresAttunement ? 1 : 0;
            }

            character.Inventory.TotalWeight -= item.Weight * invItem.Quantity ?? 0;
            character.Inventory.StoredItems.Remove(invItem);
        }
        if (quantity <= invItem.Quantity)
        {
            invItem.Quantity -= quantity;
            character.Inventory.TotalWeight -= item.Weight * quantity ?? 0;
        }

        await characterRepo.UpdateAsync(character);
        logger.LogInformation("Discarded item with ID: {ItemId} from {CharacterName}'s inventory", itemId, character.Name);
        return character.Inventory;
    }

    public async Task UnEquipAsync(Character character, int itemId)
    {
        var equippedSlot = character.Inventory.EquippedItems.FirstOrDefault(e => e.EquipmentId == itemId)
            ?? throw new NotFoundException($"Item with id {itemId} is not equipped in {character.Name}'s inventory");

        var itemInInventory = character.Inventory.StoredItems.FirstOrDefault(i => i.ItemId == equippedSlot.EquipmentId)
            ?? throw new NotFoundException($"Item with id {equippedSlot.EquipmentId} is not in {character.Name}'s inventory");
        
        var item = await itemRepo.GetByIdAsync(itemInInventory.ItemId);
       
        logger.LogInformation("Unequipping item with ID: {ItemId} from {CharacterName}'s inventory", itemId, character.Id);
        equippedSlot.EquipmentId = null;
        character.Inventory.AttunedItems += item.RequiresAttunement ? 1 : 0;
        await characterRepo.UpdateAsync(character);
        logger.LogInformation("Unequipped item with ID: {ItemId} from {CharacterName}'s inventory", itemId, character.Id);
    }

    public async Task UnEquipAsync(Character character, string slot)
    {
        var normalizedSlot = NormalizeValueOrThrow<EquipSlot>(slot);
        var equippedSlot = character.Inventory.EquippedItems.FirstOrDefault(e => e.Slot == normalizedSlot)
            ?? throw new NotFoundException($"No slot {normalizedSlot} in {character.Name}'s inventory");

        var itemInInventory = character.Inventory.StoredItems.FirstOrDefault(i => i.ItemId == equippedSlot.EquipmentId)
            ?? throw new NotFoundException($"Item with id {equippedSlot.EquipmentId} is not in {character.Name}'s inventory");
        
        var item = await itemRepo.GetByIdAsync(itemInInventory.ItemId);
       
        logger.LogInformation("Unequipping item from slot: {EquipmentSlot} in {CharacterName}'s inventory", normalizedSlot, character.Id);
        equippedSlot.EquipmentId = null;
        character.Inventory.AttunedItems += item.RequiresAttunement ? 1 : 0;

        await characterRepo.UpdateAsync(character);
        logger.LogInformation("Unequipped item from solot: {slot} from {CharacterName}'s inventory", slot, character.Id);
    }

    public async Task<Inventory> EquipAsync(Character character, int itemId, string slot)
    {
        var item = await itemRepo.GetByIdAsync(itemId);

        if (item is not IEquippable equippableItem)
            throw new InvalidOperationException($"Item with id {itemId} is not equippable");

        var normalizedSlot = NormalizeValueOrThrow<EquipSlot>(slot);

        if (equippableItem.MainSlot != normalizedSlot && equippableItem.SecondarySlot != normalizedSlot)
            throw new InvalidOperationException($"Item with id {itemId} cannot be equipped in slot {normalizedSlot}");
            
        var invItem = character.Inventory.StoredItems.FirstOrDefault(i => i.ItemId == itemId)
            ?? throw new NotFoundException($"Item with id {itemId} is not in {character.Name}'s inventory");

        logger.LogInformation("Equipping item with ID: {ItemId} to slot: {EquipmentSlot} in {CharacterName}'s inventory", itemId, normalizedSlot, character.Name);

        var equipmentSlot = FindEmptyEquipmentSlotAsync(character, equippableItem.MainSlot, equippableItem.SecondarySlot);

        if(equipmentSlot.EquipmentId != null)
        {
            var currentlyEquippedInInv = character.Inventory.StoredItems.FirstOrDefault(i => i.ItemId == equipmentSlot.EquipmentId)
                ?? throw new NotFoundException($"Item with id {equipmentSlot.EquipmentId} is not in {character.Name}'s inventory");
            
            var currentlyEquipped = await itemRepo.GetByIdAsync(currentlyEquippedInInv.ItemId);
            character.Inventory.AttunedItems += currentlyEquipped.RequiresAttunement ? 1 : 0;
        }

        character.Inventory.AttunedItems += item.RequiresAttunement ? 1 : 0;
        equipmentSlot.EquipmentId = itemId;
        await characterRepo.UpdateAsync(character);
        logger.LogInformation("Equipped item with ID: {ItemId} to slot: {EquipmentSlot} in {CharacterName}'s inventory", itemId, normalizedSlot, character.Name);
        return character.Inventory;
    }

    public async Task<Inventory> EquipAsync(Character character, int itemId)
    {
        var item = await itemRepo.GetByIdAsync(itemId);

        if (item is not IEquippable equippableItem)
            throw new InvalidOperationException($"Item with id {itemId} is not equippable");

        var invItem = character.Inventory.StoredItems.FirstOrDefault(i => i.ItemId == itemId)
            ?? throw new NotFoundException($"Item with id {itemId} is not in {character.Name}'s inventory");

        logger.LogInformation("Equipping item with ID: {ItemId} to {CharacterName}", itemId, character.Name);

        var equipmentSlot = FindEmptyEquipmentSlotAsync(character, equippableItem.MainSlot, equippableItem.SecondarySlot);

        if(equipmentSlot.EquipmentId != null)
        {
            var currentlyEquippedInInv = character.Inventory.StoredItems.FirstOrDefault(i => i.ItemId == equipmentSlot.EquipmentId)
                ?? throw new NotFoundException($"Item with id {equipmentSlot.EquipmentId} is not in {character.Name}'s inventory");
            
            var currentlyEquipped = await itemRepo.GetByIdAsync(currentlyEquippedInInv.ItemId);
            character.Inventory.AttunedItems += currentlyEquipped.RequiresAttunement ? 1 : 0;
        }

        equipmentSlot.EquipmentId = itemId;
        character.Inventory.AttunedItems += item.RequiresAttunement ? 1 : 0;
        await characterRepo.UpdateAsync(character);

        logger.LogInformation("Equipped item with ID: {ItemId} to slot: {EquipmentSlot} in {CharacterName}'s inventory", itemId, equippableItem.MainSlot, character.Name);
        return character.Inventory;
    }

    private static EquipmentSlot FindEmptyEquipmentSlotAsync(Character character, string mainSlot, string? secondarySlot)
    {
        return character.Inventory.EquippedItems.FirstOrDefault(e => e.Slot == mainSlot && e.EquipmentId == null)
            ?? character.Inventory.EquippedItems.FirstOrDefault(e => e.Slot == secondarySlot && e.EquipmentId == null) 
                ?? character.Inventory.EquippedItems.FirstOrDefault(e => e.Slot == mainSlot)
                    ?? throw new NotFoundException($"Could not find a slot of type {mainSlot} in the inventory");
    }

    public async Task<Inventory> GetByCharacterIdAsync(int characterId)
    {
        var character = await characterRepo.GetByIdAsync(characterId);
        return character.Inventory;
    }

    public async Task<ICollection<EquippedItemDto>> GetAllEquippedItemsAsync(Character character, string? slot)
    {
        List<EquippedItemDto> result = [];
        var targetSlots = slot is null 
            ? character.Inventory.EquippedItems 
            : character.Inventory.EquippedItems.Where(e => e.Slot == NormalizeValueOrThrow<EquipSlot>(slot)); 

        foreach (var equippedSlot in targetSlots)
        {
            if (equippedSlot.EquipmentId is null) 
                continue;

            var dto = equippedSlot.Slot switch
            {
                EquipSlot.MainHand or EquipSlot.OffHand or EquipSlot.Ranged =>
                    mapper.Map<EquippedItemDto>(await weaponRepo.GetByIdAsync(equippedSlot.EquipmentId.Value)),
                EquipSlot.Armor or EquipSlot.Head or EquipSlot.Waist or EquipSlot.Hands or EquipSlot.Feet =>
                    mapper.Map<EquippedItemDto>(await armorRepo.GetByIdAsync(equippedSlot.EquipmentId.Value)),
                _ =>
                    mapper.Map<EquippedItemDto>(await itemRepo.GetByIdAsync(equippedSlot.EquipmentId.Value)),
            };
            result.Add(dto);
        }
        return result;
    }
}