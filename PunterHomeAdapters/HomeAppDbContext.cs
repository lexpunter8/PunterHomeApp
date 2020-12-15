using Microsoft.EntityFrameworkCore;
using PunterHomeAdapters.Models;

namespace PunterHomeAdapters
{
    public class HomeAppDbContext : DbContext
    {
        public HomeAppDbContext() : base(new DbContextOptions<HomeAppDbContext>())
        {

        }

        public HomeAppDbContext(DbContextOptions<HomeAppDbContext> options) : base(options)
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseNpgsql("Host=localhost;Database=punterhomeapp;Username=postgres;Password=2964Lppos");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DbIngredient>()
                .HasKey(c => new { c.ProductId, c.RecipeId });

            modelBuilder.Entity<DbIngredient>()
                .HasKey(c => new { c.ProductId, c.RecipeId });
        }

        public DbSet<DbIngredient> Ingredients { get; set; }
        public DbSet<DbRecipe> Recipes { get; set; }
        public DbSet<DbProduct> Products { get; set; }
        public DbSet<DbProductQuantity> ProductQuantities { get; set; }
        public DbSet<DbRecipeStep> RecipeSteps { get; set; }
        public DbSet<DbShoppingList> ShoppingLists { get; set; }
        public DbSet<DbShoppingListItem> ShoppingListItems { get; set; }
    }
}
