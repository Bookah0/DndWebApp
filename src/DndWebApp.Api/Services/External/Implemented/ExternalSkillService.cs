namespace DndWebApp.Api.Services.External.Implemented;

using System.Text.Json;
using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs.ExternalDTOs;
using DndWebApp.Api.Repositories.Interfaces;
using DndWebApp.Api.Services.External.Interfaces;
using DndWebApp.Api.Services.Util;

public class ExternalSkillService(ISkillRepository repo, IAbilityRepository abilityRepo, ILogger<ExternalSkillService> logger) : IExternalSkillService
{
    private readonly HttpClient client = new();

    public async Task FetchExternalSkillsAsync(CancellationToken cancellationToken = default)
    {
        if ((await repo.GetAllAsync()).Count > 0)
        {
            logger.LogInformation("Skills already exist in the database. Skipping fetch.");
            return;
        }
        
        logger.LogInformation("Fetching external skills.");

        var getListResponse = await client.GetAsync("https://www.dnd5eapi.co/api/2014/skills/", cancellationToken);
        var result = await JsonSerializer.DeserializeAsync<EIndexListDto>(getListResponse.Content.ReadAsStream(cancellationToken), cancellationToken: cancellationToken);

        if (result is null || result.Results.Count == 0)
        {
            throw new InvalidOperationException("No skills found in external API.");
        }

        foreach (var item in result.Results)
        {
            var getResponse = await client.GetAsync($"https://www.dnd5eapi.co/api/skills/{item.Index}", cancellationToken);
            var eSkill = await JsonSerializer.DeserializeAsync<ESkillDto>(getResponse.Content.ReadAsStream(cancellationToken), cancellationToken: cancellationToken);

            if (eSkill is null)
            {
                throw new InvalidOperationException($"Failed to deserialize skill {item.Index}.");
            }

            var ability = await abilityRepo.GetByShortNameAsync(eSkill.Ability.Name)
                ?? throw new ArgumentException($"Ability with short name {eSkill.Ability.Name} not found.");

            var skill = new Skill
            {
                Name = eSkill.Name,
                Description = string.Join("\n", eSkill.Description),
                Ability = ability,
                AbilityId = ability.Id
            };

            await repo.CreateAsync(skill);
        }

        logger.LogInformation("Successfully fetched external skills. Count: {SkillCount}", result.Results.Count);
    }
}

        