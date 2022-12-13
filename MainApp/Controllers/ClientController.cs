using InventoryLibrary.Entity;
using MainApp.Interfaces;
using MainApp.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace MainApp.Controllers;

[ApiController]
[Route("/client")]
public class ClientController : ControllerBase
{
 private readonly IClientRepository _clientRepository;

 public ClientController(IClientRepository clientRepository)
 {
  _clientRepository = clientRepository;
 }
 
[HttpGet("get")]
  public IEnumerable<WareHouse> GetWareHouses()
  {
   return _clientRepository.GetAllWareHouses();
  }
}