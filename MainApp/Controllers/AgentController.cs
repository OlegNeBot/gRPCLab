using Grpc.Core;
using InventoryLibrary.Entity;
using MainApp.Interfaces;

namespace MainApp.Controllers;

public class AgentController : AgenttService.AgenttServiceBase
{
    private readonly IAgentRepository _agentRepository;

    public AgentController(IAgentRepository agentRepository)
    {
        _agentRepository = agentRepository;
    }
    public Agentt Auth(AuthRequest request)
    {
        if (request is null || string.IsNullOrWhiteSpace(request.Login) || string.IsNullOrWhiteSpace(request.Password))
            return new Agentt() {Id = -1, Login = "", Password = ""};
        var agent = _agentRepository.AuthAgent(request.Login, request.Password);
        return agent is null
            ? new Agentt() {Id = -1, Login = "", Password = ""}
            : new Agentt() {Id = agent.Id, Login = agent.Login, Password = agent.Password};
    }

    public override Task<Agentt> Auth(AuthRequest request, ServerCallContext context)
    {
        return Task.FromResult(Auth(request));
    }

    public NewResponse AddNewItem(NewRequest item)
    {
        if (item is null || item.Warehouse <= 0 || item.Agentid <= 0 ||
            string.IsNullOrWhiteSpace(item.Name))
            return new NewResponse {Res = false};
        var house = _agentRepository.GetWareHouseById(item.Warehouse);
        if (house is null)
            return new NewResponse {Res = false};
        var agent = _agentRepository.GetAgentByID(item.Agentid);
        if (agent is null)
            return new NewResponse {Res = false};
        var tmp_item = new Item(item.Name, house, agent);
        var res = _agentRepository.AddNewItem(tmp_item);
        return new NewResponse {Res = res};
    }
    
    public override Task<NewResponse> AddNewItem(NewRequest request, ServerCallContext context)
    {
        return Task.FromResult(AddNewItem(request));
    }
}