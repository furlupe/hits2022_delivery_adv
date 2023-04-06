using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DeliveryDeck_Backend_Final.Backend.DAL.Migrations
{
    /// <inheritdoc />
    public partial class RestaurantSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Dishes",
                keyColumn: "Id",
                keyValue: new Guid("1e794d90-9da3-4bcf-bc40-7e24e735a98f"));

            migrationBuilder.InsertData(
                table: "Dishes",
                columns: new[] { "Id", "Category", "Description", "IsVegeterian", "MenuId", "Name", "Photo", "Price" },
                values: new object[] { new Guid("baeee9c2-5ab8-4e2f-b3d3-ed4a2e7e23f4"), 3, "aaaaa", false, null, "Fish w/ Qiwi", null, 50 });

            migrationBuilder.InsertData(
                table: "Restaurants",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("3b105fd9-d824-48d6-875f-23cdb129a821"), "New Amogus" },
                    { new Guid("4920700d-ef32-4c0b-891f-e1e4d381416e"), "FeastingHub" },
                    { new Guid("82987288-7588-4bed-a4b3-c3b0e8f66f4d"), "Old Amogus" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Dishes",
                keyColumn: "Id",
                keyValue: new Guid("baeee9c2-5ab8-4e2f-b3d3-ed4a2e7e23f4"));

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: new Guid("3b105fd9-d824-48d6-875f-23cdb129a821"));

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: new Guid("4920700d-ef32-4c0b-891f-e1e4d381416e"));

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: new Guid("82987288-7588-4bed-a4b3-c3b0e8f66f4d"));

            migrationBuilder.InsertData(
                table: "Dishes",
                columns: new[] { "Id", "Category", "Description", "IsVegeterian", "MenuId", "Name", "Photo", "Price" },
                values: new object[] { new Guid("1e794d90-9da3-4bcf-bc40-7e24e735a98f"), 3, "aaaaa", false, null, "Fish w/ Qiwi", null, 50 });
        }
    }
}
