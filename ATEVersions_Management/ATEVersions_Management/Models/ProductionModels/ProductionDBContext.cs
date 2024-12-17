using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace ATEVersions_Management.Models.ProductionModels
{
    public partial class ProductionDBContext : DbContext
    {
        public ProductionDBContext()
            : base("name=Production")
        {
        }

        public virtual DbSet<FATP_TABLE> FATP_TABLE { get; set; }
        public virtual DbSet<PC_INFORS> PC_INFORS { get; set; }
        public virtual DbSet<TE_TEST_DATA> TE_TEST_DATA { get; set; }
        public virtual DbSet<TE_TEST_FINAL_DATA> TE_TEST_FINAL_DATA { get; set; }
        public virtual DbSet<TE_TEST_FINAL_DATA_V2> TE_TEST_FINAL_DATA_V2 { get; set; }
        public virtual DbSet<TE_TEST_ITEM_DATA> TE_TEST_ITEM_DATA { get; set; }
        public virtual DbSet<TABLE_DATA_FAIL> TABLE_DATA_FAIL { get; set; }
        public virtual DbSet<TEST_LOGS_DATA> TEST_LOGS_DATA { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PC_INFORS>()
                .Property(e => e.ATE)
                .IsUnicode(false);

            modelBuilder.Entity<PC_INFORS>()
                .Property(e => e.IP_ADDRESS)
                .IsUnicode(false);

            modelBuilder.Entity<TEST_LOGS_DATA>()
                .Property(e => e.PROGRAM_VERSION)
                .IsUnicode(false);

            modelBuilder.Entity<TEST_LOGS_DATA>()
                .Property(e => e.OWNER)
                .IsUnicode(false);

            modelBuilder.Entity<TEST_LOGS_DATA>()
                .Property(e => e.BUILD_TIME)
                .IsUnicode(false);
        }
    }
}
