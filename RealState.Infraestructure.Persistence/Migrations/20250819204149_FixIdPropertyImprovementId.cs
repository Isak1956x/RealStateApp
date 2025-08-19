using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealState.Infraestructure.Persistence.Migrations
{
    public partial class FixIdPropertyImprovementId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Borrar la tabla existente
            migrationBuilder.DropTable(
                name: "PropertyImprovements"
            );

            // Volver a crear la tabla con Id como int Identity
            migrationBuilder.CreateTable(
                name: "PropertyImprovements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    // agrega aquí las otras columnas de tu tabla
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropertyImprovements", x => x.Id);
                }
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Borrar la tabla actual
            migrationBuilder.DropTable(
                name: "PropertyImprovements"
            );

            // Volver a crearla con Id como string (estado anterior)
            migrationBuilder.CreateTable(
                name: "PropertyImprovements",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    // agrega aquí las otras columnas originales
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropertyImprovements", x => x.Id);
                }
            );
        }
    }
}
