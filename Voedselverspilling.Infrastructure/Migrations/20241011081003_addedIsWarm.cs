using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Voedselverspilling.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addedIsWarm : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsWarm",
                table: "Pakketten",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "Stad",
                table: "Kantines",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Locatie",
                table: "Kantines",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "Kantines",
                columns: new[] { "Id", "IsWarm", "Locatie", "Stad" },
                values: new object[] { 2, false, "LC", "Breda" });

            migrationBuilder.InsertData(
                table: "Medewerker",
                columns: new[] { "Id", "Email", "KantineId", "Naam", "PersoneelsNummer", "Stad" },
                values: new object[] { 2, "h.basten@avans.nl", 2, "Henk van Basten", 2, "Breda" });

            migrationBuilder.UpdateData(
                table: "Pakketten",
                keyColumn: "Id",
                keyValue: 1,
                column: "IsWarm",
                value: true);

            migrationBuilder.InsertData(
                table: "Studenten",
                columns: new[] { "Id", "Emailaddress", "GeboorteDatum", "Naam", "Stad", "StudentNummer", "TelefoonNr" },
                values: new object[] { 2, "t.jansen@student.avans.nl", new DateOnly(2010, 5, 15), "Theo Jansen", "Breda", 2286230, 0 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Kantines",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Medewerker",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Studenten",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DropColumn(
                name: "IsWarm",
                table: "Pakketten");

            migrationBuilder.AlterColumn<string>(
                name: "Stad",
                table: "Kantines",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Locatie",
                table: "Kantines",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
