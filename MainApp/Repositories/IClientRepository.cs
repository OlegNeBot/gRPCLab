using InventoryLibrary.Entity;

namespace MainApp.Repositories;

public interface IClientRepository
{
    IEnumerable<WareHouse> GetAllWareHouses();
}