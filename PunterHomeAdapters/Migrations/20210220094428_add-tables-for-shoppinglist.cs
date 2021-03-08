using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PunterHomeAdapters.Migrations
{
    public partial class addtablesforshoppinglist : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ShoppingListProducts",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FkShoppingListId = table.Column<Guid>(nullable: false),
                    FkProductId = table.Column<Guid>(nullable: false),
                    IsChecked = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingListProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShoppingListProducts_Products_FkProductId",
                        column: x => x.FkProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShoppingListProducts_ShoppingLists_FkShoppingListId",
                        column: x => x.FkShoppingListId,
                        principalTable: "ShoppingLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShoppingListProductsMeasurements",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FkShoppingListProductId = table.Column<Guid>(nullable: false),
                    FkProductQuantityId = table.Column<int>(nullable: false),
                    Count = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingListProductsMeasurements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShoppingListProductsMeasurements_ProductQuantities_FkProduc~",
                        column: x => x.FkProductQuantityId,
                        principalTable: "ProductQuantities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShoppingListProductsMeasurements_ShoppingListProducts_FkSho~",
                        column: x => x.FkShoppingListProductId,
                        principalTable: "ShoppingListProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingListProducts_FkProductId",
                table: "ShoppingListProducts",
                column: "FkProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingListProducts_FkShoppingListId",
                table: "ShoppingListProducts",
                column: "FkShoppingListId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingListProductsMeasurements_FkProductQuantityId",
                table: "ShoppingListProductsMeasurements",
                column: "FkProductQuantityId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingListProductsMeasurements_FkShoppingListProductId",
                table: "ShoppingListProductsMeasurements",
                column: "FkShoppingListProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShoppingListProductsMeasurements");

            migrationBuilder.DropTable(
                name: "ShoppingListProducts");
        }
    }
}
