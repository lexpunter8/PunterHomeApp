using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PunterHomeAdapters.Migrations
{
    public partial class refactorshoppinglist : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MeasurementsForShoppingListItem");

            migrationBuilder.DropTable(
                name: "RecipeShoppingListItem");

            migrationBuilder.DropTable(
                name: "ShoppingListItems");

            migrationBuilder.CreateTable(
                name: "ShoppingListProductMeasurementItem",
                columns: table => new
                {
                    ShoppingListId = table.Column<Guid>(nullable: false),
                    ProductQuantityId = table.Column<int>(nullable: false),
                    Count = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingListProductMeasurementItem", x => new { x.ShoppingListId, x.ProductQuantityId });
                    table.ForeignKey(
                        name: "FK_ShoppingListProductMeasurementItem_ProductQuantities_Produc~",
                        column: x => x.ProductQuantityId,
                        principalTable: "ProductQuantities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShoppingListProductMeasurementItem_ShoppingLists_ShoppingLi~",
                        column: x => x.ShoppingListId,
                        principalTable: "ShoppingLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShoppingListRecipeItem",
                columns: table => new
                {
                    ShoppingListId = table.Column<Guid>(nullable: false),
                    RecipeId = table.Column<Guid>(nullable: false),
                    StaticCount = table.Column<int>(nullable: false),
                    DynamicCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingListRecipeItem", x => new { x.RecipeId, x.ShoppingListId });
                    table.ForeignKey(
                        name: "FK_ShoppingListRecipeItem_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShoppingListRecipeItem_ShoppingLists_ShoppingListId",
                        column: x => x.ShoppingListId,
                        principalTable: "ShoppingLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingListProductMeasurementItem_ProductQuantityId",
                table: "ShoppingListProductMeasurementItem",
                column: "ProductQuantityId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingListRecipeItem_ShoppingListId",
                table: "ShoppingListRecipeItem",
                column: "ShoppingListId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShoppingListProductMeasurementItem");

            migrationBuilder.DropTable(
                name: "ShoppingListRecipeItem");

            migrationBuilder.CreateTable(
                name: "ShoppingListItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DynamicCount = table.Column<int>(type: "integer", nullable: false),
                    IsChecked = table.Column<bool>(type: "boolean", nullable: false),
                    ProductQuantityId = table.Column<int>(type: "integer", nullable: true),
                    RecipeId = table.Column<Guid>(type: "uuid", nullable: true),
                    ShoppingListId = table.Column<Guid>(type: "uuid", nullable: false),
                    StaticCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingListItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShoppingListItems_ProductQuantities_ProductQuantityId",
                        column: x => x.ProductQuantityId,
                        principalTable: "ProductQuantities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ShoppingListItems_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ShoppingListItems_ShoppingLists_ShoppingListId",
                        column: x => x.ShoppingListId,
                        principalTable: "ShoppingLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MeasurementsForShoppingListItem",
                columns: table => new
                {
                    ShoppingListItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductQuantityId = table.Column<int>(type: "integer", nullable: false),
                    Count = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeasurementsForShoppingListItem", x => new { x.ShoppingListItemId, x.ProductQuantityId });
                    table.ForeignKey(
                        name: "FK_MeasurementsForShoppingListItem_ProductQuantities_ProductQu~",
                        column: x => x.ProductQuantityId,
                        principalTable: "ProductQuantities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MeasurementsForShoppingListItem_ShoppingListItems_ShoppingL~",
                        column: x => x.ShoppingListItemId,
                        principalTable: "ShoppingListItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RecipeShoppingListItem",
                columns: table => new
                {
                    RecipeId = table.Column<Guid>(type: "uuid", nullable: false),
                    ShoppingListItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    DynamicPersons = table.Column<int>(type: "integer", nullable: false),
                    NrOfPersons = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeShoppingListItem", x => new { x.RecipeId, x.ShoppingListItemId });
                    table.ForeignKey(
                        name: "FK_RecipeShoppingListItem_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RecipeShoppingListItem_ShoppingListItems_ShoppingListItemId",
                        column: x => x.ShoppingListItemId,
                        principalTable: "ShoppingListItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MeasurementsForShoppingListItem_ProductQuantityId",
                table: "MeasurementsForShoppingListItem",
                column: "ProductQuantityId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeShoppingListItem_ShoppingListItemId",
                table: "RecipeShoppingListItem",
                column: "ShoppingListItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingListItems_ProductQuantityId",
                table: "ShoppingListItems",
                column: "ProductQuantityId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingListItems_RecipeId",
                table: "ShoppingListItems",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingListItems_ShoppingListId",
                table: "ShoppingListItems",
                column: "ShoppingListId");
        }
    }
}
