using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PunterHomeAdapters.Migrations
{
    public partial class addtableshopitemmeasurement : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MeasurementsForShoppingListItem",
                columns: table => new
                {
                    ShoppingListItemId = table.Column<Guid>(nullable: false),
                    ProductQuantityId = table.Column<int>(nullable: false),
                    Count = table.Column<int>(nullable: false),
                    DbShoppingListItemInfoId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeasurementsForShoppingListItem", x => new { x.ShoppingListItemId, x.ProductQuantityId });
                    table.ForeignKey(
                        name: "FK_MeasurementsForShoppingListItem_ShoppingListItemInfos_DbSho~",
                        column: x => x.DbShoppingListItemInfoId,
                        principalTable: "ShoppingListItemInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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

            migrationBuilder.CreateIndex(
                name: "IX_MeasurementsForShoppingListItem_DbShoppingListItemInfoId",
                table: "MeasurementsForShoppingListItem",
                column: "DbShoppingListItemInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_MeasurementsForShoppingListItem_ProductQuantityId",
                table: "MeasurementsForShoppingListItem",
                column: "ProductQuantityId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MeasurementsForShoppingListItem");
        }
    }
}
