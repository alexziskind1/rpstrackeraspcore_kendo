using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using RPS.Core.Models;
using RPS.Core.Models.Dto;
using RPS.Core.Models.Enums;

namespace RPS.Web.Pages.Backlog
{
        public class PtItemDetailsVm
        {
            private readonly List<ItemTypeEnum> _itemTypes = new List<ItemTypeEnum> { ItemTypeEnum.Bug, ItemTypeEnum.Chore, ItemTypeEnum.Impediment, ItemTypeEnum.PBI };
            private readonly List<StatusEnum> _statuses = new List<StatusEnum> { StatusEnum.Closed, StatusEnum.Open, StatusEnum.ReOpened, StatusEnum.Submitted };
            private readonly List<PriorityEnum> _priorities = new List<PriorityEnum> { PriorityEnum.Critical, PriorityEnum.High, PriorityEnum.Low, PriorityEnum.Medium };
            private readonly List<PtUser> _users;

            public int Id { get; set; }

            [Required, Display(Name = "Title")]
            public string Title { get; set; }

            [Display(Name = "Description")]
            public string Description { get; set; }

            [Display(Name = "Estimate")]
            public int Estimate { get; set; }

            [Display(Name = "Type")]
            public ItemTypeEnum SelectedItemType { get; set; }

            [Display(Name = "Status")]
            public StatusEnum SelectedStatus { get; set; }

            [Display(Name = "Priority")]
            public PriorityEnum SelectedPriority { get; set; }

            [Display(Name = "Assignee")]
            public int SelectedAssigneeId { get; set; }

            public IEnumerable<SelectListItem> ItemTypes
            {
                get { return new SelectList(_itemTypes, SelectedItemType); }
            }

            public IEnumerable<SelectListItem> Statuses
            {
                get { return new SelectList(_statuses, SelectedStatus); }
            }

            public IEnumerable<SelectListItem> Priorities
            {
                get { return new SelectList(_priorities, SelectedPriority); }
            }

            public IEnumerable<SelectListItem> Users
            {
                get { return new SelectList(_users, "Id", "FullName", SelectedAssigneeId); }
            }

            public PtItemDetailsVm()
            {
                SelectedItemType = ItemTypeEnum.Bug;
                SelectedStatus = StatusEnum.Open;
                SelectedPriority = PriorityEnum.Medium;
                _users = new List<PtUser>();
                SelectedAssigneeId = 0;
            }

            public PtItemDetailsVm(PtItem item, List<PtUser> users)
            {
                Title = item.Title;
                Description = item.Description;
                Estimate = item.Estimate;
                SelectedItemType = item.Type;
                SelectedStatus = item.Status;
                SelectedPriority = item.Priority;
                SelectedAssigneeId = item.Assignee.Id;
                _users = users;
            }

            public PtUpdateItem ToPtUpdateItem()
            {
                return new PtUpdateItem
                {
                    Id = Id,
                    Title = Title,
                    Description = Description,
                    Estimate = Estimate,
                    Priority = SelectedPriority,
                    Status = SelectedStatus,
                    Type = SelectedItemType,
                    AssigneeId = SelectedAssigneeId
                };
            }

        }
    }
