﻿// <auto-generated />
using System;
using ArduinoController.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ArduinoController.DataAccess.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.1-rtm-30846")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ArduinoController.Core.Models.ArduinoDevice", b =>
                {
                    b.Property<string>("MacAddress")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ApplicationUserId")
                        .IsRequired();

                    b.Property<string>("Name");

                    b.HasKey("MacAddress");

                    b.HasIndex("ApplicationUserId");

                    b.ToTable("ArduinoDevice");
                });

            modelBuilder.Entity("ArduinoController.Core.Models.Commands.Command", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.Property<int>("Order");

                    b.Property<string>("ProcedureName");

                    b.Property<string>("ProcedureUserId");

                    b.HasKey("Id");

                    b.HasIndex("ProcedureUserId", "ProcedureName");

                    b.ToTable("Commands");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Command");
                });

            modelBuilder.Entity("ArduinoController.Core.Models.Procedure", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("Name");

                    b.Property<string>("DeviceMacAddress");

                    b.HasKey("UserId", "Name");

                    b.HasIndex("DeviceMacAddress");

                    b.ToTable("Procedures");
                });

            modelBuilder.Entity("ArduinoController.DataAccess.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("RefreshToken");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("ArduinoController.Core.Models.Commands.AnalogWriteCommand", b =>
                {
                    b.HasBaseType("ArduinoController.Core.Models.Commands.Command");

                    b.Property<byte>("PinNumber");

                    b.Property<byte>("Value");

                    b.ToTable("AnalogWriteCommand");

                    b.HasDiscriminator().HasValue("AnalogWriteCommand");
                });

            modelBuilder.Entity("ArduinoController.Core.Models.Commands.DigitalWriteCommand", b =>
                {
                    b.HasBaseType("ArduinoController.Core.Models.Commands.Command");

                    b.Property<byte>("PinNumber")
                        .HasColumnName("DigitalWriteCommand_PinNumber");

                    b.Property<bool>("Value")
                        .HasColumnName("DigitalWriteCommand_Value");

                    b.ToTable("DigitalWriteCommand");

                    b.HasDiscriminator().HasValue("DigitalWriteCommand");
                });

            modelBuilder.Entity("ArduinoController.Core.Models.Commands.NegateCommand", b =>
                {
                    b.HasBaseType("ArduinoController.Core.Models.Commands.Command");

                    b.Property<byte>("PinNumber")
                        .HasColumnName("NegateCommand_PinNumber");

                    b.ToTable("NegateCommand");

                    b.HasDiscriminator().HasValue("NegateCommand");
                });

            modelBuilder.Entity("ArduinoController.Core.Models.Commands.WaitCommand", b =>
                {
                    b.HasBaseType("ArduinoController.Core.Models.Commands.Command");

                    b.Property<int>("Duration");

                    b.ToTable("WaitCommand");

                    b.HasDiscriminator().HasValue("WaitCommand");
                });

            modelBuilder.Entity("ArduinoController.Core.Models.ArduinoDevice", b =>
                {
                    b.HasOne("ArduinoController.DataAccess.ApplicationUser")
                        .WithMany("Devices")
                        .HasForeignKey("ApplicationUserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ArduinoController.Core.Models.Commands.Command", b =>
                {
                    b.HasOne("ArduinoController.Core.Models.Procedure")
                        .WithMany("Commands")
                        .HasForeignKey("ProcedureUserId", "ProcedureName");
                });

            modelBuilder.Entity("ArduinoController.Core.Models.Procedure", b =>
                {
                    b.HasOne("ArduinoController.Core.Models.ArduinoDevice", "Device")
                        .WithMany()
                        .HasForeignKey("DeviceMacAddress");

                    b.HasOne("ArduinoController.DataAccess.ApplicationUser")
                        .WithMany("Procedures")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("ArduinoController.DataAccess.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("ArduinoController.DataAccess.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ArduinoController.DataAccess.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("ArduinoController.DataAccess.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
