using AgentApp;
using AgentApp.Repositories;
using MainApp;
using Moq;

namespace AgentTest;

public class Tests
{
    [Test]
    public async Task AuthSuccess()
    {
        var mock = new Mock<IAgentRepository>();
        mock.Setup(s => s.Auth(It.IsNotNull<AuthRequest>()))
            .Returns(Task.FromResult(new Agentt {Id = 1, Login = "test", Password = "test"}));
        var worker = new Worker(mock.Object);
        var res = await worker.Auth(new AuthRequest {Login = "test", Password = "test"});
        Assert.That(res.Id, Is.EqualTo(1));
    }
    [Test]
    public async Task AuthWithNull()
    {
        var mock = new Mock<IAgentRepository>();
        mock.Setup(s => s.Auth(It.IsNotNull<AuthRequest>()))
            .Returns(Task.FromResult(new Agentt {Id = 1, Login = "", Password = ""}));
        var worker = new Worker(mock.Object);
        var res = await worker.Auth(null);
        Assert.That(res.Id, Is.EqualTo(-1));
    }
    [TestCase("  ", "test")]
    [TestCase("test", "  ")]
    
    public async Task AuthWithEmptyData(string login, string password)
    {
        var mock = new Mock<IAgentRepository>();
        mock.Setup(s =>
            s.Auth(It.Is<AuthRequest>(
                r => string.IsNullOrWhiteSpace(r.Login) || !string.IsNullOrWhiteSpace(r.Password)))).Returns(
            Task.FromResult(new Agentt()
                {Id = -1, Login = "", Password = ""}));
        var worker = new Worker(mock.Object);
        var res = await worker.Auth(new AuthRequest{Login = login, Password = password});
        Assert.That(res.Id, Is.EqualTo(-1));
    }
    [TestCase(null, "test")]
    [TestCase("test", null)]
    [TestCase(null, null)]
    public void AuthWithNullData(string login, string password)
    {
        var mock = new Mock<IAgentRepository>();
        mock.Setup(s =>
            s.Auth(It.Is<AuthRequest>(
                r => string.IsNullOrWhiteSpace(r.Login) || !string.IsNullOrWhiteSpace(r.Password)))).Returns(
            Task.FromResult(new Agentt()
                {Id = -1, Login = "", Password = ""}));
        var worker = new Worker(mock.Object);
        AuthRequest? req = null;
        Assert.Catch<ArgumentNullException>( () => req = new AuthRequest {Login = login, Password = password});

    }

    [Test]
    public async Task AddNewItemSuccess()
    {
        var mock = new Mock<IAgentRepository>();
        mock.Setup(s => s.AddNewItem(It.IsNotNull<NewRequest>()))
            .Returns(Task.FromResult(new NewResponse {Res = true}));
        var worker = new Worker(mock.Object);
        var res = await worker.AddNewItem(new NewRequest
            {Warehouse = 1, Agentid = 1, Name = "Test"});
        Assert.That(res.Res, Is.True);
    }
    
    [Test]
    public async Task AddNewItemWithNull()
    {
        var mock = new Mock<IAgentRepository>();
        mock.Setup(s => s.AddNewItem(It.Is<NewRequest>(r => r == null)))
            .Returns(Task.FromResult(new NewResponse {Res = false}));
        var worker = new Worker(mock.Object);
        var res = await worker.AddNewItem(null);
        Assert.That(res.Res, Is.False);
    }
    
    [Test]
    public async Task AddNewItemWithErrorWareHouse()
    {
        var mock = new Mock<IAgentRepository>();
        mock.Setup(s => s.AddNewItem(It.Is<NewRequest>(r => r.Warehouse > 0)))
            .Returns(Task.FromResult(new NewResponse {Res = true}));
        var worker = new Worker(mock.Object);
        var res = await worker.AddNewItem(new NewRequest
            {Warehouse = -1, Agentid = 1, Name = "Test"});
        Assert.That(res.Res, Is.False);
    }
    
    [Test]
    public async Task AddNewItemWithErrorAgent()
    {
        var mock = new Mock<IAgentRepository>();
        mock.Setup(s => s.AddNewItem(It.Is<NewRequest>(r => r.Agentid > 0)))
            .Returns(Task.FromResult(new NewResponse {Res = true}));
        var worker = new Worker(mock.Object);
        var res = await worker.AddNewItem(new NewRequest
            {Warehouse = 1, Agentid = -1,  Name = "Test"});
        Assert.That(res.Res, Is.False);
    }
    
    [Test]
    public async Task AddNewItemWithErrorName()
    {
        var mock = new Mock<IAgentRepository>();
        mock.Setup(s => s.AddNewItem(It.Is<NewRequest>(r => !string.IsNullOrWhiteSpace(r.Name))))
            .Returns(Task.FromResult(new NewResponse {Res = true}));
        var worker = new Worker(mock.Object);
        var res = await worker.AddNewItem(new NewRequest
            {Warehouse = 1, Agentid = 1, Name = "  "});
        Assert.That(res.Res, Is.False);
    }
    [Test]
    public async Task AddNewItemWithNullName()
    {
        var mock = new Mock<IAgentRepository>();
        mock.Setup(s => s.AddNewItem(It.Is<NewRequest>(r => !string.IsNullOrWhiteSpace(r.Name))))
            .Returns(Task.FromResult(new NewResponse {Res = true}));
        var worker = new Worker(mock.Object);
        NewRequest? request = null;
        Assert.Catch<ArgumentNullException>(() => request = new NewRequest {Warehouse = 1, Agentid = 1, Name = null});
    }
}