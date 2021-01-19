﻿// <auto-generated />
using System;
using KeePassShtokal.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace KeePassShtokal.Infrastructure.Migrations
{
    [DbContext(typeof(MainDbContext))]
    partial class MainDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.1");

            modelBuilder.Entity("KeePassShtokal.Infrastructure.Entities.Entry", b =>
                {
                    b.Property<int>("EntryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordE")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserOwnerUsername")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("WebAddress")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("EntryId");

                    b.ToTable("Entries");
                });

            modelBuilder.Entity("KeePassShtokal.Infrastructure.Entities.IpAddress", b =>
                {
                    b.Property<int>("AddressId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<DateTime?>("BlockedTo")
                        .HasColumnType("datetime2");

                    b.Property<int>("IncorrectLoginCount")
                        .HasColumnType("int");

                    b.Property<string>("IpAddressString")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("AddressId");

                    b.ToTable("IdAddresses");
                });

            modelBuilder.Entity("KeePassShtokal.Infrastructure.Entities.LoginAttempt", b =>
                {
                    b.Property<int>("LoginTrialId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("IpId")
                        .HasColumnType("int");

                    b.Property<bool>("IsSuccessful")
                        .HasColumnType("bit");

                    b.Property<int?>("UserId")
                        .IsRequired()
                        .HasColumnType("int");

                    b.HasKey("LoginTrialId");

                    b.HasIndex("IpId");

                    b.HasIndex("UserId");

                    b.ToTable("LoginAttempts");
                });

            modelBuilder.Entity("KeePassShtokal.Infrastructure.Entities.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<DateTime?>("BlockedTo")
                        .HasColumnType("datetime2");

                    b.Property<int>("IncorrectLoginCount")
                        .HasColumnType("int");

                    b.Property<bool>("IsPasswordKeptAsSha")
                        .HasColumnType("bit");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Salt")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("KeePassShtokal.Infrastructure.Entities.UsersEntries", b =>
                {
                    b.Property<int>("EntryId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreationDateTime")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsUserOwner")
                        .HasColumnType("bit");

                    b.HasKey("EntryId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("UsersEntries");
                });

            modelBuilder.Entity("KeePassShtokal.Infrastructure.Entities.LoginAttempt", b =>
                {
                    b.HasOne("KeePassShtokal.Infrastructure.Entities.IpAddress", "IpAddress")
                        .WithMany()
                        .HasForeignKey("IpId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("KeePassShtokal.Infrastructure.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("IpAddress");

                    b.Navigation("User");
                });

            modelBuilder.Entity("KeePassShtokal.Infrastructure.Entities.UsersEntries", b =>
                {
                    b.HasOne("KeePassShtokal.Infrastructure.Entities.Entry", "Entry")
                        .WithMany("EntryUsers")
                        .HasForeignKey("EntryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("KeePassShtokal.Infrastructure.Entities.User", "User")
                        .WithMany("UserEntries")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Entry");

                    b.Navigation("User");
                });

            modelBuilder.Entity("KeePassShtokal.Infrastructure.Entities.Entry", b =>
                {
                    b.Navigation("EntryUsers");
                });

            modelBuilder.Entity("KeePassShtokal.Infrastructure.Entities.User", b =>
                {
                    b.Navigation("UserEntries");
                });
#pragma warning restore 612, 618
        }
    }
}
