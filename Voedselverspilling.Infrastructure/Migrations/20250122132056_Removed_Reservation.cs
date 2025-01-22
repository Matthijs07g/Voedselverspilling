using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Voedselverspilling.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Removed_Reservation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reserveringen");

            migrationBuilder.DeleteData(
                table: "Pakketten",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DropColumn(
                name: "ProductenId",
                table: "Pakketten");

            migrationBuilder.AddColumn<DateTime>(
                name: "EindDatum",
                table: "Pakketten",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsOpgehaald",
                table: "Pakketten",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ReservaringDatum",
                table: "Pakketten",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReservedById",
                table: "Pakketten",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PakketProduct",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    PakketId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PakketProduct", x => new { x.ProductId, x.PakketId });
                    table.ForeignKey(
                        name: "FK_PakketProduct_Pakketten_PakketId",
                        column: x => x.PakketId,
                        principalTable: "Pakketten",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PakketProduct_Producten_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Producten",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "PakketProduct",
                columns: new[] { "PakketId", "ProductId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 1 }
                });

            migrationBuilder.InsertData(
                table: "Producten",
                columns: new[] { "Id", "Foto", "IsAlcohol", "Naam" },
                values: new object[] { 2, "https://www.praktijkmik.nl/media/tz_portfolio_plus/article/cache/een-broodje-kroket-28_o.jpg", false, "Broodje kroket" });

            migrationBuilder.InsertData(
                table: "PakketProduct",
                columns: new[] { "PakketId", "ProductId" },
                values: new object[,]
                {
                    { 1, 2 },
                    { 2, 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Pakketten_ReservedById",
                table: "Pakketten",
                column: "ReservedById");

            migrationBuilder.CreateIndex(
                name: "IX_PakketProduct_PakketId",
                table: "PakketProduct",
                column: "PakketId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pakketten_Studenten_ReservedById",
                table: "Pakketten",
                column: "ReservedById",
                principalTable: "Studenten",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pakketten_Studenten_ReservedById",
                table: "Pakketten");

            migrationBuilder.DropTable(
                name: "PakketProduct");

            migrationBuilder.DropIndex(
                name: "IX_Pakketten_ReservedById",
                table: "Pakketten");

            migrationBuilder.DeleteData(
                table: "Producten",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DropColumn(
                name: "EindDatum",
                table: "Pakketten");

            migrationBuilder.DropColumn(
                name: "IsOpgehaald",
                table: "Pakketten");

            migrationBuilder.DropColumn(
                name: "ReservaringDatum",
                table: "Pakketten");

            migrationBuilder.DropColumn(
                name: "ReservedById",
                table: "Pakketten");

            migrationBuilder.AddColumn<string>(
                name: "ProductenId",
                table: "Pakketten",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Reserveringen",
                columns: table => new
                {
                    ReserveringId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsOpgehaald = table.Column<bool>(type: "bit", nullable: false),
                    PakketId = table.Column<int>(type: "int", nullable: false),
                    ReservaringDatum = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    TijdOpgehaald = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reserveringen", x => x.ReserveringId);
                });

            migrationBuilder.InsertData(
                table: "Pakketten",
                columns: new[] { "Id", "Is18", "IsWarm", "KantineId", "Naam", "Prijs", "ProductenId", "Stad", "Type" },
                values: new object[] { 1, false, true, 1, "Zee eten", 10.99, "[1]", "Breda", "Warm" });

            migrationBuilder.InsertData(
                table: "Reserveringen",
                columns: new[] { "ReserveringId", "IsOpgehaald", "PakketId", "ReservaringDatum", "StudentId", "TijdOpgehaald" },
                values: new object[] { 1, false, 1, new DateTime(2024, 9, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });
        }
    }
}
