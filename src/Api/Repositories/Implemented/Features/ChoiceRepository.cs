using Api.Data;
using Api.Models.DTOs.Features;
using Api.Models.Features;
using Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories.Implemented.Features;

public class ChoiceRepository<T> : EfRepository<T>, IChoiceRepository<T> where T : class, IFeatureChoice
{
    public ChoiceRepository(AppDbContext context) : base(context) { }

}