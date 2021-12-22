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

        //protected override void OnConfiguring(DbContextOptionsBuilder options)
        //    => options.UseNpgsql("Host=127.0.0.1;Database=punterhomeapp;Username=pi;Password=2964Lppos");
        //protected override void OnConfiguring(DbContextOptionsBuilder options)
        //    => options.UseNpgsql("Host=192.168.68.105;Database=punterhomeapp;Username=pi;Password=2964Lppos");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DbIngredient>()
                .HasKey(c => new { c.ProductId, c.RecipeId });

            modelBuilder.Entity<DbIngredient>()
                .HasKey(c => new { c.ProductId, c.RecipeId });

            modelBuilder.Entity<DbRecipeStepIngredient>()
                .HasKey(c => new { c.ProductId, c.RecipeStepId });

            modelBuilder.Entity<DbProductTags>()
                .HasKey(c => new { c.ProductId, c.TagId });

            modelBuilder.Entity<DbShoppingListRecipeItem>()
                .HasKey(c => new { c.RecipeId, c.ShoppingListId });


            modelBuilder.Entity<DbShoppingListProductMeasurementItem>()
                .HasKey(c => new { c.ShoppingListId, c.ProductQuantityId });

        }

        public DbSet<DbIngredient> Ingredients { get; set; }
        public DbSet<DbRecipe> Recipes { get; set; }
        public DbSet<DbProduct> Products { get; set; }
        public DbSet<DbProductQuantity> ProductQuantities { get; set; }
        public DbSet<DbRecipeStep> RecipeSteps { get; set; }
        public DbSet<DbShoppingList> ShoppingLists { get; set; }
        public DbSet<DbProductTag> ProductTag { get; set; }
        public DbSet<DbProductTags> ProductTags { get; set; }
        public DbSet<DbShoppingListRecipeItem> ShoppingListRecipeItem { get; set; }
        public DbSet<DbShoppingListProductMeasurementItem> ShoppingListProductMeasurementItem { get; set; }
        public DbSet<DbShoppingListProduct> ShoppingListProducts { get; set; }
        public DbSet<DbShoppingListProductsMeasurement> ShoppingListProductsMeasurements { get; set; }
        public DbSet<DbShoppingListItem> ShoppingListTextItems { get; set; }
        public DbSet<DbRecipeStepIngredient> RecipeStepIngredient { get; set; }
    }
}
