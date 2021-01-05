using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OutlandsTool.ServiceModel.Entities;


namespace OutlandsTool.Data.Mappings
{
    internal class LootMap : IEntityTypeConfiguration<LootItem>
    {
        public void Configure(EntityTypeBuilder<LootItem> entity)
        {
            entity.ToTable("LootItems");
            //primary key
            entity.HasKey(c => c.LootItemId);
            entity.Property(c => c.Name).HasMaxLength(255).IsRequired();

        }
    }
}
