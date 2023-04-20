using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DeliveryDeck_Backend_Final.Backend.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddPriceWhenOrderedToDishInCart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderedDishes");

            migrationBuilder.DeleteData(
                table: "Dishes",
                keyColumn: "Id",
                keyValue: new Guid("fae59a25-7873-421f-b6d9-4e4b35e4aa84"));

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: new Guid("33b778dd-413a-444c-a655-1aee5b829a30"));

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: new Guid("80124889-a836-461b-a0c7-f61873ade0b2"));

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: new Guid("84dae774-31e1-4bb4-9031-7b3966b99e22"));

            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "DishesInCarts",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PriceWhenOrdered",
                table: "DishesInCarts",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "Dishes",
                columns: new[] { "Id", "Category", "Description", "IsVegeterian", "Name", "Photo", "Price" },
                values: new object[] { new Guid("dc171fe8-98bf-4f1a-b597-15c619e4294b"), 3, "aaaaa", false, "Fish w/ Qiwi", null, 50 });

            migrationBuilder.InsertData(
                table: "Restaurants",
                columns: new[] { "Id", "Cooks", "Managers", "Name" },
                values: new object[,]
                {
                    { new Guid("075674f4-5b32-4a5c-89a6-d1d1fe3fc23b"), new List<Guid>(), new List<Guid>(), "Old Amogus" },
                    { new Guid("2dc11394-c05e-4e7c-86ed-b8a80e90f3fe"), new List<Guid>(), new List<Guid>(), "New Amogus" },
                    { new Guid("5cee7be9-b2bb-4556-8d60-ca01318208ab"), new List<Guid>(), new List<Guid>(), "FeastingHub" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_DishesInCarts_OrderId",
                table: "DishesInCarts",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_DishesInCarts_Orders_OrderId",
                table: "DishesInCarts",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DishesInCarts_Orders_OrderId",
                table: "DishesInCarts");

            migrationBuilder.DropIndex(
                name: "IX_DishesInCarts_OrderId",
                table: "DishesInCarts");

            migrationBuilder.DeleteData(
                table: "Dishes",
                keyColumn: "Id",
                keyValue: new Guid("dc171fe8-98bf-4f1a-b597-15c619e4294b"));

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: new Guid("075674f4-5b32-4a5c-89a6-d1d1fe3fc23b"));

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: new Guid("2dc11394-c05e-4e7c-86ed-b8a80e90f3fe"));

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: new Guid("5cee7be9-b2bb-4556-8d60-ca01318208ab"));

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "DishesInCarts");

            migrationBuilder.DropColumn(
                name: "PriceWhenOrdered",
                table: "DishesInCarts");

            migrationBuilder.CreateTable(
                name: "OrderedDishes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DishId = table.Column<Guid>(type: "uuid", nullable: false),
                    Amount = table.Column<int>(type: "integer", nullable: false),
                    OrderId = table.Column<int>(type: "integer", nullable: true),
                    PriceWhenOrdered = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderedDishes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderedDishes_Dishes_DishId",
                        column: x => x.DishId,
                        principalTable: "Dishes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderedDishes_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Dishes",
                columns: new[] { "Id", "Category", "Description", "IsVegeterian", "Name", "Photo", "Price" },
                values: new object[] { new Guid("fae59a25-7873-421f-b6d9-4e4b35e4aa84"), 3, "aaaaa", false, "Fish w/ Qiwi", null, 50 });

            migrationBuilder.InsertData(
                table: "Restaurants",
                columns: new[] { "Id", "Cooks", "Managers", "Name" },
                values: new object[,]
                {
                    { new Guid("33b778dd-413a-444c-a655-1aee5b829a30"), new List<Guid>(), new List<Guid>(), "Old Amogus" },
                    { new Guid("80124889-a836-461b-a0c7-f61873ade0b2"), new List<Guid>(), new List<Guid>(), "New Amogus" },
                    { new Guid("84dae774-31e1-4bb4-9031-7b3966b99e22"), new List<Guid>(), new List<Guid>(), "FeastingHub" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderedDishes_DishId",
                table: "OrderedDishes",
                column: "DishId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderedDishes_OrderId",
                table: "OrderedDishes",
                column: "OrderId");
        }
    }
}
