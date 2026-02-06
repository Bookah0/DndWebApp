namespace Api.Middlewares.Mapping;

using AutoMapper;
using Api.Models.Characters;
using Api.Models.DTOs;
using Api.Models.DTOs.Features;
using Api.Models.DTOs.Inventory;
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
        CreateMap<Character, CharacterDto>();
        CreateMap<Inventory, CreateInventoryDto>();
        CreateMap<Inventory, UpdateInventoryDto>();
        CreateMap<Background, BackgroundDto>();
        CreateMap<Alignment, AlignmentDto>();
        CreateMap<Ability, AbilityDto>();
        CreateMap<AbilityValue, AbilityValueDto>();
        CreateMap<Skill, SkillDto>();
        CreateMap<Language, LanguageDto>();
        CreateMap<Spell, SpellDto>();
        
        CreateMap<Trait, TraitDto>();
        CreateMap<BackgroundFeature, BackgroundFeatureDto>();
        CreateMap<ClassFeature, ClassFeatureDto>();
        CreateMap<Feat, FeatDto>();

        CreateMap<BaseClass, ClassDto>();
        CreateMap<Subclass, ClassDto>();
        CreateMap<ClassLevel, ClassLevelDto>();
        CreateMap<Race, RaceDto>();
        CreateMap<Subrace, SubraceDto>();

        CreateMap<Item, ItemDto>();
        CreateMap<Weapon, WeaponDto>();
        CreateMap<Armor, ArmorDto>();
        CreateMap<Tool, ToolDto>();

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

        // Request DTO to Entity
        CreateMap<CharacterDto, CharacterResponseDto>();
        CreateMap<CreateInventoryDto, InventoryResponseDto>();
        CreateMap<UpdateInventoryDto, InventoryResponseDto>();
        CreateMap<BackgroundDto, BackgroundResponseDto>();
        CreateMap<AlignmentDto, AlignmentResponseDto>();
        CreateMap<AbilityDto, AbilityResponseDto>();
        CreateMap<AbilityValueDto, AbilityValueResponseDto>();
        CreateMap<SkillDto, SkillResponseDto>();
        CreateMap<LanguageDto, LanguageResponseDto>();
        CreateMap<SpellDto, SpellResponseDto>();
        
        CreateMap<TraitDto, TraitResponseDto>();
        CreateMap<BackgroundFeatureDto, BackgroundFeatureResponseDto>();
        CreateMap<ClassFeatureDto, ClassFeatureResponseDto>();
        CreateMap<FeatDto, FeatResponseDto>();

        CreateMap<ClassDto, ClassResponseDto>();
        CreateMap<ClassDto, SubclassResponseDto>();
        CreateMap<ClassLevelDto, ClassLevelResponseDto>();
        CreateMap<RaceDto, RaceResponseDto>();
        CreateMap<SubraceDto, SubraceResponseDto>();

        CreateMap<ItemDto, ItemResponseDto>();
        CreateMap<WeaponDto, WeaponResponseDto>();
        CreateMap<ArmorDto, ArmorResponseDto>();
        CreateMap<ToolDto, ToolResponseDto>();

        CreateMap<SkillProficiencyChoiceDto, SkillProficiencyChoiceResponseDto>();
        CreateMap<LanguageProficiencyChoiceDto, LanguageChoiceResponseDto>();
        CreateMap<WeaponCategoryProficiencyChoiceDto, WeaponProficiencyChoiceResponseDto>();
        CreateMap<WeaponTypeProficiencyChoiceDto, WeaponProficiencyChoiceResponseDto>();
        CreateMap<ToolProficiencyChoiceDto, ToolProficiencyChoiceResponseDto>();
        CreateMap<AbilityIncreaseChoiceDto, AbilityIncreaseChoiceResponseDto>();
        CreateMap<ArmorProficiencyChoiceDto, ArmorProficiencyChoiceResponseDto>();
        
    }
}