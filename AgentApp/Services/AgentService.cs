using AgentApp.Repositories;
using Grpc.Net.Client;
using MainApp;

namespace AgentApp.Services;

public class AgentService : IAgentRepository
{
    public async Task<Agentt> Auth(AuthRequest request)
    {
        using var channel = GrpcChannel.ForAddress("http://localhost:5180");
        var client = new AgenttService.AgenttServiceClient(channel);
        return await client.AuthAsync(request);
    }

    public async Task<NewResponse> AddNewItem(NewRequest request)
    {
        using var channel = GrpcChannel.ForAddress("http://localhost:5180");
        var client = new AgenttService.AgenttServiceClient(channel);
        return await client.AddNewItemAsync(request);
    }
}