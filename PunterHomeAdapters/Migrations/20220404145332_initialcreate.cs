using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace PunterHomeAdapters.Migrations
{
    public partial class initialcreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DbShoppingList",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DbShoppingList", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    MeasurementClass = table.Column<int>(nullable: false),
                    MeasurementValues = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductTag",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductTag", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Recipes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recipes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ShoppingLists",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingLists", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DbShoppingListProduct",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FkShoppingListId = table.Column<Guid>(nullable: false),
                    FkProductId = table.Column<Guid>(nullable: false),
                    IsChecked = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DbShoppingListProduct", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DbShoppingListProduct_Products_FkProductId",
                        column: x => x.FkProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DbShoppingListProduct_DbShoppingList_FkShoppingListId",
                        column: x => x.FkShoppingListId,
                        principalTable: "DbShoppingList",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductQuantities",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProductId = table.Column<Guid>(nullable: false),
                    QuantityTypeVolume = table.Column<int>(nullable: false),
                    UnitQuantityType = table.Column<int>(nullable: false),
                    UnitQuantity = table.Column<int>(nullable: false),
                    Barcode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductQuantities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductQuantities_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductTags",
                columns: table => new
                {
                    TagId = table.Column<Guid>(nullable: false),
                    ProductId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductTags", x => new { x.ProductId, x.TagId });
                    table.ForeignKey(
                        name: "FK_ProductTags_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductTags_ProductTag_TagId",
                        column: x => x.TagId,
                        principalTable: "ProductTag",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DbShoppingListRecipeItem",
                columns: table => new
                {
                    ShoppingListId = table.Column<Guid>(nullable: false),
                    RecipeId = table.Column<Guid>(nullable: false),
                    StaticCount = table.Column<int>(nullable: false),
                    DynamicCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DbShoppingListRecipeItem", x => new { x.RecipeId, x.ShoppingListId });
                    table.ForeignKey(
                        name: "FK_DbShoppingListRecipeItem_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DbShoppingListRecipeItem_DbShoppingList_ShoppingListId",
                        column: x => x.ShoppingListId,
                        principalTable: "DbShoppingList",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ingredients",
                columns: table => new
                {
                    RecipeId = table.Column<Guid>(nullable: false),
                    ProductId = table.Column<Guid>(nullable: false),
                    UnitQuantity = table.Column<int>(nullable: false),
                    UnitQuantityType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ingredients", x => new { x.ProductId, x.RecipeId });
                    table.ForeignKey(
                        name: "FK_Ingredients_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Ingredients_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RecipeSteps",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Order = table.Column<int>(nullable: false),
                    Text = table.Column<string>(nullable: true),
                    RecipeId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeSteps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecipeSteps_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShoppingListRecipeItem",
                columns: table => new
                {
                    ShoppingListAggregateId = table.Column<Guid>(nullable: false),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RecipeId = table.Column<Guid>(nullable: false),
                    Amount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingListRecipeItem", x => new { x.ShoppingListAggregateId, x.Id });
                    table.ForeignKey(
                        name: "FK_ShoppingListRecipeItem_ShoppingLists_ShoppingListAggregateId",
                        column: x => x.ShoppingListAggregateId,
                        principalTable: "ShoppingLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShoppingListTextItem",
                columns: table => new
                {
                    ShoppingListAggregateId = table.Column<Guid>(nullable: false),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingListTextItem", x => new { x.ShoppingListAggregateId, x.Id });
                    table.ForeignKey(
                        name: "FK_ShoppingListTextItem_ShoppingLists_ShoppingListAggregateId",
                        column: x => x.ShoppingListAggregateId,
                        principalTable: "ShoppingLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DbShoppingListProductMeasurementItem",
                columns: table => new
                {
                    ShoppingListId = table.Column<Guid>(nullable: false),
                    ProductQuantityId = table.Column<int>(nullable: false),
                    Count = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DbShoppingListProductMeasurementItem", x => new { x.ShoppingListId, x.ProductQuantityId });
                    table.ForeignKey(
                        name: "FK_DbShoppingListProductMeasurementItem_ProductQuantities_Prod~",
                        column: x => x.ProductQuantityId,
                        principalTable: "ProductQuantities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DbShoppingListProductMeasurementItem_DbShoppingList_Shoppin~",
                        column: x => x.ShoppingListId,
                        principalTable: "DbShoppingList",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DbShoppingListProductsMeasurement",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FkShoppingListProductId = table.Column<Guid>(nullable: false),
                    FkProductQuantityId = table.Column<int>(nullable: false),
                    Count = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DbShoppingListProductsMeasurement", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DbShoppingListProductsMeasurement_ProductQuantities_FkProdu~",
                        column: x => x.FkProductQuantityId,
                        principalTable: "ProductQuantities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DbShoppingListProductsMeasurement_DbShoppingListProduct_FkS~",
                        column: x => x.FkShoppingListProductId,
                        principalTable: "DbShoppingListProduct",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RecipeStepIngredient",
                columns: table => new
                {
                    RecipeStepId = table.Column<Guid>(nullable: false),
                    ProductId = table.Column<Guid>(nullable: false),
                    UnitQuantity = table.Column<double>(nullable: false),
                    UnitQuantityType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeStepIngredient", x => new { x.ProductId, x.RecipeStepId });
                    table.ForeignKey(
                        name: "FK_RecipeStepIngredient_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RecipeStepIngredient_RecipeSteps_RecipeStepId",
                        column: x => x.RecipeStepId,
                        principalTable: "RecipeSteps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DbShoppingListProduct_FkProductId",
                table: "DbShoppingListProduct",
                column: "FkProductId");

            migrationBuilder.CreateIndex(
                name: "IX_DbShoppingListProduct_FkShoppingListId",
                table: "DbShoppingListProduct",
                column: "FkShoppingListId");

            migrationBuilder.CreateIndex(
                name: "IX_DbShoppingListProductMeasurementItem_ProductQuantityId",
                table: "DbShoppingListProductMeasurementItem",
                column: "ProductQuantityId");

            migrationBuilder.CreateIndex(
                name: "IX_DbShoppingListProductsMeasurement_FkProductQuantityId",
                table: "DbShoppingListProductsMeasurement",
                column: "FkProductQuantityId");

            migrationBuilder.CreateIndex(
                name: "IX_DbShoppingListProductsMeasurement_FkShoppingListProductId",
                table: "DbShoppingListProductsMeasurement",
                column: "FkShoppingListProductId");

            migrationBuilder.CreateIndex(
                name: "IX_DbShoppingListRecipeItem_ShoppingListId",
                table: "DbShoppingListRecipeItem",
                column: "ShoppingListId");

            migrationBuilder.CreateIndex(
                name: "IX_Ingredients_RecipeId",
                table: "Ingredients",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductQuantities_ProductId",
                table: "ProductQuantities",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductTags_TagId",
                table: "ProductTags",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeStepIngredient_RecipeStepId",
                table: "RecipeStepIngredient",
                column: "RecipeStepId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeSteps_RecipeId",
                table: "RecipeSteps",
                column: "RecipeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DbShoppingListProductMeasurementItem");

            migrationBuilder.DropTable(
                name: "DbShoppingListProductsMeasurement");

            migrationBuilder.DropTable(
                name: "DbShoppingListRecipeItem");

            migrationBuilder.DropTable(
                name: "Ingredients");

            migrationBuilder.DropTable(
                name: "ProductTags");

            migrationBuilder.DropTable(
                name: "RecipeStepIngredient");

            migrationBuilder.DropTable(
                name: "ShoppingListRecipeItem");

            migrationBuilder.DropTable(
                name: "ShoppingListTextItem");

            migrationBuilder.DropTable(
                name: "ProductQuantities");

            migrationBuilder.DropTable(
                name: "DbShoppingListProduct");

            migrationBuilder.DropTable(
                name: "ProductTag");

            migrationBuilder.DropTable(
                name: "RecipeSteps");

            migrationBuilder.DropTable(
                name: "ShoppingLists");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "DbShoppingList");

            migrationBuilder.DropTable(
                name: "Recipes");
        }
    }
}
