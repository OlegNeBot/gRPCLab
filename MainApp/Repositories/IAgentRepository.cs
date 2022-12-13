using InventoryLibrary.Entity;

namespace MainApp.Interfaces;

public interface IAgentRepository
{
    bool AddNewItem(Item item);
    Agent? AuthAgent(string login, string password);
    WareHouse? GetWareHouseById(int id);
    Agent? GetAgentByID(int id);
}