using Microsoft.EntityFrameworkCore.Migrations;

namespace Marketplace.Infra.Migrations
{
    public partial class add_authorization : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "authorization",
                table: "appointments",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "authorization_sale",
                table: "appointments",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "authorization",
                table: "appointments");

            migrationBuilder.DropColumn(
                name: "authorization_sale",
                table: "appointments");
        }
    }
}
