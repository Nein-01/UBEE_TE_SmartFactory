namespace ATEVersions_Management.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addSignedDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ATE_CHECKLIST", "IsPrepared", c => c.Int());
            AddColumn("dbo.ATE_CHECKLIST", "PreparedAt", c => c.DateTime());
            AddColumn("dbo.ATE_CHECKLIST", "CheckedAt", c => c.DateTime());
            AddColumn("dbo.ATE_CHECKLIST", "ApprovedAt", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ATE_CHECKLIST", "ApprovedAt");
            DropColumn("dbo.ATE_CHECKLIST", "CheckedAt");
            DropColumn("dbo.ATE_CHECKLIST", "PreparedAt");
            DropColumn("dbo.ATE_CHECKLIST", "IsPrepared");
        }
    }
}
