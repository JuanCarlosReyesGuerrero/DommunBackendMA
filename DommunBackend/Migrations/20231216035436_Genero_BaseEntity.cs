using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DommunBackend.Migrations
{
    /// <inheritdoc />
    public partial class Genero_BaseEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreateUser",
                table: "Generos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Generos",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Generos",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "Generos",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ModifiedUser",
                table: "Generos",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreateUser",
                table: "Generos");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Generos");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Generos");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "Generos");

            migrationBuilder.DropColumn(
                name: "ModifiedUser",
                table: "Generos");
        }
    }
}
