using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Marketplace.Infra.Migrations
{
    public partial class initials : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "categories",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    description_short = table.Column<string>(type: "text", nullable: true),
                    description_long = table.Column<string>(type: "text", nullable: true),
                    image = table.Column<string>(type: "varchar(50)", nullable: true),
                    name = table.Column<string>(type: "varchar(120)", nullable: true),
                    active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_categories", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "customer",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "varchar(120)", nullable: true),
                    email = table.Column<string>(type: "varchar(150)", nullable: true),
                    password = table.Column<string>(type: "text", nullable: true),
                    cnpj = table.Column<string>(type: "varchar(14)", nullable: true),
                    cpf = table.Column<string>(type: "varchar(11)", nullable: true),
                    image = table.Column<string>(type: "varchar(50)", nullable: true),
                    newsletter = table.Column<bool>(type: "boolean", nullable: false),
                    active = table.Column<bool>(type: "boolean", nullable: false),
                    recoverpassword = table.Column<string>(type: "text", nullable: true),
                    recoverqtd = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_customer", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "languages",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "varchar(120)", nullable: true),
                    active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_languages", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "provider",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    fantasy_name = table.Column<string>(type: "varchar(255)", nullable: true),
                    company_name = table.Column<string>(type: "varchar(255)", nullable: true),
                    email = table.Column<string>(type: "varchar(150)", nullable: true),
                    crp = table.Column<string>(type: "text", nullable: true),
                    phone = table.Column<string>(type: "varchar(255)", nullable: true),
                    password = table.Column<string>(type: "text", nullable: true),
                    cnpj = table.Column<string>(type: "varchar(14)", nullable: true),
                    cpf = table.Column<string>(type: "varchar(11)", nullable: true),
                    image = table.Column<string>(type: "varchar(50)", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    curriculum = table.Column<string>(type: "text", nullable: true),
                    biography = table.Column<string>(type: "text", nullable: true),
                    academic_training = table.Column<string>(type: "text", nullable: true),
                    interval_between_appointment = table.Column<int>(type: "integer", nullable: false),
                    origin = table.Column<int>(type: "integer", nullable: false),
                    situation = table.Column<int>(type: "integer", nullable: false),
                    active = table.Column<bool>(type: "boolean", nullable: false),
                    remove = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_provider", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "topics",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "varchar(120)", nullable: true),
                    active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_topics", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "customer_address",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    customer_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    city = table.Column<string>(type: "text", nullable: true),
                    address = table.Column<string>(type: "text", nullable: true),
                    neighborhood = table.Column<string>(type: "text", nullable: true),
                    complement = table.Column<string>(type: "text", nullable: true),
                    number = table.Column<string>(type: "varchar(10)", nullable: true),
                    zipcode = table.Column<string>(type: "varchar(12)", nullable: true),
                    uf = table.Column<string>(type: "varchar(2)", nullable: true),
                    country = table.Column<string>(type: "varchar(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_customer_address", x => x.id);
                    table.ForeignKey(
                        name: "FK_customer_address_customer_customer_id",
                        column: x => x.customer_id,
                        principalTable: "customer",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "customer_assessments",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    stars = table.Column<double>(type: "double precision", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    active = table.Column<bool>(type: "boolean", nullable: false),
                    customer_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_customer_assessments", x => x.id);
                    table.ForeignKey(
                        name: "FK_customer_assessments_customer_customer_id",
                        column: x => x.customer_id,
                        principalTable: "customer",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "appointments",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    price_commission = table.Column<decimal>(type: "numeric", nullable: false),
                    price_cost = table.Column<decimal>(type: "numeric", nullable: false),
                    price_full = table.Column<decimal>(type: "numeric", nullable: false),
                    price = table.Column<decimal>(type: "numeric", nullable: false),
                    booking_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    payment_status = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    customer_id = table.Column<int>(type: "integer", nullable: false),
                    provider_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_appointments", x => x.id);
                    table.ForeignKey(
                        name: "FK_appointments_customer_provider_id",
                        column: x => x.provider_id,
                        principalTable: "customer",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_appointments_provider_provider_id",
                        column: x => x.provider_id,
                        principalTable: "provider",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "provider_address",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    provider_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    city = table.Column<string>(type: "text", nullable: true),
                    address = table.Column<string>(type: "text", nullable: true),
                    neighborhood = table.Column<string>(type: "text", nullable: true),
                    complement = table.Column<string>(type: "text", nullable: true),
                    number = table.Column<string>(type: "varchar(10)", nullable: true),
                    zipcode = table.Column<string>(type: "varchar(12)", nullable: true),
                    uf = table.Column<string>(type: "varchar(2)", nullable: true),
                    country = table.Column<string>(type: "varchar(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_provider_address", x => x.id);
                    table.ForeignKey(
                        name: "FK_provider_address_provider_provider_id",
                        column: x => x.provider_id,
                        principalTable: "provider",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "provider_bank_account",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    agency_number = table.Column<string>(type: "varchar(20)", nullable: true),
                    agency_digit = table.Column<string>(type: "varchar(3)", nullable: true),
                    account_digit = table.Column<string>(type: "varchar(3)", nullable: true),
                    account_number = table.Column<string>(type: "varchar(20)", nullable: true),
                    bank_code = table.Column<string>(type: "varchar(10)", nullable: true),
                    provider_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_provider_bank_account", x => x.id);
                    table.ForeignKey(
                        name: "FK_provider_bank_account_provider_provider_id",
                        column: x => x.provider_id,
                        principalTable: "provider",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "provider_categories",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    category_id = table.Column<int>(type: "integer", nullable: false),
                    provider_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_provider_categories", x => x.id);
                    table.ForeignKey(
                        name: "FK_provider_categories_categories_category_id",
                        column: x => x.category_id,
                        principalTable: "categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_provider_categories_provider_provider_id",
                        column: x => x.provider_id,
                        principalTable: "provider",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "provider_languages",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    language_id = table.Column<int>(type: "integer", nullable: false),
                    provider_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_provider_languages", x => x.id);
                    table.ForeignKey(
                        name: "FK_provider_languages_languages_language_id",
                        column: x => x.language_id,
                        principalTable: "languages",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_provider_languages_provider_provider_id",
                        column: x => x.provider_id,
                        principalTable: "provider",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "provider_schedules",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    start = table.Column<TimeSpan>(type: "interval", nullable: false),
                    end = table.Column<TimeSpan>(type: "interval", nullable: false),
                    day_week = table.Column<int>(type: "integer", nullable: false),
                    provider_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_provider_schedules", x => x.id);
                    table.ForeignKey(
                        name: "FK_provider_schedules_provider_provider_id",
                        column: x => x.provider_id,
                        principalTable: "provider",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "provider_split_accounts",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    json = table.Column<string>(type: "jsonb", nullable: true),
                    payment_provider = table.Column<int>(type: "integer", nullable: false),
                    provider_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_provider_split_accounts", x => x.id);
                    table.ForeignKey(
                        name: "FK_provider_split_accounts_provider_provider_id",
                        column: x => x.provider_id,
                        principalTable: "provider",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "provider_topics",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    topic_id = table.Column<int>(type: "integer", nullable: false),
                    provider_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_provider_topics", x => x.id);
                    table.ForeignKey(
                        name: "FK_provider_topics_provider_provider_id",
                        column: x => x.provider_id,
                        principalTable: "provider",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_provider_topics_topics_topic_id",
                        column: x => x.topic_id,
                        principalTable: "topics",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_appointments_provider_id",
                table: "appointments",
                column: "provider_id");

            migrationBuilder.CreateIndex(
                name: "IX_customer_address_customer_id",
                table: "customer_address",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "IX_customer_assessments_customer_id",
                table: "customer_assessments",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "IX_provider_address_provider_id",
                table: "provider_address",
                column: "provider_id");

            migrationBuilder.CreateIndex(
                name: "IX_provider_bank_account_provider_id",
                table: "provider_bank_account",
                column: "provider_id");

            migrationBuilder.CreateIndex(
                name: "IX_provider_categories_category_id",
                table: "provider_categories",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "IX_provider_categories_provider_id",
                table: "provider_categories",
                column: "provider_id");

            migrationBuilder.CreateIndex(
                name: "IX_provider_languages_language_id",
                table: "provider_languages",
                column: "language_id");

            migrationBuilder.CreateIndex(
                name: "IX_provider_languages_provider_id",
                table: "provider_languages",
                column: "provider_id");

            migrationBuilder.CreateIndex(
                name: "IX_provider_schedules_provider_id",
                table: "provider_schedules",
                column: "provider_id");

            migrationBuilder.CreateIndex(
                name: "IX_provider_split_accounts_provider_id",
                table: "provider_split_accounts",
                column: "provider_id");

            migrationBuilder.CreateIndex(
                name: "IX_provider_topics_provider_id",
                table: "provider_topics",
                column: "provider_id");

            migrationBuilder.CreateIndex(
                name: "IX_provider_topics_topic_id",
                table: "provider_topics",
                column: "topic_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "appointments");

            migrationBuilder.DropTable(
                name: "customer_address");

            migrationBuilder.DropTable(
                name: "customer_assessments");

            migrationBuilder.DropTable(
                name: "provider_address");

            migrationBuilder.DropTable(
                name: "provider_bank_account");

            migrationBuilder.DropTable(
                name: "provider_categories");

            migrationBuilder.DropTable(
                name: "provider_languages");

            migrationBuilder.DropTable(
                name: "provider_schedules");

            migrationBuilder.DropTable(
                name: "provider_split_accounts");

            migrationBuilder.DropTable(
                name: "provider_topics");

            migrationBuilder.DropTable(
                name: "customer");

            migrationBuilder.DropTable(
                name: "categories");

            migrationBuilder.DropTable(
                name: "languages");

            migrationBuilder.DropTable(
                name: "provider");

            migrationBuilder.DropTable(
                name: "topics");
        }
    }
}
