using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DeliveryDeck_Backend_Final.Backend.DAL.Migrations
{
    /// <inheritdoc />
    public partial class removeCock : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cooks");

            migrationBuilder.DropTable(
                name: "Manager");

            migrationBuilder.DeleteData(
                table: "Dishes",
                keyColumn: "Id",
                keyValue: new Guid("6039a9eb-5072-47ee-9a03-e671ca27d6a7"));

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: new Guid("03dc135e-01bb-4264-827c-ea3e4f1f39b2"));

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: new Guid("33d5257e-d6bd-489e-a418-70c3b51048a2"));

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: new Guid("445fbdab-9bdf-4bff-843d-a52fe9ec558e"));

            migrationBuilder.AddColumn<List<Guid>>(
                name: "Cooks",
                table: "Restaurants",
                type: "uuid[]",
                nullable: false);

            migrationBuilder.AddColumn<List<Guid>>(
                name: "Managers",
                table: "Restaurants",
                type: "uuid[]",
                nullable: false);

            migrationBuilder.InsertData(
                table: "Dishes",
                columns: new[] { "Id", "Category", "Description", "IsVegeterian", "Name", "Photo", "Price" },
                values: new object[] { new Guid("7dd7af49-d281-499e-ba09-9b775ae5b2ba"), 3, "aaaaa", false, "Fish w/ Qiwi", null, 50 });

            migrationBuilder.InsertData(
                table: "Restaurants",
                columns: new[] { "Id", "Cooks", "Managers", "Name" },
                values: new object[,]
                {
                    { new Guid("41a2617f-a04c-43e7-a263-3e3009f9e1e3"), new List<Guid>(), new List<Guid>(), "FeastingHub" },
                    { new Guid("59d835c6-701a-47d4-baaa-cc58d2f1b239"), new List<Guid>(), new List<Guid>(), "New Amogus" },
                    { new Guid("8141cd70-2552-44ec-b0c4-aa2b4affb68d"), new List<Guid>(), new List<Guid>(), "Old Amogus" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Dishes",
                keyColumn: "Id",
                keyValue: new Guid("7dd7af49-d281-499e-ba09-9b775ae5b2ba"));

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: new Guid("41a2617f-a04c-43e7-a263-3e3009f9e1e3"));

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: new Guid("59d835c6-701a-47d4-baaa-cc58d2f1b239"));

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: new Guid("8141cd70-2552-44ec-b0c4-aa2b4affb68d"));

            migrationBuilder.DropColumn(
                name: "Cooks",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "Managers",
                table: "Restaurants");

            migrationBuilder.CreateTable(
                name: "Cooks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RestaurantId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cooks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cooks_Restaurants_RestaurantId",
                        column: x => x.RestaurantId,
                        principalTable: "Restaurants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Manager",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RestaurantId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Manager", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Manager_Restaurants_RestaurantId",
                        column: x => x.RestaurantId,
                        principalTable: "Restaurants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Dishes",
                columns: new[] { "Id", "Category", "Description", "IsVegeterian", "Name", "Photo", "Price" },
                values: new object[] { new Guid("6039a9eb-5072-47ee-9a03-e671ca27d6a7"), 3, "aaaaa", false, "Fish w/ Qiwi", null, 50 });

            migrationBuilder.InsertData(
                table: "Restaurants",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("03dc135e-01bb-4264-827c-ea3e4f1f39b2"), "Old Amogus" },
                    { new Guid("33d5257e-d6bd-489e-a418-70c3b51048a2"), "FeastingHub" },
                    { new Guid("445fbdab-9bdf-4bff-843d-a52fe9ec558e"), "New Amogus" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cooks_RestaurantId",
                table: "Cooks",
                column: "RestaurantId");

            migrationBuilder.CreateIndex(
                name: "IX_Manager_RestaurantId",
                table: "Manager",
                column: "RestaurantId");
        }
    }
}
