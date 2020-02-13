using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OutlandsTool.ServiceModel.Entities.Base;

namespace OutlandsTool.ServiceModel.Entities
{
    public class SplitItem 
    {
        public int LootSplitId { get; set; }
        public int LootItemId { get; set; }
        public int LootItemQuantity { get; set; }
        public virtual LootSplit LootSplit { get; set; }
        public virtual LootItem LootItem { get; set; }
    }
}
