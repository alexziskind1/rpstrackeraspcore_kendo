using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RPS.Core.Models;

namespace RPS.Web.Pages.Backlog
{
    public class PtItemCommentsVm
    {
        public int ItemId { get; set; }

        [DataType(DataType.MultilineText)]
        public string NewCommentText { get; set; }

        public List<PtComment> Comments { get; set; }

        public PtUser CurrentUser { get; set; }

        public PtItemCommentsVm()
        {
            Comments = new List<PtComment>();
        }

        public PtItemCommentsVm(PtItem item, PtUser currentUser)
        {
            ItemId = item.Id;
            Comments = item.Comments;
            CurrentUser = currentUser;
        }
    }
}