﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace relation.Data.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20230705205014_Relation")]
    partial class Relation
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Product", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(125)
                        .HasColumnType("character varying(125)");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("ProductDetail", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Color")
                        .HasMaxLength(125)
                        .HasColumnType("character varying(125)");

                    b.Property<string>("CountryOfOrigin")
                        .HasMaxLength(125)
                        .HasColumnType("character varying(125)");

                    b.Property<string>("Description")
                        .HasMaxLength(125)
                        .HasColumnType("character varying(125)");

                    b.Property<DateTime>("ExpiryDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("ManufactureDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Manufacturer")
                        .HasMaxLength(125)
                        .HasColumnType("character varying(125)");

                    b.Property<string>("Material")
                        .HasMaxLength(125)
                        .HasColumnType("character varying(125)");

                    b.Property<Guid?>("ProductId")
                        .HasColumnType("uuid");

                    b.Property<int>("QuantityInStock")
                        .HasColumnType("integer");

                    b.Property<string>("Size")
                        .HasMaxLength(125)
                        .HasColumnType("character varying(125)");

                    b.Property<decimal>("Weight")
                        .HasColumnType("numeric");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("ProductDetails");
                });

            modelBuilder.Entity("ProductDetail", b =>
                {
                    b.HasOne("Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId");

                    b.Navigation("Product");
                });
#pragma warning restore 612, 618
        }
    }
}
