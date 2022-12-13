using InventoryLibrary.Entity;
using MainApp;
using MainApp.Controllers;
using MainApp.Interfaces;
using MainApp.Repositories;
using Moq;

namespace Tests.TestServices;

public class MainAppMockTest
{
    private readonly IEnumerable<WareHouse> _testWareHouses;
    private readonly Agent _agent;

    public MainAppMockTest()
    {
        var testHouses = new List<WareHouse>();
        _agent = new Agent {Id = 1, Login = "Test", Password = "Test"};
        for (var i = 1; i < 3; i++)
        {
            var wh = new WareHouse() {Id = i};
            var item0 = new Item("Картина", wh,_agent);
            var item1 = new Item("Ваза", wh,_agent);
            wh.Items.Add(item0);
            wh.Items.Add(item1);
            testHouses.Add(wh);
        }

        _testWareHouses = testHouses;
    }

    [Test]
    public void GetAllWareHousesSuccess()
    {
        var mock = new Mock<IClientRepository>();
        mock.Setup(r => r.GetAllWareHouses()).Returns(_testWareHouses);
        var clientController = new ClientController(mock.Object);
        var result = clientController.GetWareHouses();
        Assert.That(result.Count(), Is.EqualTo(_testWareHouses.Count()));
    }

    [Test]
    public void AddNewItemSuccess()
    {
        var mock = new Mock<IAgentRepository>();
        mock.Setup(r => r.AddNewItem(It.IsNotNull<Item>())).Returns(true);
        mock.Setup(r => r.GetAgentByID(It.IsAny<int>())).Returns(_agent);
        mock.Setup(r => r.GetWareHouseById(It.IsAny<int>())).Returns(_testWareHouses.First(h => h.Id == 1));
        var agentController = new AgentController(mock.Object);
        var res = agentController.AddNewItem(new NewRequest{Name = "test", Warehouse = 1, Agentid = 1});
        Assert.That(res.Res, Is.True);
    }

    [Test]
    public void AddNewItemWithNull()
    {
        var mock = new Mock<IAgentRepository>();
        mock.Setup(r => r.AddNewItem(It.IsNotNull<Item>())).Returns(true);
        var agentController = new AgentController(mock.Object);
#pragma warning disable CS8625
        var res = agentController.AddNewItem(null);
#pragma warning restore CS8625
        Assert.That(res.Res, Is.False);
    }

    [Test]
    public void AddNewItemWithErrorWareHouse()
    {
        var mock = new Mock<IAgentRepository>();
        mock.Setup(r => r.AddNewItem(It.Is<Item>(i => i.WareHouseId > 0))).Returns(true);
        var agentController = new AgentController(mock.Object);
        var res = agentController.AddNewItem(new NewRequest {Warehouse = -1});
        Assert.That(res.Res, Is.False);
    }

    [Test]
    public void AddNewItemWithErrorAgent()
    {
        var mock = new Mock<IAgentRepository>();
        mock.Setup(r => r.AddNewItem(It.Is<Item>(i => i.AgentId > 0))).Returns(true);
        var agentController = new AgentController(mock.Object);
        var res = agentController.AddNewItem(new NewRequest {Agentid = -1});
        Assert.That(res.Res, Is.False);
    }
    
    [Test]
    public void AddNewItemWithEmptyString()
    {
        var mock = new Mock<IAgentRepository>();
        mock.Setup(r => r.AddNewItem(It.Is<Item>(i => !string.IsNullOrWhiteSpace(i.Name)))).Returns(true);
        var agentController = new AgentController(mock.Object);
        var res = agentController.AddNewItem(new NewRequest {Name = "  "}).Res;
        Assert.That(res, Is.False);
    }
    [Test]
    public void AddNewItemWithNullString()
    {
        var mock = new Mock<IAgentRepository>();
        mock.Setup(r => r.AddNewItem(It.Is<Item>(i => !string.IsNullOrWhiteSpace(i.Name)))).Returns(true);
        var agentController = new AgentController(mock.Object);
        NewRequest? request = null;
        Assert.Catch<ArgumentNullException>(() => request = new NewRequest {Name = null});
    }

    [Test]
    public void AuthSuccess()
    {
        var mock = new Mock<IAgentRepository>();
        mock.Setup(r => r.AuthAgent(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(new Agent() {Id = 1, Login = "Test", Password = "Test"});
        var agentController = new AgentController(mock.Object);
        var res = agentController.Auth(new AuthRequest {Login = "Test", Password = "Test"});
        Assert.That(res, Is.Not.Null);
    }

    [TestCase("   ", "test")]
    [TestCase("test", "    ")]
    [TestCase("   ", "    ")]
    public void AuthWithErrorData(string login, string password)
    {
        var mock = new Mock<IAgentRepository>();
        mock.Setup(r => r.AuthAgent(It.Is<string>(s => !string.IsNullOrWhiteSpace(s)),
                It.Is<string>(s => !string.IsNullOrWhiteSpace(s))))
            .Returns(new Agent {Id = 1, Login = "Test", Password = "Test"});
        var agentController = new AgentController(mock.Object);
        var res = agentController.Auth(new AuthRequest {Login = login, Password = password});
        Assert.That(res.Id, Is.EqualTo(-1));
    }
    
    
    
    [TestCase(null, "test")]
    [TestCase("test", null)]
    [TestCase(null, null)]
    public void AuthWithNullData(string login, string password)
    {
        var mock = new Mock<IAgentRepository>();
        mock.Setup(r => r.AuthAgent(It.Is<string>(s => !string.IsNullOrWhiteSpace(s)),
                It.Is<string>(s => !string.IsNullOrWhiteSpace(s))))
            .Returns(new Agent {Id = 1, Login = "Test", Password = "Test"});
        var agentController = new AgentController(mock.Object);
        Assert.Catch<ArgumentNullException>(() =>
            agentController.Auth(new AuthRequest {Login = login, Password = password}));
    }
}