using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DeliveryDeck_Backend_Final.Backend.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddNameNormalizing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<string>(
                name: "NormalizedName",
                table: "Restaurants",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NormalizedName",
                table: "Menus",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropColumn(
                name: "NormalizedName",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "NormalizedName",
                table: "Menus");
        }
    }
}
