using Microsoft.EntityFrameworkCore.Migrations;

namespace Marketplace.Infra.Migrations
{
    public partial class add_log_appointment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_appointments_logs_appointments_Appointmentid",
                table: "appointments_logs");

            migrationBuilder.DropIndex(
                name: "IX_appointments_logs_Appointmentid",
                table: "appointments_logs");

            migrationBuilder.DropColumn(
                name: "Appointmentid",
                table: "appointments_logs");

            migrationBuilder.CreateIndex(
                name: "IX_appointments_logs_appointment_id",
                table: "appointments_logs",
                column: "appointment_id");

            migrationBuilder.AddForeignKey(
                name: "FK_appointments_logs_appointments_appointment_id",
                table: "appointments_logs",
                column: "appointment_id",
                principalTable: "appointments",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_appointments_logs_appointments_appointment_id",
                table: "appointments_logs");

            migrationBuilder.DropIndex(
                name: "IX_appointments_logs_appointment_id",
                table: "appointments_logs");

            migrationBuilder.AddColumn<int>(
                name: "Appointmentid",
                table: "appointments_logs",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_appointments_logs_Appointmentid",
                table: "appointments_logs",
                column: "Appointmentid");

            migrationBuilder.AddForeignKey(
                name: "FK_appointments_logs_appointments_Appointmentid",
                table: "appointments_logs",
                column: "Appointmentid",
                principalTable: "appointments",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
