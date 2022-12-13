using AgentApp.Repositories;
using MainApp;

namespace AgentApp;

public class Worker
{
    private readonly IAgentRepository _agentRepository;

    public Worker(IAgentRepository agentRepository)
    {
        _agentRepository = agentRepository;
    }
    public async Task<Agentt> Auth(AuthRequest request)
    {
        
        if (request is null|| string.IsNullOrWhiteSpace(request.Login) || string.IsNullOrWhiteSpace(request.Password))
        {
            return new Agentt {Id = -1, Login = "", Password = ""};
        }

        return await _agentRepository.Auth(request);
    }
    public async Task<NewResponse> AddNewItem(NewRequest request)
    {
        if (request is null ||string.IsNullOrWhiteSpace(request.Name)||request.Warehouse<=0||request.Agentid<=0) return new NewResponse {Res = false};
        return await _agentRepository.AddNewItem(request);
    }
}