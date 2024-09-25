using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Voedselverspilling.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addedTestData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pakketten_Kantines_KantineId",
                table: "Pakketten");

            migrationBuilder.DropForeignKey(
                name: "FK_Reserveringen_Pakketten_PakketId",
                table: "Reserveringen");

            migrationBuilder.DropForeignKey(
                name: "FK_Reserveringen_Studenten_StudentId",
                table: "Reserveringen");

            migrationBuilder.DropTable(
                name: "PakketProduct");

            migrationBuilder.DropIndex(
                name: "IX_Reserveringen_PakketId",
                table: "Reserveringen");

            migrationBuilder.DropIndex(
                name: "IX_Reserveringen_StudentId",
                table: "Reserveringen");

            migrationBuilder.DropIndex(
                name: "IX_Pakketten_KantineId",
                table: "Pakketten");

            migrationBuilder.AddColumn<string>(
                name: "ProductenId",
                table: "Pakketten",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");

            migrationBuilder.InsertData(
                table: "Kantines",
                columns: new[] { "Id", "IsWarm", "Locatie", "Stad" },
                values: new object[] { 1, true, "LA", "Breda" });

            migrationBuilder.InsertData(
                table: "Pakketten",
                columns: new[] { "Id", "Is18", "KantineId", "Naam", "Prijs", "ProductenId", "Stad", "Type" },
                values: new object[] { 1, false, 1, "Zee eten", 10.99, "[1]", "Breda", "Warm" });

            migrationBuilder.InsertData(
                table: "Producten",
                columns: new[] { "Id", "Foto", "IsAlcohol", "Naam" },
                values: new object[] { 1, "https://www.broodje.nl/wp-content/uploads/2022/03/lunch-broodje-zalm-luxe.jpg", false, "Broodje zalm" });

            migrationBuilder.InsertData(
                table: "Reserveringen",
                columns: new[] { "ReserveringId", "IsOpgehaald", "PakketId", "ReservaringDatum", "StudentId", "TijdOpgehaald" },
                values: new object[] { 1, false, 1, new DateTime(2024, 9, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Studenten",
                columns: new[] { "Id", "Emailaddress", "GeboorteDatum", "Naam", "Stad", "StudentNummer", "TelefoonNr" },
                values: new object[] { 1, "mmj.vangastel@student.avans.nl", new DateOnly(2002, 11, 7), "Matthijs van Gastel", "Breda", 2186230, 0 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Kantines",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Pakketten",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Producten",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Reserveringen",
                keyColumn: "ReserveringId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Studenten",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DropColumn(
                name: "ProductenId",
                table: "Pakketten");

            migrationBuilder.CreateTable(
                name: "PakketProduct",
                columns: table => new
                {
                    PakketenId = table.Column<int>(type: "int", nullable: false),
                    ProductenId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PakketProduct", x => new { x.PakketenId, x.ProductenId });
                    table.ForeignKey(
                        name: "FK_PakketProduct_Pakketten_PakketenId",
                        column: x => x.PakketenId,
                        principalTable: "Pakketten",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PakketProduct_Producten_ProductenId",
                        column: x => x.ProductenId,
                        principalTable: "Producten",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reserveringen_PakketId",
                table: "Reserveringen",
                column: "PakketId");

            migrationBuilder.CreateIndex(
                name: "IX_Reserveringen_StudentId",
                table: "Reserveringen",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Pakketten_KantineId",
                table: "Pakketten",
                column: "KantineId");

            migrationBuilder.CreateIndex(
                name: "IX_PakketProduct_ProductenId",
                table: "PakketProduct",
                column: "ProductenId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pakketten_Kantines_KantineId",
                table: "Pakketten",
                column: "KantineId",
                principalTable: "Kantines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reserveringen_Pakketten_PakketId",
                table: "Reserveringen",
                column: "PakketId",
                principalTable: "Pakketten",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reserveringen_Studenten_StudentId",
                table: "Reserveringen",
                column: "StudentId",
                principalTable: "Studenten",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
