using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    public partial class DeleteBehavior_to_cascade_in_OrderPositions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderPositionRealizations_OrderPositions_OrderPositionId",
                table: "OrderPositionRealizations");

            migrationBuilder.DropTable(
                name: "ArticleFabricRealizationsGroups");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderPositionRealizations_OrderPositions_OrderPositionId",
                table: "OrderPositionRealizations",
                column: "OrderPositionId",
                principalTable: "OrderPositions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderPositionRealizations_OrderPositions_OrderPositionId",
                table: "OrderPositionRealizations");

            migrationBuilder.CreateTable(
                name: "ArticleFabricRealizationsGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleFabricRealizationsGroups", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_OrderPositionRealizations_OrderPositions_OrderPositionId",
                table: "OrderPositionRealizations",
                column: "OrderPositionId",
                principalTable: "OrderPositions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
