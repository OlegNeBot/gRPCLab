using AgentApp;
using AgentApp.Repositories;
using AgentApp.Services;
using MainApp;
using Microsoft.Extensions.DependencyInjection;

public static class Program
{
    public static void Main(string[] args)
    {
        bool numberCheck = true;
        var service = new ServiceCollection().AddTransient<IAgentRepository,AgentService>();

        using var serviceProvider = service.BuildServiceProvider();
        var worker = new Worker(serviceProvider.GetService<IAgentRepository>()!);
        Console.WriteLine("Введите логин");
        var login = Console.ReadLine();
        Console.WriteLine("Введите пароль");
        var pass = Console.ReadLine();
        var agent = worker.Auth(new AuthRequest {Login = login, Password = pass});
        Console.WriteLine("Введите название предмета");
        var itemName = Console.ReadLine();
        int wareHouseNumber = 0;
        while (numberCheck)
        {
            Console.WriteLine("Введите номер склада от 1 до 10");
            try
            {
                wareHouseNumber = int.Parse(Console.ReadLine());
                numberCheck = false;
            }
            catch
            {
                Console.WriteLine("Вы ввели не число");
            }
        }

        var response = worker.AddNewItem(new NewRequest {Warehouse = wareHouseNumber , Agentid = agent.Id, Name = itemName}).Result;
        if (response.Res) Console.WriteLine("Предмет успешно добавлен");
        else Console.WriteLine("Предмет не был добавлен. Попробуйте снова");
        Console.WriteLine("Для выхода нажмите любую клавишу");
        Console.ReadKey();


    }
}

