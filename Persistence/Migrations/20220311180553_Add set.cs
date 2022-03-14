using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    public partial class Addset : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Sets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sets", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderPositions_SetId",
                table: "OrderPositions",
                column: "SetId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderPositions_Sets_SetId",
                table: "OrderPositions",
                column: "SetId",
                principalTable: "Sets",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderPositions_Sets_SetId",
                table: "OrderPositions");

            migrationBuilder.DropTable(
                name: "Sets");

            migrationBuilder.DropIndex(
                name: "IX_OrderPositions_SetId",
                table: "OrderPositions");
        }
    }
}
