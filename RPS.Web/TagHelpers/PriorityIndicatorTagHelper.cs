using Microsoft.AspNetCore.Razor.TagHelpers;
using RPS.Core.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RPS.Web.TagHelpers
{
    [HtmlTargetElement("priority-indicator")]
    public class PriorityIndicatorTagHelper : TagHelper
    {
        [HtmlAttributeName("priority")]
        public PriorityEnum Priority { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var priorityClassName = "";

            switch (Priority)
            {
                case PriorityEnum.Critical:
                    priorityClassName = "priority-critical";
                    break;
                case PriorityEnum.High:
                    priorityClassName = "priority-high";
                    break;
                case PriorityEnum.Low:
                    priorityClassName = "priority-low";
                    break;
                case PriorityEnum.Medium:
                    priorityClassName = "priority-medium";
                    break;
            }

            output.TagName = "span";
            output.TagMode = TagMode.StartTagAndEndTag;

            output.Attributes.Add("class", "badge " + priorityClassName);
            output.Content.AppendHtml(Priority.ToString());

        }
    }
}
