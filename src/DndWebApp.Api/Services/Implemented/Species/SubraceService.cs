
using DndWebApp.Api.Middlewares.ExceptionHandling;
using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs.RequestDtos.Character;
using DndWebApp.Api.Repositories.Interfaces;
using DndWebApp.Api.Services.Interfaces;
using DndWebApp.Api.Services.Interfaces.Species;

namespace DndWebApp.Api.Services.Implemented;

public class SubraceService(ISubraceRepository repo, IRaceRepository parentRaceRepo, ILogger<SubraceService> logger) : ISubraceService
{
    public async Task<Subrace> CreateAsync(SubraceDto dto)
    {
        logger.LogInformation("Creating subrace, Name: {SubraceName}", dto.Name);

        var raceDescription = new RaceDescription
        {
            General = dto.GeneralDescription,
            Aging = dto.AgingDescription ?? "",
            CommonAlignment = dto.CommonAlignmentDescription ?? "",
            Size = dto.SizeDescription ?? "",
            Languages = dto.LanguageDescription ?? ""
        };

        var parentRace = await parentRaceRepo.GetByIdAsync(dto.ParentRaceId) 
            ?? throw new NotFoundException($"Parent race with id {dto.ParentRaceId} could not be found");
        
        var subrace = await repo.CreateAsync(new()
        {
            Name = dto.Name,
            RaceDescription = raceDescription,
            ParentRaceId = dto.ParentRaceId,
            ParentRace = parentRace,
            Speed = dto.Speed,
            IsHomebrew = dto.IsHomebrew,
            Size = dto.Size
        });

        parentRace.SubRaces.Add(subrace);
        await parentRaceRepo.UpdateAsync(parentRace);
        logger.LogInformation("Successfully created subrace, Name: {SubraceName}, ID: {SubraceId}", subrace.Name, subrace.Id);

        return subrace;
    }

    public async Task DeleteAsync(int id)
    {
        var subrace = await repo.GetByIdAsync(id) 
        ?? throw new NotFoundException($"Subrace with id {id} could not be found");

        logger.LogInformation("Deleting subrace, Name: {SubraceName}, ID: {SubraceId}", subrace.Name, id);
        await repo.DeleteAsync(subrace);
        logger.LogInformation("Successfully deleted subrace, Name: {SubraceName}, ID: {SubraceId}", subrace.Name, id);
    }

    public async Task<ICollection<Subrace>> GetAllAsync()
    {
        return await repo.GetAllAsync();
    }

    public async Task<Subrace> GetByIdAsync(int id)
    {
        return await repo.GetByIdAsync(id) ?? throw new NotFoundException($"Subrace with id {id} could not be found");
    }

    public async Task<Subrace> GetWithAllDataAsync(int id)
    {
        return await repo.GetWithAllDataAsync(id) ?? throw new NotFoundException($"Subrace with id {id} could not be found");
    }

    public async Task<Subrace> GetWithTraitsAsync(int id)
    {
        return await repo.GetWithTraitsAsync(id) ?? throw new NotFoundException($"Subrace with id {id} could not be found");
    }

    public async Task<Subrace> UpdateAsync(int id, SubraceDto dto)
    {
        var subrace = await repo.GetByIdAsync(id) 
            ?? throw new NotFoundException($"Subrace with id {id} could not be found");

        logger.LogInformation("Updating subrace, Name: {SubraceName}, ID: {SubraceId}", subrace.Name, id);

        subrace.Name = dto.Name;
        subrace.Speed = dto.Speed;
        subrace.Size = dto.Size;
        subrace.IsHomebrew = dto.IsHomebrew;
        subrace.RaceDescription.General = dto.GeneralDescription;
        subrace.RaceDescription.Aging = dto.AgingDescription ?? "";
        subrace.RaceDescription.CommonAlignment = dto.CommonAlignmentDescription ?? "";
        subrace.RaceDescription.Size = dto.SizeDescription ?? "";
        subrace.RaceDescription.Languages = dto.LanguageDescription ?? "";

        if(dto.NewParentRaceId is not null)
        {
            var newParentRace = await parentRaceRepo.GetByIdAsync((int)dto.NewParentRaceId) 
                ?? throw new NotFoundException($"Parent Race with id {(int)dto.NewParentRaceId} could not be found");

            subrace.ParentRaceId = (int)dto.NewParentRaceId;
            subrace.ParentRace = newParentRace;
        }

        await repo.UpdateAsync(subrace);
        logger.LogInformation("Successfully updated subrace, Name: {SubraceName}, ID: {SubraceId}", subrace.Name, id);
        return subrace;
    }
}