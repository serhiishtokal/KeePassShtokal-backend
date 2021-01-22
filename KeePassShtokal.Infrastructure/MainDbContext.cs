using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using KeePassShtokal.Infrastructure.DefaultData;
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
        public DbSet<EntryAction> EntryActions { get; set; }
        public DbSet<EntryState> EntryStates { get; set; }

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

            modelBuilder.Entity<EntryAction>(ea =>
            {
                ea.HasOne(x => x.EntryState).WithMany().OnDelete(DeleteBehavior.ClientSetNull);
                ea.HasOne(x => x.Entry).WithMany().OnDelete(DeleteBehavior.ClientSetNull);
                ea.HasOne(x => x.User).WithMany().OnDelete(DeleteBehavior.ClientSetNull);

                ea.Property(e => e.ActionType).HasConversion<string>();
            });

            modelBuilder.Entity<Entry>(ea =>
            {
                ea.HasOne(x => x.CurrentEntryState).WithOne().OnDelete(DeleteBehavior.ClientSetNull);
            });
        }
    }
}
