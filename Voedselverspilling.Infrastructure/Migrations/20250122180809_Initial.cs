using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

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
                    Stad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Locatie = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Stad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    KantineId = table.Column<int>(type: "int", nullable: false)
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
                    IsWarm = table.Column<bool>(type: "bit", nullable: false),
                    Prijs = table.Column<double>(type: "float", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReservedById = table.Column<int>(type: "int", nullable: true),
                    ReserveringDatum = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsOpgehaald = table.Column<bool>(type: "bit", nullable: false),
                    EindDatum = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pakketten", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pakketten_Studenten_ReservedById",
                        column: x => x.ReservedById,
                        principalTable: "Studenten",
                        principalColumn: "Id");
                });

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
                table: "Kantines",
                columns: new[] { "Id", "IsWarm", "Locatie", "Stad" },
                values: new object[,]
                {
                    { 1, true, "LA", "Breda" },
                    { 2, false, "LC", "Breda" }
                });

            migrationBuilder.InsertData(
                table: "Medewerker",
                columns: new[] { "Id", "Email", "KantineId", "Naam", "PersoneelsNummer", "Stad" },
                values: new object[,]
                {
                    { 1, "i.jansen@avans.nl", 1, "Ingrid Jansen", 1, "Breda" },
                    { 2, "h.basten@avans.nl", 2, "Henk van Basten", 2, "Breda" }
                });

            migrationBuilder.InsertData(
                table: "Pakketten",
                columns: new[] { "Id", "EindDatum", "Is18", "IsOpgehaald", "IsWarm", "KantineId", "Naam", "Prijs", "ReservedById", "ReserveringDatum", "Stad", "Type" },
                values: new object[] { 1, new DateTime(2025, 1, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), false, false, true, 1, "Zee eten", 10.99, null, null, "Breda", "Warm" });

            migrationBuilder.InsertData(
                table: "Producten",
                columns: new[] { "Id", "Foto", "IsAlcohol", "Naam" },
                values: new object[,]
                {
                    { 1, "https://www.broodje.nl/wp-content/uploads/2022/03/lunch-broodje-zalm-luxe.jpg", false, "Broodje zalm" },
                    { 2, "https://www.praktijkmik.nl/media/tz_portfolio_plus/article/cache/een-broodje-kroket-28_o.jpg", false, "Broodje kroket" }
                });

            migrationBuilder.InsertData(
                table: "Studenten",
                columns: new[] { "Id", "Emailaddress", "GeboorteDatum", "Naam", "Stad", "StudentNummer", "TelefoonNr" },
                values: new object[,]
                {
                    { 1, "mmj.vangastel@student.avans.nl", new DateOnly(2002, 11, 7), "Matthijs van Gastel", "Breda", 2186230, 0 },
                    { 2, "t.jansen@student.avans.nl", new DateOnly(2010, 5, 15), "Theo Jansen", "Breda", 2286230, 0 }
                });

            migrationBuilder.InsertData(
                table: "PakketProduct",
                columns: new[] { "PakketId", "ProductId" },
                values: new object[] { 1, 1 });

            migrationBuilder.InsertData(
                table: "Pakketten",
                columns: new[] { "Id", "EindDatum", "Is18", "IsOpgehaald", "IsWarm", "KantineId", "Naam", "Prijs", "ReservedById", "ReserveringDatum", "Stad", "Type" },
                values: new object[] { 2, new DateTime(2025, 1, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), false, false, true, 2, "Broodje kroket", 6.9900000000000002, 1, new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Breda", "Brood" });

            migrationBuilder.InsertData(
                table: "PakketProduct",
                columns: new[] { "PakketId", "ProductId" },
                values: new object[] { 2, 2 });

            migrationBuilder.CreateIndex(
                name: "IX_PakketProduct_PakketId",
                table: "PakketProduct",
                column: "PakketId");

            migrationBuilder.CreateIndex(
                name: "IX_Pakketten_ReservedById",
                table: "Pakketten",
                column: "ReservedById");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Kantines");

            migrationBuilder.DropTable(
                name: "Medewerker");

            migrationBuilder.DropTable(
                name: "PakketProduct");

            migrationBuilder.DropTable(
                name: "Pakketten");

            migrationBuilder.DropTable(
                name: "Producten");

            migrationBuilder.DropTable(
                name: "Studenten");
        }
    }
}
