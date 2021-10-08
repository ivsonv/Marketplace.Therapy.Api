using Microsoft.EntityFrameworkCore.Migrations;

namespace Marketplace.Infra.Migrations
{
    public partial class customer_assessments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Appointmentid",
                table: "customer_assessments",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Providerid",
                table: "customer_assessments",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "appointment_id",
                table: "customer_assessments",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "provider_id",
                table: "customer_assessments",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_customer_assessments_Appointmentid",
                table: "customer_assessments",
                column: "Appointmentid");

            migrationBuilder.CreateIndex(
                name: "IX_customer_assessments_Providerid",
                table: "customer_assessments",
                column: "Providerid");

            migrationBuilder.AddForeignKey(
                name: "FK_customer_assessments_appointments_Appointmentid",
                table: "customer_assessments",
                column: "Appointmentid",
                principalTable: "appointments",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_customer_assessments_provider_Providerid",
                table: "customer_assessments",
                column: "Providerid",
                principalTable: "provider",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_customer_assessments_appointments_Appointmentid",
                table: "customer_assessments");

            migrationBuilder.DropForeignKey(
                name: "FK_customer_assessments_provider_Providerid",
                table: "customer_assessments");

            migrationBuilder.DropIndex(
                name: "IX_customer_assessments_Appointmentid",
                table: "customer_assessments");

            migrationBuilder.DropIndex(
                name: "IX_customer_assessments_Providerid",
                table: "customer_assessments");

            migrationBuilder.DropColumn(
                name: "Appointmentid",
                table: "customer_assessments");

            migrationBuilder.DropColumn(
                name: "Providerid",
                table: "customer_assessments");

            migrationBuilder.DropColumn(
                name: "appointment_id",
                table: "customer_assessments");

            migrationBuilder.DropColumn(
                name: "provider_id",
                table: "customer_assessments");
        }
    }
}
