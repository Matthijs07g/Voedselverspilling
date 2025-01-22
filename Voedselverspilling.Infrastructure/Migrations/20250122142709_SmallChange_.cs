using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Voedselverspilling.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SmallChange_ : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ReservaringDatum",
                table: "Pakketten",
                newName: "ReserveringDatum");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ReserveringDatum",
                table: "Pakketten",
                newName: "ReservaringDatum");
        }
    }
}
