namespace DndWebApp.Api.Models;

public class Feature
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required Class FromClass { get; set; }
    public required string LevelGained { get; set; }
    public List<string> Prerequisites { get; set; } = [];
}

/**


desc string[] — OPTIONAL

Description of the resource.
level number — OPTIONAL

The level this feature is gained.
class object — OPTIONAL

APIReference
index string — OPTIONAL

Resource index for shorthand searching.
name string — OPTIONAL

Name of the referenced resource.
url string — OPTIONAL

URL of the referenced resource.
updated_at string — OPTIONAL

Date and time the resource was last updated.
subclass object — OPTIONAL

APIReference
index string — OPTIONAL

Resource index for shorthand searching.
name string — OPTIONAL

Name of the referenced resource.
url string — OPTIONAL

URL of the referenced resource.
updated_at string — OPTIONAL

Date and time the resource was last updated.
parent object — OPTIONAL

APIReference
index string — OPTIONAL

Resource index for shorthand searching.
name string — OPTIONAL

Name of the referenced resource.
url string — OPTIONAL

URL of the referenced resource.
updated_at string — OPTIONAL

Date and time the resource was last updated.
prerequisites undefined[] — OPTIONAL

The prerequisites for this feature.
feature_specific — OPTIONAL

Information specific to this feature.

**/