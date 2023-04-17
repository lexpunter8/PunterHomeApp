using System.Reflection.Metadata;
using HomeAppDomain.AggregateRoots;
using HomeAppRepositories.Entities;
using Microsoft.EntityFrameworkCore;

namespace HomeAppRepositories;
public class HomeAppContext : DbContext
{

    //public DbSet<DbShoppingList> ShoppingList { get; set; }
    public DbSet<ShoppingListItem> ShoppingListItem { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql(@"Host=192.168.68.105;Username=pi;Password=2964Lppos;Database=homeapp");

}

