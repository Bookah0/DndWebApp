using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.Items;
using DndWebApp.Api.Models.Spells;
using DndWebApp.Api.Models.World;
using Microsoft.EntityFrameworkCore;

namespace DndWebApp.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Character> Characters { get; set; }
    public DbSet<Ability> AbilityScores { get; set; }
    public DbSet<Class> Classes { get; set; }
    public DbSet<Race> Races { get; set; }
    public DbSet<Subrace> SubRaces { get; set; }
    public DbSet<Skill> Skills { get; set; }

    public DbSet<PassiveEffect> PassiveEffects { get; set; }
    public DbSet<Trait> Traits { get; set; }
    public DbSet<Feat> Feats { get; set; }
    public DbSet<ClassFeature> Features { get; set; }
        
    public DbSet<Item> Items { get; set; }
    public DbSet<Item> Weapons { get; set; }
    public DbSet<Item> Armor { get; set; }
    public DbSet<Item> Tools { get; set; }
    
    public DbSet<Spell> Spells { get; set; }
    public DbSet<Alignment> Alignments { get; set; }
    public DbSet<Language> Languages { get; set; }
    
}
