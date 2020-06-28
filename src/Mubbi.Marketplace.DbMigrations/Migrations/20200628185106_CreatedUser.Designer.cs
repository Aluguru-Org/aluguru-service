﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mubbi.Marketplace.Data;

namespace Mubbi.Marketplace.Data.Migrations
{
    [DbContext(typeof(MubbiContext))]
    [Migration("20200628185106_CreatedUser")]
    partial class CreatedUser
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Mubbi.Marketplace.Catalog.Domain.Category", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Code")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DateUpdated")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(250)");

                    b.HasKey("Id");

                    b.ToTable("Category");
                });

            modelBuilder.Entity("Mubbi.Marketplace.Catalog.Domain.Product", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CategoryId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DateUpdated")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("varchar(1000)");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("varchar(250)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<TimeSpan?>("MaxRentTime")
                        .HasColumnType("time");

                    b.Property<TimeSpan>("MinRentTime")
                        .HasColumnType("time");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(250)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("RentType")
                        .HasColumnType("int");

                    b.Property<int>("StockQuantity")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("Product");
                });

            modelBuilder.Entity("Mubbi.Marketplace.Register.Domain.Models.User", b =>
                {
                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DateUpdated")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Password")
                        .HasColumnType("int");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.HasKey("Username");

                    b.ToTable("User");
                });

            modelBuilder.Entity("Mubbi.Marketplace.Catalog.Domain.Product", b =>
                {
                    b.HasOne("Mubbi.Marketplace.Catalog.Domain.Category", "Category")
                        .WithMany("Products")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Mubbi.Marketplace.Register.Domain.Models.User", b =>
                {
                    b.OwnsOne("Mubbi.Marketplace.Register.Domain.Address", "Address", b1 =>
                        {
                            b1.Property<string>("Username")
                                .HasColumnType("nvarchar(450)");

                            b1.Property<string>("City")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("Country")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("Neighborhood")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("Number")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("State")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("Street")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("ZipCode")
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("Username");

                            b1.ToTable("User");

                            b1.WithOwner()
                                .HasForeignKey("Username");
                        });

                    b.OwnsOne("Mubbi.Marketplace.Register.Domain.Document", "Document", b1 =>
                        {
                            b1.Property<string>("Username")
                                .HasColumnType("nvarchar(450)");

                            b1.Property<int>("DocumentType")
                                .HasColumnType("int");

                            b1.Property<string>("Number")
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("Username");

                            b1.ToTable("User");

                            b1.WithOwner()
                                .HasForeignKey("Username");
                        });

                    b.OwnsOne("Mubbi.Marketplace.Register.Domain.Email", "Email", b1 =>
                        {
                            b1.Property<string>("Username")
                                .HasColumnType("nvarchar(450)");

                            b1.Property<string>("Address")
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("Username");

                            b1.ToTable("User");

                            b1.WithOwner()
                                .HasForeignKey("Username");
                        });

                    b.OwnsOne("Mubbi.Marketplace.Register.Domain.Name", "Name", b1 =>
                        {
                            b1.Property<string>("Username")
                                .HasColumnType("nvarchar(450)");

                            b1.Property<string>("FirstName")
                                .HasColumnName("FirstName")
                                .HasColumnType("varchar(250)");

                            b1.Property<string>("FullName")
                                .HasColumnName("FullName")
                                .HasColumnType("varchar(1000)");

                            b1.Property<string>("LastName")
                                .HasColumnName("LastName")
                                .HasColumnType("varchar(250)");

                            b1.HasKey("Username");

                            b1.ToTable("User");

                            b1.WithOwner()
                                .HasForeignKey("Username");
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
