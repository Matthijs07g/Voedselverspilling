using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Voedselverspilling.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Kantines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Stad = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Locatie = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsWarm = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kantines", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Medewerker",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naam = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PersoneelsNummer = table.Column<int>(type: "int", nullable: false),
                    Stad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Locatie = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medewerker", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Producten",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naam = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsAlcohol = table.Column<bool>(type: "bit", nullable: false),
                    Foto = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Producten", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Studenten",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naam = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GeboorteDatum = table.Column<DateOnly>(type: "date", nullable: false),
                    StudentNummer = table.Column<int>(type: "int", nullable: false),
                    Emailaddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Stad = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TelefoonNr = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Studenten", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pakketten",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naam = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Stad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    KantineId = table.Column<int>(type: "int", nullable: false),
                    Is18 = table.Column<bool>(type: "bit", nullable: false),
                    Prijs = table.Column<double>(type: "float", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pakketten", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pakketten_Kantines_KantineId",
                        column: x => x.KantineId,
                        principalTable: "Kantines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateTable(
                name: "Reserveringen",
                columns: table => new
                {
                    ReserveringId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReservaringDatum = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsOpgehaald = table.Column<bool>(type: "bit", nullable: false),
                    TijdOpgehaald = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    PakketId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reserveringen", x => x.ReserveringId);
                    table.ForeignKey(
                        name: "FK_Reserveringen_Pakketten_PakketId",
                        column: x => x.PakketId,
                        principalTable: "Pakketten",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reserveringen_Studenten_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Studenten",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PakketProduct_ProductenId",
                table: "PakketProduct",
                column: "ProductenId");

            migrationBuilder.CreateIndex(
                name: "IX_Pakketten_KantineId",
                table: "Pakketten",
                column: "KantineId");

            migrationBuilder.CreateIndex(
                name: "IX_Reserveringen_PakketId",
                table: "Reserveringen",
                column: "PakketId");

            migrationBuilder.CreateIndex(
                name: "IX_Reserveringen_StudentId",
                table: "Reserveringen",
                column: "StudentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Medewerker");

            migrationBuilder.DropTable(
                name: "PakketProduct");

            migrationBuilder.DropTable(
                name: "Reserveringen");

            migrationBuilder.DropTable(
                name: "Producten");

            migrationBuilder.DropTable(
                name: "Pakketten");

            migrationBuilder.DropTable(
                name: "Studenten");

            migrationBuilder.DropTable(
                name: "Kantines");
        }
    }
}
