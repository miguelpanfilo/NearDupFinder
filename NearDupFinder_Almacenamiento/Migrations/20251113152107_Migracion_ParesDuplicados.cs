using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NearDupFinder_Almacenamiento.Migrations
{
    /// <inheritdoc />
    public partial class Migracion_ParesDuplicados : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ParesDuplicados",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemACompararId = table.Column<int>(type: "int", nullable: false),
                    ItemPosibleDuplicadoId = table.Column<int>(type: "int", nullable: false),
                    Score = table.Column<float>(type: "real", nullable: false),
                    Tipo = table.Column<int>(type: "int", nullable: false),
                    JaccardTitulo = table.Column<float>(type: "real", nullable: false),
                    JaccardDescripcion = table.Column<float>(type: "real", nullable: false),
                    ScoreMarca = table.Column<int>(type: "int", nullable: false),
                    ScoreModelo = table.Column<int>(type: "int", nullable: false),
                    TokensCompartidosTitulo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TokensCompartidosDescripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdCatalogo = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParesDuplicados", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ParesDuplicados_Catalogos_IdCatalogo",
                        column: x => x.IdCatalogo,
                        principalTable: "Catalogos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParesDuplicados_Items_ItemACompararId",
                        column: x => x.ItemACompararId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ParesDuplicados_Items_ItemPosibleDuplicadoId",
                        column: x => x.ItemPosibleDuplicadoId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ParesDuplicados_IdCatalogo",
                table: "ParesDuplicados",
                column: "IdCatalogo");

            migrationBuilder.CreateIndex(
                name: "IX_ParesDuplicados_ItemACompararId",
                table: "ParesDuplicados",
                column: "ItemACompararId");

            migrationBuilder.CreateIndex(
                name: "IX_ParesDuplicados_ItemPosibleDuplicadoId",
                table: "ParesDuplicados",
                column: "ItemPosibleDuplicadoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ParesDuplicados");
        }
    }
}
