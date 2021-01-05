using System.Collections.Generic;
using System.Threading.Tasks;
using OutlandsTool.ServiceModel.Entities;
using OutlandsTool.ServiceModel.Business;
using OutlandsTool.Data.Repositories;
using OutlandsTool.ServiceModel.Messaging;
using System;

namespace OutlandsTool.Business.Managers
{
    public class LootManager : ILootManager
    {
        private readonly IUnitOfWork _unitOfWork;
        public LootManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        //public async Task<JsonResultMessage> AddNewLootItemAsync(LootItem lootitem)
        //{
        //    var result = new JsonResultMessage()
        //    {
        //        Success = true,
        //        Message = "Successfully added new restaurant."
        //    };

        //    try
        //    {
        //        restaurant = await _unitOfWork.Repository<Restaurant>().InsertAsync(restaurant);
        //        _unitOfWork.Commit();
        //    }
        //    catch (Exception ex)
        //    {
        //        result.Success = false;
        //        result.Message = "Error adding config. " + ex.Message;
        //    }
        //    return result;
        //}
        public async Task<LootItem> GetLootItemAsync(int Id)
        {
            return await _unitOfWork.Repository<LootItem>().GetByIdAsync(Id);
        }
        public LootSplit GetLootSplit(int? Id)
        {
            return _unitOfWork.Repository<LootSplit>().GetById(Id);
        }
        public IEnumerable<SplitItem>GetSplitItems(int? Id)
        {
            return _unitOfWork.Repository<SplitItem>().FindAll(x => x.LootSplitId == Id);
        }
        public async Task<IEnumerable<LootItem>> GetAllLootItemsAsync()
        {
            return await _unitOfWork.Repository<LootItem>().GetAllAsync();
        }
        public IEnumerable<Aspect> GetAllAspects()
        {
            return _unitOfWork.Repository<Aspect>().GetAll();
        }
     
        //public IEnumerable<SkillScroll> GetAllSkillScrolls()
        //{
        //    return _unitOfWork.Repository<SkillScroll>().GetAll();
        //}
        public IEnumerable<LootItem> GetAllLootItems()
        {
            return _unitOfWork.Repository<LootItem>().GetAll();
        }

        public async Task<LootItem> GetLootItemsAsync(int Id)
        {
            return await _unitOfWork.Repository<LootItem>().GetByIdAsync(Id);
        }
        public LootSplit InsertLootSplit(LootSplit lootSplit)
        {
            var result = new JsonResultMessage()
            {
                Success = true,
                Message = "Successfully added new loot split."
            };

            try
            {
                lootSplit = _unitOfWork.Repository<LootSplit>().Insert(lootSplit);
                _unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error adding loot split. " + ex.Message;
            }
            return lootSplit;
        }
        public SplitItem InsertOrUpdateSplitItem(SplitItem splitItem)
        {
            var result = new JsonResultMessage()
            {
                Success = true,
                Message = "Successfully processed split item"
            };

            try
            {
                splitItem = _unitOfWork.Repository<SplitItem>().InsertOrUpdate(splitItem);
                _unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error adding/updating split item. " + ex.Message;
            }
            return splitItem;
        }
        public JsonResultMessage InsertSplitItems(List<SplitItem> splitItems, int lootSplitId)
        {
            var result = new JsonResultMessage()
            {
                Success = true,
                Message = "Successfully added new loot split."
            };

            try
            {
                foreach(SplitItem item in splitItems)
                {
                    _unitOfWork.Repository<SplitItem>().Insert(item);
                }
                _unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error adding config. " + ex.Message;
            }
            return result;
        }
        public JsonResultMessage CreateLootSplit(LootSplit lootsplit)
        {
            var result = new JsonResultMessage()
            {
                Success = true,
                Message = "Successfully added new loot split."
            };

            try
            {
                lootsplit = _unitOfWork.Repository<LootSplit>().Insert(lootsplit);
                _unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error adding config. " + ex.Message;
            }
            return result;
        }
        public IEnumerable<LootSplit> GetLootSplitDropdownValues()
        {
            var lootsplits = _unitOfWork.Repository<LootSplit>().GetAll();

            return lootsplits;
        }
        public LootItem GetLootItemByName(string lootItemName)
        {
            LootItem lootItem = _unitOfWork.Repository<LootItem>().GetByName(lootItemName);
            return lootItem;
        }
    }
}