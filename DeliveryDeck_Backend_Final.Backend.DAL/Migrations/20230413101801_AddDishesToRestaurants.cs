using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DeliveryDeck_Backend_Final.Backend.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddDishesToRestaurants : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.AddColumn<Guid>(
                name: "RestaurantId",
                table: "Dishes",
                type: "uuid",
                nullable: true);

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

            migrationBuilder.CreateIndex(
                name: "IX_Dishes_RestaurantId",
                table: "Dishes",
                column: "RestaurantId");

            migrationBuilder.AddForeignKey(
                name: "FK_Dishes_Restaurants_RestaurantId",
                table: "Dishes",
                column: "RestaurantId",
                principalTable: "Restaurants",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dishes_Restaurants_RestaurantId",
                table: "Dishes");

            migrationBuilder.DropIndex(
                name: "IX_Dishes_RestaurantId",
                table: "Dishes");

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

            migrationBuilder.DropColumn(
                name: "RestaurantId",
                table: "Dishes");

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
        }
    }
}
