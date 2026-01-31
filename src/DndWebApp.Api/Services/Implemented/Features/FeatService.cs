using AutoMapper;
using DndWebApp.Api.Middlewares.ExceptionHandling;
using DndWebApp.Api.Models.DTOs.Features;
using DndWebApp.Api.Models.DTOs.ResponseDtos;
using DndWebApp.Api.Models.Features;
using DndWebApp.Api.Repositories.Interfaces;
using DndWebApp.Api.Services.Interfaces.Features;
using static DndWebApp.Api.Services.Util.SortUtil;

namespace DndWebApp.Api.Services.Implemented.Features;

public class FeatService : BaseFeatureService<Feat>, IFeatService
{

    public FeatService(IFeatRepository repo, ISpellRepository spellRepo, ILogger<FeatService> logger, IMapper mapper) : base(repo, spellRepo, logger, mapper)
    {
    }

    public async Task<FeatResponseDto> CreateAsync(FeatDto dto)
    {
        var feat = new Feat
        {
            Name = dto.Name,
            Description = dto.Description,
            IsHomebrew = dto.IsHomebrew
        };

        return mapper.Map<FeatResponseDto>(await repo.CreateAsync(feat));
    }

    public async Task DeleteAsync(int id)
    {
        var feat = await repo.GetByIdAsync(id) ?? throw new NotFoundException($"Feat with id {id} could not be found");
        await repo.DeleteAsync(feat);
    }

    public async Task<ICollection<FeatResponseDto>> GetAllAsync()
    {
        return mapper.Map<ICollection<FeatResponseDto>>(await repo.GetAllAsync());
    }

    public async Task<FeatResponseDto> GetByIdAsync(int id)
    {
        return mapper.Map<FeatResponseDto>(await repo.GetByIdAsync(id) 
            ?? throw new NotFoundException($"Feat with id {id} could not be found"));
    }

    public async Task UpdateAsync(int id, FeatDto dto)
    {
        var feat = await repo.GetByIdAsync(id) ?? throw new NotFoundException($"Feat with id {id} could not be found");

        feat.Name = dto.Name;
        feat.Description = dto.Description;
        await repo.UpdateAsync(feat);
    }

    public Task UpdateCollectionsAsync(int id, FeatDto dto)
    {
        throw new NotImplementedException();
    }

    public ICollection<Feat> SortBy(ICollection<Feat> feats, bool descending = false)
    {
        return OrderByMany(feats, [(f => f.Name)], descending);
    }
}