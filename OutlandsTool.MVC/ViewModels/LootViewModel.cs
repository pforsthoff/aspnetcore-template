using Microsoft.AspNetCore.Mvc.Rendering;
using OutlandsTool.ServiceModel.Entities;
using System.Collections.Generic;
using System.ComponentModel;

namespace OutlandsTool.MVC.ViewModels
{
    public class LootViewModel
    {
        public List<LootItem> LootItems { get; set; }
        public IEnumerable<SelectListItem> LootSplitSelectList { get; set; }
        public int SelectedLootSplitId { get; set; }
        [DisplayName("Loot Split")]
        public string Loot_Split { get; set; }
        public string Name { get; set; }
        public int Id { get; set; }
        public bool resultSuccess { get; set; }
        public string resultMessage { get; set; }
        [DisplayName("Actions")]
        public string Actions { get; set; }

        public int LootItemId { get; set; }
        public string Type { get; set; }
        public int Price { get; set; }
        public string ImagePath { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public int Order { get; set; }
        public virtual ICollection<SplitItem> SplitItems { get; set; }
    }
}
