
using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;

namespace toupiao.Areas.zvote.Models
{
    public class ZVote
    {
        public Guid Id{ get; set;}
        public String Title{ get; set;}
        public DateTimeOffset DOCreating { get; set; } = DateTimeOffset.Now;
        public DateTimeOffset DOEnd{ get; set;} 
            = DateTimeOffset.Now.AddDays(1);
        
        public IdentityUser Submitter{get;set;}
        public bool IsSaveOnly{get; set;} = false;
        public String ItemType{ get;set;}
    }

    public class ZVoteItem
    {
        public Guid Id{ get;set;}
        public ZVote ForZVote{ get;set;}
        public string ItemSource{ get;set;}
        public string Description{ get;set;}
    }

    public class ZVoteMix
    {
        public SelectList ItemTypes(IStringLocalizer<Program> _localizer) => 
            new SelectList( new[]{
                new SelectListItem{ Value = "0", Text = _localizer["Text"] },
                new SelectListItem{ Value = "1", Text = _localizer["Image"] },
            });
    }
}