using ClientApp.Controllers;
using ClientApp.Models;
using ClientApp.Repositories;
using InventoryLibrary.Entity;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace ClientAppTest;

public class ClientAppMockTest
{
    private readonly IEnumerable<WareHouse> _testWareHouses;

    public ClientAppMockTest()
    {
        var testHouses = new List<WareHouse>();
        var a = new Agent {Id = 1, Login = "Test", Password = "Test"};
        for (var i = 1; i < 3; i++)
        {
            var wh = new WareHouse() {Id = i};
            var item0 = new Item("Картина", wh,a);
            var item1 = new Item("Ваза", wh,a);
            wh.Items.Add(item0);
            wh.Items.Add(item1);
            testHouses.Add(wh);
        }

        _testWareHouses = testHouses;
    }

    [Test]
    public async Task GetWareHouseSuccess()
    {
        var mock = new Mock<IClientRepository>();
        mock.Setup(s => s.GetAllWareHouses()).Returns(Task.FromResult(_testWareHouses));
        var controller = new HomeController(mock.Object);
        var actionResult = await controller.Index();
        Assert.That(actionResult, Is.TypeOf<ViewResult>());
        var viewResult = (ViewResult) actionResult;
        Assert.That(viewResult.ViewData.Model, Is.TypeOf<WareHouseViewModel>());
        var model = (WareHouseViewModel) viewResult.ViewData.Model!;
        Assert.That(model.WareHouses.Count(), Is.EqualTo(_testWareHouses.Count()));
    }
    [Test]
    public async Task GetWareHouseServerError()
    {
        var mock = new Mock<IClientRepository>();
        mock.Setup(s => s.GetAllWareHouses()).Returns(Task.FromResult(Enumerable.Empty<WareHouse>()));
        var controller = new HomeController(mock.Object);
        var actionResult = await controller.Index();
        Assert.That(actionResult, Is.TypeOf<ViewResult>());
        var viewResult = (ViewResult) actionResult;
        Assert.That(viewResult.ViewData.Model, Is.TypeOf<WareHouseViewModel>());
        var model = (WareHouseViewModel) viewResult.ViewData.Model!;
        Assert.That(model.WareHouses.Count(), Is.EqualTo(0));
    }
}