namespace DndWebApp.Api.Services.External.Implemented;

using System.Text.Json;
using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs.ExternalDTOs;
using DndWebApp.Api.Repositories.Interfaces;
using DndWebApp.Api.Services.External.Interfaces;

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
            throw new InvalidOperationException("Languages already exist in the database. Skipping fetch.");
        }
        
        var getListResponse = await client.GetAsync("https://www.dnd5eapi.co/api/2014/languages/", cancellationToken);
        var result = await JsonSerializer.DeserializeAsync<EIndexListDto>(getListResponse.Content.ReadAsStream(cancellationToken), cancellationToken: cancellationToken);

        if (result is null || result.Results.Count == 0)
        {
            throw new InvalidOperationException("No languages found in external API.");
        }

        foreach (var item in result.Results)
        {
            var getResponse = await client.GetAsync($"https://www.dnd5eapi.co/api/2014/languages/{item.Index}", cancellationToken);
            var eLanguage = await JsonSerializer.DeserializeAsync<ELanguageDto>(getResponse.Content.ReadAsStream(cancellationToken), cancellationToken: cancellationToken);

            if (eLanguage is null)
            {
                throw new InvalidOperationException($"Failed to deserialize language {item.Index}.");
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

        