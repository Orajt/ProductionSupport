using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    public partial class AddpropertiestoOrdercs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CompletionDate",
                table: "Orders",
                newName: "ProductionDate");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfShipment",
                table: "Orders",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateOfShipment",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "ProductionDate",
                table: "Orders",
                newName: "CompletionDate");
        }
    }
}
