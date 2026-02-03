using DndWebApp.Api.Data;
using DndWebApp.Api.Models.DTOs.Features;
using DndWebApp.Api.Models.Features;
using DndWebApp.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DndWebApp.Api.Repositories.Implemented.Features;

public class ChoiceRepository<T> : EfRepository<T>, IChoiceRepository<T> where T : class, IFeatureChoice
{
    public ChoiceRepository(AppDbContext context) : base(context) { }

}