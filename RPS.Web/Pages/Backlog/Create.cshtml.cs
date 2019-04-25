using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using RPS.Core.Models.Dto;
using RPS.Core.Models.Enums;
using RPS.Data;

namespace RPS.Web.Pages.Backlog
{
    public class CreateModel : PageModel
    {
        private const int CURRENT_USER_ID = 21; //Fake user id for demo

        private readonly IPtItemsRepository rpsItemsRepo;

        private readonly List<ItemTypeEnum> _itemTypes = new List<ItemTypeEnum> { ItemTypeEnum.Bug, ItemTypeEnum.Chore, ItemTypeEnum.Impediment, ItemTypeEnum.PBI };

        [Required, Display(Name = "Title")]
        [BindProperty]
        public string Title { get; set; }

        [Display(Name = "Description")]
        [BindProperty]
        public string Description { get; set; }

        [BindProperty]
        public ItemTypeEnum TypeStr { get; set; }

        [BindProperty]
        public ItemTypeEnum TypeStr2 { get; set; }

        public IEnumerable<SelectListItem> ItemTypes
        {
            get { return new SelectList(_itemTypes, ItemTypeEnum.Bug); }
        }

        public CreateModel(IPtItemsRepository rpsItemsData)
        {
            rpsItemsRepo = rpsItemsData;
        }

        public void OnGet()
        {
            TypeStr = ItemTypeEnum.Bug;
            TypeStr2 = ItemTypeEnum.Bug;
        }

        public IActionResult OnPost()
        {
            var newItem = ToPtNewItem();
            newItem.UserId = CURRENT_USER_ID;

            rpsItemsRepo.AddNewItem(newItem);

            return RedirectToPage("/Backlog/Items");
        }

        public PtNewItem ToPtNewItem()
        {
            return new PtNewItem
            {
                Title = Title,
                Description = Description,
                TypeStr = TypeStr
            };
        }
    }
}