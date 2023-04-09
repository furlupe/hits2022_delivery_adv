using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DeliveryDeck_Backend_Final.Backend.DAL.Migrations
{
    /// <inheritdoc />
    public partial class MtoMForMenuAndDish : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cooks_Restaurants_RestaurantId",
                table: "Cooks");

            migrationBuilder.DropForeignKey(
                name: "FK_Dishes_Menus_MenuId",
                table: "Dishes");

            migrationBuilder.DropForeignKey(
                name: "FK_Managers_Restaurants_RestaurantId",
                table: "Managers");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Cooks_CookId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_CookId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Dishes_MenuId",
                table: "Dishes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Managers",
                table: "Managers");

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

            migrationBuilder.DropColumn(
                name: "MenuId",
                table: "Dishes");

            migrationBuilder.RenameTable(
                name: "Managers",
                newName: "Manager");

            migrationBuilder.RenameColumn(
                name: "CookId",
                table: "Orders",
                newName: "Cook");

            migrationBuilder.RenameIndex(
                name: "IX_Managers_RestaurantId",
                table: "Manager",
                newName: "IX_Manager_RestaurantId");

            migrationBuilder.AlterColumn<Guid>(
                name: "RestaurantId",
                table: "Cooks",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Manager",
                table: "Manager",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "DishMenu",
                columns: table => new
                {
                    DishesId = table.Column<Guid>(type: "uuid", nullable: false),
                    MenuId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DishMenu", x => new { x.DishesId, x.MenuId });
                    table.ForeignKey(
                        name: "FK_DishMenu_Dishes_DishesId",
                        column: x => x.DishesId,
                        principalTable: "Dishes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DishMenu_Menus_MenuId",
                        column: x => x.MenuId,
                        principalTable: "Menus",
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
                name: "IX_DishMenu_MenuId",
                table: "DishMenu",
                column: "MenuId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cooks_Restaurants_RestaurantId",
                table: "Cooks",
                column: "RestaurantId",
                principalTable: "Restaurants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Manager_Restaurants_RestaurantId",
                table: "Manager",
                column: "RestaurantId",
                principalTable: "Restaurants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cooks_Restaurants_RestaurantId",
                table: "Cooks");

            migrationBuilder.DropForeignKey(
                name: "FK_Manager_Restaurants_RestaurantId",
                table: "Manager");

            migrationBuilder.DropTable(
                name: "DishMenu");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Manager",
                table: "Manager");

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

            migrationBuilder.RenameTable(
                name: "Manager",
                newName: "Managers");

            migrationBuilder.RenameColumn(
                name: "Cook",
                table: "Orders",
                newName: "CookId");

            migrationBuilder.RenameIndex(
                name: "IX_Manager_RestaurantId",
                table: "Managers",
                newName: "IX_Managers_RestaurantId");

            migrationBuilder.AddColumn<Guid>(
                name: "MenuId",
                table: "Dishes",
                type: "uuid",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "RestaurantId",
                table: "Cooks",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Managers",
                table: "Managers",
                column: "Id");

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

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CookId",
                table: "Orders",
                column: "CookId");

            migrationBuilder.CreateIndex(
                name: "IX_Dishes_MenuId",
                table: "Dishes",
                column: "MenuId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cooks_Restaurants_RestaurantId",
                table: "Cooks",
                column: "RestaurantId",
                principalTable: "Restaurants",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Dishes_Menus_MenuId",
                table: "Dishes",
                column: "MenuId",
                principalTable: "Menus",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Managers_Restaurants_RestaurantId",
                table: "Managers",
                column: "RestaurantId",
                principalTable: "Restaurants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Cooks_CookId",
                table: "Orders",
                column: "CookId",
                principalTable: "Cooks",
                principalColumn: "Id");
        }
    }
}
