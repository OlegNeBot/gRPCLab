using InventoryLibrary.Entity;
using MainApp.Interfaces;
using MainApp.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MainApp.Services;

public class ClientService: IClientRepository
{
    private ApplicationContext _context;

    public ClientService(ApplicationContext context)
    {
        _context = context;
    }
    public IEnumerable<WareHouse> GetAllWareHouses()
    {
        var items = _context.WareHouses.Include(h => h.Items).ToList();
        items = items.Select(h =>
        {
            h.Items = h.Items.Select(i =>
            {
                i.WareHouse = null;
                return i;
            }).ToList();
            return h;
        }).ToList();
        return items;
    }
}