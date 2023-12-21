using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DommunBackend.Migrations
{
    /// <inheritdoc />
    public partial class ActoresPeliculas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GeneroPeliculas_Generos_GeneroId",
                table: "GeneroPeliculas");

            migrationBuilder.DropForeignKey(
                name: "FK_GeneroPeliculas_Peliculas_PeliculaId",
                table: "GeneroPeliculas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GeneroPeliculas",
                table: "GeneroPeliculas");

            migrationBuilder.DropColumn(
                name: "CreateUser",
                table: "GeneroPeliculas");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "GeneroPeliculas");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "GeneroPeliculas");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "GeneroPeliculas");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "GeneroPeliculas");

            migrationBuilder.DropColumn(
                name: "ModifiedUser",
                table: "GeneroPeliculas");

            migrationBuilder.RenameTable(
                name: "GeneroPeliculas",
                newName: "GenerosPeliculas");

            migrationBuilder.RenameIndex(
                name: "IX_GeneroPeliculas_PeliculaId",
                table: "GenerosPeliculas",
                newName: "IX_GenerosPeliculas_PeliculaId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GenerosPeliculas",
                table: "GenerosPeliculas",
                columns: new[] { "GeneroId", "PeliculaId" });

            migrationBuilder.CreateTable(
                name: "ActoresPeliculas",
                columns: table => new
                {
                    ActorId = table.Column<int>(type: "int", nullable: false),
                    PeliculaId = table.Column<int>(type: "int", nullable: false),
                    Orden = table.Column<int>(type: "int", nullable: false),
                    Personaje = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActoresPeliculas", x => new { x.ActorId, x.PeliculaId });
                    table.ForeignKey(
                        name: "FK_ActoresPeliculas_Actores_ActorId",
                        column: x => x.ActorId,
                        principalTable: "Actores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActoresPeliculas_Peliculas_PeliculaId",
                        column: x => x.PeliculaId,
                        principalTable: "Peliculas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActoresPeliculas_PeliculaId",
                table: "ActoresPeliculas",
                column: "PeliculaId");

            migrationBuilder.AddForeignKey(
                name: "FK_GenerosPeliculas_Generos_GeneroId",
                table: "GenerosPeliculas",
                column: "GeneroId",
                principalTable: "Generos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GenerosPeliculas_Peliculas_PeliculaId",
                table: "GenerosPeliculas",
                column: "PeliculaId",
                principalTable: "Peliculas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GenerosPeliculas_Generos_GeneroId",
                table: "GenerosPeliculas");

            migrationBuilder.DropForeignKey(
                name: "FK_GenerosPeliculas_Peliculas_PeliculaId",
                table: "GenerosPeliculas");

            migrationBuilder.DropTable(
                name: "ActoresPeliculas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GenerosPeliculas",
                table: "GenerosPeliculas");

            migrationBuilder.RenameTable(
                name: "GenerosPeliculas",
                newName: "GeneroPeliculas");

            migrationBuilder.RenameIndex(
                name: "IX_GenerosPeliculas_PeliculaId",
                table: "GeneroPeliculas",
                newName: "IX_GeneroPeliculas_PeliculaId");

            migrationBuilder.AddColumn<string>(
                name: "CreateUser",
                table: "GeneroPeliculas",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "GeneroPeliculas",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "GeneroPeliculas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "GeneroPeliculas",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "GeneroPeliculas",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ModifiedUser",
                table: "GeneroPeliculas",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_GeneroPeliculas",
                table: "GeneroPeliculas",
                columns: new[] { "GeneroId", "PeliculaId" });

            migrationBuilder.AddForeignKey(
                name: "FK_GeneroPeliculas_Generos_GeneroId",
                table: "GeneroPeliculas",
                column: "GeneroId",
                principalTable: "Generos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GeneroPeliculas_Peliculas_PeliculaId",
                table: "GeneroPeliculas",
                column: "PeliculaId",
                principalTable: "Peliculas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
