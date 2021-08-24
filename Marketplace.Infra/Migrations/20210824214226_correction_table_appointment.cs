using Microsoft.EntityFrameworkCore.Migrations;

namespace Marketplace.Infra.Migrations
{
    public partial class correction_table_appointment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_appointments_customer_provider_id",
                table: "appointments");

            migrationBuilder.CreateIndex(
                name: "IX_appointments_customer_id",
                table: "appointments",
                column: "customer_id");

            migrationBuilder.AddForeignKey(
                name: "FK_appointments_customer_customer_id",
                table: "appointments",
                column: "customer_id",
                principalTable: "customer",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_appointments_customer_customer_id",
                table: "appointments");

            migrationBuilder.DropIndex(
                name: "IX_appointments_customer_id",
                table: "appointments");

            migrationBuilder.AddForeignKey(
                name: "FK_appointments_customer_provider_id",
                table: "appointments",
                column: "provider_id",
                principalTable: "customer",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
