using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealState.Infraestructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class NoActionOnPropertyWhenImprovementIsDeleted : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PropertyImprovements_Properties_PropertyId",
                table: "PropertyImprovements");

            migrationBuilder.AddForeignKey(
                name: "FK_PropertyImprovements_Properties_PropertyId",
                table: "PropertyImprovements",
                column: "PropertyId",
                principalTable: "Properties",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PropertyImprovements_Properties_PropertyId",
                table: "PropertyImprovements");

            migrationBuilder.AddForeignKey(
                name: "FK_PropertyImprovements_Properties_PropertyId",
                table: "PropertyImprovements",
                column: "PropertyId",
                principalTable: "Properties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
