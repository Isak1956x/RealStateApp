using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealState.Infraestructure.Persistence.Migrations
{
    public partial class ComingBack : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Borrar la tabla existente
            migrationBuilder.DropTable(
                name: "PropertyImprovements"
            );

            // Volver a crear la tabla como tabla de relación
            migrationBuilder.CreateTable(
                name: "PropertyImprovements",
                columns: table => new
                {
                    PropertyId = table.Column<int>(type: "int", nullable: false),
                    ImprovementId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropertyImprovements", x => new { x.PropertyId, x.ImprovementId });
                    table.ForeignKey(
                        name: "FK_PropertyImprovements_Properties_PropertyId",
                        column: x => x.PropertyId,
                        principalTable: "Properties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PropertyImprovements_Improvements_ImprovementId",
                        column: x => x.ImprovementId,
                        principalTable: "Improvements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                }
            );

            // Crear índice en ImprovementId para optimizar consultas
            migrationBuilder.CreateIndex(
                name: "IX_PropertyImprovements_ImprovementId",
                table: "PropertyImprovements",
                column: "ImprovementId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PropertyImprovements"
            );
        }
    }
}
