﻿// <auto-generated />
using System;
using System.Collections.Generic;
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

            modelBuilder.Entity("DataModels.Models.DbIngredient", b =>
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

            modelBuilder.Entity("DataModels.Models.DbProduct", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("DataModels.Models.DbProductQuantity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

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

            modelBuilder.Entity("DataModels.Models.DbRecipe", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<List<string>>("Steps")
                        .HasColumnType("text[]");

                    b.HasKey("Id");

                    b.ToTable("Recipes");
                });

            modelBuilder.Entity("DataModels.Models.DbIngredient", b =>
                {
                    b.HasOne("DataModels.Models.DbProduct", "Product")
                        .WithMany("Ingredients")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DataModels.Models.DbRecipe", "Recipe")
                        .WithMany("Ingredients")
                        .HasForeignKey("RecipeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DataModels.Models.DbProductQuantity", b =>
                {
                    b.HasOne("DataModels.Models.DbProduct", "ProductId")
                        .WithMany("ProductQuantities")
                        .HasForeignKey("ProductIdId");
                });
#pragma warning restore 612, 618
        }
    }
}
