using Microsoft.EntityFrameworkCore.Migrations;

namespace Marketplace.Infra.Migrations
{
    public partial class add_new_field_type_appointment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "type",
                table: "appointments",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "type",
                table: "appointments");
        }
    }
}
