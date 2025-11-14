using System.Text.Json.Serialization;

namespace DndWebApp.Api.Models.DTOs.ExternalDtos;

public class EClassDto
{
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("hit_die")]
    public required int HitDie { get; set; }

    [JsonPropertyName("proficiency_choices")]
    public required List<EProficiencyChoiceDto> StartingProficiencyChoices { get; set; }

    [JsonPropertyName("proficiencies")]
    public required List<EIndexDto> Proficiencies { get; set; }

    [JsonPropertyName("saving_throws")]
    public required List<EIndexDto> SavingThrows { get; set; }

    [JsonPropertyName("starting_equipment")]
    public required List<EStartingEquipmentIndexDto> StartingEquipment { get; set; }

    [JsonPropertyName("starting_equipment_options")]
    public required List<EEquipmentChoiceDto> StartingEquipmentChoices { get; set; }

    [JsonPropertyName("class_levels")]
    public required string ClassLevels { get; set; }

    [JsonPropertyName("subclasses")]
    public required List<EIndexDto> Subclasses { get; set; }

    [JsonPropertyName("spellcasting")]
    public ESpellcastingLevelDto? SpellcastingAbility { get; set; }
}

public class ESubclassDto
{
    [JsonPropertyName("name")]
    public required string Name { get; set; }  

    [JsonPropertyName("class")]
    public required EIndexDto Class { get; set; }

    [JsonPropertyName("description")]
    public required List<string> Description { get; set; }
}

public class EProficiencyChoiceDto
{
    [JsonPropertyName("desc")]
    public required string Description { get; set; }

    [JsonPropertyName("choose")]
    public required int NumberOfChoices { get; set; }

    [JsonPropertyName("from")]
    public required EProficiencyOptionsDto From { get; set; }
}

public class EProficiencyOptionsDto
{
    [JsonPropertyName("options")]
    public required List<EProficiencyOptionDto> Options { get; set; }
}

public class EProficiencyOptionDto
{
    [JsonPropertyName("item")]
    public required EIndexDto Proficiency { get; set; }
}

public class EStartingEquipmentIndexDto
{
    [JsonPropertyName("equipment")]
    public required EIndexDto Equipment { get; set; }

    [JsonPropertyName("quantity")]
    public required int Quantity { get; set; }
}

public class EEquipmentOptionDto
{
    [JsonPropertyName("count")] 
    public int? Count { get; set; }
    
    [JsonPropertyName("of")]
    public EIndexDto? Equipment { get; set; }
    
    [JsonPropertyName("from")]
    public EEquipmentCategoryChoiceDto? EquipmentFromCategoryChoice { get; set; }
}

public class EEquipmentCategoryChoiceDto
{
    [JsonPropertyName("desc")]
    public required string Description { get; set; }

    [JsonPropertyName("choose")]
    public required int Choose { get; set; }

    [JsonPropertyName("from")]
    public required EEquipmentCategoryDto From { get; set; }
}

public class EEquipmentCategoryDto
{
    [JsonPropertyName("equipment_category")]
    public required EIndexDto EquipmentCategory { get; set; }
}

public class EEquipmentChoiceDto
{
    [JsonPropertyName("desc")]
    public required string Description { get; set; }

    [JsonPropertyName("choose")]
    public required int NumberOfChoices { get; set; }
    
    [JsonPropertyName("from")]
    public required EEquipmentOptionsDto Options { get; set; }
}

public class EEquipmentOptionsDto
{
    [JsonPropertyName("options")]
    public required List<EEquipmentOptionDto> Options { get; set; }
}

public class ESpellcastingInfoDto
{
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("desc")]
    public required List<string> Description { get; set; }
}

public class ESpellcastingLevelDto
{
    [JsonPropertyName("level")]
    public required int Level { get; set; }
    
    [JsonPropertyName("spellcasting_ability")]
    public required EIndexDto SpellcastingAbility { get; set; }

    [JsonPropertyName("info")]
    public required List<ESpellcastingInfoDto> Info { get; set; }

    [JsonPropertyName("spells")]
    public required string Spells { get; set; }
}