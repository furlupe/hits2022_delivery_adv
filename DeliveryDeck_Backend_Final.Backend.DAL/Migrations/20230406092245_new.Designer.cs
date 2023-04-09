﻿// <auto-generated />
using System;
using DeliveryDeck_Backend_Final.Backend.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DeliveryDeck_Backend_Final.Backend.DAL.Migrations
{
    [DbContext(typeof(BackendContext))]
    [Migration("20230406092245_new")]
    partial class @new
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("DeliveryDeck_Backend_Final.Backend.DAL.Entities.Cart", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("OrderId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("OrderId");

                    b.ToTable("Carts");
                });

            modelBuilder.Entity("DeliveryDeck_Backend_Final.Backend.DAL.Entities.Cook", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("Cooks");
                });

            modelBuilder.Entity("DeliveryDeck_Backend_Final.Backend.DAL.Entities.Dish", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("Category")
                        .HasColumnType("integer");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsVegeterian")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Photo")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Price")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Dishes");
                });

            modelBuilder.Entity("DeliveryDeck_Backend_Final.Backend.DAL.Entities.DishInCart", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("Amount")
                        .HasColumnType("integer");

                    b.Property<Guid>("CartId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("DishId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("CartId");

                    b.HasIndex("DishId");

                    b.ToTable("DishesInCarts");
                });

            modelBuilder.Entity("DeliveryDeck_Backend_Final.Backend.DAL.Entities.Order", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("CookId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("CourierId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("DeliveryTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("OrderTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Price")
                        .HasColumnType("integer");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CookId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("DeliveryDeck_Backend_Final.Backend.DAL.Entities.Rating", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("AuthorId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("DishId")
                        .HasColumnType("uuid");

                    b.Property<int>("Value")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("DishId");

                    b.ToTable("Ratings");
                });

            modelBuilder.Entity("DeliveryDeck_Backend_Final.Backend.DAL.Entities.Cart", b =>
                {
                    b.HasOne("DeliveryDeck_Backend_Final.Backend.DAL.Entities.Order", "Order")
                        .WithMany()
                        .HasForeignKey("OrderId");

                    b.Navigation("Order");
                });

            modelBuilder.Entity("DeliveryDeck_Backend_Final.Backend.DAL.Entities.DishInCart", b =>
                {
                    b.HasOne("DeliveryDeck_Backend_Final.Backend.DAL.Entities.Cart", "Cart")
                        .WithMany("Dishes")
                        .HasForeignKey("CartId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DeliveryDeck_Backend_Final.Backend.DAL.Entities.Dish", "Dish")
                        .WithMany()
                        .HasForeignKey("DishId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cart");

                    b.Navigation("Dish");
                });

            modelBuilder.Entity("DeliveryDeck_Backend_Final.Backend.DAL.Entities.Order", b =>
                {
                    b.HasOne("DeliveryDeck_Backend_Final.Backend.DAL.Entities.Cook", "Cook")
                        .WithMany()
                        .HasForeignKey("CookId");

                    b.Navigation("Cook");
                });

            modelBuilder.Entity("DeliveryDeck_Backend_Final.Backend.DAL.Entities.Rating", b =>
                {
                    b.HasOne("DeliveryDeck_Backend_Final.Backend.DAL.Entities.Dish", "Dish")
                        .WithMany("Ratings")
                        .HasForeignKey("DishId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Dish");
                });

            modelBuilder.Entity("DeliveryDeck_Backend_Final.Backend.DAL.Entities.Cart", b =>
                {
                    b.Navigation("Dishes");
                });

            modelBuilder.Entity("DeliveryDeck_Backend_Final.Backend.DAL.Entities.Dish", b =>
                {
                    b.Navigation("Ratings");
                });
#pragma warning restore 612, 618
        }
    }
}