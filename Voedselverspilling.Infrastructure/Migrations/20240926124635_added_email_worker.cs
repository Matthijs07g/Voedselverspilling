using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Voedselverspilling.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class added_email_worker : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Locatie",
                table: "Medewerker",
                newName: "Email");

            migrationBuilder.AddColumn<int>(
                name: "KantineId",
                table: "Medewerker",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "Medewerker",
                columns: new[] { "Id", "Email", "KantineId", "Naam", "PersoneelsNummer", "Stad" },
                values: new object[] { 1, "i.jansen@avans.nl", 1, "Ingrid Jansen", 1, "Breda" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Medewerker",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DropColumn(
                name: "KantineId",
                table: "Medewerker");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Medewerker",
                newName: "Locatie");
        }
    }
}
