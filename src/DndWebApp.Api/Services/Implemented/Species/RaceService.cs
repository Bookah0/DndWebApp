
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
        logger.LogInformation("Creating race, Name: {RaceName}", dto.Name);
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

        logger.LogInformation("Successfully created race, Name: {RaceName}, ID: {RaceId}", race.Name, race.Id);
        return race;
    }

    public async Task DeleteAsync(int id)
    {
        var race = await repo.GetByIdAsync(id);
        logger.LogInformation("Deleting race, Name: {RaceName}, ID: {RaceId}", race.Name, id);
        await repo.DeleteAsync(race);
        logger.LogInformation("Successfully deleted race, Name: {RaceName}, ID: {RaceId}", race.Name, id);
    }

    public async Task<ICollection<Race>> GetAllAsync() => await repo.GetAllAsync();
    public async Task<Race> GetByIdAsync(int id) => await repo.GetByIdAsync(id);
    public async Task<Race> GetWithAllDataAsync(int id) => await repo.GetWithAllDataAsync(id);
    public async Task<Race> GetWithTraitsAsync(int id) => await repo.GetWithTraitsAsync(id);
    public async Task<Race> GetWithSubracesAsync(int id) => await repo.GetWithSubracesAsync(id);

    public async Task<Race> UpdateAsync(int id, RaceDto dto)
    {
        var race = await repo.GetByIdAsync(id);

        logger.LogInformation("Updating race, Name: {RaceName}, ID: {RaceId}", race.Name, id);

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
        logger.LogInformation("Successfully updated race, Name: {RaceName}, ID: {RaceId}", race.Name, id);
        return race;
    }
}