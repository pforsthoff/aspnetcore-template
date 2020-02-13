using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using OutlandsTool.ServiceModel.Entities;

namespace OutlandsTool.Data
{
    public class SQLiteDBContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseLazyLoadingProxies();
        }

        public DbSet<LootItem> LootItems { get; set; }
        public DbSet<LootSplit> LootSplit { get; set; }
        protected override void
        OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SplitItem>()
                .HasKey(x => new { x.LootSplitId, x.LootItemId });

            modelBuilder.Entity<SplitItem>()
             .HasOne(x => x.LootItem)
             .WithMany(y => y.SplitItems)
             .HasForeignKey(x => x.LootItemId);
            modelBuilder.Entity<SplitItem>()
            .HasOne(x => x.LootSplit)
            .WithMany(y => y.SplitItems)
            .HasForeignKey(x => x.LootSplitId);
        }
        public SQLiteDBContext(DbContextOptions<SQLiteDBContext> options)
        : base(options)
        { }

    }
}
