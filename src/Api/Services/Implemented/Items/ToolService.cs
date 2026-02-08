using Api.Middlewares.ExceptionHandling;
using Api.Models.DTOs.RequestDtos.Inventory;
using Api.Models.Items;
using Api.Repositories.Interfaces;
using Api.Services.Constants;
using static Api.Services.Util.SortUtil;
using static Api.Services.Util.ConstantsUtil;
using Api.Models.Items.Constants;
using Api.Services.Interfaces.Items;
using Api.Services.Interfaces;

namespace Api.Services.Implemented.Items;

public class ToolService(IToolRepository repo, ICurrentUserService currentUserService, ILogger<ToolService> logger) : IToolService
{
    public async Task<Tool> CreateAsync(CreateToolRequestDto dto)
    {
        var dtoToolCategory = ResolveOptionOrThrow(dto.ToolCategory, ToolCategory.AllowedValues, "Tool Category");
        var dtoRarity = dto.Rarity is not null ? ResolveOptionOrThrow(dto.Rarity, ItemRarity.AllowedValues, "Item Rarity") : null;

        logger.LogInformation("Creating tool, Name: {ToolName}", dto.Name);
        
        Tool tool = await repo.CreateAsync(new()
        {
            Name = dto.Name,
            Description = dto.Description,
            Categories = [ItemCategory.Tools],
            ToolCategory = dtoToolCategory,
            Value = dto.Value ?? 0,
            Rarity = dtoRarity ?? ItemRarity.Common,
            RequiresAttunement = dto.RequiresAttunement ?? false,
            Weight = dto.Weight ?? 0,

            CreatedAt = DateTime.UtcNow,
            CreatedBy = currentUserService.GetCurrentUserId(),
        });

        logger.LogInformation("Successfully created tool, Name: {ToolName}, ID: {ToolId}", tool.Name, tool.Id);
        return tool;
    }

    public async Task<Tool> AddProperty(ToolPropertyDto dto, int toolId)
    {
        var tool = await repo.GetWithAllDataAsync(toolId) ;
        logger.LogInformation("Adding property to tool, ToolId: {ToolId}, PropertyTitle: {PropertyTitle}", toolId, dto.Title);
        tool.Properties.Add(new ToolProperty { Title = dto.Title, Description = dto.Description });

        await repo.UpdateAsync(tool);
        logger.LogInformation("Successfully added property to tool, ToolId: {ToolId}, PropertyTitle: {PropertyTitle}", toolId, dto.Title);
        return tool;
    }

    public async Task<Tool> AddActivity(ToolActivityDto dto, int toolId)
    {
        var tool = await repo.GetWithAllDataAsync(toolId);

        logger.LogInformation("Adding activity to tool, ToolId: {ToolId}, ActivityTitle: {ActivityTitle}", toolId, dto.Title);
        tool.Activities.Add(new ToolActivity { Title = dto.Title, SkillId = dto.SkillId, AbilityId = dto.AbilityId, DC = dto.DC });

        await repo.UpdateAsync(tool);
        logger.LogInformation("Successfully added activity to tool, ToolId: {ToolId}, ActivityTitle: {ActivityTitle}", toolId, dto.Title);
        return tool;
    }
    
    public async Task DeleteAsync(int id)
    {
        var tool = await repo.GetByIdAsync(id);
        logger.LogInformation("Deleting tool, Name: {ToolName}, ID: {ToolId}", tool.Name, id);
        await repo.DeleteAsync(tool);
        logger.LogInformation("Successfully deleted tool, Name: {ToolName}, ID: {ToolId}", tool.Name, id);
    }

    public async Task<ICollection<Tool>> GetAllAsync() => await repo.GetAllAsync();
    public async Task<Tool> GetByIdAsync(int id) => await repo.GetByIdAsync(id);

    public async Task<Tool> UpdateAsync(UpdateToolRequestDto dto, int id)
    {
        var dtoToolCategory = dto.ToolCategory is not null ? ResolveOptionOrThrow(dto.ToolCategory, ToolCategory.AllowedValues, "Tool Category") : null;
        var dtoRarity = dto.Rarity is not null ? ResolveOptionOrThrow(dto.Rarity, ItemRarity.AllowedValues, "Item Rarity") : null;

        var tool = await repo.GetByIdAsync(id);
        logger.LogInformation("Updating tool, Name: {ToolName}, ID: {ToolId}", dto.Name, id);

        tool.Name = dto.Name ?? tool.Name;
        tool.Description = dto.Description ?? tool.Description;
        tool.ToolCategory = dtoToolCategory ?? tool.ToolCategory;
        tool.Value = dto.Value ?? tool.Value;
        tool.Rarity = dtoRarity ?? tool.Rarity;
        tool.RequiresAttunement = dto.RequiresAttunement ?? tool.RequiresAttunement;
        tool.Weight = dto.Weight ?? tool.Weight;
        
        tool.IsPublic = dto.IsPublic ?? tool.IsPublic;
        tool.CloningAllowed = dto.CloningAllowed ?? tool.CloningAllowed;
        tool.UpdatedAt = DateTime.UtcNow;

        await repo.UpdateAsync(tool);
        logger.LogInformation("Successfully updated tool, Name: {ToolName}, ID: {ToolId}", tool.Name, tool.Id);
        return tool;
    }

    // TODO replace with database level sorting
    public ICollection<Tool> SortBy(ICollection<Tool> tools, string sortFilter, bool descending = false)
    {
        if(!TryResolveOption(sortFilter, SortToolOption.AllowedValues, out string? resolved))
            return tools;

        return resolved switch
        {
            SortToolOption.Name => OrderByMany(tools, [(i => i.Name)], descending),
            SortToolOption.Category => OrderByMany(tools, [(i => i.ToolCategory), (i => i.Name)], descending),
            SortToolOption.Value => OrderByMany(tools, [(i => i.Value), (i => i.Name)], descending),
            SortToolOption.Rarity => OrderByMany(tools, [(i => i.Rarity == null), (i => i.Rarity!), (i => i.Name)], descending),
            _ => throw new ValidationException($"Invalid sort option: {sortFilter}")
        };
    }
}