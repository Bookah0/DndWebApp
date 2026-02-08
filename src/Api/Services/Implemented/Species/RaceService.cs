
using Api.Middlewares.ExceptionHandling;
using Api.Models.Characters;
using Api.Models.Characters.Constants;
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
        var raceDescription = new SpeciesDescriptions
        {
            General = dto.SpeciesDescriptions?.GeneralDescription ?? "",
            Aging = dto.SpeciesDescriptions?.AgingDescription ?? "",
            CommonAlignment = dto.SpeciesDescriptions?.AlignmentDescription ?? "",
            Size = dto.SpeciesDescriptions?.SizesDescription ?? "",
            Languages = dto.SpeciesDescriptions?.LanguagesDescription ?? ""
        };
        
        var race = await repo.CreateAsync(new()
        {
            Name = dto.Name,
            RaceDescription = raceDescription,
            Speed = dto.Speed ?? 30,
            Size = dto.Size ?? CreatureSize.Medium,

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

        race.RaceDescription.General = dto.SpeciesDescriptions?.GeneralDescription ?? race.RaceDescription.General;
        race.RaceDescription.Aging = dto.SpeciesDescriptions?.AgingDescription ?? race.RaceDescription.Aging;
        race.RaceDescription.CommonAlignment = dto.SpeciesDescriptions?.AlignmentDescription ?? race.RaceDescription.CommonAlignment;
        race.RaceDescription.Size = dto.SpeciesDescriptions?.SizesDescription ?? race.RaceDescription.Size;
        race.RaceDescription.Languages = dto.SpeciesDescriptions?.LanguagesDescription ?? race.RaceDescription.Languages;

        race.IsPublic = dto.IsPublic ?? race.IsPublic;
        race.CloningAllowed = dto.CloningAllowed ?? race.CloningAllowed;
        await repo.UpdateAsync(race);
        logger.LogInformation("Successfully updated race, Name: {RaceName}, ID: {RaceId}", race.Name, id);
        return race;
    }
}