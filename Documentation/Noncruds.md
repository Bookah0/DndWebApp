# Task
I should identify and document all operations beyond standard CRUD for each model, including:

- When the model is used in services or controllers.
- What Specialized operations are required for that model.
- Excluding Get(ClassDto dto) and GetAllDto() when the dto contains all non-collection data

# Models
Not all models will require operations beyond standard CRUD.

## Ability
### When its used
- Creating [AbilityValue]s
- Display what skills use the ability 
### ### Specialized operations
- [`GetSkillCheckMod(AbilityValue val, SkillType skill)`] 

## AbilityValue
### When its used
- Added to a AbilityValue List in an [AFeature] if the feature contains an ability score increase
- Added to a Choices list in an [AFeature] if the feature contains a choice of ability score increase 
- Added to a new [Character] when created in a list tracking ability scores
- Displaying [AFeature]s ability scores increases
- Displaying and updating a [Character]s ability scores
- Calculate skill check modifications of a [Character]
### ### Specialized operations
- None *(Everything related is done in aggregate roots)*

## Alignment
### When its used
- A dropdown in the [CharacterDetails] part of character creation or editing 
### ### Specialized operations
- None *(Readonly)*

## Armor
### When its used
- Used by inventory
### Specialized operations
- None *(Armor class calculations are in [Character], Equip logic is in [Inventory], etc.)*

## Character
### When its used
- Core entity for almost all player-related actions
- Used by most services: InventoryService, SpellService, FeatureService, etc.
### Specialized operations
- [CreateCharacterAsync()]              *Handles initialization from multiple other repositories*
- [UpdateStats()]                       *Updates stats, when the character gains a level, feature, equippes something, etc. May be broken down into smaller methods*
- [ApplyLevelUp()]                      *Adds new class features, spell slots, etc. Calls on stats update*
- [`ApplyFeature(AFeature feature)`]    *Modifies stats, grants abilities or items*
- [CalculateArmorClass()]               *Calculates armor class based on equipment and features. More helper methods to UpdateStats() may be used*
- [CalculateWeaponDamage()]             *Calculates weapon damage based on equipment and features. More helper methods to UpdateStats() may be used*

## Choice
### When its used
- Presented to the player when selecting between multiple options in Features (e.g., “Choose a language”, “Choose a skill proficiency”)
### Specialized operations
- None

## Class
### When its used
- Character creation, viewing classes
- Used when applying class features per level, when calculating HP, spell progression
### Specialized operations
- None

## ClassLevel
### When its used
- Defines what features, spells, or proficiencies are gained at each level
### Specialized operations
- Should probably be [Owned]

## Currency
### When its used
- Character inventory and trading/shop systems
### Specialized operations
-  None

## Feat
### When its used
- At character creation or on level-up
### Specialized operations
- FilterItems(.....)

## Trait
### When its used
- Attached to Race or Subrace, granting passive bonuses or abilities
### Specialized operations
- None

## ClassFeature
### When its used
- Gained by leveling up
### Specialized operations
- None

## Inventory
### When its used
- Stores items, weapons, armor, and currency for a character
### Specialized operations
- AddItem(Item item)
- RemoveItem(Item item)
- EquipItem(Item item)
- UnequipItem(Item item)
- SortBy()
- CalculateTotalWeight()
- GetEquippedItems()
- ChangeCurrency()
- ConvertCurrency()

## Item
### When its used
- Base model for Weapon, Armor, Tool, etc.
### Specialized operations
- FilterItems(.....)

## Language
### When its used
- Assigned to Race, Subrace, and Character
### Specialized operations
- None

## Race
### When its used
- Character creation
- Provides base ability bonuses, traits, and starting languages
### Specialized operations
- FilterRaces(.....)?

## Subrace
### When its used
- Selected after choosing a Race
- Extends base race traits and bonuses
### Specialized operations
- FilterSubraces(.....)?

## Skill
### When its used
- Used in ability checks and proficiency systems
- Shown in character sheets with linked ability
### Specialized operations
- None

## Spell
### When its used
- Character creation and level-up for casters
- Spellcasting and damage calculations
### Specialized operations
- FilterSpells(int classId, int spellLevel, ....)

## Tool
### When its used
- Used by inventory
### Specialized operations
- Everying in ItemService

## Weapon
### When its used
- Used by inventory
### Specialized operations
- Everying in ItemService


## Services with additional operations
- ItemService,
- CharacterService
- InventoryService
- SpellService

- SpeciesService
- AbilityService
- AlignmentService
- ClassService
- FeatureService
- LanguageService
- SkillService
