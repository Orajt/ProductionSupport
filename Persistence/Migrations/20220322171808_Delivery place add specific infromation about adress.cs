using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    public partial class Deliveryplaceaddspecificinfromationaboutadress : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DepotName",
                table: "DeliveryPlaces",
                newName: "Street");

            migrationBuilder.RenameColumn(
                name: "Adress",
                table: "DeliveryPlaces",
                newName: "PostalCode");

            migrationBuilder.AddColumn<int>(
                name: "Apartment",
                table: "DeliveryPlaces",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "DeliveryPlaces",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "DeliveryPlaces",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "DeliveryPlaces",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfBuilding",
                table: "DeliveryPlaces",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Apartment",
                table: "DeliveryPlaces");

            migrationBuilder.DropColumn(
                name: "City",
                table: "DeliveryPlaces");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "DeliveryPlaces");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "DeliveryPlaces");

            migrationBuilder.DropColumn(
                name: "NumberOfBuilding",
                table: "DeliveryPlaces");

            migrationBuilder.RenameColumn(
                name: "Street",
                table: "DeliveryPlaces",
                newName: "DepotName");

            migrationBuilder.RenameColumn(
                name: "PostalCode",
                table: "DeliveryPlaces",
                newName: "Adress");
        }
    }
}
