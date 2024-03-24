﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using PRMS.Data.Contexts;

#nullable disable

namespace PRMS.Data.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240324081635_updated_MedicationStatus")]
    partial class updated_MedicationStatus
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.14")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("MedicalCenterMedicalCenterCategory", b =>
                {
                    b.Property<string>("MedicalCenterCategoriesId")
                        .HasColumnType("text");

                    b.Property<string>("MedicalCentersId")
                        .HasColumnType("text");

                    b.HasKey("MedicalCenterCategoriesId", "MedicalCentersId");

                    b.HasIndex("MedicalCentersId");

                    b.ToTable("MedicalCenterCategoryPivot", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("text");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .HasColumnType("text");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Value")
                        .HasColumnType("text");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("PRMS.Domain.Entities.Address", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<double?>("Latitude")
                        .HasColumnType("double precision");

                    b.Property<double?>("Longitude")
                        .HasColumnType("double precision");

                    b.Property<string>("State")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Street")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("Addresses");
                });

            modelBuilder.Entity("PRMS.Domain.Entities.Appointment", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTimeOffset>("Date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("PatientId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PhysicianId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Reason")
                        .HasColumnType("text");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("PatientId");

                    b.HasIndex("PhysicianId");

                    b.ToTable("Appointments");
                });

            modelBuilder.Entity("PRMS.Domain.Entities.Favorite", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("EntityId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("EntityType")
                        .HasColumnType("integer");

                    b.Property<string>("PatientId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("PatientId");

                    b.ToTable("Favorites");
                });

            modelBuilder.Entity("PRMS.Domain.Entities.MedicalCenter", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("AddressId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PublicId")
                        .HasColumnType("text");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("AddressId");

                    b.ToTable("MedicalCenters");
                });

            modelBuilder.Entity("PRMS.Domain.Entities.MedicalCenterCategory", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("MedicalCenterCategories");
                });

            modelBuilder.Entity("PRMS.Domain.Entities.MedicalCenterReview", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("Content")
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("MedicalCenterId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PatientId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Rating")
                        .HasColumnType("integer");

                    b.Property<string>("Title")
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("MedicalCenterId");

                    b.HasIndex("PatientId");

                    b.ToTable("MedicalCenterReviews");
                });

            modelBuilder.Entity("PRMS.Domain.Entities.MedicalDetail", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("MedicalDetailsType")
                        .HasColumnType("integer");

                    b.Property<string>("PatientId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("PatientId");

                    b.ToTable("MedicalDetails");
                });

            modelBuilder.Entity("PRMS.Domain.Entities.Medication", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Dosage")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Duration")
                        .HasColumnType("text");

                    b.Property<string>("Frequency")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Instruction")
                        .HasColumnType("text");

                    b.Property<int>("MedicationStatus")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PatientId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PrescriptionId")
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("PatientId");

                    b.HasIndex("PrescriptionId");

                    b.ToTable("Medications");
                });

            modelBuilder.Entity("PRMS.Domain.Entities.Patient", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<int>("BloodGroup")
                        .HasColumnType("integer");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateOnly>("DateOfBirth")
                        .HasColumnType("date");

                    b.Property<string>("EmergencyContactName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("EmergencyContactPhoneNo")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("EmergencyContactRelationship")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Gender")
                        .HasColumnType("integer");

                    b.Property<float>("Height")
                        .HasColumnType("real");

                    b.Property<string>("MedicalCenterId")
                        .HasColumnType("text");

                    b.Property<string>("PhysicianId")
                        .HasColumnType("text");

                    b.Property<string>("PrimaryPhysicanEmail")
                        .HasColumnType("text");

                    b.Property<string>("PrimaryPhysicanName")
                        .HasColumnType("text");

                    b.Property<string>("PrimaryPhysicanPhoneNo")
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<float>("Weight")
                        .HasColumnType("real");

                    b.HasKey("Id");

                    b.HasIndex("MedicalCenterId");

                    b.HasIndex("PhysicianId");

                    b.HasIndex("UserId");

                    b.ToTable("Patients");
                });

            modelBuilder.Entity("PRMS.Domain.Entities.Physician", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("About")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("MedicalCenterId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Speciality")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("WorkingTime")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("YearsOfExperience")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("MedicalCenterId");

                    b.HasIndex("UserId");

                    b.ToTable("Physicians");
                });

            modelBuilder.Entity("PRMS.Domain.Entities.PhysicianReview", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("Content")
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("PatientId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PhysicianId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Rating")
                        .HasColumnType("integer");

                    b.Property<string>("Title")
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("PatientId");

                    b.HasIndex("PhysicianId");

                    b.ToTable("PhysicianReviews");
                });

            modelBuilder.Entity("PRMS.Domain.Entities.Prescription", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Diagnosis")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Note")
                        .HasColumnType("text");

                    b.Property<string>("PatientId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PhysicianId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("PatientId");

                    b.HasIndex("PhysicianId");

                    b.ToTable("Prescriptions");
                });

            modelBuilder.Entity("PRMS.Domain.Entities.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("integer");

                    b.Property<string>("AddressId")
                        .HasColumnType("text");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("text");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("MiddleName")
                        .HasColumnType("text");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("PublicId")
                        .HasColumnType("text");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("text");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("AddressId");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("MedicalCenterMedicalCenterCategory", b =>
                {
                    b.HasOne("PRMS.Domain.Entities.MedicalCenterCategory", null)
                        .WithMany()
                        .HasForeignKey("MedicalCenterCategoriesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PRMS.Domain.Entities.MedicalCenter", null)
                        .WithMany()
                        .HasForeignKey("MedicalCentersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("PRMS.Domain.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("PRMS.Domain.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PRMS.Domain.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("PRMS.Domain.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PRMS.Domain.Entities.Appointment", b =>
                {
                    b.HasOne("PRMS.Domain.Entities.Patient", "Patient")
                        .WithMany("Appointments")
                        .HasForeignKey("PatientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PRMS.Domain.Entities.Physician", "Physician")
                        .WithMany("Appointments")
                        .HasForeignKey("PhysicianId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Patient");

                    b.Navigation("Physician");
                });

            modelBuilder.Entity("PRMS.Domain.Entities.Favorite", b =>
                {
                    b.HasOne("PRMS.Domain.Entities.Patient", "Patient")
                        .WithMany("Favorites")
                        .HasForeignKey("PatientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Patient");
                });

            modelBuilder.Entity("PRMS.Domain.Entities.MedicalCenter", b =>
                {
                    b.HasOne("PRMS.Domain.Entities.Address", "Address")
                        .WithMany()
                        .HasForeignKey("AddressId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Address");
                });

            modelBuilder.Entity("PRMS.Domain.Entities.MedicalCenterReview", b =>
                {
                    b.HasOne("PRMS.Domain.Entities.MedicalCenter", "MedicalCenter")
                        .WithMany("Reviews")
                        .HasForeignKey("MedicalCenterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PRMS.Domain.Entities.Patient", "Patient")
                        .WithMany()
                        .HasForeignKey("PatientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MedicalCenter");

                    b.Navigation("Patient");
                });

            modelBuilder.Entity("PRMS.Domain.Entities.MedicalDetail", b =>
                {
                    b.HasOne("PRMS.Domain.Entities.Patient", null)
                        .WithMany("MedicalDetails")
                        .HasForeignKey("PatientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PRMS.Domain.Entities.Medication", b =>
                {
                    b.HasOne("PRMS.Domain.Entities.Patient", "Patient")
                        .WithMany("Medications")
                        .HasForeignKey("PatientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PRMS.Domain.Entities.Prescription", "Prescription")
                        .WithMany("Medications")
                        .HasForeignKey("PrescriptionId");

                    b.Navigation("Patient");

                    b.Navigation("Prescription");
                });

            modelBuilder.Entity("PRMS.Domain.Entities.Patient", b =>
                {
                    b.HasOne("PRMS.Domain.Entities.MedicalCenter", null)
                        .WithMany("Patients")
                        .HasForeignKey("MedicalCenterId");

                    b.HasOne("PRMS.Domain.Entities.Physician", "Physician")
                        .WithMany("Patients")
                        .HasForeignKey("PhysicianId");

                    b.HasOne("PRMS.Domain.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Physician");

                    b.Navigation("User");
                });

            modelBuilder.Entity("PRMS.Domain.Entities.Physician", b =>
                {
                    b.HasOne("PRMS.Domain.Entities.MedicalCenter", "MedicalCenter")
                        .WithMany("Physicians")
                        .HasForeignKey("MedicalCenterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PRMS.Domain.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MedicalCenter");

                    b.Navigation("User");
                });

            modelBuilder.Entity("PRMS.Domain.Entities.PhysicianReview", b =>
                {
                    b.HasOne("PRMS.Domain.Entities.Patient", "Patient")
                        .WithMany()
                        .HasForeignKey("PatientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PRMS.Domain.Entities.Physician", "Physician")
                        .WithMany("Reviews")
                        .HasForeignKey("PhysicianId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Patient");

                    b.Navigation("Physician");
                });

            modelBuilder.Entity("PRMS.Domain.Entities.Prescription", b =>
                {
                    b.HasOne("PRMS.Domain.Entities.Patient", "Patient")
                        .WithMany("Prescriptions")
                        .HasForeignKey("PatientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PRMS.Domain.Entities.Physician", "Physician")
                        .WithMany()
                        .HasForeignKey("PhysicianId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Patient");

                    b.Navigation("Physician");
                });

            modelBuilder.Entity("PRMS.Domain.Entities.User", b =>
                {
                    b.HasOne("PRMS.Domain.Entities.Address", "Address")
                        .WithMany()
                        .HasForeignKey("AddressId");

                    b.Navigation("Address");
                });

            modelBuilder.Entity("PRMS.Domain.Entities.MedicalCenter", b =>
                {
                    b.Navigation("Patients");

                    b.Navigation("Physicians");

                    b.Navigation("Reviews");
                });

            modelBuilder.Entity("PRMS.Domain.Entities.Patient", b =>
                {
                    b.Navigation("Appointments");

                    b.Navigation("Favorites");

                    b.Navigation("MedicalDetails");

                    b.Navigation("Medications");

                    b.Navigation("Prescriptions");
                });

            modelBuilder.Entity("PRMS.Domain.Entities.Physician", b =>
                {
                    b.Navigation("Appointments");

                    b.Navigation("Patients");

                    b.Navigation("Reviews");
                });

            modelBuilder.Entity("PRMS.Domain.Entities.Prescription", b =>
                {
                    b.Navigation("Medications");
                });
#pragma warning restore 612, 618
        }
    }
}
