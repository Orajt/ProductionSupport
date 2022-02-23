using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    public partial class AddarticleFiles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArticleArticle_Articles_ParentId",
                table: "ArticleArticle");

            migrationBuilder.DropColumn(
                name: "IsParent",
                table: "Articles");

            migrationBuilder.RenameColumn(
                name: "PdfPath",
                table: "Articles",
                newName: "NameWithoutFamilly");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Articles",
                newName: "FullName");

            migrationBuilder.RenameColumn(
                name: "EditionDate",
                table: "Articles",
                newName: "EditDate");

            migrationBuilder.RenameColumn(
                name: "CreationDate",
                table: "Articles",
                newName: "CreateDate");

            migrationBuilder.CreateTable(
                name: "ArticlesFilesPaths",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ArticleId = table.Column<int>(type: "INTEGER", nullable: false),
                    FileType = table.Column<string>(type: "TEXT", nullable: false),
                    Path = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticlesFilesPaths", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArticlesFilesPaths_Articles_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "Articles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArticlesFilesPaths_ArticleId",
                table: "ArticlesFilesPaths",
                column: "ArticleId");

            migrationBuilder.AddForeignKey(
                name: "FK_ArticleArticle_Articles_ParentId",
                table: "ArticleArticle",
                column: "ParentId",
                principalTable: "Articles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArticleArticle_Articles_ParentId",
                table: "ArticleArticle");

            migrationBuilder.DropTable(
                name: "ArticlesFilesPaths");

            migrationBuilder.RenameColumn(
                name: "NameWithoutFamilly",
                table: "Articles",
                newName: "PdfPath");

            migrationBuilder.RenameColumn(
                name: "FullName",
                table: "Articles",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "EditDate",
                table: "Articles",
                newName: "EditionDate");

            migrationBuilder.RenameColumn(
                name: "CreateDate",
                table: "Articles",
                newName: "CreationDate");

            migrationBuilder.AddColumn<bool>(
                name: "IsParent",
                table: "Articles",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_ArticleArticle_Articles_ParentId",
                table: "ArticleArticle",
                column: "ParentId",
                principalTable: "Articles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
