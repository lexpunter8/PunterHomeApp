﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using PunterHomeAdapters;

namespace PunterHomeAdapters.Migrations
{
    [DbContext(typeof(HomeAppDbContext))]
    partial class HomeAppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("PunterHomeAdapters.Models.DbIngredient", b =>
                {
                    b.Property<Guid>("ProductId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("RecipeId")
                        .HasColumnType("uuid");

                    b.Property<int>("UnitQuantity")
                        .HasColumnType("integer");

                    b.Property<int>("UnitQuantityType")
                        .HasColumnType("integer");

                    b.HasKey("ProductId", "RecipeId");

                    b.HasIndex("RecipeId");

                    b.ToTable("Ingredients");
                });

            modelBuilder.Entity("PunterHomeAdapters.Models.DbProduct", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("MeasurementClass")
                        .HasColumnType("integer");

                    b.Property<string>("MeasurementValues")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("PunterHomeAdapters.Models.DbProductQuantity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Barcode")
                        .HasColumnType("text");

                    b.Property<Guid?>("ProductIdId")
                        .HasColumnType("uuid");

                    b.Property<int>("QuantityTypeVolume")
                        .HasColumnType("integer");

                    b.Property<int>("UnitQuantity")
                        .HasColumnType("integer");

                    b.Property<int>("UnitQuantityType")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ProductIdId");

                    b.ToTable("ProductQuantities");
                });

            modelBuilder.Entity("PunterHomeAdapters.Models.DbProductTag", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("ProductTag");
                });

            modelBuilder.Entity("PunterHomeAdapters.Models.DbProductTags", b =>
                {
                    b.Property<Guid>("ProductId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("TagId")
                        .HasColumnType("uuid");

                    b.HasKey("ProductId", "TagId");

                    b.HasIndex("TagId");

                    b.ToTable("ProductTags");
                });

            modelBuilder.Entity("PunterHomeAdapters.Models.DbRecipe", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Recipes");
                });

            modelBuilder.Entity("PunterHomeAdapters.Models.DbRecipeStep", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("Order")
                        .HasColumnType("integer");

                    b.Property<Guid?>("RecipeId")
                        .HasColumnType("uuid");

                    b.Property<string>("Text")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("RecipeId");

                    b.ToTable("RecipeSteps");
                });

            modelBuilder.Entity("PunterHomeAdapters.Models.DbShoppingList", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("timestamp without time zone");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("ShoppingLists");
                });

            modelBuilder.Entity("PunterHomeAdapters.Models.DbShoppingListItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<bool>("Checked")
                        .HasColumnType("boolean");

                    b.Property<int>("Count")
                        .HasColumnType("integer");

                    b.Property<int?>("ProductQuantitiesId")
                        .HasColumnType("integer");

                    b.Property<Guid?>("ShoppingListId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("ProductQuantitiesId");

                    b.HasIndex("ShoppingListId");

                    b.ToTable("ShoppingListItems");
                });

            modelBuilder.Entity("PunterHomeAdapters.Models.DbIngredient", b =>
                {
                    b.HasOne("PunterHomeAdapters.Models.DbProduct", "Product")
                        .WithMany("Ingredients")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PunterHomeAdapters.Models.DbRecipe", "Recipe")
                        .WithMany("Ingredients")
                        .HasForeignKey("RecipeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PunterHomeAdapters.Models.DbProductQuantity", b =>
                {
                    b.HasOne("PunterHomeAdapters.Models.DbProduct", "ProductId")
                        .WithMany("ProductQuantities")
                        .HasForeignKey("ProductIdId");
                });

            modelBuilder.Entity("PunterHomeAdapters.Models.DbProductTags", b =>
                {
                    b.HasOne("PunterHomeAdapters.Models.DbProduct", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PunterHomeAdapters.Models.DbProductTag", "Tag")
                        .WithMany()
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PunterHomeAdapters.Models.DbRecipeStep", b =>
                {
                    b.HasOne("PunterHomeAdapters.Models.DbRecipe", "Recipe")
                        .WithMany("Steps")
                        .HasForeignKey("RecipeId");
                });

            modelBuilder.Entity("PunterHomeAdapters.Models.DbShoppingListItem", b =>
                {
                    b.HasOne("PunterHomeAdapters.Models.DbProductQuantity", "ProductQuantities")
                        .WithMany("ShoppingListItems")
                        .HasForeignKey("ProductQuantitiesId");

                    b.HasOne("PunterHomeAdapters.Models.DbShoppingList", "ShoppingList")
                        .WithMany()
                        .HasForeignKey("ShoppingListId");
                });
#pragma warning restore 612, 618
        }
    }
}
