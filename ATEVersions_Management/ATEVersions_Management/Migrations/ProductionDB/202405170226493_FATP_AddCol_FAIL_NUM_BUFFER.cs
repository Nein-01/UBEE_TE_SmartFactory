namespace ATEVersions_Management.Migrations.ProductionDB
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FATP_AddCol_FAIL_NUM_BUFFER : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FATP_TABLE", "FAIL_NUM_BUFFER", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.FATP_TABLE", "FAIL_NUM_BUFFER");
        }
    }
}
