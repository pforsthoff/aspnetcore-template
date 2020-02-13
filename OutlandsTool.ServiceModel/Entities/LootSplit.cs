using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using OutlandsTool.ServiceModel.Entities.Base;

namespace OutlandsTool.ServiceModel.Entities
{
    public class LootSplit 
    {
        public int LootSplitId { get; set; }
        public string Name { get; set; }
        public virtual ICollection<SplitItem> SplitItems { get; set; }
    }
}
