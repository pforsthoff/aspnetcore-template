using OutlandsTool.ServiceModel.Entities;
using OutlandsTool.ServiceModel.Messaging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OutlandsTool.ServiceModel.Business
{
    public interface ILootManager
    {
        Task<LootItem> GetLootItemAsync(int Id);
        IEnumerable<LootItem> GetAllLootItems();
        Task<IEnumerable<LootItem>> GetAllLootItemsAsync();
        IEnumerable<Aspect> GetAllAspects();
        //IEnumerable<SkillScroll> GetAllSkillScrolls();

        //JsonResultMessage InsertLootSplit(LootSplit_old lootsplit);
        IEnumerable<LootSplit> GetLootSplitDropdownValues();
        LootSplit GetLootSplit(int? Id);
        JsonResultMessage InsertSplitItems(List<SplitItem> splitItems, int lootSplitId);
        LootItem GetLootItemByName(string lootItemName);
        LootSplit InsertLootSplit(LootSplit lootsplit);
        IEnumerable<SplitItem> GetSplitItems(int? Id);
        SplitItem InsertOrUpdateSplitItem(SplitItem splitItem);
        //LootSplit GetLootSplit(string lootSplitName);
        //Task<JsonResultMessage> AddNewRestaurantAsync(Restaurant restaurant);
        //Task<Restaurant> UpdateAsync(Restaurant restaurantChanges);
        //Task<JsonResultMessage> DeleteAsync(int id);
        //IEnumerable<Restaurant> GetRestaurantsbyCuisineType(int cuisineNumber);
        //JsonResultMessage UpdateRestaurant(Restaurant restaurant);
    }
}
