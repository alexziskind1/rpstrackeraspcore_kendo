using Microsoft.AspNetCore.Razor.TagHelpers;
using RPS.Core.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RPS.Web.TagHelpers
{
    [HtmlTargetElement("item-type-indicator")]
    public class ItemTypeIndicatorImageTagHelper : TagHelper
    {
        [HtmlAttributeName("item-type")]
        public ItemTypeEnum ItemType { get; set; }

        [HtmlAttributeName("class")]
        public string className { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var imgSrc = "";

            switch (ItemType)
            {
                case ItemTypeEnum.Bug:
                    imgSrc = "/images/icon_bug.png";
                    break;
                case ItemTypeEnum.Chore:
                    imgSrc = "/images/icon_chore.png";
                    break;
                case ItemTypeEnum.Impediment:
                    imgSrc = "/images/icon_impediment.png";
                    break;
                case ItemTypeEnum.PBI:
                    imgSrc = "/images/icon_pbi.png";
                    break;
            }

            output.TagName = "img";
            output.TagMode = TagMode.SelfClosing;

            output.Attributes.Add("src", imgSrc);
            output.Attributes.Add("class", className);
            output.Attributes.Add("alt", ItemType.ToString());
        }
    }
}
