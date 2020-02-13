using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OutlandsTool.ServiceModel.Entities.Base;

namespace OutlandsTool.ServiceModel.Entities
{
    public class LootItem 
    {
        public int LootItemId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int Price { get; set; }
        public string ImagePath { get; set; }
        public string Description { get; set; }
        [NotMapped]
        public int Quantity { get; set; }
        public int Order { get; set; }
        public virtual ICollection<SplitItem> SplitItems { get; set; }
    }
}
