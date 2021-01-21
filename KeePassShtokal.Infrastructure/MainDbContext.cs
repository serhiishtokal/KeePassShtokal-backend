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
        public DbSet<IpAddress> IdAddresses { get; set; }
        public DbSet<LoginAttempt> LoginAttempts { get; set; }
        public DbSet<UserIpAddress> UserIpAddresses { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UsersEntries>(sharedEntry =>
            {
                sharedEntry.HasKey(se => new {se.EntryId, se.UserId});
            });
            modelBuilder.Entity<UserIpAddress>(userIp =>
            {
                userIp.HasKey(ui => new { ui.IpAddressId, ui.UserId });
            });
        }
    }
}
