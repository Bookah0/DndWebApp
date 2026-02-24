using static Api.Domain.Shared.Utils.QueryUtil;
using Api.Domain.Items.Models;
using Api.Infrastructure.Data;
using Api.Domain.Items.DTOs;
using Api.Domain.Shared.DTOs;
using Api.Domain.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using Api.Infrastructure.Validation;
using Api.Infrastructure.Middleware.ExceptionHandling;
using Api.Domain.Shared.Utils;

namespace Api.Domain.Items.Repositories;

public class ToolRepository(AppDbContext context) : IToolRepository
{
    public async Task<Tool> GetByIdAsync(int id) => 
        await context.Tools.FirstOrDefaultAsync(t => t.Id == id)
            ?? throw new Exception($"Tool with id {id} could not be found");

    public async Task<Tool> GetWithAllDataAsync(int id) => 
        await context.Tools
            .Include(t => t.ToolProperties)
            .Include(t => t.Activities)
            .FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new Exception($"Tool with id {id} could not be found");

    public async Task<ICollection<Tool>> GetAllAsync() => await context.Tools.ToListAsync();

    public async Task<ICollection<Tool>> GetAllWithAllDataAsync() => 
        await context.Tools
            .Include(t => t.ToolProperties)
            .Include(t => t.Activities)
            .ToListAsync();

    public async Task<Tool> CreateAsync(Tool entity)
    {
        await context.Tools.AddAsync(entity);
        await context.SaveChangesAsync();
        return entity;
    }

        public async Task DeleteAsync(Tool entity)
    {
        context.Tools.Remove(entity);
        await context.SaveChangesAsync();
    }

    public async Task<Tool> UpdateAsync(Tool updatedEntity)
    {
        context.Tools.Update(updatedEntity);
        await context.SaveChangesAsync();
        return updatedEntity;
    }

    public async Task<(int, ICollection<Tool>)> GetFilteredAsync(ToolFilterDto filter, PaginationRequestDto pagination)
    {      
        var normalizedSortBy = ValuesValidator.NormalizeValue<SortToolOption>(filter.SortBy ?? SortToolOption.Default);

        var query = context.Tools
            .AsQueryable()
            .WhereIf(filter.CreatedBy, i => i.CreatedBy == filter.CreatedBy)
            .WhereIf(filter.Name, t => t.Name.Contains(filter.Name!))
            .WhereIf(filter.Category, t => t.Categories.Any(c => filter.Category!.Contains(c)))
            .WhereIf(filter.Rarity, t => t.Rarity.Contains(filter.Rarity!)) 
            .WhereIf(filter.RequiresAttunement, t => t.RequiresAttunement == filter.RequiresAttunement)
            
            .WhereIf(filter.MinWeight, t => t.Weight >= filter.MinWeight)
            .WhereIf(filter.MaxWeight, t => t.Weight <= filter.MaxWeight)
            .WhereIf(filter.MinValue, t => t.Value >= filter.MinValue)
            .WhereIf(filter.MaxValue, t => t.Value <= filter.MaxValue)

            .WhereIf(filter.ToolCategory, t => t.ToolCategory.Contains(filter.ToolCategory!))

            .WhereIf(filter.IsHomebrew, t => t.IsHomebrew == filter.IsHomebrew)
            .WhereIf(filter.CloningAllowed, t => t.CloningAllowed == filter.CloningAllowed)
            .SortBy(normalizedSortBy, sortSelectorsMap, SortToolOption.Default, filter.SortDescending); 
        
        var itemCount = await query.CountAsync();
        var filteredItems = await query
            .Skip((pagination.Page - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToListAsync();

        return (itemCount, filteredItems);
    }

    private readonly Dictionary<string, IEnumerable<Func<Tool, object>>> sortSelectorsMap = new()
    {
        { SortToolOption.Name, [(s => s.Name)] },
        { SortToolOption.Category, [(s => s.ToolCategory), (s => s.Name)] },
        { SortToolOption.Value, [(s => s.Value!), (s => s.Name)] },
        { SortToolOption.Rarity, [(s => s.Rarity == null), (s => s.Rarity!), (s => s.Name)] },
    };
}