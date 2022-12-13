using InventoryLibrary.Entity;
using Microsoft.AspNetCore.Routing.Matching;

namespace ClientApp.Models;

public class WareHouseViewModel
{
    public IEnumerable<WareHouse> WareHouses { get; init; }

    public WareHouseViewModel()
    {
        WareHouses = new List<WareHouse>();
    }

    public WareHouseViewModel(IEnumerable<WareHouse> wareHouses)
    {
        WareHouses = wareHouses;
    }
}