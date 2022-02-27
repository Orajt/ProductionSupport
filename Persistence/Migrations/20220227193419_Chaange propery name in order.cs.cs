using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    public partial class Chaangeproperynameinordercs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DateOfShipment",
                table: "Orders",
                newName: "ShipmentDate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ShipmentDate",
                table: "Orders",
                newName: "DateOfShipment");
        }
    }
}
