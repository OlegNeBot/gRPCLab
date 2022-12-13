using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MainApp.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Agents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Login = table.Column<string>(type: "TEXT", nullable: false),
                    Password = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Agents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WareHouses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WareHouses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    TimeStamp = table.Column<DateTime>(type: "TEXT", nullable: false),
                    WareHouseId = table.Column<int>(type: "INTEGER", nullable: false),
                    AgentId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Items_Agents_AgentId",
                        column: x => x.AgentId,
                        principalTable: "Agents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Items_WareHouses_WareHouseId",
                        column: x => x.WareHouseId,
                        principalTable: "WareHouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Agents",
                columns: new[] { "Id", "Login", "Password" },
                values: new object[] { 1, "Test", "Test" });

            migrationBuilder.InsertData(
                table: "WareHouses",
                column: "Id",
                value: 1);

            migrationBuilder.InsertData(
                table: "WareHouses",
                column: "Id",
                value: 2);

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "Id", "AgentId", "Name", "TimeStamp", "WareHouseId" },
                values: new object[] { 1, 1, "Люстра", new DateTime(2022, 12, 11, 13, 31, 22, 902, DateTimeKind.Local).AddTicks(2676), 1 });

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "Id", "AgentId", "Name", "TimeStamp", "WareHouseId" },
                values: new object[] { 2, 1, "Ковер", new DateTime(2022, 12, 11, 13, 31, 22, 902, DateTimeKind.Local).AddTicks(2686), 1 });

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "Id", "AgentId", "Name", "TimeStamp", "WareHouseId" },
                values: new object[] { 3, 1, "Холодильник", new DateTime(2022, 12, 11, 13, 31, 22, 902, DateTimeKind.Local).AddTicks(2687), 2 });

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "Id", "AgentId", "Name", "TimeStamp", "WareHouseId" },
                values: new object[] { 4, 1, "Дверь", new DateTime(2022, 12, 11, 13, 31, 22, 902, DateTimeKind.Local).AddTicks(2688), 2 });

            migrationBuilder.CreateIndex(
                name: "IX_Items_AgentId",
                table: "Items",
                column: "AgentId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_WareHouseId",
                table: "Items",
                column: "WareHouseId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "Agents");

            migrationBuilder.DropTable(
                name: "WareHouses");
        }
    }
}
