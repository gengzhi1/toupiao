
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
        [DataType(DataType.Date)]
        public DateTimeOffset? DOStart { get; set; } = DateTimeOffset.Now;

        [Display(Name="投票截至")]
        [DataType(DataType.Date)]
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

        [StringLength(128)]
        public string XuanxiangA { get; set; }


        [StringLength(128)]
        public string XuanxiangB { get; set; }

        [StringLength(128)]
        public string XuanxiangC { get; set; }

        [StringLength(128)]
        public string XuanxiangD { get; set; }


        public bool IsLegal { get; set; } = false;


    }

   

    public class ZUserVote
    {
        public Guid Id { get; set; }
        public Guid ZVoteId { get; set; }
        public string VoteItem { get; set; }
        public String VoterId { get; set; }
        public DateTimeOffset DOVoting { get; set; } = DateTimeOffset.Now;
    }

}