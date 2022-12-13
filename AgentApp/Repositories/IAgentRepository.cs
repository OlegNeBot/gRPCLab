using MainApp;

namespace AgentApp.Repositories;

public interface IAgentRepository
{
    public Task<Agentt> Auth(AuthRequest request);

    public Task<NewResponse> AddNewItem(NewRequest request);
}