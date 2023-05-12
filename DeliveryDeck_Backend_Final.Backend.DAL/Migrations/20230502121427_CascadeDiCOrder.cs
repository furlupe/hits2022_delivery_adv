using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DeliveryDeck_Backend_Final.Backend.DAL.Migrations
{
    /// <inheritdoc />
    public partial class CascadeDiCOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DishesInCarts_Orders_OrderId",
                table: "DishesInCarts");

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: new Guid("5b448e49-0d95-47cd-a26c-536404dee101"));

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: new Guid("c0b3322e-0eef-4087-9b69-6f7defd8085c"));

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: new Guid("dfc01d9a-6b08-493d-ad4b-093fed949f01"));

            migrationBuilder.InsertData(
                table: "Restaurants",
                columns: new[] { "Id", "Cooks", "Managers", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("6ca0fd62-ff84-4ce0-84ab-906eb54d8307"), new List<Guid>(), new List<Guid>(), "New Amogus", "NEW AMOGUS" },
                    { new Guid("78b6d3bb-4806-4415-a5b7-c8066af91a4c"), new List<Guid>(), new List<Guid>(), "Old Amogus", "OLD AMOGUS" },
                    { new Guid("cdd104ff-0077-4986-b860-80e08d1ae83e"), new List<Guid>(), new List<Guid>(), "FeastingHub", "FEASTINGHUB" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_DishesInCarts_Orders_OrderId",
                table: "DishesInCarts",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DishesInCarts_Orders_OrderId",
                table: "DishesInCarts");

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: new Guid("6ca0fd62-ff84-4ce0-84ab-906eb54d8307"));

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: new Guid("78b6d3bb-4806-4415-a5b7-c8066af91a4c"));

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: new Guid("cdd104ff-0077-4986-b860-80e08d1ae83e"));

            migrationBuilder.InsertData(
                table: "Restaurants",
                columns: new[] { "Id", "Cooks", "Managers", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("5b448e49-0d95-47cd-a26c-536404dee101"), new List<Guid>(), new List<Guid>(), "New Amogus", "NEW AMOGUS" },
                    { new Guid("c0b3322e-0eef-4087-9b69-6f7defd8085c"), new List<Guid>(), new List<Guid>(), "Old Amogus", "OLD AMOGUS" },
                    { new Guid("dfc01d9a-6b08-493d-ad4b-093fed949f01"), new List<Guid>(), new List<Guid>(), "FeastingHub", "FEASTINGHUB" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_DishesInCarts_Orders_OrderId",
                table: "DishesInCarts",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id");
        }
    }
}
