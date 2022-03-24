using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    public partial class CreateArticleTypeStuffentity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stuffs_ArticleTypes_ArticleTypeId",
                table: "Stuffs");

            migrationBuilder.DropIndex(
                name: "IX_Stuffs_ArticleTypeId",
                table: "Stuffs");

            migrationBuilder.DropColumn(
                name: "ArticleTypeId",
                table: "Stuffs");

            migrationBuilder.CreateTable(
                name: "ArticleTypesStuffs",
                columns: table => new
                {
                    ArticleTypeId = table.Column<int>(type: "INTEGER", nullable: false),
                    StuffId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleTypesStuffs", x => new { x.ArticleTypeId, x.StuffId });
                    table.ForeignKey(
                        name: "FK_ArticleTypesStuffs_ArticleTypes_ArticleTypeId",
                        column: x => x.ArticleTypeId,
                        principalTable: "ArticleTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArticleTypesStuffs_Stuffs_StuffId",
                        column: x => x.StuffId,
                        principalTable: "Stuffs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArticleTypesStuffs_StuffId",
                table: "ArticleTypesStuffs",
                column: "StuffId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArticleTypesStuffs");

            migrationBuilder.AddColumn<int>(
                name: "ArticleTypeId",
                table: "Stuffs",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Stuffs_ArticleTypeId",
                table: "Stuffs",
                column: "ArticleTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Stuffs_ArticleTypes_ArticleTypeId",
                table: "Stuffs",
                column: "ArticleTypeId",
                principalTable: "ArticleTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
