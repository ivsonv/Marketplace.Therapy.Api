﻿// <auto-generated />
using System;
using Marketplace.Infra.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Marketplace.Infra.Migrations
{
    [DbContext(typeof(MarketPlaceContext))]
    [Migration("20210824151714_add_new_field_price_transfer_appointment")]
    partial class add_new_field_price_transfer_appointment
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.6")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("Marketplace.Domain.Entities.Appointment", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("booking_date")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime?>("created_at")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("customer_id")
                        .HasColumnType("integer");

                    b.Property<int>("origin")
                        .HasColumnType("integer");

                    b.Property<int>("payment_status")
                        .HasColumnType("integer");

                    b.Property<decimal>("price")
                        .HasColumnType("numeric");

                    b.Property<decimal>("price_commission")
                        .HasColumnType("numeric");

                    b.Property<decimal>("price_cost")
                        .HasColumnType("numeric");

                    b.Property<decimal>("price_full")
                        .HasColumnType("numeric");

                    b.Property<decimal>("price_transfer")
                        .HasColumnType("numeric");

                    b.Property<int>("provider_id")
                        .HasColumnType("integer");

                    b.Property<int>("status")
                        .HasColumnType("integer");

                    b.Property<int>("type")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("updated_at")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("id");

                    b.HasIndex("provider_id");

                    b.ToTable("appointments");
                });

            modelBuilder.Entity("Marketplace.Domain.Entities.Bank", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<bool>("active")
                        .HasColumnType("boolean");

                    b.Property<string>("code")
                        .HasColumnType("varchar(3)");

                    b.Property<DateTime?>("created_at")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("name")
                        .HasColumnType("varchar(200)");

                    b.Property<DateTime?>("updated_at")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("id");

                    b.ToTable("banks");
                });

            modelBuilder.Entity("Marketplace.Domain.Entities.Category", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<bool>("active")
                        .HasColumnType("boolean");

                    b.Property<DateTime?>("created_at")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("description_long")
                        .HasColumnType("text");

                    b.Property<string>("description_short")
                        .HasColumnType("text");

                    b.Property<string>("image")
                        .HasColumnType("varchar(50)");

                    b.Property<string>("name")
                        .HasColumnType("varchar(120)");

                    b.Property<DateTime?>("updated_at")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("id");

                    b.ToTable("categories");
                });

            modelBuilder.Entity("Marketplace.Domain.Entities.Customer", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<bool>("active")
                        .HasColumnType("boolean");

                    b.Property<string>("cnpj")
                        .HasColumnType("varchar(14)");

                    b.Property<string>("cpf")
                        .HasColumnType("varchar(11)");

                    b.Property<DateTime?>("created_at")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("email")
                        .HasColumnType("varchar(150)");

                    b.Property<string>("image")
                        .HasColumnType("varchar(50)");

                    b.Property<string>("name")
                        .HasColumnType("varchar(120)");

                    b.Property<bool>("newsletter")
                        .HasColumnType("boolean");

                    b.Property<string>("password")
                        .HasColumnType("text");

                    b.Property<string>("recoverpassword")
                        .HasColumnType("text");

                    b.Property<int>("recoverqtd")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("updated_at")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("id");

                    b.ToTable("customer");
                });

            modelBuilder.Entity("Marketplace.Domain.Entities.CustomerAddress", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("address")
                        .HasColumnType("text");

                    b.Property<string>("city")
                        .HasColumnType("text");

                    b.Property<string>("complement")
                        .HasColumnType("text");

                    b.Property<string>("country")
                        .HasColumnType("varchar(2)");

                    b.Property<DateTime?>("created_at")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("customer_id")
                        .HasColumnType("integer");

                    b.Property<string>("neighborhood")
                        .HasColumnType("text");

                    b.Property<string>("number")
                        .HasColumnType("varchar(10)");

                    b.Property<string>("uf")
                        .HasColumnType("varchar(2)");

                    b.Property<DateTime?>("updated_at")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("zipcode")
                        .HasColumnType("varchar(12)");

                    b.HasKey("id");

                    b.HasIndex("customer_id");

                    b.ToTable("customer_address");
                });

            modelBuilder.Entity("Marketplace.Domain.Entities.CustomerAssessment", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<bool>("active")
                        .HasColumnType("boolean");

                    b.Property<DateTime?>("created_at")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("customer_id")
                        .HasColumnType("integer");

                    b.Property<string>("description")
                        .HasColumnType("text");

                    b.Property<double>("stars")
                        .HasColumnType("double precision");

                    b.Property<DateTime?>("updated_at")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("id");

                    b.HasIndex("customer_id");

                    b.ToTable("customer_assessments");
                });

            modelBuilder.Entity("Marketplace.Domain.Entities.GroupPermission", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime?>("created_at")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("name")
                        .HasColumnType("varchar(50)");

                    b.Property<DateTime?>("updated_at")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("id");

                    b.ToTable("group_permissions");
                });

            modelBuilder.Entity("Marketplace.Domain.Entities.GroupPermissionAttached", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime?>("created_at")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("group_permission_id")
                        .HasColumnType("integer");

                    b.Property<string>("name")
                        .HasColumnType("varchar(120)");

                    b.Property<DateTime?>("updated_at")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("id");

                    b.HasIndex("group_permission_id");

                    b.ToTable("group_permissions_attached");
                });

            modelBuilder.Entity("Marketplace.Domain.Entities.Language", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<bool>("active")
                        .HasColumnType("boolean");

                    b.Property<DateTime?>("created_at")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("name")
                        .HasColumnType("varchar(120)");

                    b.Property<DateTime?>("updated_at")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("id");

                    b.ToTable("languages");
                });

            modelBuilder.Entity("Marketplace.Domain.Entities.Provider", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("academic_training")
                        .HasColumnType("text");

                    b.Property<bool>("active")
                        .HasColumnType("boolean");

                    b.Property<string>("biography")
                        .HasColumnType("text");

                    b.Property<DateTime>("birthdate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("cnpj")
                        .HasColumnType("varchar(14)");

                    b.Property<string>("company_name")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("cpf")
                        .HasColumnType("varchar(11)");

                    b.Property<DateTime?>("created_at")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("crp")
                        .HasColumnType("text");

                    b.Property<string>("description")
                        .HasColumnType("text");

                    b.Property<string>("email")
                        .HasColumnType("varchar(150)");

                    b.Property<string>("fantasy_name")
                        .HasColumnType("varchar(255)");

                    b.Property<int>("gender")
                        .HasColumnType("integer");

                    b.Property<string>("image")
                        .HasColumnType("varchar(50)");

                    b.Property<int>("interval_between_appointment")
                        .HasColumnType("integer");

                    b.Property<string>("link")
                        .HasColumnType("text");

                    b.Property<string>("nickname")
                        .HasColumnType("text");

                    b.Property<string>("password")
                        .HasColumnType("text");

                    b.Property<string>("phone")
                        .HasColumnType("varchar(255)");

                    b.Property<decimal>("price")
                        .HasColumnType("numeric(10,2)");

                    b.Property<decimal?>("price_for_thirty")
                        .HasColumnType("numeric");

                    b.Property<bool>("remove")
                        .HasColumnType("boolean");

                    b.Property<int>("situation")
                        .HasColumnType("integer");

                    b.Property<string>("time_zone")
                        .HasColumnType("text");

                    b.Property<DateTime?>("updated_at")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("id");

                    b.ToTable("provider");
                });

            modelBuilder.Entity("Marketplace.Domain.Entities.ProviderAddress", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("address")
                        .HasColumnType("text");

                    b.Property<string>("city")
                        .HasColumnType("text");

                    b.Property<string>("complement")
                        .HasColumnType("text");

                    b.Property<string>("country")
                        .HasColumnType("varchar(2)");

                    b.Property<DateTime?>("created_at")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("neighborhood")
                        .HasColumnType("text");

                    b.Property<string>("number")
                        .HasColumnType("varchar(10)");

                    b.Property<int>("provider_id")
                        .HasColumnType("integer");

                    b.Property<string>("uf")
                        .HasColumnType("varchar(2)");

                    b.Property<DateTime?>("updated_at")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("zipcode")
                        .HasColumnType("varchar(12)");

                    b.HasKey("id");

                    b.HasIndex("provider_id");

                    b.ToTable("provider_address");
                });

            modelBuilder.Entity("Marketplace.Domain.Entities.ProviderBankAccount", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("account_bank_type")
                        .HasColumnType("integer");

                    b.Property<string>("account_digit")
                        .HasColumnType("varchar(3)");

                    b.Property<string>("account_number")
                        .HasColumnType("varchar(20)");

                    b.Property<string>("agency_digit")
                        .HasColumnType("varchar(3)");

                    b.Property<string>("agency_number")
                        .HasColumnType("varchar(20)");

                    b.Property<string>("bank_code")
                        .HasColumnType("varchar(10)");

                    b.Property<DateTime?>("created_at")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("provider_id")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("updated_at")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("id");

                    b.HasIndex("provider_id");

                    b.ToTable("provider_bank_account");
                });

            modelBuilder.Entity("Marketplace.Domain.Entities.ProviderCategories", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("category_id")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("created_at")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("provider_id")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("updated_at")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("id");

                    b.HasIndex("category_id");

                    b.HasIndex("provider_id");

                    b.ToTable("provider_categories");
                });

            modelBuilder.Entity("Marketplace.Domain.Entities.ProviderLanguages", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime?>("created_at")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("language_id")
                        .HasColumnType("integer");

                    b.Property<int>("provider_id")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("updated_at")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("id");

                    b.HasIndex("language_id");

                    b.HasIndex("provider_id");

                    b.ToTable("provider_languages");
                });

            modelBuilder.Entity("Marketplace.Domain.Entities.ProviderReceipt", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("address")
                        .HasColumnType("text");

                    b.Property<string>("cnpj")
                        .HasColumnType("text");

                    b.Property<string>("cpf")
                        .HasColumnType("text");

                    b.Property<DateTime?>("created_at")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("fantasy_name")
                        .HasColumnType("text");

                    b.Property<int>("provider_id")
                        .HasColumnType("integer");

                    b.Property<string>("signature")
                        .HasColumnType("text");

                    b.Property<DateTime?>("updated_at")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("id");

                    b.HasIndex("provider_id");

                    b.ToTable("provider_receipt");
                });

            modelBuilder.Entity("Marketplace.Domain.Entities.ProviderSchedule", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime?>("created_at")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("day_week")
                        .HasColumnType("integer");

                    b.Property<TimeSpan>("end")
                        .HasColumnType("interval");

                    b.Property<int>("provider_id")
                        .HasColumnType("integer");

                    b.Property<TimeSpan>("start")
                        .HasColumnType("interval");

                    b.Property<DateTime?>("updated_at")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("id");

                    b.HasIndex("provider_id");

                    b.ToTable("provider_schedules");
                });

            modelBuilder.Entity("Marketplace.Domain.Entities.ProviderSplitAccount", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime?>("created_at")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("json")
                        .HasColumnType("jsonb");

                    b.Property<int>("payment_provider")
                        .HasColumnType("integer");

                    b.Property<int>("provider_id")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("updated_at")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("id");

                    b.HasIndex("provider_id");

                    b.ToTable("provider_split_accounts");
                });

            modelBuilder.Entity("Marketplace.Domain.Entities.ProviderTopics", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime?>("created_at")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("provider_id")
                        .HasColumnType("integer");

                    b.Property<int>("topic_id")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("updated_at")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("id");

                    b.HasIndex("provider_id");

                    b.HasIndex("topic_id");

                    b.ToTable("provider_topics");
                });

            modelBuilder.Entity("Marketplace.Domain.Entities.Topic", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<bool>("active")
                        .HasColumnType("boolean");

                    b.Property<DateTime?>("created_at")
                        .HasColumnType("timestamp without time zone");

                    b.Property<bool>("experience")
                        .HasColumnType("boolean");

                    b.Property<string>("name")
                        .HasColumnType("varchar(120)");

                    b.Property<DateTime?>("updated_at")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("id");

                    b.ToTable("topics");
                });

            modelBuilder.Entity("Marketplace.Domain.Entities.User", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<bool>("active")
                        .HasColumnType("boolean");

                    b.Property<DateTime?>("created_at")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("email")
                        .HasColumnType("text");

                    b.Property<string>("name")
                        .HasColumnType("varchar(200)");

                    b.Property<string>("password")
                        .HasColumnType("text");

                    b.Property<DateTime?>("updated_at")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("id");

                    b.ToTable("users");
                });

            modelBuilder.Entity("Marketplace.Domain.Entities.UserGroupPermission", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime?>("created_at")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("group_permission_id")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("updated_at")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("user_id")
                        .HasColumnType("integer");

                    b.HasKey("id");

                    b.HasIndex("group_permission_id");

                    b.HasIndex("user_id");

                    b.ToTable("user_group_permissions");
                });

            modelBuilder.Entity("Marketplace.Domain.Entities.Appointment", b =>
                {
                    b.HasOne("Marketplace.Domain.Entities.Customer", "Customer")
                        .WithMany("Appointments")
                        .HasForeignKey("provider_id")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Marketplace.Domain.Entities.Provider", "Provider")
                        .WithMany("Appointments")
                        .HasForeignKey("provider_id")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Customer");

                    b.Navigation("Provider");
                });

            modelBuilder.Entity("Marketplace.Domain.Entities.CustomerAddress", b =>
                {
                    b.HasOne("Marketplace.Domain.Entities.Customer", "Customer")
                        .WithMany("Address")
                        .HasForeignKey("customer_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("Marketplace.Domain.Entities.CustomerAssessment", b =>
                {
                    b.HasOne("Marketplace.Domain.Entities.Customer", "Customer")
                        .WithMany("Assessment")
                        .HasForeignKey("customer_id")
                        .IsRequired();

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("Marketplace.Domain.Entities.GroupPermissionAttached", b =>
                {
                    b.HasOne("Marketplace.Domain.Entities.GroupPermission", "GroupPermission")
                        .WithMany("PermissionsAttached")
                        .HasForeignKey("group_permission_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("GroupPermission");
                });

            modelBuilder.Entity("Marketplace.Domain.Entities.ProviderAddress", b =>
                {
                    b.HasOne("Marketplace.Domain.Entities.Provider", "Provider")
                        .WithMany("Address")
                        .HasForeignKey("provider_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Provider");
                });

            modelBuilder.Entity("Marketplace.Domain.Entities.ProviderBankAccount", b =>
                {
                    b.HasOne("Marketplace.Domain.Entities.Provider", "Provider")
                        .WithMany("BankAccounts")
                        .HasForeignKey("provider_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Provider");
                });

            modelBuilder.Entity("Marketplace.Domain.Entities.ProviderCategories", b =>
                {
                    b.HasOne("Marketplace.Domain.Entities.Category", "Category")
                        .WithMany("ProviderCategories")
                        .HasForeignKey("category_id")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Marketplace.Domain.Entities.Provider", "Provider")
                        .WithMany("Categories")
                        .HasForeignKey("provider_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("Provider");
                });

            modelBuilder.Entity("Marketplace.Domain.Entities.ProviderLanguages", b =>
                {
                    b.HasOne("Marketplace.Domain.Entities.Language", "Language")
                        .WithMany("ProviderLanguages")
                        .HasForeignKey("language_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Marketplace.Domain.Entities.Provider", "Provider")
                        .WithMany("Languages")
                        .HasForeignKey("provider_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Language");

                    b.Navigation("Provider");
                });

            modelBuilder.Entity("Marketplace.Domain.Entities.ProviderReceipt", b =>
                {
                    b.HasOne("Marketplace.Domain.Entities.Provider", "Provider")
                        .WithMany("ProviderReceipts")
                        .HasForeignKey("provider_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Provider");
                });

            modelBuilder.Entity("Marketplace.Domain.Entities.ProviderSchedule", b =>
                {
                    b.HasOne("Marketplace.Domain.Entities.Provider", "Provider")
                        .WithMany("Schedules")
                        .HasForeignKey("provider_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Provider");
                });

            modelBuilder.Entity("Marketplace.Domain.Entities.ProviderSplitAccount", b =>
                {
                    b.HasOne("Marketplace.Domain.Entities.Provider", "Provider")
                        .WithMany("SplitAccounts")
                        .HasForeignKey("provider_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Provider");
                });

            modelBuilder.Entity("Marketplace.Domain.Entities.ProviderTopics", b =>
                {
                    b.HasOne("Marketplace.Domain.Entities.Provider", "Provider")
                        .WithMany("Topics")
                        .HasForeignKey("provider_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Marketplace.Domain.Entities.Topic", "Topic")
                        .WithMany("ProviderTopics")
                        .HasForeignKey("topic_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Provider");

                    b.Navigation("Topic");
                });

            modelBuilder.Entity("Marketplace.Domain.Entities.UserGroupPermission", b =>
                {
                    b.HasOne("Marketplace.Domain.Entities.GroupPermission", "GroupPermission")
                        .WithMany("Users")
                        .HasForeignKey("group_permission_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Marketplace.Domain.Entities.User", "User")
                        .WithMany("GroupPermissions")
                        .HasForeignKey("user_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("GroupPermission");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Marketplace.Domain.Entities.Category", b =>
                {
                    b.Navigation("ProviderCategories");
                });

            modelBuilder.Entity("Marketplace.Domain.Entities.Customer", b =>
                {
                    b.Navigation("Address");

                    b.Navigation("Appointments");

                    b.Navigation("Assessment");
                });

            modelBuilder.Entity("Marketplace.Domain.Entities.GroupPermission", b =>
                {
                    b.Navigation("PermissionsAttached");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("Marketplace.Domain.Entities.Language", b =>
                {
                    b.Navigation("ProviderLanguages");
                });

            modelBuilder.Entity("Marketplace.Domain.Entities.Provider", b =>
                {
                    b.Navigation("Address");

                    b.Navigation("Appointments");

                    b.Navigation("BankAccounts");

                    b.Navigation("Categories");

                    b.Navigation("Languages");

                    b.Navigation("ProviderReceipts");

                    b.Navigation("Schedules");

                    b.Navigation("SplitAccounts");

                    b.Navigation("Topics");
                });

            modelBuilder.Entity("Marketplace.Domain.Entities.Topic", b =>
                {
                    b.Navigation("ProviderTopics");
                });

            modelBuilder.Entity("Marketplace.Domain.Entities.User", b =>
                {
                    b.Navigation("GroupPermissions");
                });
#pragma warning restore 612, 618
        }
    }
}
