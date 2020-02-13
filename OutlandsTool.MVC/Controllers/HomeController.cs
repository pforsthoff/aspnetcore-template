using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OutlandsTool.MVC.Models;
using OutlandsTool.ServiceModel.Business;
using OutlandsTool.ServiceModel.Entities;
using OutlandsTool.ServiceModel.Messaging;
using AutoMapper;
using OutlandsTool.MVC.ViewModels;
using System;
using OutlandsTool.Common.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Collections.Generic;

namespace OutlandsTool.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ILootManager _lootManager;
        public HomeController(IMapper mapper, ILootManager lootManager)
        {
            _mapper = mapper;
            _lootManager = lootManager;
        }
        public ActionResult Index()
        {
            var model = new LootViewModel();
            var lootItems = _lootManager.GetAllLootItems();
            model.LootItems = lootItems.ToList();
            model.LootSplitSelectList = new SelectList(_lootManager.GetLootSplitDropdownValues(),
               "LootSplitId", "Name");
            return View(model);
        }
        public ActionResult LootSplit()
        {
            return View();
        }
        public ActionResult Grid()
        {
            var model = new LootViewModel();
            var lootItems = _lootManager.GetAllLootItems();
            model.LootItems = lootItems.ToList();
            return View(model);
        }

        private readonly ILogger<HomeController> _logger;

        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}

        //public IActionResult Index()
        //{
        //    return View();
        //}

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
     
        public ActionResult SubmitLootSplit(LootViewModel model, string lootSplitName)
        {
            var resultMessage = ResultMessages.JsonResultMessage();
            resultMessage.Message = "Successfully Submitted Loot Split.";
            resultMessage.Success = true;
            //save
            if (model.SelectedLootSplitId == 0)
            {
                try
                {
                    var failures = 0;
                    var successes = 0;
                    var result = resultMessage;
                    LootSplit lootSplit = new LootSplit();
                    lootSplit.Name = model.Name;
                    List<SplitItem> splitItems = new List<SplitItem>();

                    foreach (LootItem lootitem in model.LootItems)
                    {
                        if (lootitem.Quantity > 0)
                        {
                            SplitItem splitItem = new SplitItem();
                            splitItem.LootSplitId = lootSplit.LootSplitId;
                            splitItem.LootItemId = lootitem.LootItemId;
                            splitItem.LootItemQuantity = lootitem.Quantity;
                            splitItems.Add(splitItem);
                        }
                    }

                    lootSplit.SplitItems = splitItems;
                    lootSplit = _lootManager.InsertLootSplit(lootSplit);

                    //_lootManager.InsertSplitItems(splitItems, lootSplit.Id);
                    if (failures > 0)
                    {
                        resultMessage.Success = false;
                        resultMessage.Message = "Insert Failed ";
                        return Json(resultMessage);
                    }
                    resultMessage.Success = true;
                    resultMessage.Message = "Inserted {0} Loot Splits(s).".FormatWith(successes);
                }
                catch (Exception ex)
                {
                    resultMessage.Success = false;
                    resultMessage.Message = "Error Inserting Loot Split. " + ex.Message;
                }
            }
            //load
            else
            {
                model.LootSplitSelectList = new SelectList(_lootManager.GetLootSplitDropdownValues(),
             "LootSplitId", "Name");
                model.LootItems = _lootManager.GetAllLootItems().ToList();
                IEnumerable<SplitItem> splitItems = _lootManager.GetSplitItems(model.SelectedLootSplitId);
                foreach (LootItem lootItem in model.LootItems)
                {
                    foreach (SplitItem splitItem in splitItems)
                    {
                        if (splitItem.LootItemId == lootItem.LootItemId)
                        {
                            lootItem.Quantity = splitItem.LootItemQuantity;
                        }
                    }
                }
                foreach (SelectListItem item in model.LootSplitSelectList)
                {
                    if (item.Value == model.SelectedLootSplitId.ToString())
                    {
                        item.Selected = true;
                    }
                }
            }
                return View("Index", model);
        }
        //[HttpPost]
        //public ActionResult GetLootSplit(int? id)
        //{
        //    if (id != null)
        //    {
        //        LootSplit lootsplit = _lootManager.GetLootSplit(id);

        //        return Json(new { Success = "true", lootsplit });
        //    }
        //    return Json(new { Success = "false" });
        //}
        //[HttpPost]
        //public ActionResult GetLootSplit(int? id)
        //{
        //    var model = new LootViewModel();
        //    if (id != null)
        //    {
        //        LootSplit lootsplit = _lootManager.GetLootSplit(id);
        //        model.LootItems = lootsplit.SplitItems.ToList();
        //        model.LootSplitSelectList = new SelectList(_lootManager.GetLootSplitDropdownValues(),
        //           "LootSplitId", "Name");
        //    }

        //    return View(model);
        //}
        [HttpPost]
        public JsonResult AjaxLoot(string lootSplitId)
        {
            if (lootSplitId == "")
            {
                lootSplitId = "0";
            }
            var draw = HttpContext.Request.Form["draw"].FirstOrDefault();

            // Skip number of Rows count  
            var start = Request.Form["start"].FirstOrDefault();

            // Paging Length 10,20  
            var length = Request.Form["length"].FirstOrDefault();

            // Sort Column Name  
            var sortColumn = Request.Form["order[0][column]"].FirstOrDefault();

            // Sort Column Direction (asc, desc)  
            var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();

            // Search Value from (Search box)  
            var searchValue = Request.Form["search[value]"].FirstOrDefault();

            //Paging Size (10, 20, 50,100)  
            int pageSize = length != null ? Convert.ToInt32(length) : 0;

            int skip = start != null ? Convert.ToInt32(start) : 0;

            int recordsTotal = 0;

            IEnumerable<LootItem> lootItems = _lootManager.GetAllLootItems();
            // getting all Customer data  
            if (lootSplitId == "0")
            {

            }

            //Search    
            if (!string.IsNullOrEmpty(searchValue))
            {
                lootItems = lootItems.Where(d => d.Name != null && d.Name.ToLower().Contains(searchValue.ToLower())).ToList();
            }
            //Sorting
            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
            {
                lootItems = SortByColumnWithOrder(sortColumn, sortColumnDirection, lootItems);
            }
            //total number of rows counts   
            recordsTotal = lootItems.Count();
            //Paging   
            //var mappedResults = _mapper.Map<IEnumerable<LootItem>, ICollection<LootViewModel>>(lootItems);

 
             //var data = lootItems.Skip(skip).Take(pageSize).ToList();
            var data = lootItems;

            //Returning Json Data  

            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
        }
        private IEnumerable<LootItem> SortByColumnWithOrder(string sortColumn, string sortColumnDir, IEnumerable<LootItem> data)
        {
            // Sorting   
            switch (sortColumn)
            {
                case "0":
                    data = sortColumnDir.Equals("desc", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Order).ToList() : data.OrderBy(p => p.Order).ToList();
                    break;
                case "1":
                    data = sortColumnDir.Equals("desc", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Name).ToList() : data.OrderBy(p => p.Name).ToList();
                    break;
                case "2":
                    data = sortColumnDir.Equals("desc", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Quantity.ToString()).ToList() : data.OrderBy(p => p.Quantity.ToString()).ToList();
                    break;
                default:
                    data = sortColumnDir.Equals("desc", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Order).ToList() : data.OrderBy(p => p.Order).ToList();
                    break;
            }
            return data.AsEnumerable();
        }
    }
}
