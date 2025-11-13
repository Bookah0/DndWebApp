using System.Text.Json;
using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.Characters.Enums;
using DndWebApp.Api.Models.DTOs;
using DndWebApp.Api.Models.DTOs.ExternalDtos;
using DndWebApp.Api.Repositories.Interfaces;
using DndWebApp.Api.Services.External.Interfaces;
using DndWebApp.Api.Services.Interfaces;
using DndWebApp.Api.Services.Util;

public class ExternalSkillService : IExternalSkillService
{
    private readonly ISkillRepository repo;
    private readonly IAbilityRepository abilityRepo;
    private readonly HttpClient client = new();

    public ExternalSkillService(ISkillRepository repo, IAbilityRepository abilityRepo)
    {
        this.repo = repo;
        this.abilityRepo = abilityRepo;
    }

    public async Task FetchExternalSkillsAsync(CancellationToken cancellationToken = default)
    {
        if ((await repo.GetAllAsync()).Count > 0)
        {
            Console.WriteLine("Skills already exist in the database. Skipping fetch.");
            return;
        }
        
        var getListResponse = await client.GetAsync("https://www.dnd5eapi.co/api/2014/skills/", cancellationToken);
        var result = await JsonSerializer.DeserializeAsync<EIndexListDto>(getListResponse.Content.ReadAsStream(cancellationToken), cancellationToken: cancellationToken);

        if (result is null || result.Results.Count == 0)
        {
            Console.WriteLine("No skills found in external API.");
            return;
        }

        foreach (var item in result.Results)
        {
            var getResponse = await client.GetAsync($"https://www.dnd5eapi.co/api/skills/{item.Index}", cancellationToken);
            var eSkill = await JsonSerializer.DeserializeAsync<ESkillDto>(getResponse.Content.ReadAsStream(cancellationToken), cancellationToken: cancellationToken);

            if (eSkill is null)
            {
                Console.WriteLine($"Failed to deserialize skill {item.Index}.");
                continue;
            }

            var abilityType = NormalizationUtil.ParseEnumOrThrow<AbilityShortType>(eSkill.Ability.Index);

            var ability = await abilityRepo.GetByTypeAsync(abilityType)
                ?? throw new ArgumentException($"Ability with short name {eSkill.Ability.Index} not found.");

            var skill = new Skill
            {
                Name = eSkill.Name,
                Description = string.Join("\n", eSkill.Description),
                Ability = ability,
                AbilityId = ability.Id
            };

            await repo.CreateAsync(skill);
        }
    }
}

        