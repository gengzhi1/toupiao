
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;

namespace toupiao.Areas.zvote.Models
{
    // 投票类

    public class ZVote
    {
        public Guid Id{ get; set;}

        [Display(Name="标题")]
        [StringLength( 64,
            ErrorMessage = "写个标题吧!({1}字以内)" , 
            MinimumLength = 2)]
        public string Title{ get; set;}

        // Date Of Creating
        [Display(Name="创建时间")]
        public DateTimeOffset DOCreating { get; set; } = DateTimeOffset.Now;


        // Date Of Start
        // 默认为立即开始
        [Display(Name = "投票起始")]
        [DataType(DataType.DateTime)]
        public DateTimeOffset? DOStart { get; set; } = DateTimeOffset.Now;

        [Display(Name="投票截至")]
        [DataType(DataType.DateTime)]
        public DateTimeOffset DOEnd{ get; set;} 
            = DateTimeOffset.Now.AddDays(1);
            
        [Display(Name = "创建者")]
        public IdentityUser Submitter{get;set;}

        [Display(Name = "仅保存")]
        public bool IsSaveOnly{get; set;} = false;

        [Display(Name = "描述")]
        [StringLength(256,
            ErrorMessage = "描述一下吧!({1}以内)",
            MinimumLength = 5)]
        public string Description { get; set; }

        [Display(Name = "类型")]
        public String ItemType{ get;set;}

        // Max vote iten count for per user
        [Display(Name = "投几项?")]
        public Int32 MaxItemCountpu { get; set; } = 1;

    }

    // 投票项
    public class ZVoteItem
    {
        public Guid Id{ get;set;}

        [ForeignKey("ZVote")]
        public Guid ZVoteId { get;set;}

        public ZVote ZVote { get; set; }

        public string Content{ get;set;}

        public string Description{ get;set;}
        
        public string ImageFileName { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }
    }

    public class ZUserVote
    {
        public Guid Id { get; set; }
        public Guid ZVoteItemId { get; set; }
        public ZVoteItem ZVoteItem { get; set; }
        public IdentityUser Voter { get; set; }
    }

}