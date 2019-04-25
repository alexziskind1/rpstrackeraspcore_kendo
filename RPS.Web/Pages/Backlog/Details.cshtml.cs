using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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


        public DetailsModel(
            IPtUserRepository rpsUserData,
            IPtItemsRepository rpsItemsData,
            IPtTasksRepository rpsTasksData,
            IPtCommentsRepository rpsCommentsData)
        {
            rpsUserRepo = rpsUserData;
            rpsItemsRepo = rpsItemsData;
            rpsTasksRepo = rpsTasksData;
            rpsCommentsRepo = rpsCommentsData;
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