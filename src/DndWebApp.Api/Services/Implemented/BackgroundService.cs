
using DndWebApp.Api.Middlewares.ExceptionHandling;
using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs.RequestDtos.Character;
using DndWebApp.Api.Models.Items;
using DndWebApp.Api.Repositories.Interfaces;
using DndWebApp.Api.Services.Interfaces;

namespace DndWebApp.Api.Services.Implemented;

public class BackgroundService(IBackgroundRepository repo, IItemRepository itemRepo, ILogger<BackgroundService> logger) : IBackgroundService
{
    public async Task<Background> CreateAsync(BackgroundDto dto)
    {
        var StartingCurrency = new Currency
        {
            Brass = dto.Currency.Brass,
            Copper = dto.Currency.Copper,
            Electrum = dto.Currency.Electrum,
            Gold = dto.Currency.Gold,
            Platinum = dto.Currency.Platinum,
            Silver = dto.Currency.Silver
        };
        
        var background = await repo.CreateAsync(new()
        {
            Name = dto.Name,
            Description = dto.Description,
            IsHomebrew = dto.IsHomebrew,
            StartingCurrency = StartingCurrency
        });

        return background;
    }

    public async Task DeleteAsync(int id)
    {
        var background = await repo.GetByIdAsync(id) ?? throw new NotFoundException($"Background with id {id} could not be found");
        await repo.DeleteAsync(background);
    }

    public async Task<ICollection<Background>> GetAllAsync()
    {
        return await repo.GetAllAsync();
    }

    public async Task<Background> GetByIdAsync(int id)
    {
        return await repo.GetByIdAsync(id) ?? throw new NotFoundException($"Background with id {id} could not be found");
    }

    public async Task<Background> GetWithAllDataAsync(int id)
    {
        return await repo.GetWithAllDataAsync(id) ?? throw new NotFoundException($"Background with id {id} could not be found");
    }

    public async Task<Background> GetWithFeaturesAsync(int id)
    {
        return await repo.GetWithFeaturesAsync(id) ?? throw new NotFoundException($"Background with id {id} could not be found");
    }

    public async Task UpdateAsync(int id, BackgroundDto dto)
    {
        var background = await repo.GetByIdAsync(id) 
            ?? throw new NotFoundException($"Background with id {id} could not be found");

        background.Name = dto.Name;
        background.Description = dto.Description;
        background.IsHomebrew = dto.IsHomebrew;

        background.StartingCurrency.Brass = dto.Currency.Brass;
        background.StartingCurrency.Copper = dto.Currency.Copper;
        background.StartingCurrency.Electrum = dto.Currency.Electrum;
        background.StartingCurrency.Gold = dto.Currency.Gold;
        background.StartingCurrency.Platinum = dto.Currency.Platinum;
        background.StartingCurrency.Silver = dto.Currency.Silver;

        await repo.UpdateAsync(background);
    }

    public async Task AddStartingItemsAsync(int id, int itemId)
    {
        var item = await itemRepo.GetByIdAsync(itemId) 
            ?? throw new NotFoundException($"Item with id {itemId} could not be found");
        
        var background = await repo.GetByIdAsync(id) 
            ?? throw new NotFoundException($"Background with id {id} could not be found");

        background.StartingItems.Add(item);
        await repo.UpdateAsync(background);
    }

    public async Task RemoveStartingItemsAsync(int id, int itemId)
    {
        var background = await repo.GetByIdAsync(id) 
            ?? throw new NotFoundException($"Background with id {id} could not be found");

        var item = background.StartingItems.FirstOrDefault(i => i.Id == itemId) 
            ?? throw new NotFoundException($"Item with id {itemId} is not a starting item for background with id {id}");

        background.StartingItems.Remove(item);
        await repo.UpdateAsync(background);
    }

    public async Task AddStartingItemChoiceAsync(int id, StartingItemOptionDto dto)
    {
        foreach (var itemId in dto.ItemOptionIds)
        {
            if(await itemRepo.GetByIdAsync(itemId) is null)
                throw new NotFoundException($"Item with id {itemId} could not be found");
        }

        var background = await repo.GetByIdAsync(id) 
            ?? throw new NotFoundException($"Background with id {id} could not be found");

        background.StartingItemsOptions.Add(new StartingItemOption
        {
            Description = dto.Description,
            ItemOptionIds = dto.ItemOptionIds,
        });

        await repo.UpdateAsync(background);
    }

    public async Task RemoveStartingItemChoiceAsync(int id, int optionId)
    {
        var background = await repo.GetByIdAsync(id) 
            ?? throw new NotFoundException($"Background with id {id} could not be found");

        var option = background.StartingItemsOptions.FirstOrDefault(o => o.Id == optionId) 
            ?? throw new NotFoundException($"Starting Item Option with id {optionId} is not a starting item option for background with id {id}");

        background.StartingItemsOptions.Remove(option);
        await repo.UpdateAsync(background);
    }
}