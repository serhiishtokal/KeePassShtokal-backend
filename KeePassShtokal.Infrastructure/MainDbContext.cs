using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using KeePassShtokal.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace KeePassShtokal.Infrastructure
{
    public class MainDbContext:DbContext
    {
        public MainDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Entry> Entries { get; set; }
        public DbSet<SharedEntry> SharedEntries { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(user =>
            {
                user.HasIndex(t => t.Login).IsUnique();

                user.HasMany(u => u.Entries)
                    .WithOne(e => e.UserOwner)
                    .HasForeignKey(e=>e.UserOwnerId);

                user.HasMany(u => u.SharedEntriesForUser)
                    .WithOne(se => se.User)
                    .HasForeignKey(se=>se.UserId);
            });

            modelBuilder.Entity<Entry>(entry =>
            {
                entry.HasOne(e => e.UserOwner)
                    .WithMany(u => u.Entries)
                    .HasForeignKey(e=>e.UserOwnerId);
                //entry.HasOne(e => e.UserOwner).WithMany();
                entry.HasMany(e => e.SharedFor)
                    .WithOne(se => se.Entry)
                    .HasForeignKey(se=>se.EntryId);
            });

            modelBuilder.Entity<SharedEntry>(sharedEntry =>
            {
                sharedEntry.HasKey(se => new {se.EntryId, se.UserId});
                //sharedEntry.HasOne(se => se.User).WithMany(u => u.SharedEntriesForUser).OnDelete(DeleteBehavior.Cascade);
                sharedEntry.HasOne(se => se.User)
                    .WithMany(u=>u.SharedEntriesForUser)
                    .HasForeignKey(se=>se.UserId);

                sharedEntry.HasOne(se => se.Entry)
                    .WithMany(e => e.SharedFor)
                    .HasForeignKey(se => se.EntryId);


            });

        }
    }
}
