using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RPS.Core.Models;

namespace RPS.Web.Pages.Backlog
{
    public class PtItemTasksVm
    {
        public int ItemId { get; set; }
        public string NewTaskTitle { get; set; }

        public List<PtTask> Tasks { get; set; }

        public PtItemTasksVm()
        {
            Tasks = new List<PtTask>();
        }

        public PtItemTasksVm(PtItem item)
        {
            ItemId = item.Id;
            Tasks = item.Tasks;
        }
    }
}