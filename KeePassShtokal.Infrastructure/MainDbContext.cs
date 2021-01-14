using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
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
        public DbSet<UsersEntries> UsersEntries { get; set; }

        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UsersEntries>(sharedEntry =>
            {
                sharedEntry.HasKey(se => new {se.EntryId, se.UserId});
                //sharedEntry.HasOne(se => se.User).WithMany(u => u.SharedEntriesForUser).OnDelete(DeleteBehavior.Cascade);
                //sharedEntry.HasOne(se => se.User)
                //    .WithMany(u => u.UserEntries);

                //sharedEntry.HasOne(se => se.Entry)
                //    .WithMany(e => e.EntryUsers);
            });

        }
    }
}
