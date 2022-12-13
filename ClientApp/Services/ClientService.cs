using System.Text.Json;
using ClientApp.Repositories;
using InventoryLibrary.Entity;

namespace ClientApp.Services;

public class ClientService :IClientRepository
{
    private readonly IHttpClientFactory _clientFactory;

    public ClientService(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }

    public async Task<IEnumerable<WareHouse>> GetAllWareHouses()
    {
        var client = _clientFactory.CreateClient();
        try
        {
            var responseMessage = await client.GetAsync("http://localhost:5181/client/get");
            if (responseMessage.IsSuccessStatusCode)
            {
                return JsonSerializer.Deserialize<IEnumerable<WareHouse>>(await responseMessage.Content
                    .ReadAsStringAsync()) ?? Enumerable.Empty<WareHouse>();
            }
            return Enumerable.Empty<WareHouse>();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Enumerable.Empty<WareHouse>();
        }
    }
}