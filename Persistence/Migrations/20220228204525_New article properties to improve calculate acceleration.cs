using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    public partial class Newarticlepropertiestoimprovecalculateacceleration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "First",
                table: "OrderPositions");

            migrationBuilder.DropColumn(
                name: "Last",
                table: "OrderPositions");

            migrationBuilder.DropColumn(
                name: "PositionOnList",
                table: "ArticleArticle");

            migrationBuilder.RenameColumn(
                name: "HasInfluenceOnOrder",
                table: "Articles",
                newName: "HasChildSameArticleType");

            migrationBuilder.RenameColumn(
                name: "CompletedInCompany",
                table: "Articles",
                newName: "HasChild");

            migrationBuilder.AlterColumn<int>(
                name: "SetId",
                table: "OrderPositions",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HasChildSameArticleType",
                table: "Articles",
                newName: "HasInfluenceOnOrder");

            migrationBuilder.RenameColumn(
                name: "HasChild",
                table: "Articles",
                newName: "CompletedInCompany");

            migrationBuilder.AlterColumn<int>(
                name: "SetId",
                table: "OrderPositions",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "First",
                table: "OrderPositions",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Last",
                table: "OrderPositions",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "PositionOnList",
                table: "ArticleArticle",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
