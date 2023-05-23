using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DeliveryDeck_Backend_Final.Backend.DAL.Migrations
{
    /// <inheritdoc />
    public partial class seed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Dishes",
                columns: new[] { "Id", "Category", "Description", "IsVegeterian", "Name", "Photo", "Price" },
                values: new object[] { new Guid("2d2ef2de-1f77-4e19-8bf1-a07d4c210c2c"), 3, "aaaaa", false, "Fish w/ Qiwi", null, 50 });

            migrationBuilder.InsertData(
                table: "Restaurants",
                columns: new[] { "Id", "Cooks", "Managers", "Name" },
                values: new object[,]
                {
                    { new Guid("40a809f6-d40b-4ff2-932b-ebe1d4506034"), new List<Guid>(), new List<Guid>(), "FeastingHub" },
                    { new Guid("a936dde0-0ed6-4f66-8ff1-a68fab5eae9f"), new List<Guid>(), new List<Guid>(), "New Amogus" },
                    { new Guid("d6966c0c-45c4-4428-951b-9759400829c1"), new List<Guid>(), new List<Guid>(), "Old Amogus" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Dishes",
                keyColumn: "Id",
                keyValue: new Guid("2d2ef2de-1f77-4e19-8bf1-a07d4c210c2c"));

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: new Guid("40a809f6-d40b-4ff2-932b-ebe1d4506034"));

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: new Guid("a936dde0-0ed6-4f66-8ff1-a68fab5eae9f"));

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: new Guid("d6966c0c-45c4-4428-951b-9759400829c1"));
        }
    }
}
