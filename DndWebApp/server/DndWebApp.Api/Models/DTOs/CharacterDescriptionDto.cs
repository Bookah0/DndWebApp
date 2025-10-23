using DndWebApp.Api.Models.Items;
using DndWebApp.Api.Models.Items.Enums;
using DndWebApp.Api.Models.Spells;
using DndWebApp.Api.Models.World;
using Microsoft.EntityFrameworkCore;

namespace DndWebApp.Api.Models.DTOs;

// Might be used?
public class CharacterDescriptionDto
{
    public required int? AlignmentId { get; set; }
    public required string PersonalityTraits { get; set; }
    public required string Ideals { get; set; }
    public required string Bonds { get; set; }
    public required string Flaws { get; set; }
    public required int? Age { get; set; }
    public required int? Height { get; set; }
    public required int? Weight { get; set; }
    public required string Eyes { get; set; }
    public required string Skin { get; set; }
    public required string Hair { get; set; }
    public required string AlliesAndOrganizations { get; set; }
    public required string Backstory { get; set; }
    public required string CharacterPictureUrl { get; set; }
}