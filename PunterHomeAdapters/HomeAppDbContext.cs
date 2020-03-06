using Microsoft.EntityFrameworkCore;
using PunterHomeAdapters.Models;
using PunterHomeApp;
using PunterHomeApp.Models;

namespace PunterHomeAdapters
{
    public class HomeAppDbContext : DbContext
    {
        public HomeAppDbContext() : base(new DbContextOptions<HomeAppDbContext>())
        {

        }

        public HomeAppDbContext (DbContextOptions<HomeAppDbContext> options) : base (options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DbIngredient>()
                .HasKey(c => new { c.ProductId, c.RecipeId });
        }

        public DbSet<DbIngredient> Ingredients { get; set; }
        public DbSet<DbRecipe> Recipes { get; set; }
        public DbSet<DbProduct> Products { get; set; }
    }
}
