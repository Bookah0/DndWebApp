using Microsoft.EntityFrameworkCore;
using DndWebApp.Api.Data;
using DndWebApp.Api.Repositories;
using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.Items;
using DndWebApp.Api.Models.Spells;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));

builder.Services.AddScoped<IRepository<Character>, CharacterRepository>();
builder.Services.AddScoped<IRepository<Ability>, AbilityRepository>();
builder.Services.AddScoped<IRepository<Skill>, SkillRepository>();

builder.Services.AddScoped<IRepository<Trait>, TraitRepository>();
builder.Services.AddScoped<IRepository<Feat>, FeatRepository>();
builder.Services.AddScoped<IRepository<ClassFeature>, ClassFeatureRepository>();
builder.Services.AddScoped<IRepository<PassiveEffect>, PassiveEffectRepository>();

builder.Services.AddScoped<IRepository<Item>, ItemRepository>();
builder.Services.AddScoped<IRepository<Weapon>, WeaponRepository>();
builder.Services.AddScoped<IRepository<Armor>, ArmorRepository>();
builder.Services.AddScoped<IRepository<Tool>, ToolRepository>();

builder.Services.AddScoped<IRepository<Race>, RaceRepository>();
builder.Services.AddScoped<IRepository<Subrace>, SubraceRepository>();

builder.Services.AddScoped<IRepository<Spell>, SpellRepository>();

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
