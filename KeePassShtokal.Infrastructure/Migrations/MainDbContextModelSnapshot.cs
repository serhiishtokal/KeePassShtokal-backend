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

                    b.Property<int>("CurrentEntryStateId")
                        .HasColumnType("int");

                    b.Property<string>("UserOwnerUsername")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("EntryId");

                    b.HasIndex("CurrentEntryStateId")
                        .IsUnique();

                    b.ToTable("Entries");
                });

            modelBuilder.Entity("KeePassShtokal.Infrastructure.Entities.EntryAction", b =>
                {
                    b.Property<int>("ActionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("ActionType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("EntryId")
                        .HasColumnType("int");

                    b.Property<int>("EntryStateId")
                        .HasColumnType("int");

                    b.Property<bool>("IsRestorable")
                        .HasColumnType("bit");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("ActionId");

                    b.HasIndex("EntryId");

                    b.HasIndex("EntryStateId");

                    b.HasIndex("UserId");

                    b.ToTable("EntryActions");
                });

            modelBuilder.Entity("KeePassShtokal.Infrastructure.Entities.EntryState", b =>
                {
                    b.Property<int>("EntryStateId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("PasswordE")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("WebAddress")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("EntryStateId");

                    b.ToTable("EntryStates");
                });

            modelBuilder.Entity("KeePassShtokal.Infrastructure.Entities.IpAddress", b =>
                {
                    b.Property<int>("AddressId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

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

                    b.Property<int>("IpAddressId")
                        .HasColumnType("int");

                    b.Property<bool>("IsSuccessful")
                        .HasColumnType("bit");

                    b.Property<int?>("UserId")
                        .IsRequired()
                        .HasColumnType("int");

                    b.HasKey("LoginTrialId");

                    b.HasIndex("IpAddressId");

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

            modelBuilder.Entity("KeePassShtokal.Infrastructure.Entities.UserIpAddress", b =>
                {
                    b.Property<int>("IpAddressId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("BlockedTo")
                        .HasColumnType("datetime2");

                    b.Property<int>("IncorrectLoginCount")
                        .HasColumnType("int");

                    b.HasKey("IpAddressId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("UserIpAddresses");
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

            modelBuilder.Entity("KeePassShtokal.Infrastructure.Entities.Entry", b =>
                {
                    b.HasOne("KeePassShtokal.Infrastructure.Entities.EntryState", "CurrentEntryState")
                        .WithOne()
                        .HasForeignKey("KeePassShtokal.Infrastructure.Entities.Entry", "CurrentEntryStateId")
                        .IsRequired();

                    b.Navigation("CurrentEntryState");
                });

            modelBuilder.Entity("KeePassShtokal.Infrastructure.Entities.EntryAction", b =>
                {
                    b.HasOne("KeePassShtokal.Infrastructure.Entities.Entry", "Entry")
                        .WithMany()
                        .HasForeignKey("EntryId")
                        .IsRequired();

                    b.HasOne("KeePassShtokal.Infrastructure.Entities.EntryState", "EntryState")
                        .WithMany()
                        .HasForeignKey("EntryStateId")
                        .IsRequired();

                    b.HasOne("KeePassShtokal.Infrastructure.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .IsRequired();

                    b.Navigation("Entry");

                    b.Navigation("EntryState");

                    b.Navigation("User");
                });

            modelBuilder.Entity("KeePassShtokal.Infrastructure.Entities.LoginAttempt", b =>
                {
                    b.HasOne("KeePassShtokal.Infrastructure.Entities.IpAddress", "IpAddress")
                        .WithMany()
                        .HasForeignKey("IpAddressId")
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

            modelBuilder.Entity("KeePassShtokal.Infrastructure.Entities.UserIpAddress", b =>
                {
                    b.HasOne("KeePassShtokal.Infrastructure.Entities.IpAddress", "IpAddress")
                        .WithMany()
                        .HasForeignKey("IpAddressId")
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
                        .WithMany()
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

            modelBuilder.Entity("KeePassShtokal.Infrastructure.Entities.User", b =>
                {
                    b.Navigation("UserEntries");
                });
#pragma warning restore 612, 618
        }
    }
}
