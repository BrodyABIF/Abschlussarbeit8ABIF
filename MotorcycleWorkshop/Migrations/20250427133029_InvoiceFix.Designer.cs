﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MotorcycleWorkshop.Infrastructure;

#nullable disable

namespace MotorcycleWorkshop.Migrations
{
    [DbContext(typeof(WorkshopDBContext))]
    [Migration("20250427133029_InvoiceFix")]
    partial class InvoiceFix
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.1")
                .HasAnnotation("Proxies:ChangeTracking", false)
                .HasAnnotation("Proxies:CheckEquality", false)
                .HasAnnotation("Proxies:LazyLoading", true);

            modelBuilder.Entity("MotorcycleWorkshop.model.Invoice", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("CustomerId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int?>("RepairId")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("TotalPrice")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("RepairId");

                    b.ToTable("Invoices");
                });

            modelBuilder.Entity("MotorcycleWorkshop.model.Motorcycle", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("AlternateId")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Mileage")
                        .HasColumnType("TEXT");

                    b.Property<string>("Model")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("TEXT");

                    b.Property<int>("OwnerId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Year")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasAlternateKey("AlternateId");

                    b.HasIndex("OwnerId");

                    b.ToTable("Motorcycles");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            AlternateId = new Guid("24f2ce5e-9832-41ca-8f4d-6b214cc46916"),
                            Mileage = 5000.00m,
                            Model = "Honda CBR600RR",
                            OwnerId = 1,
                            Year = 2020
                        });
                });

            modelBuilder.Entity("MotorcycleWorkshop.model.Part", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("AlternateId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Price")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasAlternateKey("AlternateId");

                    b.ToTable("Parts");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            AlternateId = new Guid("6fc80d59-4ac6-4872-bfbb-536d975c606f"),
                            Name = "Oil Filter",
                            Price = 20.99m
                        },
                        new
                        {
                            Id = 2,
                            AlternateId = new Guid("0233245d-85cc-480b-aee1-ac6a330aadb8"),
                            Name = "Brake Pads",
                            Price = 45.50m
                        });
                });

            modelBuilder.Entity("MotorcycleWorkshop.model.Person", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("AlternateId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("PersonType")
                        .IsRequired()
                        .HasMaxLength(8)
                        .HasColumnType("TEXT");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasAlternateKey("AlternateId");

                    b.ToTable("Person");

                    b.HasDiscriminator<string>("PersonType").HasValue("Person");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("MotorcycleWorkshop.model.Repair", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("AlternateId")
                        .HasColumnType("TEXT");

                    b.Property<int>("CustomerId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("MechanicId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("RepairDate")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasAlternateKey("AlternateId");

                    b.HasIndex("CustomerId");

                    b.HasIndex("MechanicId");

                    b.ToTable("Repairs");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            AlternateId = new Guid("ba994b95-8cde-4da2-9198-9df8e3dca064"),
                            CustomerId = 1,
                            MechanicId = 2,
                            RepairDate = new DateTime(2023, 12, 15, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        });
                });

            modelBuilder.Entity("RepairParts", b =>
                {
                    b.Property<int>("PartId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("RepairId")
                        .HasColumnType("INTEGER");

                    b.HasKey("PartId", "RepairId");

                    b.HasIndex("RepairId");

                    b.ToTable("RepairParts");

                    b.HasData(
                        new
                        {
                            PartId = 1,
                            RepairId = 1
                        },
                        new
                        {
                            PartId = 2,
                            RepairId = 1
                        });
                });

            modelBuilder.Entity("MotorcycleWorkshop.model.Customer", b =>
                {
                    b.HasBaseType("MotorcycleWorkshop.model.Person");

                    b.HasDiscriminator().HasValue("Customer");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            AlternateId = new Guid("22b6b861-5520-479e-afb4-d4fe3b7a78fe"),
                            Email = "customer@mail.at",
                            Name = "Customer Horst",
                            PhoneNumber = "012345"
                        });
                });

            modelBuilder.Entity("MotorcycleWorkshop.model.Mechanic", b =>
                {
                    b.HasBaseType("MotorcycleWorkshop.model.Person");

                    b.Property<string>("Certification")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("TEXT");

                    b.Property<decimal>("HourlyRate")
                        .HasColumnType("TEXT");

                    b.HasDiscriminator().HasValue("Mechanic");

                    b.HasData(
                        new
                        {
                            Id = 2,
                            AlternateId = new Guid("eebe378d-83ca-4524-98be-64d5bdc524c5"),
                            Email = "jane.smith@example.com",
                            Name = "Jane Smith",
                            PhoneNumber = "012345",
                            Certification = "Certified Mechanic",
                            HourlyRate = 50.0m
                        });
                });

            modelBuilder.Entity("MotorcycleWorkshop.model.Invoice", b =>
                {
                    b.HasOne("MotorcycleWorkshop.model.Repair", "Repair")
                        .WithMany()
                        .HasForeignKey("RepairId");

                    b.Navigation("Repair");
                });

            modelBuilder.Entity("MotorcycleWorkshop.model.Motorcycle", b =>
                {
                    b.HasOne("MotorcycleWorkshop.model.Customer", "Owner")
                        .WithMany("Motorcycles")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("MotorcycleWorkshop.model.Repair", b =>
                {
                    b.HasOne("MotorcycleWorkshop.model.Customer", "Customer")
                        .WithMany("Repairs")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MotorcycleWorkshop.model.Mechanic", "Mechanic")
                        .WithMany("Repairs")
                        .HasForeignKey("MechanicId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");

                    b.Navigation("Mechanic");
                });

            modelBuilder.Entity("RepairParts", b =>
                {
                    b.HasOne("MotorcycleWorkshop.model.Part", null)
                        .WithMany()
                        .HasForeignKey("PartId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MotorcycleWorkshop.model.Repair", null)
                        .WithMany()
                        .HasForeignKey("RepairId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MotorcycleWorkshop.model.Customer", b =>
                {
                    b.OwnsOne("MotorcycleWorkshop.model.Address", "Address", b1 =>
                        {
                            b1.Property<int>("CustomerId")
                                .HasColumnType("INTEGER");

                            b1.Property<string>("City")
                                .IsRequired()
                                .HasMaxLength(20)
                                .HasColumnType("TEXT")
                                .HasColumnName("CustomerCity");

                            b1.Property<string>("PostalCode")
                                .IsRequired()
                                .HasMaxLength(20)
                                .HasColumnType("TEXT")
                                .HasColumnName("CustomerPostalCode");

                            b1.Property<string>("Street")
                                .IsRequired()
                                .HasMaxLength(20)
                                .HasColumnType("TEXT")
                                .HasColumnName("CustomerStreet");

                            b1.HasKey("CustomerId");

                            b1.ToTable("Person");

                            b1.WithOwner()
                                .HasForeignKey("CustomerId");

                            b1.HasData(
                                new
                                {
                                    CustomerId = 1,
                                    City = "Vienna",
                                    PostalCode = "1010",
                                    Street = "Customerstrasse 1"
                                });
                        });

                    b.Navigation("Address")
                        .IsRequired();
                });

            modelBuilder.Entity("MotorcycleWorkshop.model.Mechanic", b =>
                {
                    b.OwnsOne("MotorcycleWorkshop.model.Address", "Address", b1 =>
                        {
                            b1.Property<int>("MechanicId")
                                .HasColumnType("INTEGER");

                            b1.Property<string>("City")
                                .IsRequired()
                                .HasMaxLength(20)
                                .HasColumnType("TEXT")
                                .HasColumnName("MechanicCity");

                            b1.Property<string>("PostalCode")
                                .IsRequired()
                                .HasMaxLength(20)
                                .HasColumnType("TEXT")
                                .HasColumnName("MechanicPostalCode");

                            b1.Property<string>("Street")
                                .IsRequired()
                                .HasMaxLength(20)
                                .HasColumnType("TEXT")
                                .HasColumnName("MechanicStreet");

                            b1.HasKey("MechanicId");

                            b1.ToTable("Person");

                            b1.WithOwner()
                                .HasForeignKey("MechanicId");

                            b1.HasData(
                                new
                                {
                                    MechanicId = 2,
                                    City = "Vienna",
                                    PostalCode = "1020",
                                    Street = "Repair St. 456"
                                });
                        });

                    b.Navigation("Address")
                        .IsRequired();
                });

            modelBuilder.Entity("MotorcycleWorkshop.model.Customer", b =>
                {
                    b.Navigation("Motorcycles");

                    b.Navigation("Repairs");
                });

            modelBuilder.Entity("MotorcycleWorkshop.model.Mechanic", b =>
                {
                    b.Navigation("Repairs");
                });
#pragma warning restore 612, 618
        }
    }
}
