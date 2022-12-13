using InventoryLibrary.Entity;
using Microsoft.EntityFrameworkCore;

namespace MainApp;

public class ApplicationContext : DbContext
{
    public DbSet<Agent> Agents { get; set; } = null!;
    public DbSet<WareHouse> WareHouses { get; set; } = null!;
    public DbSet<Item> Items { get; set; } = null!;
    private readonly bool _test;

    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
    {
        _test = false;
        Database.Migrate();
    }

    public ApplicationContext(DbContextOptions<ApplicationContext> options, bool test) : base(options)
    {
        _test = test;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        if (_test)
        {
            base.OnModelCreating(modelBuilder);
            return;
        }

        var a = new Agent("Test", "Test") {Id = 1};

        var testWareHouses = new List<WareHouse>(new[]
        {
            new WareHouse {Id = 1},
            new WareHouse {Id = 2}
        });
        var items = new List<object>(new[]
        {
            new {Id = 1, AgentId = 1, WareHouseId = 1, Name = "Люстра", TimeStamp = DateTime.Now},
            new {Id = 2, AgentId = 1, WareHouseId = 1, Name = "Ковер", TimeStamp = DateTime.Now},
            new {Id = 3, AgentId = 1, WareHouseId = 2, Name = "Холодильник", TimeStamp = DateTime.Now},
            new {Id = 4, AgentId = 1, WareHouseId = 2, Name = "Дверь", TimeStamp = DateTime.Now},
        });

        modelBuilder.Entity<Agent>().HasData(a);
        modelBuilder.Entity<WareHouse>().HasData(testWareHouses);
        modelBuilder.Entity<Item>(i =>
        {
            i.HasOne(item => item.Agent).WithMany(a => a.Items).HasForeignKey(i => i.AgentId);
            i.HasOne(item => item.WareHouse).WithMany(h => h.Items).HasForeignKey(i => i.WareHouseId);
            i.HasData(items);
        });
        base.OnModelCreating(modelBuilder);
    }
}