using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    public partial class ChaangeentitynamefromFabricvarianttoFabricVariants : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FabricVariantsGroupVariants_FabricVariant_FabricVariantId",
                table: "FabricVariantsGroupVariants");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderPositionRealizations_FabricVariant_VarriantId",
                table: "OrderPositionRealizations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FabricVariant",
                table: "FabricVariant");

            migrationBuilder.RenameTable(
                name: "FabricVariant",
                newName: "FabricVariants");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FabricVariants",
                table: "FabricVariants",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FabricVariantsGroupVariants_FabricVariants_FabricVariantId",
                table: "FabricVariantsGroupVariants",
                column: "FabricVariantId",
                principalTable: "FabricVariants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderPositionRealizations_FabricVariants_VarriantId",
                table: "OrderPositionRealizations",
                column: "VarriantId",
                principalTable: "FabricVariants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FabricVariantsGroupVariants_FabricVariants_FabricVariantId",
                table: "FabricVariantsGroupVariants");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderPositionRealizations_FabricVariants_VarriantId",
                table: "OrderPositionRealizations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FabricVariants",
                table: "FabricVariants");

            migrationBuilder.RenameTable(
                name: "FabricVariants",
                newName: "FabricVariant");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FabricVariant",
                table: "FabricVariant",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FabricVariantsGroupVariants_FabricVariant_FabricVariantId",
                table: "FabricVariantsGroupVariants",
                column: "FabricVariantId",
                principalTable: "FabricVariant",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderPositionRealizations_FabricVariant_VarriantId",
                table: "OrderPositionRealizations",
                column: "VarriantId",
                principalTable: "FabricVariant",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
