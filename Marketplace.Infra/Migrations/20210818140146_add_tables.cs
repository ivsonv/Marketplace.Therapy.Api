using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Marketplace.Infra.Migrations
{
    public partial class add_tables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "curriculum",
                table: "provider",
                newName: "time_zone");

            migrationBuilder.AddColumn<DateTime>(
                name: "birthdate",
                table: "provider",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "gender",
                table: "provider",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "link",
                table: "provider",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "price_for_thirty",
                table: "provider",
                type: "numeric",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "provider_receipt",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    signature = table.Column<string>(type: "text", nullable: true),
                    cpf = table.Column<string>(type: "text", nullable: true),
                    fantasy_name = table.Column<string>(type: "text", nullable: true),
                    cnpj = table.Column<string>(type: "text", nullable: true),
                    address = table.Column<string>(type: "text", nullable: true),
                    provider_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_provider_receipt", x => x.id);
                    table.ForeignKey(
                        name: "FK_provider_receipt_provider_provider_id",
                        column: x => x.provider_id,
                        principalTable: "provider",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_provider_receipt_provider_id",
                table: "provider_receipt",
                column: "provider_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "provider_receipt");

            migrationBuilder.DropColumn(
                name: "birthdate",
                table: "provider");

            migrationBuilder.DropColumn(
                name: "gender",
                table: "provider");

            migrationBuilder.DropColumn(
                name: "link",
                table: "provider");

            migrationBuilder.DropColumn(
                name: "price_for_thirty",
                table: "provider");

            migrationBuilder.RenameColumn(
                name: "time_zone",
                table: "provider",
                newName: "curriculum");
        }
    }
}
