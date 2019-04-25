using Microsoft.AspNetCore.Razor.TagHelpers;
using RPS.Core.Models;
using System;

namespace RPS.Web.TagHelpers
{
    [HtmlTargetElement("user-avatar")]
    public class UserAvatarTagHelper : TagHelper
    {
        [HtmlAttributeName("user")]
        public PtUser User { get; set; }

        [HtmlAttributeName("class")]
        public string className { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "img";
            output.TagMode = TagMode.SelfClosing;

            string cl = "li-avatar rounded mx-auto d-block";
            if (!String.IsNullOrEmpty(className))
            {
                cl = className;
            }

            output.Attributes.Add("src", "/" + User.Avatar);
            output.Attributes.Add("class", cl);
            output.Attributes.Add("alt", User.FullName);

        }
    }
}
