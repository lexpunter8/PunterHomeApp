using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PunterHomeAdapters.Models;
using PunterHomeDomain.ShoppingList;
using System;

namespace PunterHomeAdapters
{
    public class HomeAppDbContext : DbContext
    {
        public HomeAppDbContext() : base(new DbContextOptions<HomeAppDbContext>())
        {

        }

        public HomeAppDbContext(DbContextOptions<HomeAppDbContext> options) : base(options)
        { }

        //protected override void OnConfiguring(DbContextOptionsBuilder options)
        //    => options.UseNpgsql("Host=localhost;Database=punterhomeapp;Username=postgres;Password=2964Lppos");

        //protected override void OnConfiguring(DbContextOptionsBuilder options)
        //    => options.UseNpgsql("Host=127.0.0.1;Database=punterhomeapp;Username=pi;Password=2964Lppos");

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseNpgsql("Host=192.168.68.105;Database=punterhomeapp;Username=pi;Password=2964Lppos");

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

            modelBuilder.Entity<ShoppingListAggregate>()
                .HasKey(k => k.Id);

            modelBuilder.ApplyConfiguration(new ShoppingListEntityConfig());
        }

        public DbSet<DbIngredient> Ingredients { get; set; }
        public DbSet<DbRecipe> Recipes { get; set; }
        public DbSet<DbProduct> Products { get; set; }
        public DbSet<DbProductQuantity> ProductQuantities { get; set; }
        public DbSet<DbRecipeStep> RecipeSteps { get; set; }
        //public DbSet<DbShoppingList> ShoppingLists { get; set; }

        public DbSet<ShoppingListAggregate> ShoppingLists { get; set; }

        //public DbSet<ShoppingListRecipeItem> ShoppingListRecipe { get; set; }
        //public DbSet<ShoppingListProductItem> ShoppingListProduct { get; set; }

        //public DbSet<ShoppingListTextItem> ShoppingListText { get; set; }
        public DbSet<DbProductTag> ProductTag { get; set; }
        public DbSet<DbProductTags> ProductTags { get; set; }
        //public DbSet<DbShoppingListProductMeasurementItem> ShoppingListProductMeasurementItem { get; set; }
        //public DbSet<DbShoppingListProduct> ShoppingListProducts { get; set; }
        //public DbSet<DbShoppingListProductsMeasurement> ShoppingListProductsMeasurements { get; set; }
        //public DbSet<DbShoppingListItem> ShoppingListTextItems { get; set; }
        public DbSet<DbRecipeStepIngredient> RecipeStepIngredient { get; set; }
    }

    public class ShoppingListEntityConfig : IEntityTypeConfiguration<ShoppingListAggregate>
    {
        public void Configure(EntityTypeBuilder<ShoppingListAggregate> builder)
        {
            //var navigation = builder.Metadata.FindNavigation(nameof(ShoppingListAggregate.RecipeItems));

            //navigation.SetPropertyAccessMode(PropertyAccessMode.Field);

            //var navigation2 = builder.Metadata.FindNavigation(nameof(ShoppingListAggregate.ProductItems));

            //navigation2.SetPropertyAccessMode(PropertyAccessMode.Field);

            //var navigation3 = builder.Metadata.FindNavigation(nameof(ShoppingListAggregate.TextItems));
            //navigation3.SetPropertyAccessMode(PropertyAccessMode.Field);

            var textItemConfig = builder.OwnsMany(x => x.TextItems);
            var navigation3 = builder.Metadata.FindNavigation(nameof(ShoppingListAggregate.TextItems));
            navigation3.SetField("myTextItems");
            navigation3.SetPropertyAccessMode(PropertyAccessMode.Field);


            var recipeItemConfig = builder.OwnsMany(x => x.RecipeItems);
            var navigationRecipe = builder.Metadata.FindNavigation(nameof(ShoppingListAggregate.RecipeItems));
            navigationRecipe.SetField("myRecipeItems");
            navigationRecipe.SetPropertyAccessMode(PropertyAccessMode.Field);


            var productItemConfig = builder.OwnsMany(x => x.ProductItems);
            var navigationProduct = builder.Metadata.FindNavigation(nameof(ShoppingListAggregate.ProductItems));
            navigationProduct.SetField("myProductItems");
            navigationProduct.SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }


    //public class ShoppingListProductEntityConfig : IEntityTypeConfiguration<ShoppingListProductItem>
    //{
    //    public void Configure(EntityTypeBuilder<ShoppingListProductItem> builder)
    //    {
    //        builder.HAs
    //        var navigation = builder.Metadata.FindNavigation(nameof(DbRecipe.Id));

    //        navigation.SetPropertyAccessMode(PropertyAccessMode.Field);

    //    }
    //}
}
    