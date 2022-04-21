using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    public partial class DeleteunecessaryfieldsinArticleFabricRealization : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArticleFabricRealizations_FabricVariants_FabricVariantId",
                table: "ArticleFabricRealizations");

            migrationBuilder.DropIndex(
                name: "IX_ArticleFabricRealizations_FabricVariantId",
                table: "ArticleFabricRealizations");

            migrationBuilder.DropColumn(
                name: "FabricVariantId",
                table: "ArticleFabricRealizations");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "ArticleFabricRealizations");

            migrationBuilder.DropColumn(
                name: "PlaceInGroup",
                table: "ArticleFabricRealizations");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FabricVariantId",
                table: "ArticleFabricRealizations",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "GroupId",
                table: "ArticleFabricRealizations",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PlaceInGroup",
                table: "ArticleFabricRealizations",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ArticleFabricRealizations_FabricVariantId",
                table: "ArticleFabricRealizations",
                column: "FabricVariantId");

            migrationBuilder.AddForeignKey(
                name: "FK_ArticleFabricRealizations_FabricVariants_FabricVariantId",
                table: "ArticleFabricRealizations",
                column: "FabricVariantId",
                principalTable: "FabricVariants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
