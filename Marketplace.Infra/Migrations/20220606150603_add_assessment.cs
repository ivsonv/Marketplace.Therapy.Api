using Microsoft.EntityFrameworkCore.Migrations;

namespace Marketplace.Infra.Migrations
{
    public partial class add_assessment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateIndex(
                name: "IX_customer_assessments_appointment_id",
                table: "customer_assessments",
                column: "appointment_id");

            migrationBuilder.CreateIndex(
                name: "IX_customer_assessments_provider_id",
                table: "customer_assessments",
                column: "provider_id");

            migrationBuilder.AddForeignKey(
                name: "FK_customer_assessments_appointments_appointment_id",
                table: "customer_assessments",
                column: "appointment_id",
                principalTable: "appointments",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_customer_assessments_provider_provider_id",
                table: "customer_assessments",
                column: "provider_id",
                principalTable: "provider",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_customer_assessments_appointments_appointment_id",
                table: "customer_assessments");

            migrationBuilder.DropForeignKey(
                name: "FK_customer_assessments_provider_provider_id",
                table: "customer_assessments");

            migrationBuilder.DropIndex(
                name: "IX_customer_assessments_appointment_id",
                table: "customer_assessments");

            migrationBuilder.DropIndex(
                name: "IX_customer_assessments_provider_id",
                table: "customer_assessments");

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
    }
}
