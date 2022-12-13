using InventoryLibrary.Entity;

namespace ClientApp.Repositories;

public interface IClientRepository
{
    Task<IEnumerable<WareHouse>> GetAllWareHouses();
}