using Microsoft.EntityFrameworkCore;
using DndWebApp.Api.Data;
using DndWebApp.Api.Repositories.Interfaces;
using DndWebApp.Api.Repositories.Implemented;
using DndWebApp.Api.Repositories.Implemented.Classes;
using DndWebApp.Api.Repositories.Implemented.Species;
using DndWebApp.Api.Repositories.Implemented.Features;
using DndWebApp.Api.Repositories.Implemented.Items;
using DndWebApp.Api.Repositories.Implemented.Spells;
using DndWebApp.Api.Services.Interfaces;
using DndWebApp.Api.Services.Implemented;
using DndWebApp.Api.Services.Implemented.Features;
using DndWebApp.Api.Services.Interfaces.Features;

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
builder.Services.AddScoped<IBackgroundRepository, BackgroundRepository>();
builder.Services.AddScoped<IRaceRepository, RaceRepository>();
builder.Services.AddScoped<ISubraceRepository, SubraceRepository>();

builder.Services.AddScoped<IBackgroundFeatureRepository, BackgroundFeatureRepository>();
builder.Services.AddScoped<IFeatRepository, FeatRepository>();
builder.Services.AddScoped<ITraitRepository, TraitRepository>();
builder.Services.AddScoped<IClassFeatureRepository, ClassFeatureRepository>();

builder.Services.AddScoped<ICharacterRepository, CharacterRepository>();
builder.Services.AddScoped<IInventoryRepository, InventoryRepository>();

builder.Services.AddScoped<IItemRepository, ItemRepository>();
builder.Services.AddScoped<IToolRepository, ToolRepository>();
builder.Services.AddScoped<ISpellRepository, SpellRepository>();


builder.Services.AddScoped<IAlignmentService, AlignmentService>();
builder.Services.AddScoped<ISkillService, SkillService>();
builder.Services.AddScoped<IAbilityService, AbilityService>();
builder.Services.AddScoped<ILanguageService, LanguageService>();
builder.Services.AddScoped<ISpellService, SpellService>();

builder.Services.AddScoped(typeof(IBaseFeatureService<>), typeof(BaseFeatureService<>));
builder.Services.AddScoped<IBackgroundFeatureService, BackgroundFeatureService>();
builder.Services.AddScoped<IFeatService, FeatService>();
builder.Services.AddScoped<ITraitService, TraitService>();
builder.Services.AddScoped<IClassFeatureService, ClassFeatureService>();

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
