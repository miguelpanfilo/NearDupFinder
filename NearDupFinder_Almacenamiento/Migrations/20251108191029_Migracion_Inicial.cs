using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NearDupFinder_Almacenamiento.Migrations
{
    /// <inheritdoc />
    public partial class Migracion_Inicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Auditorias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Usuario = table.Column<string>(type: "nvarchar(320)", maxLength: 320, nullable: false),
                    Accion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Detalles = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Auditorias", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Catalogos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Titulo = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Catalogos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Apellido = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AnioNacimiento = table.Column<int>(type: "int", nullable: false),
                    MesNacimiento = table.Column<int>(type: "int", nullable: false),
                    DiaNacimiento = table.Column<int>(type: "int", nullable: false),
                    HashClave = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UsuarioRoles",
                columns: table => new
                {
                    Rol = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UsuarioId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuarioRoles", x => new { x.UsuarioId, x.Rol });
                    table.ForeignKey(
                        name: "FK_UsuarioRoles_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                });

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    EstadoDuplicado = table.Column<bool>(type: "bit", nullable: false),
                    Titulo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Marca = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Modelo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Categoria = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CatalogoId = table.Column<int>(type: "int", nullable: false),
                    ClusterId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Items_Catalogos_CatalogoId",
                        column: x => x.CatalogoId,
                        principalTable: "Catalogos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Items_Clusters_ClusterId",
                        column: x => x.ClusterId,
                        principalTable: "Clusters",
                        principalColumn: "Id");
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_Items_CatalogoId",
                table: "Items",
                column: "CatalogoId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_ClusterId",
                table: "Items",
                column: "ClusterId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Email",
                table: "Usuarios",
                column: "Email",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Clusters_Items_CanonicoId",
                table: "Clusters",
                column: "CanonicoId",
                principalTable: "Items",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clusters_Catalogos_CatalogoId",
                table: "Clusters");

            migrationBuilder.DropForeignKey(
                name: "FK_Items_Catalogos_CatalogoId",
                table: "Items");

            migrationBuilder.DropForeignKey(
                name: "FK_Clusters_Items_CanonicoId",
                table: "Clusters");

            migrationBuilder.DropTable(
                name: "Auditorias");

            migrationBuilder.DropTable(
                name: "UsuarioRoles");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Catalogos");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "Clusters");
        }
    }
}
