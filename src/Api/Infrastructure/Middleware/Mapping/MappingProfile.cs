using Api.Domain.Characters.DTOs;
using Api.Domain.Characters.Models;
using Api.Domain.Items.DTOs;
using Api.Domain.Items.Models;
using Api.Domain.Languages.DTOs;
using Api.Domain.Languages.Models;
using Api.Domain.Skills.DTOs;
using Api.Domain.Skills.Models;
using Api.Domain.Spells.DTOs;
using Api.Domain.Spells.Models;
using Api.Domain.Species.DTOs;
using Api.Domain.Species.Models;
using Api.Domain.Alignments.DTOs;
using Api.Domain.Alignments.Models;
using Api.Domain.Backgrounds.DTOs;
using Api.Domain.Backgrounds.Models;
using Api.Domain.Abilities.DTOs;
using Api.Domain.Abilities.Models;
using AutoMapper;
using Api.Domain.Shared.DTOs;
using Api.Domain.Classes.Models;
using Api.Domain.Feats.Models;
using Api.Domain.Classes.DTOs;
using Api.Domain.Shared.Models;
using Api.Domain.Users.DTOs;
using Api.Domain.Users.Models;

namespace Api.Infrastructure.Middleware.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Entity to Request DTO
        CreateMap<Character, CreateCharacterRequestDto>();
        CreateMap<Character, UpdateCharacterRequestDto>();
        CreateMap<CharacterInfo, CharacterInfoRequestDto>();
        CreateMap<Currency, CurrencyDto>();
        CreateMap<Inventory, CreateInventoryDto>();
        CreateMap<Background, CreateBackgroundRequestDto>();
        CreateMap<Background, UpdateBackgroundRequestDto>();
        CreateMap<Alignment, AlignmentRequestDto>();
        CreateMap<Ability, AbilityRequestDto>();
        CreateMap<AbilityValue, AbilityValueDto>();
        CreateMap<Skill, CreateSkillRequestDto>();
        CreateMap<Skill, UpdateSkillRequestDto>();
        CreateMap<Language, CreateLanguageRequestDto>();
        CreateMap<Language, UpdateLanguageRequestDto>();
        CreateMap<Spell, CreateSpellRequestDto>();
        CreateMap<Spell, UpdateSpellRequestDto>();
        CreateMap<SpeciesInfo, SpeciesInfoDto>();
        
        CreateMap<Trait, CreateTraitRequestDto>();
        CreateMap<Trait, UpdateTraitRequestDto>();
        CreateMap<BackgroundFeature, CreateBackgroundFeatureRequestDto>();
        CreateMap<BackgroundFeature, UpdateBackgroundFeatureRequestDto>();
        CreateMap<ClassFeature, CreateClassFeatureRequestDto>();
        CreateMap<ClassFeature, UpdateClassFeatureRequestDto>();
        CreateMap<Feat, CreateFeatRequestDto>();
        CreateMap<Feat, UpdateFeatRequestDto>();

        CreateMap<BaseClass, CreateClassRequestDto>();
        CreateMap<Subclass, CreateSubclassRequestDto>();
        CreateMap<BaseClass, UpdateClassRequestDto>();
        CreateMap<Subclass, UpdateSubclassRequestDto>();
        CreateMap<ClassLevel, CreateClassLevelRequestDto>();
        CreateMap<ClassLevel, UpdateClassLevelRequestDto>();
        CreateMap<Race, CreateRaceRequestDto>();
        CreateMap<Race, UpdateRaceRequestDto>();
        CreateMap<Subrace, CreateSubraceRequestDto>();
        CreateMap<Subrace, UpdateSubraceRequestDto>();

        CreateMap<Item, CreateItemRequestDto>();
        CreateMap<Weapon, CreateWeaponRequestDto>();
        CreateMap<Armor, CreateArmorRequestDto>();
        CreateMap<Tool, CreateToolRequestDto>();
        CreateMap<InventoryItem, InventoryItemResponseDto>();

        CreateMap<SkillProficiencyChoice, SkillProficiencyChoiceDto>();
        CreateMap<LanguageChoice, LanguageProficiencyChoiceDto>();
        CreateMap<WeaponCategoryProficiencyChoice, WeaponCategoryProficiencyChoiceDto>();
        CreateMap<WeaponTypeProficiencyChoice, WeaponTypeProficiencyChoiceDto>();
        CreateMap<ToolProficiencyChoice, ToolProficiencyChoiceDto>();
        CreateMap<AbilityIncreaseChoice, AbilityIncreaseChoiceDto>();
        CreateMap<ArmorProficiencyChoice, ArmorProficiencyChoiceDto>();

        // Entity to Response DTO
        CreateMap<Character, CharacterResponseDto>();
        CreateMap<CombatStats, CombatStatsResponseDto>();
        CreateMap<CharacterInfo, CharacterInfoResponseDto>();
        CreateMap<EquipmentSlot, EquipmentSlotDto>();
        CreateMap<Inventory, InventoryResponseDto>();
        CreateMap<EquipmentSlot, EquippedItemDto>();
        CreateMap<Background, BackgroundResponseDto>();
        CreateMap<Alignment, AlignmentResponseDto>();
        CreateMap<Ability, AbilityResponseDto>();
        CreateMap<AbilityValue, AbilityValueDto>();
        CreateMap<Skill, SkillResponseDto>();
        CreateMap<Language, LanguageResponseDto>();
        CreateMap<Spell, SpellResponseDto>();
        
        CreateMap<Trait, TraitResponseDto>();
        CreateMap<BackgroundFeature, BackgroundFeatureResponseDto>();
        CreateMap<ClassFeature, ClassFeatureResponseDto>();
        CreateMap<Feat, FeatResponseDto>();

        CreateMap<BaseClass, ClassResponseDto>();
        CreateMap<Subclass, SubclassResponseDto>();
        CreateMap<ClassLevel, ClassLevelResponseDto>();
        CreateMap<Race, RaceResponseDto>();
        CreateMap<Subrace, SubraceResponseDto>();

        CreateMap<Item, ItemResponseDto>().IncludeAllDerived();
        CreateMap<Weapon, WeaponResponseDto>();
        CreateMap<Armor, ArmorResponseDto>();
        CreateMap<Tool, ToolResponseDto>();

        CreateMap<User, GetUserResponseDto>();
        CreateMap<User, RegisterUserResponseDto>();
        CreateMap<User, UpdateUserResponseDto>();

        CreateMap<SkillProficiencyChoice, SkillProficiencyChoiceResponseDto>();
        CreateMap<LanguageChoice, LanguageChoiceResponseDto>();
        CreateMap<WeaponCategoryProficiencyChoice, WeaponProficiencyChoiceResponseDto>();
        CreateMap<WeaponTypeProficiencyChoice, WeaponProficiencyChoiceResponseDto>();
        CreateMap<ToolProficiencyChoice, ToolProficiencyChoiceResponseDto>();
        CreateMap<AbilityIncreaseChoice, AbilityIncreaseChoiceResponseDto>();
        CreateMap<ArmorProficiencyChoice, ArmorProficiencyChoiceResponseDto>();

        // Owned types to DTO
        CreateMap<SpellTargeting, SpellTargetingDto>();
        CreateMap<CastingRequirements, CastingRequirementsDto>();

        // Request DTO to Response DTO
        CreateMap<CreateCharacterRequestDto, CharacterResponseDto>();
        CreateMap<UpdateCharacterRequestDto, CharacterResponseDto>();
        CreateMap<CharacterInfoRequestDto, CharacterInfoResponseDto>();
        CreateMap<CreateInventoryDto, InventoryResponseDto>();
        CreateMap<CreateBackgroundRequestDto, BackgroundResponseDto>();
        CreateMap<UpdateBackgroundRequestDto, BackgroundResponseDto>();
        CreateMap<AlignmentRequestDto, AlignmentResponseDto>();
        CreateMap<AbilityRequestDto, AbilityResponseDto>();
        CreateMap<AbilityValueDto, AbilityValueDto>();
        CreateMap<CreateSkillRequestDto, SkillResponseDto>();
        CreateMap<UpdateSkillRequestDto, SkillResponseDto>();
        CreateMap<CreateLanguageRequestDto, LanguageResponseDto>();
        CreateMap<UpdateLanguageRequestDto, LanguageResponseDto>();
        CreateMap<CreateSpellRequestDto, SpellResponseDto>();
        CreateMap<UpdateSpellRequestDto, SpellResponseDto>();
        
        CreateMap<CreateTraitRequestDto, TraitResponseDto>();
        CreateMap<UpdateTraitRequestDto, TraitResponseDto>();
        CreateMap<CreateBackgroundFeatureRequestDto, Domain.Shared.DTOs.BackgroundFeatureResponseDto>();
        CreateMap<UpdateBackgroundFeatureRequestDto, BackgroundFeatureResponseDto>();
        CreateMap<CreateClassFeatureRequestDto, ClassFeatureResponseDto>();
        CreateMap<UpdateClassFeatureRequestDto, ClassFeatureResponseDto>();
        CreateMap<CreateFeatRequestDto, FeatResponseDto>();
        CreateMap<UpdateFeatRequestDto, FeatResponseDto>();

        CreateMap<CreateClassRequestDto, ClassResponseDto>();
        CreateMap<UpdateClassRequestDto, ClassResponseDto>();
        CreateMap<CreateSubclassRequestDto, SubclassResponseDto>();
        CreateMap<UpdateSubclassRequestDto, SubclassResponseDto>();
        CreateMap<CreateClassLevelRequestDto, ClassLevelResponseDto>();
        CreateMap<UpdateClassLevelRequestDto, ClassLevelResponseDto>();
        CreateMap<CreateRaceRequestDto, RaceResponseDto>();
        CreateMap<CreateRaceRequestDto, RaceResponseDto>();
        CreateMap<CreateSubraceRequestDto, SubraceResponseDto>();
        CreateMap<UpdateSubraceRequestDto, SubraceResponseDto>();

        CreateMap<CreateItemRequestDto, ItemResponseDto>();
        CreateMap<CreateWeaponRequestDto, WeaponResponseDto>();
        CreateMap<CreateArmorRequestDto, ArmorResponseDto>();
        CreateMap<CreateToolRequestDto, ToolResponseDto>();

        CreateMap<SkillProficiencyChoiceDto, SkillProficiencyChoiceResponseDto>();
        CreateMap<LanguageProficiencyChoiceDto, LanguageChoiceResponseDto>();
        CreateMap<WeaponCategoryProficiencyChoiceDto, WeaponProficiencyChoiceResponseDto>();
        CreateMap<WeaponTypeProficiencyChoiceDto, WeaponProficiencyChoiceResponseDto>();
        CreateMap<ToolProficiencyChoiceDto, ToolProficiencyChoiceResponseDto>();
        CreateMap<AbilityIncreaseChoiceDto, AbilityIncreaseChoiceResponseDto>();
        CreateMap<ArmorProficiencyChoiceDto, ArmorProficiencyChoiceResponseDto>();
        
    }
}