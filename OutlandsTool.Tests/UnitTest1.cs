using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OutlandsTool.Data;
using OutlandsTool.ServiceModel.Entities;
using System.Collections.Generic;
using Xunit;

namespace OutlandsTool.Tests
{
    public class OutlandsToolTests
    {
        [Fact]
        public void Add_writes_to_database()
        {
            // In-memory database only exists while the connection is open
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            try
            {
                var options = new DbContextOptionsBuilder<SQLiteDBContext>()
                    .UseSqlite(connection)
                    .Options;

                // Create the schema in the database
                using (var context = new SQLiteDBContext(options))
                {
                    context.Database.EnsureCreated();
                }
                using (var context = new SQLiteDBContext(options))
                {
                    LootSplit lootsplit = new LootSplit
                    {
                        Name = "Test Loot Split",
                        LootItemId = 1,
                        LootItems = new List<LootItem>
                        {
                            new LootItem
                            {
                                Name = "test loot item",
                                Price = 5,
                                Description = "test"
                            },
                            new LootItem
                            {
                                Name = "test loot item 2",
                                Price = 10,
                                Description = "test2"
                            }
                        }
                    };
                    LootItem lootitem = new LootItem
                    {
                        Name = "Test Loot Item",
                        Price = 20,
                        LootSplits = new List<LootSplit>
                        {
                            new LootSplit
                            {
                                Name = "Loot Split Test 3",
                                LootItemId = 2,
                            },
                            new LootSplit
                            {
                                Name = "Loot Split 4",
                                LootItemId = 3
                            }
                        }
                    };
                    context.Add(lootitem);
                    context.Add(lootsplit);
                    context.SaveChanges();
                }

                using (var context = new SQLiteDBContext(options))
                {
                   
                    Assert.Equals(2, context.LootSplit.CountAsync());
                }
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
