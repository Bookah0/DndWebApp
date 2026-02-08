namespace Api.Middlewares.Mapping;

using AutoMapper;
using Api.Models.Characters;
using Api.Models.DTOs;
using Api.Models.DTOs.Features;
using Api.Models.DTOs.RequestDtos.Inventory;
using Api.Models.DTOs.RequestDtos.Character;
using Api.Models.DTOs.ResponseDtos;
using Api.Models.DTOs.Spells;
using Api.Models.Features;
using Api.Models.Items;
using Api.Models.Spells;
using Api.Models.World;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Entity to Request DTO
        CreateMap<Character, CreateCharacterRequestDto>();
        CreateMap<Character, UpdateCharacterRequestDto>();
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

        CreateMap<SkillProficiencyChoice, SkillProficiencyChoiceDto>();
        CreateMap<LanguageChoice, LanguageProficiencyChoiceDto>();
        CreateMap<WeaponCategoryProficiencyChoice, WeaponCategoryProficiencyChoiceDto>();
        CreateMap<WeaponTypeProficiencyChoice, WeaponTypeProficiencyChoiceDto>();
        CreateMap<ToolProficiencyChoice, ToolProficiencyChoiceDto>();
        CreateMap<AbilityIncreaseChoice, AbilityIncreaseChoiceDto>();
        CreateMap<ArmorProficiencyChoice, ArmorProficiencyChoiceDto>();

        // Entity to Response DTO
        CreateMap<Character, CharacterResponseDto>();
        CreateMap<Inventory, InventoryResponseDto>();
        CreateMap<Background, BackgroundResponseDto>();
        CreateMap<Alignment, AlignmentResponseDto>();
        CreateMap<Ability, AbilityResponseDto>();
        CreateMap<AbilityValue, AbilityValueResponseDto>();
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

        CreateMap<Item, ItemResponseDto>();
        CreateMap<Weapon, WeaponResponseDto>();
        CreateMap<Armor, ArmorResponseDto>();
        CreateMap<Tool, ToolResponseDto>();

        CreateMap<SkillProficiencyChoice, SkillProficiencyChoiceResponseDto>();
        CreateMap<LanguageChoice, LanguageChoiceResponseDto>();
        CreateMap<WeaponCategoryProficiencyChoice, WeaponProficiencyChoiceResponseDto>();
        CreateMap<WeaponTypeProficiencyChoice, WeaponProficiencyChoiceResponseDto>();
        CreateMap<ToolProficiencyChoice, ToolProficiencyChoiceResponseDto>();
        CreateMap<AbilityIncreaseChoice, AbilityIncreaseChoiceResponseDto>();
        CreateMap<ArmorProficiencyChoice, ArmorProficiencyChoiceResponseDto>();

        // Request DTO to Response DTO
        CreateMap<CreateCharacterRequestDto, CharacterResponseDto>();
        CreateMap<UpdateCharacterRequestDto, CharacterResponseDto>();
        CreateMap<CreateInventoryDto, InventoryResponseDto>();
        CreateMap<CreateBackgroundRequestDto, BackgroundResponseDto>();
        CreateMap<UpdateBackgroundRequestDto, BackgroundResponseDto>();
        CreateMap<AlignmentRequestDto, AlignmentResponseDto>();
        CreateMap<AbilityRequestDto, AbilityResponseDto>();
        CreateMap<AbilityValueDto, AbilityValueResponseDto>();
        CreateMap<CreateSkillRequestDto, SkillResponseDto>();
        CreateMap<UpdateSkillRequestDto, SkillResponseDto>();
        CreateMap<CreateLanguageRequestDto, LanguageResponseDto>();
        CreateMap<UpdateLanguageRequestDto, LanguageResponseDto>();
        CreateMap<CreateSpellRequestDto, SpellResponseDto>();
        CreateMap<UpdateSpellRequestDto, SpellResponseDto>();
        
        CreateMap<CreateTraitRequestDto, TraitResponseDto>();
        CreateMap<UpdateTraitRequestDto, TraitResponseDto>();
        CreateMap<CreateBackgroundFeatureRequestDto, BackgroundFeatureResponseDto>();
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