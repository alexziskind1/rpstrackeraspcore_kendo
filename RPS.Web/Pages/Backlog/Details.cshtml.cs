using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RPS.Core.Models;
using RPS.Core.Models.Dto;
using RPS.Data;
using RPS.Web.Models.Routing;

namespace RPS.Web.Pages.Backlog
{
    public class DetailsModel : PageModel
    {
        private const int CURRENT_USER_ID = 21; //Fake user id for demo

        private readonly IPtUserRepository rpsUserRepo;
        private readonly IPtItemsRepository rpsItemsRepo;
        private readonly IPtTasksRepository rpsTasksRepo;
        private readonly IPtCommentsRepository rpsCommentsRepo;

        public PtItem Item { get; set; }

        [BindProperty(SupportsGet = true)]
        public DetailScreenEnum Screen { get; set; }

        public List<PtUser> Users { get; set; }

        [BindProperty]
        public PtItemDetailsVm DetailsFormVm { get; set; }

        [BindProperty]
        public PtItemTasksVm TasksFormVm { get; set; }

        [BindProperty]
        public PtItemCommentsVm ChitchatFormVm { get; set; }

        private readonly IWebHostEnvironment webHostEnvironment;

        public DetailsModel(
            IPtUserRepository rpsUserData,
            IPtItemsRepository rpsItemsData,
            IPtTasksRepository rpsTasksData,
            IPtCommentsRepository rpsCommentsData,
            IWebHostEnvironment webHostEnvironment
            )
        {
            rpsUserRepo = rpsUserData;
            rpsItemsRepo = rpsItemsData;
            rpsTasksRepo = rpsTasksData;
            rpsCommentsRepo = rpsCommentsData;
            this.webHostEnvironment = webHostEnvironment;
        }

        public ActionResult OnPostUpload(IEnumerable<IFormFile> files)
        {
            var uploads = Path.Combine(webHostEnvironment.WebRootPath, "uploads");

            if (files != null)
            {
                foreach (var file in files)
                {
                    var fileContent = ContentDispositionHeaderValue.Parse(file.ContentDisposition);
                    var fileName = Path.GetFileName(fileContent.FileName.Trim('"'));

                    var physicalPath = Path.Combine(uploads, fileName);

                    using (Stream fileStream = new FileStream(physicalPath, FileMode.Create))
                    {
                        file.CopyToAsync(fileStream);
                    }
                }
            }

            return Content("");
        }

        public IActionResult OnGet(int id)
        {
            Item = rpsItemsRepo.GetItemById(id);
            Users = rpsUserRepo.GetAll().ToList();
            var currentUser = Users.Single(u => u.Id == CURRENT_USER_ID);

            DetailsFormVm = new PtItemDetailsVm(Item, Users);
            TasksFormVm = new PtItemTasksVm(Item);
            ChitchatFormVm = new PtItemCommentsVm(Item, currentUser);

            return Page();
        }

        public IActionResult OnPost()
        {
            switch (Screen)
            {
                case DetailScreenEnum.Details:
                    SaveDetails();
                    break;
                case DetailScreenEnum.Tasks:
                    SaveTask();
                    break;
                case DetailScreenEnum.Chitchat:
                    SaveComment();
                    break;
            }
            return RedirectToPage("Details", new { id = DetailsFormVm.Id, Screen });
        }

        public IActionResult OnPostUpdate(int taskId, string title, bool? completed)
        {
            PtUpdateTask uTask = new PtUpdateTask
            {
                Id = taskId,
                ItemId = DetailsFormVm.Id,
                Title = title,
                Completed = completed.HasValue ? completed.Value : false
            };
            rpsTasksRepo.UpdateTask(uTask);

            return RedirectToPage("Details", new { id = DetailsFormVm.Id, Screen });
        }

        public IActionResult OnPostDelete(int taskId)
        {
            var result = rpsTasksRepo.DeleteTask(taskId, DetailsFormVm.Id);
            return RedirectToPage("Details", new { id = DetailsFormVm.Id, Screen });
        }


        private void SaveDetails()
        {
            var updatedItem = rpsItemsRepo.UpdateItem(DetailsFormVm.ToPtUpdateItem());
        }

        private void SaveTask()
        {
            PtNewTask taskNew = new PtNewTask
            {
                ItemId = TasksFormVm.ItemId,
                Title = TasksFormVm.NewTaskTitle
            };

            rpsTasksRepo.AddNewTask(taskNew);
        }

        private void SaveComment()
        {
            PtNewComment commentNew = new PtNewComment
            {
                ItemId = ChitchatFormVm.ItemId,
                Title = ChitchatFormVm.NewCommentText,
                UserId = CURRENT_USER_ID
            };

            rpsCommentsRepo.AddNewComment(commentNew);
        }
    }
}