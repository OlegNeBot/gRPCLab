using System.Text.Json.Serialization;

namespace InventoryLibrary.Entity;

public class WareHouse
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("items")] public List<Item> Items { get; set; }

    public WareHouse() {
        Items = new List<Item>();
    }
}
