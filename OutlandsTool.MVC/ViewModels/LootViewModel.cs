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
    }
}
