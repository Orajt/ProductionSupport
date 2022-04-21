using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    public partial class ChaangesinentityarticleFabricRealization : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CalculatedCode",
                table: "ArticleFabricRealizations",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<int>(
                name: "PlaceInGroup",
                table: "ArticleFabricRealizations",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

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

            migrationBuilder.CreateIndex(
                name: "IX_ArticleFabricRealizations_FabricVariantId",
                table: "ArticleFabricRealizations",
                column: "FabricVariantId");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleFabricRealizations_StuffId",
                table: "ArticleFabricRealizations",
                column: "StuffId");

            migrationBuilder.AddForeignKey(
                name: "FK_ArticleFabricRealizations_FabricVariants_FabricVariantId",
                table: "ArticleFabricRealizations",
                column: "FabricVariantId",
                principalTable: "FabricVariants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ArticleFabricRealizations_Stuffs_StuffId",
                table: "ArticleFabricRealizations",
                column: "StuffId",
                principalTable: "Stuffs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArticleFabricRealizations_FabricVariants_FabricVariantId",
                table: "ArticleFabricRealizations");

            migrationBuilder.DropForeignKey(
                name: "FK_ArticleFabricRealizations_Stuffs_StuffId",
                table: "ArticleFabricRealizations");

            migrationBuilder.DropTable(
                name: "ArticleFabricRealizationsGroups");

            migrationBuilder.DropIndex(
                name: "IX_ArticleFabricRealizations_FabricVariantId",
                table: "ArticleFabricRealizations");

            migrationBuilder.DropIndex(
                name: "IX_ArticleFabricRealizations_StuffId",
                table: "ArticleFabricRealizations");

            migrationBuilder.DropColumn(
                name: "PlaceInGroup",
                table: "ArticleFabricRealizations");

            migrationBuilder.AlterColumn<int>(
                name: "CalculatedCode",
                table: "ArticleFabricRealizations",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);
        }
    }
}
