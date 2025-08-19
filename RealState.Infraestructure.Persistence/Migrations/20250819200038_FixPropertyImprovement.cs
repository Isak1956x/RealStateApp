using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealState.Infraestructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FixPropertyImprovement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PropertyImprovements",
                table: "PropertyImprovements");

            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "PropertyImprovements",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PropertyImprovements",
                table: "PropertyImprovements",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyImprovements_PropertyId",
                table: "PropertyImprovements",
                column: "PropertyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PropertyImprovements",
                table: "PropertyImprovements");

            migrationBuilder.DropIndex(
                name: "IX_PropertyImprovements_PropertyId",
                table: "PropertyImprovements");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "PropertyImprovements");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PropertyImprovements",
                table: "PropertyImprovements",
                columns: new[] { "PropertyId", "ImprovementId" });
        }
    }
}
