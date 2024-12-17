using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace ATEVersions_Management.Models.ATEVersionModels
{
    public partial class ATEVersionContext : DbContext
    {
        public ATEVersionContext()
            : base("name=SmartFactory") // DevSide: ATEVersion | PublishSide: SmartFactory
        {
        }

        public virtual DbSet<ATE_CHECKLIST> ATE_CHECKLIST { get; set; }
        public virtual DbSet<CHECKLIST_DETAIL> CHECKLIST_DETAIL { get; set; }
        public virtual DbSet<CHECKLIST_ITEM> CHECKLIST_ITEM { get; set; }
        public virtual DbSet<PERMISSION> PERMISSIONs { get; set; }
        public virtual DbSet<PROGRAM> PROGRAMs { get; set; }
        public virtual DbSet<ROLE> ROLES { get; set; }
        public virtual DbSet<USER> USERS { get; set; }
        public virtual DbSet<VERSION> VERSIONs { get; set; }        
        public virtual DbSet<TEST_PLAN> TEST_PLANs { get; set; }
        public virtual DbSet<GRR_TABLE> GRR_TABLE { get; set; }

        // ========= MESSAGE SYSTEM DATA TABLES =========
        public virtual DbSet<MESSAGE_GROUP> MESSAGE_GROUP { get; set; }
        public virtual DbSet<MESSAGE_RECIPIENT> MESSAGE_RECIPIENT { get; set; }
        public virtual DbSet<MESSAGE_SENDER> MESSAGE_SENDER { get; set; }
        public virtual DbSet<MESSAGE_USER_GROUP> MESSAGE_USER_GROUP { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PERMISSION>()
                .HasMany(e => e.ROLES)
                .WithMany(e => e.PERMISSIONs)
                .Map(m => m.ToTable("ROLE_PERMISSION").MapLeftKey("PermissionID").MapRightKey("RoleID"));

            modelBuilder.Entity<USER>()
                .HasMany(e => e.MESSAGE_SENDER)
                .WithRequired(e => e.USER)
                .HasForeignKey(e => e.SENDER_ID);

            modelBuilder.Entity<USER>()
                .HasMany(e => e.MESSAGE_USER_GROUP)
                .WithRequired(e => e.USER)
                .HasForeignKey(e => e.USER_ID);
        }
    }
}
