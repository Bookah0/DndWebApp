using AutoMapper;
using DndWebApp.Api.Middlewares.ExceptionHandling;
using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs.Character;
using DndWebApp.Api.Models.DTOs.ResponseDtos;
using DndWebApp.Api.Repositories.Interfaces;
using DndWebApp.Api.Services.Interfaces;
using static DndWebApp.Api.Services.Util.SortUtil;

namespace DndWebApp.Api.Services.Implemented;

public class AbilityService : IAbilityService
{
    private readonly IAbilityRepository repo;
    private readonly ILogger<AbilityService> logger;
    private readonly IMapper mapper;

    public AbilityService(IAbilityRepository repo, ILogger<AbilityService> logger, IMapper mapper)
    {
        this.repo = repo;
        this.logger = logger;
        this.mapper = mapper;
    }

    public async Task<AbilityResponseDto> CreateAsync(AbilityDto dto)
    {
        Ability ability = new()
        {
            FullName = dto.FullName,
            ShortName = dto.ShortName,
            Description = dto.Description,
            Skills = []
        };

        await repo.CreateAsync(ability);
        return mapper.Map<AbilityResponseDto>(ability);
    }

    public async Task DeleteAsync(int id)
    {
        var ability = await repo.GetByIdAsync(id) ?? throw new NotFoundException("Ability could not be found");
        await repo.DeleteAsync(ability);
    }

    public async Task<ICollection<AbilityResponseDto>> GetAllAsync()
    {
        var abilities = await repo.GetAllAsync();
        return [.. abilities.Select(a => mapper.Map<AbilityResponseDto>(a))];
    }

    public async Task<AbilityResponseDto> GetByIdAsync(int id)
    {
        var ability = await repo.GetByIdAsync(id) ?? throw new NotFoundException("Ability could not be found");
        return mapper.Map<AbilityResponseDto>(ability);
    }

    public async Task UpdateAsync(int id, AbilityDto dto)
    {
        var ability = await repo.GetByIdAsync(id) ?? throw new NotFoundException("Ability could not be found");

        ability.FullName = dto.FullName;
        ability.ShortName = dto.ShortName;
        ability.Description = dto.Description;

        await repo.UpdateAsync(ability);
    }

    public int GetModifier(AbilityValue val)
    {
        return val.Value - 10 / 2;
    }

    public ICollection<Ability> SortBy(ICollection<Ability> abilities)
    {
        var abilityOrder = CreateOrderLookup(["Strength", "Dexterity", "Constitution", "Intelligence", "Wisdom", "Charisma"]);

        return [.. abilities.OrderBy(a => abilityOrder[a.FullName])];
    }
}