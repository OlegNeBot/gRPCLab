using InventoryLibrary.Entity;
using MainApp;
using MainApp.Services;
using Microsoft.EntityFrameworkCore;

namespace Tests.TestServices;

public class MainAppRepositoriesTest
{
    private ApplicationContext _context;

    public MainAppRepositoriesTest()
    {
        var options = new DbContextOptionsBuilder<ApplicationContext>()
            .UseInMemoryDatabase($"ContextDb_{DateTime.Now.ToFileTimeUtc()}").Options;
        _context = new ApplicationContext(options,true);
        FillDb();
    }

    private void FillDb()
    {
        var testHouses = new List<WareHouse>();
        var agent = new Agent {Id = 1, Login = "Test", Password = "Test"};
        _context.Agents.Add(agent);
        for (var i = 1; i < 3; i++)
        {
            var wh = new WareHouse() {Id = i};
            var item0 = new Item("Картина", wh,agent);
            var item1 = new Item("Ваза", wh,agent);
            wh.Items.Add(item0);
            wh.Items.Add(item1);
            testHouses.Add(wh);
        }

        _context.AddRange(testHouses);
        _context.SaveChanges();
    }

    ~MainAppRepositoriesTest()
    {
        _context.Dispose();
    }

    [Test]
    public void GetAllWareHouses()
    {
        var service = new ClientService(_context);
        Assert.That(service.GetAllWareHouses(), Is.InstanceOf<IEnumerable<WareHouse>>());
    }

    [Test]
    public void AddNewItemSuccess()
    {
        var service = new AgentService(_context);
        var wareHouse = _context.WareHouses.FirstOrDefault();
        var agent = _context.Agents.FirstOrDefault();
        Assert.That(service.AddNewItem(new Item("Статуетка", wareHouse, agent)), Is.True);
    }

    [Test]
    public void AddNewItemWithNullWareHouse()
    {
        var service = new AgentService(_context);
        WareHouse wareHouse = null;
        var agent = _context.Agents.FirstOrDefault();
        Assert.Catch<NullReferenceException>(() =>
            service.AddNewItem(new Item("Пианино", wareHouse, agent)));
    }

    [Test]
    public void AddNewItemWithNullAgent()
    {
        var service = new AgentService(_context);
        var wareHouse = _context.WareHouses.FirstOrDefault();
        Agent agent = null;
        Assert.Catch<NullReferenceException>(() =>
            service.AddNewItem(new Item("Зеркало", wareHouse, agent)));
    }

    [TestCase("   ")]
    [TestCase(null)]
    public void AddNewItemWithErrorName(string title)
    {
        var service = new AgentService(_context);
        var wareHouse = _context.WareHouses.FirstOrDefault();
        var agent = _context.Agents.FirstOrDefault();
        Assert.That(service.AddNewItem(new Item(title, wareHouse, agent)), Is.False);
    }

    [TestCase("Test", "Test", ExpectedResult = false)]
    [TestCase("   ", "test", ExpectedResult = true)]
    [TestCase("test", "    ", ExpectedResult = true)]
    [TestCase("   ", "    ", ExpectedResult = true)]
    [TestCase(null, "test", ExpectedResult = true)]
    [TestCase("test", null, ExpectedResult = true)] 
    [TestCase(null, null, ExpectedResult = true)]
    public bool AuthWithErrorData(string login, string password)
    {
        var service = new AgentService(_context);
        return service.AuthAgent(login, password) is null;
    }
    [TestCase(1, ExpectedResult = false)]
    [TestCase(15, ExpectedResult = true)]
    public bool GetWareHouseById(int id)
    {
        var service = new AgentService(_context);
        return service.GetWareHouseById(id) is null;
    }

    [TestCase(1, ExpectedResult = false)]
    [TestCase(15, ExpectedResult = true)]
    public bool GetAgentById(int id)
    {
        var service = new AgentService(_context);
        return service.GetAgentByID(id) is null;
    }
}