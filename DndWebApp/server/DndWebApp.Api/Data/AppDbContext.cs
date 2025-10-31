using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.Features;
using DndWebApp.Api.Models.Items;
using DndWebApp.Api.Models.Spells;
using DndWebApp.Api.Models.World;
using Microsoft.EntityFrameworkCore;

namespace DndWebApp.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Character> Characters { get; set; }
    public DbSet<Inventory> Inventories { get; set; }

    public DbSet<Ability> AbilityScores { get; set; }
    public DbSet<AbilityValue> AbilityValues { get; set; }
    public DbSet<Skill> Skills { get; set; }

    public DbSet<Alignment> Alignments { get; set; }
    public DbSet<Language> Languages { get; set; }
    public DbSet<Option> Choices { get; set; }

    public DbSet<Class> Classes { get; set; }
    public DbSet<ClassLevel> ClassLevels { get; set; }

    public DbSet<Race> Races { get; set; }
    public DbSet<Subrace> SubRaces { get; set; }
    public DbSet<Background> Backgrounds { get; set; }

    public DbSet<Trait> Traits { get; set; }
    public DbSet<Feat> Feats { get; set; }
    public DbSet<ClassFeature> ClassFeatures { get; set; }
    public DbSet<BackgroundFeature> BackgroundFeatures { get; set; }

    public DbSet<Item> Items { get; set; }
    public DbSet<Weapon> Weapons { get; set; }
    public DbSet<Armor> Armor { get; set; }
    public DbSet<Tool> Tools { get; set; }

    public DbSet<Spell> Spells { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ClassLevel>()
            .OwnsMany(c => c.ClassSpecificSlotsAtLevel, slot =>
            {
                slot.WithOwner().HasForeignKey("ClassLevelId");
                slot.HasKey(s => s.Id);
            });

        modelBuilder.Entity<Character>()
            .OwnsMany(c => c.SavingThrows);

        modelBuilder.Entity<Character>()
            .OwnsMany(c => c.ArmorProficiencies);

        modelBuilder.Entity<Character>()
            .OwnsMany(c => c.WeaponProficiencies);

        modelBuilder.Entity<Character>()
            .OwnsMany(c => c.Languages);

        modelBuilder.Entity<Character>()
            .OwnsMany(c => c.DamageAffinities);

        modelBuilder.Entity<Character>()
            .OwnsMany(c => c.ToolProficiencies);

        modelBuilder.Entity<Character>()
            .OwnsMany(c => c.SkillProficiencies);

        modelBuilder.Entity<Tool>()
            .OwnsMany(t => t.Activities);

        modelBuilder.Entity<Tool>()
            .OwnsMany(t => t.Properties);

    }
}
