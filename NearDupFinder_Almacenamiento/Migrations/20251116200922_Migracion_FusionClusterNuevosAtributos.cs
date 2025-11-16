using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NearDupFinder_Almacenamiento.Migrations
{
    /// <inheritdoc />
    public partial class Migracion_FusionClusterNuevosAtributos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImagenCanonicaBase64",
                table: "Clusters",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PrecioCanonico",
                table: "Clusters",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StockMinimoCanonico",
                table: "Clusters",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagenCanonicaBase64",
                table: "Clusters");

            migrationBuilder.DropColumn(
                name: "PrecioCanonico",
                table: "Clusters");

            migrationBuilder.DropColumn(
                name: "StockMinimoCanonico",
                table: "Clusters");
        }
    }
}
