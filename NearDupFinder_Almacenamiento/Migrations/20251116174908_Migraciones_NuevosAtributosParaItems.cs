using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NearDupFinder_Almacenamiento.Migrations
{
    /// <inheritdoc />
    public partial class Migraciones_NuevosAtributosParaItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImagenBase64",
                table: "Items",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Precio",
                table: "Items",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagenBase64",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "Precio",
                table: "Items");
        }
    }
}
