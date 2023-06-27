﻿// <auto-generated />
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Domain.Model.Planet", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<double>("DiameterKilometers")
                        .HasColumnType("double precision");

                    b.Property<double>("DistanceFromSunKilometers")
                        .HasColumnType("double precision");

                    b.Property<double>("LengthOfDayHours")
                        .HasColumnType("double precision");

                    b.Property<double>("MassTonnes")
                        .HasColumnType("double precision");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PhotoUrl")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Planets");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            DiameterKilometers = 4879.0,
                            DistanceFromSunKilometers = 57910000.0,
                            LengthOfDayHours = 4222.6000000000004,
                            MassTonnes = 330.19999999999999,
                            Name = "Mercury",
                            PhotoUrl = "https://dummyimage.com/300x300/000000/ffffff"
                        },
                        new
                        {
                            Id = 2,
                            DiameterKilometers = 12104.0,
                            DistanceFromSunKilometers = 108200000.0,
                            LengthOfDayHours = 2802.0,
                            MassTonnes = 4868.5,
                            Name = "Venus",
                            PhotoUrl = "https://dummyimage.com/300x300/000000/ffffff"
                        },
                        new
                        {
                            Id = 3,
                            DiameterKilometers = 12742.0,
                            DistanceFromSunKilometers = 149600000.0,
                            LengthOfDayHours = 24.0,
                            MassTonnes = 5973.6000000000004,
                            Name = "Earth",
                            PhotoUrl = "https://dummyimage.com/300x300/000000/ffffff"
                        },
                        new
                        {
                            Id = 4,
                            DiameterKilometers = 6779.0,
                            DistanceFromSunKilometers = 227900000.0,
                            LengthOfDayHours = 24.699999999999999,
                            MassTonnes = 641.71000000000004,
                            Name = "Mars",
                            PhotoUrl = "https://dummyimage.com/300x300/000000/ffffff"
                        },
                        new
                        {
                            Id = 5,
                            DiameterKilometers = 139820.0,
                            DistanceFromSunKilometers = 778500000.0,
                            LengthOfDayHours = 9.9000000000000004,
                            MassTonnes = 1898600.0,
                            Name = "Jupiter",
                            PhotoUrl = "https://dummyimage.com/300x300/000000/ffffff"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
