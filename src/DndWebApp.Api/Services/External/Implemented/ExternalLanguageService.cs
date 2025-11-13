using System.Text.Json;
using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs;
using DndWebApp.Api.Models.DTOs.ExternalDtos;
using DndWebApp.Api.Models.ExternalDTOs;
using DndWebApp.Api.Models.World;
using DndWebApp.Api.Repositories.Interfaces;
using DndWebApp.Api.Services.External.Interfaces;
using DndWebApp.Api.Services.Interfaces;

public class ExternalLanguageService : IExternalLanguageService
{
    private readonly ILanguageRepository repo;
    private readonly HttpClient client = new();

    public ExternalLanguageService(ILanguageRepository repo)
    {
        this.repo = repo;
    }

    public async Task FetchExternalLanguagesAsync(CancellationToken cancellationToken = default)
    {
        if ((await repo.GetAllAsync()).Count > 0)
        {
            Console.WriteLine("Languages already exist in the database. Skipping fetch.");
            return;
        }
        
        var getListResponse = await client.GetAsync("https://www.dnd5eapi.co/api/2014/languages/", cancellationToken);
        var result = await JsonSerializer.DeserializeAsync<EIndexListDto>(getListResponse.Content.ReadAsStream(cancellationToken), cancellationToken: cancellationToken);

        if (result is null || result.Results.Count == 0)
        {
            Console.WriteLine("No languages found in external API.");
            return;
        }

        foreach (var item in result.Results)
        {
            var getResponse = await client.GetAsync($"https://www.dnd5eapi.co/api/2014/languages/{item.Index}", cancellationToken);
            var eLanguage = await JsonSerializer.DeserializeAsync<ELanguageDto>(getResponse.Content.ReadAsStream(cancellationToken), cancellationToken: cancellationToken);

            if (eLanguage is null)
            {
                Console.WriteLine($"Failed to deserialize language {item.Index}.");
                continue;
            }
            if (await repo.GetByNameAsync(eLanguage.Name) is not null)
            {
                Console.WriteLine($"Language {eLanguage.Name} already exists. Skipping.");
                continue;
            }

            var language = new Language
            {
                Name = eLanguage.Name,
                Family = eLanguage.Type,
                Script = eLanguage.Script,
                TypicalSpeakers = eLanguage.TypicalSpeakers,
                IsExotic = eLanguage.Type == "Exotic"
            };

            await repo.CreateAsync(language);
        }
    }
}

        