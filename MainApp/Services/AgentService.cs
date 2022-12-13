using InventoryLibrary.Entity;
using MainApp.Interfaces;

namespace MainApp.Services;

public class AgentService : IAgentRepository
{
    private ApplicationContext _context;

    public AgentService(ApplicationContext context)
    {
        _context = context;
    }
    public bool AddNewItem(Item item)
    {
        if (item is null || string.IsNullOrWhiteSpace(item.Name) ||
            !_context.WareHouses.Any(h => h.Id == item.WareHouseId) ||
            !_context.Agents.Any(e => e.Id == item.AgentId)) return false;
        try
        {
            _context.Add(item);
            _context.SaveChanges();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }

    public Agent? AuthAgent(string login, string password)
    {
        if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password)) return null;
        return _context.Agents.FirstOrDefault(e => e.Login == login && e.Password == password);
    }

    public WareHouse? GetWareHouseById(int id)
    {
        if (id <= 0) return null;
        return _context.WareHouses.FirstOrDefault(h => h.Id == id);
    }

    public Agent? GetAgentByID(int id)
    {
        if (id <= 0) return null;
        return _context.Agents.FirstOrDefault(e => e.Id == id);
    }
}