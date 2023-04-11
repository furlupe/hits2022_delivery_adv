using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DeliveryDeck_Backend_Final.Backend.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ChangeCartLogic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Carts_Orders_OrderId",
                table: "Carts");

            migrationBuilder.DropIndex(
                name: "IX_Carts_OrderId",
                table: "Carts");

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
                name: "OrderId",
                table: "Carts");

            migrationBuilder.AddColumn<Guid>(
                name: "OrderId",
                table: "DishesInCarts",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "WasOrdered",
                table: "Carts",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "Dishes",
                columns: new[] { "Id", "Category", "Description", "IsVegeterian", "Name", "Photo", "Price" },
                values: new object[] { new Guid("04e7d0c7-5897-4f53-bc10-0d401b413c07"), 3, "aaaaa", false, "Fish w/ Qiwi", null, 50 });

            migrationBuilder.InsertData(
                table: "Restaurants",
                columns: new[] { "Id", "Cooks", "Managers", "Name" },
                values: new object[,]
                {
                    { new Guid("15360ae6-761b-434e-b53b-6bc8f1a827a4"), new List<Guid>(), new List<Guid>(), "New Amogus" },
                    { new Guid("2a013577-437f-4209-a972-a0061e5a50c0"), new List<Guid>(), new List<Guid>(), "Old Amogus" },
                    { new Guid("3d4cc8fd-0a17-4151-8ebc-c24bf1aec8ec"), new List<Guid>(), new List<Guid>(), "FeastingHub" }
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
                keyValue: new Guid("04e7d0c7-5897-4f53-bc10-0d401b413c07"));

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: new Guid("15360ae6-761b-434e-b53b-6bc8f1a827a4"));

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: new Guid("2a013577-437f-4209-a972-a0061e5a50c0"));

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: new Guid("3d4cc8fd-0a17-4151-8ebc-c24bf1aec8ec"));

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "DishesInCarts");

            migrationBuilder.DropColumn(
                name: "WasOrdered",
                table: "Carts");

            migrationBuilder.AddColumn<Guid>(
                name: "OrderId",
                table: "Carts",
                type: "uuid",
                nullable: true);

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

            migrationBuilder.CreateIndex(
                name: "IX_Carts_OrderId",
                table: "Carts",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_Orders_OrderId",
                table: "Carts",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id");
        }
    }
}
