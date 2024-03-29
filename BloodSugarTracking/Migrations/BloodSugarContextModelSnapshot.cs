﻿// <auto-generated />
using BloodSugarTracking.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BloodSugarTracking.Migrations;

[DbContext(typeof(BloodSugarContext))]
partial class BloodSugarContextModelSnapshot : ModelSnapshot
{
    protected override void BuildModel(ModelBuilder modelBuilder)
    {
#pragma warning disable 612, 618
        modelBuilder
            .HasAnnotation("Relational:MaxIdentifierLength", 128)
            .HasAnnotation("ProductVersion", "5.0.5")
            .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

        modelBuilder.Entity("BloodSugarTracking.Models.BloodSugarTestResult", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int")
                    .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                b.Property<DateTime>("MealTime")
                    .HasColumnType("datetime2");

                b.Property<double>("Result")
                    .HasColumnType("float");

                b.Property<DateTime>("TestTime")
                    .HasColumnType("datetime2");

                b.Property<int?>("UserId")
                    .HasColumnType("int");

                b.HasKey("Id");

                b.HasIndex("UserId");

                b.ToTable("BloodSugarTestResults");
            });

        modelBuilder.Entity("BloodSugarTracking.Models.User", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int")
                    .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                b.Property<string>("FirstName")
                    .IsRequired()
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("LastName")
                    .IsRequired()
                    .HasColumnType("nvarchar(max)");

                b.HasKey("Id");

                b.ToTable("Users");
            });

        modelBuilder.Entity("BloodSugarTracking.Models.BloodSugarTestResult", b =>
            {
                b.HasOne("BloodSugarTracking.Models.User", "User")
                    .WithMany("BloodSugarTestResults")
                    .HasForeignKey("UserId");

                b.Navigation("User");
            });

        modelBuilder.Entity("BloodSugarTracking.Models.User", b =>
            {
                b.Navigation("BloodSugarTestResults");
            });
#pragma warning restore 612, 618
    }
}
