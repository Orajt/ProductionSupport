using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    public partial class AddMerchantSellerincompanyproperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Seller",
                table: "Companies",
                newName: "Supplier");

            migrationBuilder.AddColumn<bool>(
                name: "Merchant",
                table: "Companies",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Merchant",
                table: "Companies");

            migrationBuilder.RenameColumn(
                name: "Supplier",
                table: "Companies",
                newName: "Seller");
        }
    }
}
