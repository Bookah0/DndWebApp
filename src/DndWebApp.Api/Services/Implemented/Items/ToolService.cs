using DndWebApp.Api.Middlewares.ExceptionHandling;
using DndWebApp.Api.Models.DTOs.Inventory;
using DndWebApp.Api.Models.Items;
using DndWebApp.Api.Repositories.Interfaces;
using DndWebApp.Api.Services.Constants;
using static DndWebApp.Api.Services.Util.SortUtil;
using static DndWebApp.Api.Services.Util.ValidationUtil;
using static DndWebApp.Api.Services.Util.ConstantsUtil;
using DndWebApp.Api.Models.Items.Constants;

namespace DndWebApp.Api.Services.Implemented.Items;

public class ToolService
{
    private readonly IToolRepository repo;
    private readonly ILogger<ToolService> logger;

    public ToolService(IToolRepository repo, ILogger<ToolService> logger)
    {
        this.repo = repo;
        this.logger = logger;
    }

    public async Task<Tool> CreateAsync(ToolDto dto)
    {
        var dtoToolCategory = ResolveOptionOrThrow(dto.ToolCategory, ToolCategory.AllowedValues, "Tool Category");
        var dtoRarity = dto.Rarity is not null ? ResolveOptionOrThrow(dto.Rarity, ItemRarity.AllowedValues, "Item Rarity") : null;

        Tool tool = new()
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
        };

        return await repo.CreateAsync(tool);
    }

    public async Task AddProperty(string title, string description, int toolId)
    {
        HasContentOrThrow(title);
        HasContentOrThrow(description);

        var tool = await repo.GetWithAllDataAsync(toolId) ?? throw new NotFoundException($"Tool with id {toolId} could not be found");
        tool.Properties.Add(new ToolProperty { Title = title, Description = description });
    }

    public async Task AddActivity(string title, int? skillId, int? abilityId, string dc, int toolId)
    {
        HasContentOrThrow(title);
        HasContentOrThrow(dc);

        var tool = await repo.GetWithAllDataAsync(toolId) ?? throw new NotFoundException($"Tool with id {toolId} could not be found");

        tool.Activities.Add(new ToolActivity { Title = title, SkillId = skillId, AbilityId = abilityId, DC = dc });
    }
    
    public async Task DeleteAsync(int id)
    {
        var tool = await repo.GetByIdAsync(id) ?? throw new NotFoundException($"Tool with id {id} could not be found");
        await repo.DeleteAsync(tool);
    }

    public async Task<ICollection<Tool>> GetAllAsync()
    {
        return await repo.GetAllAsync();
    }

    public async Task<Tool> GetByIdAsync(int id)
    {
        return await repo.GetByIdAsync(id) ?? throw new NotFoundException($"Tool with id {id} could not be found");
    }

    public async Task UpdateAsync(ToolDto dto)
    {
        var dtoToolCategory = ResolveOptionOrThrow(dto.ToolCategory, ToolCategory.AllowedValues, "Tool Category");
        var dtoRarity = dto.Rarity is not null ? ResolveOptionOrThrow(dto.Rarity, ItemRarity.AllowedValues, "Item Rarity") : null;

        var tool = await repo.GetByIdAsync(dto.Id) ?? throw new NotFoundException($"Tool with id {dto.Id} could not be found"); ;

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