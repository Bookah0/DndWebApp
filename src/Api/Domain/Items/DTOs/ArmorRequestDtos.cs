using System.ComponentModel.DataAnnotations;
using Api.Domain.Shared.Enums.Items;

namespace Api.Domain.Items.DTOs;

public class CreateArmorRequestDto : CreateItemRequestBaseDto
{
    [MinLength(1)]
    [MaxLength(50)]
    public required string Category { get; set; }

    [Range(1, 30)]
    public required int BaseArmorClass { get; set; }
    public required bool PlusDexMod { get; set; }

    [Range(1, int.MaxValue)]
    public int? ModCap { get; set; }

    [Range(0, 30)]
    public int StrengthScoreRequired { get; set; } = 0;
    public bool StealthDisadvantage { get; set; } = false;
}

public class UpdateArmorRequestDto : UpdateItemRequestBaseDto
{
    [MaxLength(50)]
    public string? Category { get; set; }

    [Range(1, 30)]
    public int? BaseArmorClass { get; set; }
    public bool? PlusDexMod { get; set; }

    [Range(1, int.MaxValue)]
    public int? ModCap { get; set; }

    [Range(1, 30)]
    public int? StrengthScoreRequired { get; set; }
    public bool? StealthDisadvantage { get; set; }
}