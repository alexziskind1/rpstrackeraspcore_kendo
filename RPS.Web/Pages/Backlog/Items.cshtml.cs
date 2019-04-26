using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RPS.Core.Models;
using RPS.Data;
using RPS.Web.Models;
using RPS.Web.Models.Routing;

namespace RPS.Web.Pages.Backlog
{
    public class ItemsModel : PageModel
    {
        private const int CURRENT_USER_ID = 21; //Fake user id for demo

        private readonly IPtItemsRepository rpsItemsRepo;

        public List<PtItem> Items { get; set; }

        public List<PtItemGridModel> GridData
        {
            get
            {
                if (Items != null)
                {
                    return Items.Select(i => new PtItemGridModel(i)).ToList();
                } else
                {
                    return new List<PtItemGridModel>();
                }
            }
        }

        [BindProperty(SupportsGet = true)]
        public PresetEnum? Preset { get; set; }

        public ItemsModel(
            IPtItemsRepository rpsItemsData
            )
        {
            rpsItemsRepo = rpsItemsData;
        }

        public IActionResult OnGet()
        {
            if (!Preset.HasValue)
            {
                return RedirectToPage("Items", new { Preset = PresetEnum.Open });
            }

            IEnumerable<PtItem> items = null;
            switch (Preset)
            {
                case PresetEnum.My:
                    items = rpsItemsRepo.GetUserItems(CURRENT_USER_ID);
                    break;
                case PresetEnum.Open:
                    items = rpsItemsRepo.GetOpenItems();
                    break;
                case PresetEnum.Closed:
                    items = rpsItemsRepo.GetClosedItems();
                    break;
                default:
                    items = rpsItemsRepo.GetOpenItems();
                    break;
            }
            Items = items.ToList();

            return Page();
        }
    }
}