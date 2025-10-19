namespace DndWebApp.Api.Utils;

public class ChoiceOption<T>
{
    public required string Description { get; set; }
    public required int NumberOfChoices { get; set; }
    public required List<T> Choices { get; set; }
}