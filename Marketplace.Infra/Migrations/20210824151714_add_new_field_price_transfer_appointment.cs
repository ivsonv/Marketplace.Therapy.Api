using Microsoft.EntityFrameworkCore.Migrations;

namespace Marketplace.Infra.Migrations
{
    public partial class add_new_field_price_transfer_appointment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "price_transfer",
                table: "appointments",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "price_transfer",
                table: "appointments");
        }
    }
}
