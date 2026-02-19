using Api.Models.Characters;
using Api.Models.DTOs.RequestDtos.Character;
using Api.Repositories.Interfaces;
using Api.Services.Interfaces;
using Api.Services.Interfaces.Species;

namespace Api.Services.Implemented;

public class RaceService(IRaceRepository repo, ICurrentUserService currentUserService, ILogger<RaceService> logger) : IRaceService
{
    public async Task<Race> CreateAsync(CreateRaceRequestDto dto)
    {
        logger.LogInformation("Creating race, Name: {RaceName}", dto.Name);
        var raceDescription = new SpeciesInfo
        {
            General = dto.Info?.General ?? "",
            Aging = dto.Info?.Aging ?? "",
            CommonAlignment = dto.Info?.CommonAlignment ?? "",
            Size = dto.Info?.Size ?? "",
            Languages = dto.Info?.Languages ?? ""
        };
        
        var race = await repo.CreateAsync(new()
        {
            Name = dto.Name,
            Info = raceDescription,
            Speed = dto.Speed,
            Size = dto.Size,

            CreatedAt = DateTime.UtcNow,
            CreatedBy = currentUserService.GetCurrentUserId(),
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

    public async Task<Race> UpdateAsync(int id, UpdateRaceRequestDto dto)
    {
        var race = await repo.GetByIdAsync(id);

        logger.LogInformation("Updating race, Name: {RaceName}, ID: {RaceId}", race.Name, id);

        race.Name = dto.Name ?? race.Name;
        race.Speed = dto.Speed ?? race.Speed;
        race.Size = dto.Size ?? race.Size;

        race.Info.General = dto.Info?.General ?? race.Info.General;
        race.Info.Aging = dto.Info?.Aging ?? race.Info.Aging;
        race.Info.CommonAlignment = dto.Info?.CommonAlignment ?? race.Info.CommonAlignment;
        race.Info.Size = dto.Info?.Size ?? race.Info.Size;
        race.Info.Languages = dto.Info?.Languages ?? race.Info.Languages;

        race.IsPublic = dto.IsPublic ?? race.IsPublic;
        race.CloningAllowed = dto.CloningAllowed ?? race.CloningAllowed;
        await repo.UpdateAsync(race);
        logger.LogInformation("Successfully updated race, Name: {RaceName}, ID: {RaceId}", race.Name, id);
        return race;
    }
}