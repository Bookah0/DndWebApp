namespace DndWebApp.Api.Models;

public class Item
{
    public int Id { get; set; }
    public string ItemType { get; set; }
    public bool Equippable { get; set; }
    public int Value { get; set; }
}