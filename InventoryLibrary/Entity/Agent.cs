namespace InventoryLibrary.Entity;

public class Agent
{
    public int Id { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
    public List<Item> Items { get; set; }

    public Agent()
    {
        Login = string.Empty;
        Password = String.Empty;
        Items = new List<Item>();
    }

    public Agent(string login,string password)
    {
        Login = login;
        Password = password;
        Items = new List<Item>();
    }
}