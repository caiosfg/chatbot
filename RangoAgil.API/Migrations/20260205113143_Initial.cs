using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RangoAgil.API.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Ingredientes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ingredientes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rangos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rangos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IngredienteRango",
                columns: table => new
                {
                    IngredientesId = table.Column<int>(type: "INTEGER", nullable: false),
                    RangosId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IngredienteRango", x => new { x.IngredientesId, x.RangosId });
                    table.ForeignKey(
                        name: "FK_IngredienteRango_Ingredientes_IngredientesId",
                        column: x => x.IngredientesId,
                        principalTable: "Ingredientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IngredienteRango_Rangos_RangosId",
                        column: x => x.RangosId,
                        principalTable: "Rangos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Ingredientes",
                columns: new[] { "Id", "Nome" },
                values: new object[,]
                {
                    { 1, "Carne de Vaca" },
                    { 2, "Cebola" },
                    { 3, "Alho" },
                    { 4, "Arroz" },
                    { 5, "Feijão" },
                    { 6, "Farofa" }
                });

            migrationBuilder.InsertData(
                table: "Rangos",
                columns: new[] { "Id", "Nome" },
                values: new object[,]
                {
                    { 1, "Vaca atolada" },
                    { 2, "Bife com Fritas" },
                    { 3, "Strogonoff de carne" },
                    { 4, "Escondidinho de carne" },
                    { 5, "Bife a Cavala" }
                });

            migrationBuilder.InsertData(
                table: "IngredienteRango",
                columns: new[] { "IngredientesId", "RangosId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 1 },
                    { 3, 1 },
                    { 4, 1 },
                    { 5, 1 },
                    { 6, 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_IngredienteRango_RangosId",
                table: "IngredienteRango",
                column: "RangosId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IngredienteRango");

            migrationBuilder.DropTable(
                name: "Ingredientes");

            migrationBuilder.DropTable(
                name: "Rangos");
        }
    }
}
