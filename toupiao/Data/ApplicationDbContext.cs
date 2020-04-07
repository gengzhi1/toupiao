using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using toupiao.Areas.zvote.Models;

namespace toupiao.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<toupiao.Areas.zvote.Models.ZVote> ZVote { get; set; }
        public DbSet<toupiao.Areas.zvote.Models.ZVoteItem> ZVoteItem { get; set; }
        public DbSet<toupiao.Areas.zvote.Models.ZUserVote> ZUserVote { get; set; }
    }
}
