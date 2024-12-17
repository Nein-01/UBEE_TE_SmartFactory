using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace ATEVersions_Management.Models.CPKModels
{
    public partial class CPKDbContext : DbContext
    {
        public CPKDbContext()
            : base("name=CPKContext")
        {
        }

        public virtual DbSet<CPK_TABLE> CPK_TABLE { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
