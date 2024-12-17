using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace ATEVersions_Management.Models.TestMonitorModels
{
    public partial class TestMonitorDBContext : DbContext
    {
        public TestMonitorDBContext()
            : base("name=TestMonitor")
        {
        }

        public virtual DbSet<AIR_INFORMATION> AIR_INFORMATION { get; set; }
        public virtual DbSet<BLACKLIST_SOFTWARE> BLACKLIST_SOFTWARE { get; set; }
        public virtual DbSet<EMPLOYEE> EMPLOYEES { get; set; }
        public virtual DbSet<ENERGY_RECORD> ENERGY_RECORD { get; set; }
        public virtual DbSet<EQUIPMENT_MANAGEMENT> EQUIPMENT_MANAGEMENT { get; set; }
        public virtual DbSet<ESD_INFORMATION> ESD_INFORMATION { get; set; }
        public virtual DbSet<MACHINE_INFORMATION> MACHINE_INFORMATION { get; set; }
        public virtual DbSet<MACHINE_INFORMATION_REGISTER> MACHINE_INFORMATION_REGISTER { get; set; }
        public virtual DbSet<STATION_INFORMATION> STATION_INFORMATION { get; set; }
        public virtual DbSet<UPDATE_TOOL> UPDATE_TOOL { get; set; }
        public virtual DbSet<FACE_RECOGNITION_DATA> FACE_RECOGNITION_DATA { get; set; }
        public virtual DbSet<ISSUE_RECORD> ISSUE_RECORD { get; set; }
        public virtual DbSet<MACHINE_INFORMATION_CHANGE_RECORD> MACHINE_INFORMATION_CHANGE_RECORD { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EMPLOYEE>()
                .Property(e => e.CARD_ID)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<EMPLOYEE>()
                .Property(e => e.PASSWORD)
                .IsUnicode(false);

            modelBuilder.Entity<UPDATE_TOOL>()
                .Property(e => e.NAME)
                .IsUnicode(false);

            modelBuilder.Entity<FACE_RECOGNITION_DATA>()
                .Property(e => e.CARD_ID)
                .IsFixedLength()
                .IsUnicode(false);
        }
    }
}
