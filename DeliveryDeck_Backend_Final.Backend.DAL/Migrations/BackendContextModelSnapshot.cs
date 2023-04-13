﻿// <auto-generated />
using System;
using System.Collections.Generic;
using DeliveryDeck_Backend_Final.Backend.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DeliveryDeck_Backend_Final.Backend.DAL.Migrations
{
    [DbContext(typeof(BackendContext))]
    partial class BackendContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
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

                    b.Property<bool>("WasOrdered")
                        .HasColumnType("boolean");

                    b.HasKey("Id");

                    b.ToTable("Carts");
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
                        .HasColumnType("text");

                    b.Property<int>("Price")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Dishes");

                    b.HasData(
                        new
                        {
                            Id = new Guid("dc171fe8-98bf-4f1a-b597-15c619e4294b"),
                            Category = 3,
                            Description = "aaaaa",
                            IsVegeterian = false,
                            Name = "Fish w/ Qiwi",
                            Price = 50
                        });
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

                    b.Property<int?>("OrderId")
                        .HasColumnType("integer");

                    b.Property<int>("PriceWhenOrdered")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CartId");

                    b.HasIndex("DishId");

                    b.HasIndex("OrderId");

                    b.ToTable("DishesInCarts");
                });

            modelBuilder.Entity("DeliveryDeck_Backend_Final.Backend.DAL.Entities.Menu", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("RestaurantId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("RestaurantId");

                    b.ToTable("Menus");
                });

            modelBuilder.Entity("DeliveryDeck_Backend_Final.Backend.DAL.Entities.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid?>("Cook")
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

            modelBuilder.Entity("DeliveryDeck_Backend_Final.Backend.DAL.Entities.Restaurant", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<List<Guid>>("Cooks")
                        .IsRequired()
                        .HasColumnType("uuid[]");

                    b.Property<List<Guid>>("Managers")
                        .IsRequired()
                        .HasColumnType("uuid[]");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Restaurants");

                    b.HasData(
                        new
                        {
                            Id = new Guid("2dc11394-c05e-4e7c-86ed-b8a80e90f3fe"),
                            Cooks = new List<Guid>(),
                            Managers = new List<Guid>(),
                            Name = "New Amogus"
                        },
                        new
                        {
                            Id = new Guid("075674f4-5b32-4a5c-89a6-d1d1fe3fc23b"),
                            Cooks = new List<Guid>(),
                            Managers = new List<Guid>(),
                            Name = "Old Amogus"
                        },
                        new
                        {
                            Id = new Guid("5cee7be9-b2bb-4556-8d60-ca01318208ab"),
                            Cooks = new List<Guid>(),
                            Managers = new List<Guid>(),
                            Name = "FeastingHub"
                        });
                });

            modelBuilder.Entity("DishMenu", b =>
                {
                    b.Property<Guid>("DishesId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("MenuId")
                        .HasColumnType("uuid");

                    b.HasKey("DishesId", "MenuId");

                    b.HasIndex("MenuId");

                    b.ToTable("DishMenu");
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

                    b.HasOne("DeliveryDeck_Backend_Final.Backend.DAL.Entities.Order", null)
                        .WithMany("Dishes")
                        .HasForeignKey("OrderId");

                    b.Navigation("Cart");

                    b.Navigation("Dish");
                });

            modelBuilder.Entity("DeliveryDeck_Backend_Final.Backend.DAL.Entities.Menu", b =>
                {
                    b.HasOne("DeliveryDeck_Backend_Final.Backend.DAL.Entities.Restaurant", "Restaurant")
                        .WithMany("Menus")
                        .HasForeignKey("RestaurantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Restaurant");
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

            modelBuilder.Entity("DishMenu", b =>
                {
                    b.HasOne("DeliveryDeck_Backend_Final.Backend.DAL.Entities.Dish", null)
                        .WithMany()
                        .HasForeignKey("DishesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DeliveryDeck_Backend_Final.Backend.DAL.Entities.Menu", null)
                        .WithMany()
                        .HasForeignKey("MenuId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DeliveryDeck_Backend_Final.Backend.DAL.Entities.Cart", b =>
                {
                    b.Navigation("Dishes");
                });

            modelBuilder.Entity("DeliveryDeck_Backend_Final.Backend.DAL.Entities.Dish", b =>
                {
                    b.Navigation("Ratings");
                });

            modelBuilder.Entity("DeliveryDeck_Backend_Final.Backend.DAL.Entities.Order", b =>
                {
                    b.Navigation("Dishes");
                });

            modelBuilder.Entity("DeliveryDeck_Backend_Final.Backend.DAL.Entities.Restaurant", b =>
                {
                    b.Navigation("Menus");
                });
#pragma warning restore 612, 618
        }
    }
}
