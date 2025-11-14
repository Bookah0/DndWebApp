using System.Text.Json.Serialization;

namespace DndWebApp.Api.Models.DTOs.ExternalDtos;

public class EClassLevelDto
{
    [JsonPropertyName("level")]
    public required int Level { get; set; }

    [JsonPropertyName("ability_score_bonuses")]
    public required int AbilityScoreBonuses { get; set; }

    [JsonPropertyName("prof_bonus")]
    public required int ProficiencyBonus { get; set; }

    [JsonPropertyName("features")]
    public required List<EIndexDto> Features { get; set; }

    [JsonPropertyName("class_specific")]
    public required EClassSpecificDto ClassSpecific { get; set; }
    
    [JsonPropertyName("spellcasting")]
    public required ESpellcastingDto? Spellcasting { get; set; }

    [JsonPropertyName("subclasses")]
    public required List<EIndexDto>? Subclasses { get; set; }
}

public class ESpellcastingDto
{
    [JsonPropertyName("cantrips_known")]
    public int? CantripsKnown { get; set; }
    
    [JsonPropertyName("spell_slots_level_1")]
    public int? SpellSlotsLevel1 { get; set; }

    [JsonPropertyName("spell_slots_level_2")]
    public int? SpellSlotsLevel2 { get; set; }

    [JsonPropertyName("spell_slots_level_3")]
    public int? SpellSlotsLevel3 { get; set; }

    [JsonPropertyName("spell_slots_level_4")]
    public int? SpellSlotsLevel4 { get; set; }

    [JsonPropertyName("spell_slots_level_5")]
    public int? SpellSlotsLevel5 { get; set; }

    [JsonPropertyName("spell_slots_level_6")]
    public int? SpellSlotsLevel6 { get; set; }

    [JsonPropertyName("spell_slots_level_7")]
    public int? SpellSlotsLevel7 { get; set; }

    [JsonPropertyName("spell_slots_level_8")]
    public int? SpellSlotsLevel8 { get; set; }

    [JsonPropertyName("spell_slots_level_9")]
    public int? SpellSlotsLevel9 { get; set; }
}

public class EClassSpecificDto
{
    // Fighter
    [JsonPropertyName("action_surges")]
    public int? ActionSurges { get; set; }

    [JsonPropertyName("indomitable_uses")]
    public int? IndomitableUses { get; set; }

    [JsonPropertyName("extra_attacks")]
    public int? ExtraAttacks { get; set; }

    // Barbarian
    [JsonPropertyName("rage_count")]
    public int? RageCount { get; set; }

    [JsonPropertyName("rage_damage_bonus")]
    public int? RageDamageBonus { get; set; }

    [JsonPropertyName("brutal_critical_dice")]
    public int? BrutalCriticalDice { get; set; }

    // Bard
    [JsonPropertyName("bardic_inspiration_die")]
    public int? BardicInspirationDie { get; set; }

    [JsonPropertyName("song_of_rest_die")]
    public int? SongOfRestDie { get; set; }

    [JsonPropertyName("magical_secrets_max_5")]
    public int? MagicalSecretsMax5 { get; set; }

    [JsonPropertyName("magical_secrets_max_7")]
    public int? MagicalSecretsMax7 { get; set; }

    [JsonPropertyName("magical_secrets_max_9")]
    public int? MagicalSecretsMax9 { get; set; }

    // Cleric/Paladin
    [JsonPropertyName("channel_divinity_charges")]
    public int? ChannelDivinityCharges { get; set; }

    [JsonPropertyName("destroy_undead_cr")]
    public double? DestroyUndeadCr { get; set; }

    // Druid
    [JsonPropertyName("wild_shape_max_cr")]
    public double? WildShapeMaxCr { get; set; }

    [JsonPropertyName("wild_shape_swim")]
    public bool? WildShapeSwim { get; set; }

    [JsonPropertyName("wild_shape_fly")]
    public bool? WildShapeFly { get; set; }

    // Monk
    [JsonPropertyName("ki_points")]
    public int? KiPoints { get; set; }

    [JsonPropertyName("unarmored_movement")]
    public int? UnarmoredMovement { get; set; }

    [JsonPropertyName("martial_arts")]
    public EMartialArtsDto? MartialArts { get; set; }

    // Paladin
    [JsonPropertyName("aura_range")]
    public int? AuraRange { get; set; }

    // Ranger
    [JsonPropertyName("favored_enemies")]
    public int? FavoredEnemies { get; set; }

    [JsonPropertyName("favored_terrain")]
    public int? FavoredTerrain { get; set; }

    // Rogue
    [JsonPropertyName("sneak_attack")]
    public ESneakAttackDto? SneakAttack { get; set; }

    // Sorcerer
    [JsonPropertyName("sorcery_points")]
    public int? SorceryPoints { get; set; }

    [JsonPropertyName("metamagic_known")]
    public int? MetamagicKnown { get; set; }

    [JsonPropertyName("creating_spell_slots")]
    public List<ECreatingSpellSlotsDto>? CreatingSpellSlots { get; set; }

    // Warlock
    [JsonPropertyName("invocations_known")]
    public int? InvocationsKnown { get; set; }

    [JsonPropertyName("mystic_arcanum_level_6")]
    public int? MysticArcanumLevel6 { get; set; }

    [JsonPropertyName("mystic_arcanum_level_7")]
    public int? MysticArcanumLevel7 { get; set; }

    [JsonPropertyName("mystic_arcanum_level_8")]
    public int? MysticArcanumLevel8 { get; set; }

    [JsonPropertyName("mystic_arcanum_level_9")]
    public int? MysticArcanumLevel9 { get; set; }

    // Wizard
    [JsonPropertyName("arcane_recovery_levels")]
    public int? ArcaneRecoveryLevels { get; set; }
}

public class EMartialArtsDto
{
    [JsonPropertyName("dice_count")]
    public int DiceCount { get; set; }

    [JsonPropertyName("dice_value")]
    public int DiceValue { get; set; }
}

public class ESneakAttackDto
{
    [JsonPropertyName("dice_count")]
    public int DiceCount { get; set; }

    [JsonPropertyName("dice_value")]
    public int DiceValue { get; set; }
}

public class ECreatingSpellSlotsDto
{
    [JsonPropertyName("spell_slot_level")]
    public int SpellSlotLevel { get; set; }

    [JsonPropertyName("sorcery_point_cost")]
    public int SorceryPointCost { get; set; }
}