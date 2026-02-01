
using DndWebApp.Api.Middlewares.ExceptionHandling;
using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs.RequestDtos.Character;
using DndWebApp.Api.Repositories.Interfaces;
using DndWebApp.Api.Services.Interfaces;
using DndWebApp.Api.Services.Interfaces.Species;

namespace DndWebApp.Api.Services.Implemented;

public class RaceService(IRaceRepository repo, ILogger<RaceService> logger) : IRaceService
{
    public async Task<Race> CreateAsync(RaceDto dto)
    {
        var raceDescription = new RaceDescription
        {
            General = dto.GeneralDescription,
            Aging = dto.AgingDescription ?? "",
            CommonAlignment = dto.CommonAlignmentDescription ?? "",
            Size = dto.SizeDescription ?? "",
            Languages = dto.LanguageDescription ?? ""
        };
        
        var race = await repo.CreateAsync(new()
        {
            Name = dto.Name,
            RaceDescription = raceDescription,
            Speed = dto.Speed,
            IsHomebrew = dto.IsHomebrew,
            Size = dto.Size
        });

        return race;
    }

    public async Task DeleteAsync(int id)
    {
        var race = await repo.GetByIdAsync(id) ?? throw new NotFoundException($"Race with id {id} could not be found");
        await repo.DeleteAsync(race);
    }

    public async Task<ICollection<Race>> GetAllAsync()
    {
        return await repo.GetAllAsync();
    }

    public async Task<Race> GetByIdAsync(int id)
    {
        return await repo.GetByIdAsync(id) ?? throw new NotFoundException($"Race with id {id} could not be found");
    }

    public async Task<Race> GetWithAllDataAsync(int id)
    {
        return await repo.GetWithAllDataAsync(id) ?? throw new NotFoundException($"Race with id {id} could not be found");
    }

    public async Task<Race> GetWithTraitsAsync(int id)
    {
        return await repo.GetWithTraitsAsync(id) ?? throw new NotFoundException($"Race with id {id} could not be found");
    }

    public async Task<Race> GetWithSubracesAsync(int id)
    {
        return await repo.GetWithSubracesAsync(id) ?? throw new NotFoundException($"Race with id {id} could not be found");
    }

    public async Task UpdateAsync(int id, RaceDto dto)
    {
        var race = await repo.GetByIdAsync(id) 
            ?? throw new NotFoundException($"Race with id {id} could not be found");

        race.Name = dto.Name;
        race.Speed = dto.Speed;
        race.Size = dto.Size;
        race.IsHomebrew = dto.IsHomebrew;
        race.RaceDescription.General = dto.GeneralDescription;
        race.RaceDescription.Aging = dto.AgingDescription ?? "";
        race.RaceDescription.CommonAlignment = dto.CommonAlignmentDescription ?? "";
        race.RaceDescription.Size = dto.SizeDescription ?? "";
        race.RaceDescription.Languages = dto.LanguageDescription ?? "";

        await repo.UpdateAsync(race);
    }
}