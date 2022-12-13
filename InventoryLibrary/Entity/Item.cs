using System.Text.Json.Serialization;

namespace InventoryLibrary.Entity;

public class Item
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("name")] public string Name { get; set; }
    [JsonPropertyName("timestamp")]public DateTime TimeStamp { get; set; }
    public WareHouse WareHouse { get; set; }
    public Agent Agent { get; set; }
    [JsonPropertyName("houseId")]public int WareHouseId { get; set; }
    [JsonPropertyName("agentId")]public int AgentId { get;set; }

    public Item()
    {
        Name=String.Empty;
        TimeStamp=DateTime.Now;
        WareHouse = new WareHouse();
        Agent = new Agent();
    }

    public Item(string name,WareHouse wareHouse, Agent agent)
    {
        Name = name;
        TimeStamp=DateTime.Now;
        WareHouse = wareHouse;
        WareHouseId = wareHouse.Id;
        Agent = agent;
        AgentId = agent.Id;
    }
}