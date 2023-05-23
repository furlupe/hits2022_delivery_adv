using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DeliveryDeck_Backend_Final.Backend.DAL.Migrations
{
    /// <inheritdoc />
    public partial class removeHasDataDish : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Dishes",
                keyColumn: "Id",
                keyValue: new Guid("8a8259e1-3e5c-4735-87a1-1f7facb5ac73"));

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: new Guid("5db81d08-a411-4795-ab61-47247eeed8e8"));

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: new Guid("a7458b93-dc34-4ff5-9351-65adeb603d7f"));

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: new Guid("c1ec58f7-8588-4999-92b6-b93d79c44b98"));

            migrationBuilder.InsertData(
                table: "Restaurants",
                columns: new[] { "Id", "Cooks", "Managers", "Name" },
                values: new object[,]
                {
                    { new Guid("17e4d540-b620-4903-a72f-1bdd29f83263"), new List<Guid>(), new List<Guid>(), "Old Amogus" },
                    { new Guid("d9c71a04-1b6b-463a-868e-66e78c4c67e9"), new List<Guid>(), new List<Guid>(), "FeastingHub" },
                    { new Guid("e87c983f-3c9f-45c8-9c97-e3727854c1e7"), new List<Guid>(), new List<Guid>(), "New Amogus" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: new Guid("17e4d540-b620-4903-a72f-1bdd29f83263"));

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: new Guid("d9c71a04-1b6b-463a-868e-66e78c4c67e9"));

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: new Guid("e87c983f-3c9f-45c8-9c97-e3727854c1e7"));

            migrationBuilder.InsertData(
                table: "Dishes",
                columns: new[] { "Id", "Category", "Description", "IsVegeterian", "Name", "Photo", "Price", "RestaurantId" },
                values: new object[] { new Guid("8a8259e1-3e5c-4735-87a1-1f7facb5ac73"), 3, "aaaaa", false, "Fish w/ Qiwi", null, 50, null });

            migrationBuilder.InsertData(
                table: "Restaurants",
                columns: new[] { "Id", "Cooks", "Managers", "Name" },
                values: new object[,]
                {
                    { new Guid("5db81d08-a411-4795-ab61-47247eeed8e8"), new List<Guid>(), new List<Guid>(), "New Amogus" },
                    { new Guid("a7458b93-dc34-4ff5-9351-65adeb603d7f"), new List<Guid>(), new List<Guid>(), "FeastingHub" },
                    { new Guid("c1ec58f7-8588-4999-92b6-b93d79c44b98"), new List<Guid>(), new List<Guid>(), "Old Amogus" }
                });
        }
    }
}
