﻿using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace toupiao.Areas.zvote.Models
{
    public static class ZVoteMix
    {
        public static List<SelectListItem> GetItemTypes() =>
            new List<SelectListItem>() { 
                // 文字
                new SelectListItem{ Value = "0", Text = "文字",
                    Selected = true },
                // 图片
                new SelectListItem{ Value = "1", Text = "图片"},
            };

    }
    public enum ItemTypeEnum
    {
        [Display(Name = "文本")]
        Text,
        [Display(Name = "图片")]
        Image
    }
}
