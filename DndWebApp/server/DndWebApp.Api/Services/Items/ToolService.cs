using DndWebApp.Api.Models.DTOs;
using DndWebApp.Api.Models.Items;
using DndWebApp.Api.Models.Items.Enums;
using DndWebApp.Api.Repositories;
using DndWebApp.Api.Repositories.Items;
using DndWebApp.Api.Services.Generic;
using DndWebApp.Api.Services.Util;

namespace DndWebApp.Api.Services.Items;

public class ToolService : IService<Tool, ToolDto>
{
    private readonly ToolRepository repo;
    private readonly ILogger<ToolService> logger;

    public ToolService(ToolRepository repo, ILogger<ToolService> logger)
    {
        this.repo = repo;
        this.logger = logger;
    }

    public async Task<Tool> CreateAsync(ToolDto dto)
    {
        ValidationUtil.HasContentOrThrow(dto.Name);
        ValidationUtil.HasContentOrThrow(dto.Description);
        ValidationUtil.HasContentOrThrow(dto.ToolCategory);
        ValidationUtil.AboveZeroOrThrow(dto.Value);

        var dtoToolCategory = ValidationUtil.ParseEnumOrThrow<ToolCategory>(dto.ToolCategory);
        var dtoRarity = ValidationUtil.ParseEnumOrThrow<ItemRarity>(dto.Rarity);

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
        ValidationUtil.HasContentOrThrow(title);
        ValidationUtil.HasContentOrThrow(description);

        var tool = await repo.GetWithAllDataAsync(toolId) ?? throw new NullReferenceException($"Tool with id {toolId} could not be found");
        tool.Properties.Add(new ToolProperty { Title = title, Description = description });
    }

    public async Task AddActivity(string title, int? skillId, int? abilityId, string dc, int toolId)
    {
        ValidationUtil.HasContentOrThrow(title);
        ValidationUtil.HasContentOrThrow(dc);

        var tool = await repo.GetWithAllDataAsync(toolId) ?? throw new NullReferenceException($"Tool with id {toolId} could not be found");

        tool.Activities.Add(new ToolActivity { Title = title, SkillId = skillId, AbilityId = abilityId, DC = dc });
    }
    
    public async Task DeleteAsync(int id)
    {
        var tool = await repo.GetByIdAsync(id) ?? throw new NullReferenceException($"Tool with id {id} could not be found");
        await repo.DeleteAsync(tool);
    }

    public async Task<ICollection<Tool>> GetAllAsync()
    {
        return await repo.GetAllAsync();
    }

    public async Task<Tool> GetByIdAsync(int id)
    {
        return await repo.GetByIdAsync(id) ?? throw new NullReferenceException($"Tool with id {id} could not be found");
    }

    public async Task UpdateAsync(ToolDto dto)
    {
        ValidationUtil.HasContentOrThrow(dto.Name);
        ValidationUtil.HasContentOrThrow(dto.Description);
        ValidationUtil.HasContentOrThrow(dto.ToolCategory);
        ValidationUtil.AboveZeroOrThrow(dto.Value);

        var dtoToolCategory = ValidationUtil.ParseEnumOrThrow<ToolCategory>(dto.ToolCategory);
        var dtoRarity = ValidationUtil.ParseEnumOrThrow<ItemRarity>(dto.Rarity);

        var tool = await repo.GetByIdAsync(dto.Id) ?? throw new NullReferenceException($"Tool with id {dto.Id} could not be found"); ;

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

    public enum ToolSortingFilter { Name, Category, Value, Rarity }
    public ICollection<Tool> SortBy(ICollection<Tool> tools, ToolSortingFilter sortFilter, bool descending = false)
    {
        return sortFilter switch
        {
            ToolSortingFilter.Name => SortUtil.OrderByMany(tools, [(i => i.Name)], descending),
            ToolSortingFilter.Category => SortUtil.OrderByMany(tools, [(i => i.ToolType), (i => i.Name)], descending),
            ToolSortingFilter.Value => SortUtil.OrderByMany(tools, [(i => i.Value), (i => i.Name)], descending),
            ToolSortingFilter.Rarity => SortUtil.OrderByMany(tools, [(i => i.Rarity == null), (i => i.Rarity!), (i => i.Name)], descending),
            _ => tools,
        };
    }
}