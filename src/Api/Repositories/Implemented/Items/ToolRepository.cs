using Api.Data;
using Api.Middlewares.ExceptionHandling;
using Api.Models.Items;
using Api.Repositories.Interfaces;
using Api.Validation.AllowedValues;
using Microsoft.EntityFrameworkCore;
using static Api.Validation.AllowedValues.ValuesValidator;
using static Api.Services.Util.QueryUtil;
using Api.Models.DTOs.Items;
using Api.Models.DTOs.ResponseDtos;

namespace Api.Repositories.Implemented.Items;

public class ToolRepository(AppDbContext context) : IToolRepository
{
    public async Task<Tool> GetByIdAsync(int id) => 
        await context.Tools.FirstOrDefaultAsync(t => t.Id == id)
            ?? throw new Exception($"Tool with id {id} could not be found");

    public async Task<Tool> GetWithAllDataAsync(int id) => 
        await context.Tools
            .Include(t => t.Properties)
            .Include(t => t.Activities)
            .FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new Exception($"Tool with id {id} could not be found");

    public async Task<ICollection<Tool>> GetAllAsync() => await context.Tools.ToListAsync();

    public async Task<ICollection<Tool>> GetAllWithAllDataAsync() => 
        await context.Tools
            .Include(t => t.Properties)
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
        var query = context.Tools
            .AsQueryable()
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
            .WhereIf(filter.CloningAllowed, t => t.CloningAllowed == filter.CloningAllowed);

        if (filter.SortBy is not null)
            query = SortBy(query, filter.SortBy, filter.SortDescending);
        
        var itemCount = await query.CountAsync();
        var filteredItems = await query
            .Skip((pagination.Page - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToListAsync();

        return (itemCount, filteredItems);
    }

    public IQueryable<Tool> SortBy(IQueryable<Tool> query, string sortFilter, bool descending = false)
    {
        if(!TryNormalizeValue<SortToolOption>(sortFilter, out string? normalized))
            return context.Tools;

        return normalized switch
        {
            SortToolOption.Name => OrderByMany(query, [(t => t.Name)], descending),
            SortToolOption.Category => OrderByMany(query, [(t => t.ToolCategory), (t => t.Name)], descending),
            SortToolOption.Value => OrderByMany(query, [(t => t.Value!), (t => t.Name)], descending),
            SortToolOption.Rarity => OrderByMany(query, [(t => t.Rarity == null), (t => t.Rarity!), (t => t.Name)], descending),
            _ => throw new ValidationException($"Invalid sort option: {sortFilter}")
        };
    }
}