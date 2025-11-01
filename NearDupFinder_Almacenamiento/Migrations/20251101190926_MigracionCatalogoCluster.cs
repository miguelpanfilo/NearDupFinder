using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NearDupFinder_Almacenamiento.Migrations
{
    /// <inheritdoc />
    public partial class MigracionCatalogoCluster : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CatalogoId",
                table: "Items",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ClusterId",
                table: "Items",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Catalogos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titulo = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Catalogos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Clusters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CanonicoId = table.Column<int>(type: "int", nullable: true),
                    CatalogoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clusters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Clusters_Catalogos_CatalogoId",
                        column: x => x.CatalogoId,
                        principalTable: "Catalogos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Clusters_Items_CanonicoId",
                        column: x => x.CanonicoId,
                        principalTable: "Items",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Items_CatalogoId",
                table: "Items",
                column: "CatalogoId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_ClusterId",
                table: "Items",
                column: "ClusterId");

            migrationBuilder.CreateIndex(
                name: "IX_Catalogos_Titulo",
                table: "Catalogos",
                column: "Titulo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Clusters_CanonicoId",
                table: "Clusters",
                column: "CanonicoId");

            migrationBuilder.CreateIndex(
                name: "IX_Clusters_CatalogoId",
                table: "Clusters",
                column: "CatalogoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Catalogos_CatalogoId",
                table: "Items",
                column: "CatalogoId",
                principalTable: "Catalogos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Clusters_ClusterId",
                table: "Items",
                column: "ClusterId",
                principalTable: "Clusters",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Catalogos_CatalogoId",
                table: "Items");

            migrationBuilder.DropForeignKey(
                name: "FK_Items_Clusters_ClusterId",
                table: "Items");

            migrationBuilder.DropTable(
                name: "Clusters");

            migrationBuilder.DropTable(
                name: "Catalogos");

            migrationBuilder.DropIndex(
                name: "IX_Items_CatalogoId",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_ClusterId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "CatalogoId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "ClusterId",
                table: "Items");
        }
    }
}
