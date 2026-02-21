using Api.Domain.Shared.Enums.Character;
using Api.Domain.Species.DTOs;
using Api.Domain.Species.Models;
using Api.Domain.Species.Repositories;
using Api.Domain.Users.Services;

namespace Api.Domain.Species.Services;

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

        var raceDescription = new SpeciesInfo
        {
            General = dto.Info?.General ?? "",
            Aging = dto.Info?.Aging ?? "",
            CommonAlignment = dto.Info?.CommonAlignment ?? "",
            Size = dto.Info?.Size ?? "",
            Languages = dto.Info?.Languages ?? ""
        };

        var parentRace = await parentRaceRepo.GetByIdAsync(dto.ParentRaceId);
        
        var subrace = await repo.CreateAsync(new()
        {
            Name = dto.Name,
            Info = raceDescription,
            ParentRaceId = dto.ParentRaceId,
            ParentRace = parentRace,
            Speed = dto.Speed,
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

        subrace.Info.General = dto.Info?.General ?? subrace.Info.General;
        subrace.Info.Aging = dto.Info?.Aging ?? subrace.Info.Aging;
        subrace.Info.CommonAlignment = dto.Info?.CommonAlignment ?? subrace.Info.CommonAlignment;
        subrace.Info.Size = dto.Info?.Size ?? subrace.Info.Size;
        subrace.Info.Languages = dto.Info?.Languages ?? subrace.Info.Languages;

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