using Microsoft.EntityFrameworkCore;
using DndWebApp.Api.Data;
using DndWebApp.Api.Repositories;
using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Repositories.Features;
using DndWebApp.Api.Repositories.Characters;
using DndWebApp.Api.Repositories.Abilities;
using DndWebApp.Api.Repositories.Skills;
using DndWebApp.Api.Repositories.Items;
using DndWebApp.Api.Repositories.Species;
using DndWebApp.Api.Repositories.Spells;
using DndWebApp.Api.Repositories.Classes;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"),
    npgsql => npgsql.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)));

builder.Services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));

builder.Services.AddScoped<IAbilityRepository, AbilityRepository>();
builder.Services.AddScoped<ISkillRepository, SkillRepository>();

builder.Services.AddScoped<IClassLevelRepository, ClassLevelRepository>();
builder.Services.AddScoped<IClassRepository, ClassRepository>();

builder.Services.AddScoped<IFeatureRepository, FeatureRepository>();
builder.Services.AddScoped<IFeatRepository, FeatRepository>();
builder.Services.AddScoped<ITraitRepository, TraitRepository>();
builder.Services.AddScoped<IClassFeatureRepository, ClassFeatureRepository>();

builder.Services.AddScoped<ICharacterRepository, CharacterRepository>();
builder.Services.AddScoped<IInventoryRepository, InventoryRepository>();

builder.Services.AddScoped<IItemRepository, ItemRepository>();
builder.Services.AddScoped<IToolRepository, ToolRepository>();

builder.Services.AddScoped<IRaceRepository, RaceRepository>();
builder.Services.AddScoped<ISubraceRepository, SubraceRepository>();

builder.Services.AddScoped<ISpellRepository, SpellRepository>();

builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // app.UseSwagger();
    // app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
