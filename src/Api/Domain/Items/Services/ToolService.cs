using static Api.Infrastructure.Validation.ValuesValidator;
using Api.Domain.Items.DTOs;
using Api.Domain.Items.Models;
using Api.Domain.Items.Repositories;
using Api.Domain.Shared.DTOs;
using Api.Domain.Shared.Utils;
using Api.Domain.Users.Services;
using Api.Infrastructure.Middleware.ExceptionHandling;
using Api.Domain.Shared.Enums.Items;

namespace Api.Domain.Items.Services;

public class ToolService(IToolRepository repo, ICurrentUserService currentUserService, ILogger<ToolService> logger) : IToolService
{
    public async Task<Tool> CreateAsync(CreateToolRequestDto dto)
    {
        var dtoToolCategory = NormalizeValueOrThrow<ToolCategory>(dto.ToolCategory);
        var dtoRarity = dto.Rarity is not null ? NormalizeValueOrThrow<ItemRarity>(dto.Rarity) : null;

        logger.LogInformation("Creating tool, Name: {ToolName}", dto.Name);
        
        Tool tool = await repo.CreateAsync(new()
        {
            Name = dto.Name,
            Description = dto.Description,
            Categories = [ItemCategory.Tools],
            ToolCategory = dtoToolCategory,
            Value = dto.Value ?? 0,
            Rarity = dtoRarity ?? ItemRarity.Common,
            RequiresAttunement = dto.RequiresAttunement,
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
        tool.ToolProperties.Add(new ToolProperty { Title = dto.Title, Description = dto.Description });

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
        var dtoToolCategory = dto.ToolCategory is not null ? NormalizeValueOrThrow<ToolCategory>(dto.ToolCategory) : null;
        var dtoRarity = dto.Rarity is not null ? NormalizeValueOrThrow<ItemRarity>(dto.Rarity) : null;

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

    public async Task<Tool> UpdateAsync(UpdateItemRequestDto dto, int id)
    {
        var dtoRarity = dto.Rarity is not null ? NormalizeValueOrThrow<ItemRarity>(dto.Rarity) : null;

        var item = await repo.GetByIdAsync(id);
        logger.LogInformation("Updating item, Name: {ItemName}, ID: {ItemId}", item.Name, item.Id);

        item.Name = dto.Name ?? item.Name;
        item.Description = dto.Description ?? item.Description;
        item.Value = dto.Value ?? item.Value;
        item.Rarity = dtoRarity ?? item.Rarity;
        item.RequiresAttunement = dto.RequiresAttunement ?? item.RequiresAttunement;
        item.Weight = dto.Weight ?? item.Weight;
        item.Quantity = dto.Quantity ?? item.Quantity;

        item.IsPublic = dto.IsPublic ?? item.IsPublic;
        item.CloningAllowed = dto.CloningAllowed ?? item.CloningAllowed;
        item.UpdatedAt = DateTime.UtcNow;
        
        await repo.UpdateAsync(item);
        logger.LogInformation("Successfully updated item, Name: {ItemName}, ID: {ItemId}", item.Name, item.Id);
        return item;
    }

    public async Task<(int, ICollection<Tool>)> GetFilteredAsync(ToolFilterDto filter, PaginationRequestDto pagination) 
    {
        ValidateFilterAsync(filter);
        var (count, filtered) = await repo.GetFilteredAsync(filter, pagination);

        if(!filtered.HasContent() && count > 0)
            throw new ValidationException("Page does not contain any elements");

        return (count, filtered);
    }

    public void ValidateFilterAsync(ToolFilterDto dto)
    {
        if (dto.MinValue is not null && dto.MaxValue is not null && dto.MinValue > dto.MaxValue)
            throw new ValidationException("Maximum value must be greater than or equal to minimum value");
        if (dto.MinValue is not null && dto.MinValue < 0)
            throw new ValidationException("Minimum value must be greater than or equal to zero");
        if (dto.MaxValue is not null && dto.MaxValue < 0)
            throw new ValidationException("Maximum value must be greater than or equal to zero");
        
        if (dto.MinWeight is not null && dto.MaxWeight is not null && dto.MinWeight > dto.MaxWeight)
            throw new ValidationException("Maximum weight must be greater than or equal to minimum weight");
        if (dto.MinWeight is not null && dto.MinWeight < 0)
            throw new ValidationException("Minimum weight must be greater than or equal to zero");
        if (dto.MaxWeight is not null && dto.MaxWeight < 0)
            throw new ValidationException("Maximum weight must be greater than or equal to zero");
        
        if (dto.Name is not null)
            dto.Name = NormalizationUtil.NormalizeWhiteSpace(dto.Name);
        if(dto.Rarity != null)
            dto.Rarity = NormalizeValueOrThrow<ItemRarity>(dto.Rarity);
        if(dto.ToolCategory != null)    
            dto.ToolCategory = NormalizeValueOrThrow<ToolCategory>(dto.ToolCategory);
            
        dto.Category = NormalizeValueOrThrow<ItemCategory>(dto.Category);
    }
}