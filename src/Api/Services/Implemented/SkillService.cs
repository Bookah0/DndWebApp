using Api.Middlewares.ExceptionHandling;
using Api.Models.Characters;
using Api.Repositories.Interfaces;
using Api.Services.Interfaces;
using static Api.Services.Util.SortUtil;
using Api.Models.DTOs.RequestDtos.Character;
using Api.Validation.AllowedValues;

namespace Api.Services.Implemented;

public class SkillService(
    ISkillRepository repo, 
    IAbilityRepository abilityRepo, 
    ICurrentUserService currentUserService, 
    ILogger<SkillService> logger) 
    : ISkillService
{
    public async Task<Skill> CreateAsync(CreateSkillRequestDto dto)
    {
        var ability = await abilityRepo.GetByIdAsync(dto.AbilityId);

        logger.LogInformation("Creating skill, Name: {SkillName}, AbilityId: {AbilityId}", dto.Name, dto.AbilityId);

        var skill = await repo.CreateAsync(new()
        {
            Name = dto.Name,
            Description = dto.Description,
            AbilityId = dto.AbilityId,
            Ability = ability,
            
            CreatedAt = DateTime.UtcNow,
            CreatedBy = currentUserService.GetCurrentUserId(),
        });
        logger.LogInformation("Successfully created skill, Name: {SkillName}, ID: {SkillId}", skill.Name, skill.Id);
        return skill;
    }

    public async Task DeleteAsync(int id)
    {
        var skill = await repo.GetByIdAsync(id);
        if(!skill.IsHomebrew)
            throw new ValidationException("Cannot delete a base skill.");
        
        logger.LogInformation("Deleting skill with Name: {SkillName}, ID: {SkillId}", skill.Name, id);
        await repo.DeleteAsync(skill);

        logger.LogInformation("Successfully deleted skill, Name: {SkillName}, ID: {SkillId}", skill.Name, skill.Id);
    }

    public async Task<ICollection<Skill>> GetAllAsync() => await repo.GetAllAsync();
    public async Task<ICollection<Skill>> GetAllWithAbilityAsync() => await repo.GetAllWithAbilityAsync();
    public async Task<Skill> GetByIdAsync(int id) => await repo.GetByIdAsync(id);

    public async Task<Skill> UpdateAsync(int id, UpdateSkillRequestDto dto)
    {
        var skill = await repo.GetByIdAsync(id);
        logger.LogInformation("Updating skill, Name: {SkillName}, ID: {SkillId}", skill.Name, id);

        if (dto.NewAbilityId is not null && dto.NewAbilityId != skill.AbilityId)
        {
            skill.Ability = await abilityRepo.GetByIdAsync(dto.NewAbilityId.Value);
            skill.AbilityId = dto.NewAbilityId.Value;
        }

        skill.Name = dto.Name ?? skill.Name;
        skill.Description = dto.Description ?? skill.Description;
        skill.IsPublic = dto.IsPublic ?? skill.IsPublic;
        skill.CloningAllowed = dto.CloningAllowed ?? skill.CloningAllowed;
        skill.UpdatedAt = DateTime.UtcNow;

        await repo.UpdateAsync(skill);
        logger.LogInformation("Successfully updated skill, Name: {SkillName}, ID: {SkillId}", skill.Name, skill.Id);
        return skill;
    }

    // TODO move sorting logic to repository when implementing database level sorting
    public ICollection<Skill> SortBy(ICollection<Skill> skills, string sortFilter, bool descending = false)
    {
        if(!ValuesValidator.TryResolveValue<SortSkillOption>(sortFilter, out string? resolved))
            return skills;

        var abilityOrder = CreateOrderLookup(["Strength", "Dexterity", "Constitution", "Intelligence", "Wisdom", "Charisma"]);

        return resolved switch
        {
            SortSkillOption.Name => OrderByMany(skills, [(s => s.Name)], descending),
            SortSkillOption.Ability => OrderByMany(skills, [(s => abilityOrder[s.Ability!.FullName]), (s => s.Name)], descending),
            _ => throw new ValidationException($"Invalid sort option: {sortFilter}"),
        };
    }
}