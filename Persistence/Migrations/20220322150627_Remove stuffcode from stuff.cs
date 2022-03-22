using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    public partial class Removestuffcodefromstuff : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StuffCode",
                table: "Stuffs");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StuffCode",
                table: "Stuffs",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
