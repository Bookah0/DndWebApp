using DndWebApp.Api.Middlewares.ExceptionHandling;
using DndWebApp.Api.Models.DTOs.Inventory;
using DndWebApp.Api.Models.Items;
using DndWebApp.Api.Repositories.Interfaces;
using DndWebApp.Api.Services.Constants;
using static DndWebApp.Api.Services.Util.SortUtil;
using static DndWebApp.Api.Services.Util.ValidationUtil;
using static DndWebApp.Api.Services.Util.ConstantsUtil;
using DndWebApp.Api.Models.Items.Constants;
using DndWebApp.Api.Services.Interfaces.Items;

namespace DndWebApp.Api.Services.Implemented.Items;

public class ToolService(IToolRepository repo, ILogger<ToolService> logger) : IToolService
{
    public async Task<Tool> CreateAsync(ToolDto dto)
    {
        var dtoToolCategory = ResolveOptionOrThrow(dto.ToolCategory, ToolCategory.AllowedValues, "Tool Category");
        var dtoRarity = dto.Rarity is not null ? ResolveOptionOrThrow(dto.Rarity, ItemRarity.AllowedValues, "Item Rarity") : null;

        Tool tool = await repo.CreateAsync(new()
        {
            Name = dto.Name,
            Description = dto.Description,
            Categories = [ItemCategory.Tools],
            ToolType = dtoToolCategory,
            Value = dto.Value,
            Rarity = dtoRarity,
            RequiresAttunement = dto.RequiresAttunement ?? false,
            IsHomebrew = dto.IsHomebrew ?? false,
            Weight = dto.Weight ?? 0,
            Properties = []
        });

        return tool;
    }

    public async Task AddProperty(ToolPropertyDto dto, int toolId)
    {
        var tool = await repo.GetWithAllDataAsync(toolId) ?? throw new NotFoundException($"Tool with id {toolId} could not be found");
        tool.Properties.Add(new ToolProperty { Title = dto.Title, Description = dto.Description });
    }

    public async Task AddActivity(ToolActivityDto dto, int toolId)
    {
        var tool = await repo.GetWithAllDataAsync(toolId) ?? throw new NotFoundException($"Tool with id {toolId} could not be found");

        tool.Activities.Add(new ToolActivity { Title = dto.Title, SkillId = dto.SkillId, AbilityId = dto.AbilityId, DC = dto.DC });
    }
    
    public async Task DeleteAsync(int id)
    {
        var tool = await repo.GetByIdAsync(id) ?? throw new NotFoundException($"Tool with id {id} could not be found");
        await repo.DeleteAsync(tool);
    }

    public async Task<ICollection<Tool>> GetAllAsync()
    {
        var tools = await repo.GetAllAsync();
        return tools;
    }

    public async Task<Tool> GetByIdAsync(int id)
    {
        var tool = await repo.GetByIdAsync(id) ?? throw new NotFoundException($"Tool with id {id} could not be found");
        return tool;
    }

    public async Task UpdateAsync(ToolDto dto, int id)
    {
        var dtoToolCategory = ResolveOptionOrThrow(dto.ToolCategory, ToolCategory.AllowedValues, "Tool Category");
        var dtoRarity = dto.Rarity is not null ? ResolveOptionOrThrow(dto.Rarity, ItemRarity.AllowedValues, "Item Rarity") : null;

        var tool = await repo.GetByIdAsync(id) ?? throw new NotFoundException($"Tool with id {id} could not be found"); ;

        tool.Name = dto.Name;
        tool.Description = dto.Description;
        tool.ToolType = dtoToolCategory;
        tool.Value = dto.Value;
        tool.Rarity = dtoRarity;
        tool.RequiresAttunement = dto.RequiresAttunement ?? tool.RequiresAttunement;
        tool.IsHomebrew = dto.IsHomebrew ?? tool.IsHomebrew;
        tool.Weight = dto.Weight ?? tool.Weight;

        await repo.UpdateAsync(tool);
    }

    public ICollection<Tool> SortBy(ICollection<Tool> tools, string sortFilter, bool descending = false)
    {
        if(!TryResolveOption(sortFilter, SortToolOption.AllowedValues, out string? resolved))
            return tools;

        return resolved switch
        {
            SortToolOption.Name => OrderByMany(tools, [(i => i.Name)], descending),
            SortToolOption.Category => OrderByMany(tools, [(i => i.ToolType), (i => i.Name)], descending),
            SortToolOption.Value => OrderByMany(tools, [(i => i.Value), (i => i.Name)], descending),
            SortToolOption.Rarity => OrderByMany(tools, [(i => i.Rarity == null), (i => i.Rarity!), (i => i.Name)], descending),
            _ => throw new ArgumentOutOfRangeException(nameof(sortFilter), "Invalid sort option provided.")
        };
    }
}