using System.Net;
using System.Text.Json;
using ClientApp.Services;
using InventoryLibrary.Entity;
using Moq;

namespace ClientAppTest;

public class ClientAppRepositoriesTest
{
    private readonly IEnumerable<WareHouse> _testWareHouses;

    public ClientAppRepositoriesTest()
    {
        var testHouses = new List<WareHouse>();
        var agent = new Agent {Id = 1, Login = "Test", Password = "Test"};
        for (var i = 1; i < 3; i++)
        {
            var wh = new WareHouse() {Id = i};
            var item0 = new Item("Картина", wh,agent);
            var item1 = new Item("Ваза", wh,agent);
            item0.WareHouse = null;
            item1.WareHouse = null;
            wh.Items.Add(item0);
            wh.Items.Add(item1);
            testHouses.Add(wh);
        }

        _testWareHouses = testHouses;
    }
    
    public class DelegatingHandlerStub : DelegatingHandler {
        private readonly Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> _handlerFunc;
        public DelegatingHandlerStub()
        {
            
            _handlerFunc = (request, cancellationToken) =>
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                response.RequestMessage = request;
                var res = JsonSerializer.Serialize(new List<WareHouse>());
                response.Content = new StringContent(res);
                return Task.FromResult(response);
            };
        }

        public DelegatingHandlerStub(IEnumerable<WareHouse> testHouses)
        {
            _handlerFunc = (request, cancellationToken) =>
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                response.RequestMessage = request;
                var res = JsonSerializer.Serialize(testHouses);
                response.Content = new StringContent(res);
                return Task.FromResult(response);
            };
        }

        public DelegatingHandlerStub(Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> handlerFunc) {
            _handlerFunc = handlerFunc;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) {
            return _handlerFunc(request, cancellationToken);
        }
    }
    
    [Test]
    public async Task GetHouseHttpSuccess()
    {
        var clientHandlerStub = new DelegatingHandlerStub(_testWareHouses);
        var client = new HttpClient(clientHandlerStub);
        var mock = new Mock<IHttpClientFactory>();
        mock.Setup(r => r.CreateClient(It.IsAny<string>())).Returns(client);
        var service = new ClientService(mock.Object);
        var res = await service.GetAllWareHouses();
        Assert.That(res.Count(), Is.EqualTo(_testWareHouses.Count()));
    }
    
    [Test]
    public async Task GetHouseHttpError()
    {
        var client = new HttpClient();
        var mock = new Mock<IHttpClientFactory>();
        mock.Setup(r => r.CreateClient(It.IsAny<string>())).Returns(client);
        var service = new ClientService(mock.Object);
        var res = await service.GetAllWareHouses();
        Assert.That(res.Count(), Is.EqualTo(0));
    }
}