using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Foods",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Lactose = table.Column<bool>(type: "bit", nullable: false),
                    Nuts = table.Column<bool>(type: "bit", nullable: false),
                    Vegetarian = table.Column<bool>(type: "bit", nullable: false),
                    Alcohol = table.Column<bool>(type: "bit", nullable: false),
                    GameEventId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Foods", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GameEvents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrganiserId = table.Column<int>(type: "int", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaxAmountParticipants = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameEvents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Genre = table.Column<int>(type: "int", nullable: false),
                    Adult = table.Column<bool>(type: "bit", nullable: false),
                    Picture = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GameEventId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Games_GameEvents_GameEventId",
                        column: x => x.GameEventId,
                        principalTable: "GameEvents",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Persons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Emailaddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Sex = table.Column<int>(type: "int", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GameEventId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Persons_GameEvents_GameEventId",
                        column: x => x.GameEventId,
                        principalTable: "GameEvents",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Foods_GameEventId",
                table: "Foods",
                column: "GameEventId");

            migrationBuilder.CreateIndex(
                name: "IX_GameEvents_OrganiserId",
                table: "GameEvents",
                column: "OrganiserId");

            migrationBuilder.CreateIndex(
                name: "IX_Games_GameEventId",
                table: "Games",
                column: "GameEventId");

            migrationBuilder.CreateIndex(
                name: "IX_Persons_GameEventId",
                table: "Persons",
                column: "GameEventId");

            migrationBuilder.AddForeignKey(
                name: "FK_Foods_GameEvents_GameEventId",
                table: "Foods",
                column: "GameEventId",
                principalTable: "GameEvents",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GameEvents_Persons_OrganiserId",
                table: "GameEvents",
                column: "OrganiserId",
                principalTable: "Persons",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Persons_GameEvents_GameEventId",
                table: "Persons");

            migrationBuilder.DropTable(
                name: "Foods");

            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropTable(
                name: "GameEvents");

            migrationBuilder.DropTable(
                name: "Persons");
        }
    }
}
