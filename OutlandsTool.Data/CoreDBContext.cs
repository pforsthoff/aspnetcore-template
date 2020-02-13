using System;
using System.Collections.Generic;
using System.Text;
using OutlandsTool.ServiceModel.Entities;
using Microsoft.EntityFrameworkCore;
namespace OutlandsTool.Data
{
    public class CoreDbContext : DbContext
    {
        public DbSet<LootItem> LootItems { get; set; }
        public CoreDbContext()
        {

        }
        public CoreDbContext(DbContextOptions<CoreDbContext> options)
    : base(options)
        { }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

    }
}
