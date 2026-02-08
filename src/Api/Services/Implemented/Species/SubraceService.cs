using Api.Models.Characters;
using Api.Models.Characters.Constants;
using Api.Models.DTOs.RequestDtos.Character;
using Api.Repositories.Interfaces;
using Api.Services.Interfaces;
using Api.Services.Interfaces.Species;

namespace Api.Services.Implemented;

public class SubraceService(
    ISubraceRepository repo, 
    IRaceRepository parentRaceRepo, 
    ICurrentUserService currentUserService, 
    ILogger<SubraceService> logger) 
    : ISubraceService
{
    public async Task<Subrace> CreateAsync(CreateSubraceRequestDto dto)
    {
        logger.LogInformation("Creating subrace, Name: {SubraceName}", dto.Name);

        var raceDescription = new SpeciesDescriptions
        {
            General = dto.SpeciesDescriptions?.GeneralDescription ?? "",
            Aging = dto.SpeciesDescriptions?.AgingDescription ?? "",
            CommonAlignment = dto.SpeciesDescriptions?.AlignmentDescription ?? "",
            Size = dto.SpeciesDescriptions?.SizesDescription ?? "",
            Languages = dto.SpeciesDescriptions?.LanguagesDescription ?? ""
        };

        var parentRace = await parentRaceRepo.GetByIdAsync(dto.ParentRaceId);
        
        var subrace = await repo.CreateAsync(new()
        {
            Name = dto.Name,
            RaceDescription = raceDescription,
            ParentRaceId = dto.ParentRaceId,
            ParentRace = parentRace,
            Speed = dto.Speed ?? 30,
            Size = dto.Size ?? CreatureSize.Medium,

            CreatedAt = DateTime.UtcNow,
            CreatedBy = currentUserService.GetCurrentUserId(),
        });

        parentRace.SubRaces.Add(subrace);
        await parentRaceRepo.UpdateAsync(parentRace);
        logger.LogInformation("Successfully created subrace, Name: {SubraceName}, ID: {SubraceId}", subrace.Name, subrace.Id);

        return subrace;
    }

    public async Task DeleteAsync(int id)
    {
        var subrace = await repo.GetByIdAsync(id);
        logger.LogInformation("Deleting subrace, Name: {SubraceName}, ID: {SubraceId}", subrace.Name, id);
        await repo.DeleteAsync(subrace);
        logger.LogInformation("Successfully deleted subrace, Name: {SubraceName}, ID: {SubraceId}", subrace.Name, id);
    }

    public async Task<ICollection<Subrace>> GetAllAsync() => await repo.GetAllAsync();
    public async Task<Subrace> GetByIdAsync(int id) => await repo.GetByIdAsync(id);
    public async Task<Subrace> GetWithAllDataAsync(int id) => await repo.GetWithAllDataAsync(id);
    public async Task<Subrace> GetWithTraitsAsync(int id) => await repo.GetWithTraitsAsync(id);

    public async Task<Subrace> UpdateAsync(int id, UpdateSubraceRequestDto dto)
    {
        var subrace = await repo.GetByIdAsync(id);

        logger.LogInformation("Updating subrace, Name: {SubraceName}, ID: {SubraceId}", subrace.Name, id);

        subrace.Name = dto.Name ?? subrace.Name;
        subrace.Speed = dto.Speed ?? subrace.Speed;
        subrace.Size = dto.Size ?? subrace.Size;

        subrace.RaceDescription.General = dto.SpeciesDescriptions?.GeneralDescription ?? subrace.RaceDescription.General;
        subrace.RaceDescription.Aging = dto.SpeciesDescriptions?.AgingDescription ?? subrace.RaceDescription.Aging;
        subrace.RaceDescription.CommonAlignment = dto.SpeciesDescriptions?.AlignmentDescription ?? subrace.RaceDescription.CommonAlignment;
        subrace.RaceDescription.Size = dto.SpeciesDescriptions?.SizesDescription ?? subrace.RaceDescription.Size;
        subrace.RaceDescription.Languages = dto.SpeciesDescriptions?.LanguagesDescription ?? subrace.RaceDescription.Languages;

        if(dto.NewParentRaceId is not null)
        {
            var newParentRace = await parentRaceRepo.GetByIdAsync((int)dto.NewParentRaceId);
            
            subrace.ParentRaceId = (int)dto.NewParentRaceId;
            subrace.ParentRace = newParentRace;
        }

        subrace.IsPublic = dto.IsPublic ?? subrace.IsPublic;
        subrace.CloningAllowed = dto.CloningAllowed ?? subrace.CloningAllowed;
        subrace.UpdatedAt = DateTime.UtcNow;
        await repo.UpdateAsync(subrace);
        logger.LogInformation("Successfully updated subrace, Name: {SubraceName}, ID: {SubraceId}", subrace.Name, id);
        return subrace;
    }
}