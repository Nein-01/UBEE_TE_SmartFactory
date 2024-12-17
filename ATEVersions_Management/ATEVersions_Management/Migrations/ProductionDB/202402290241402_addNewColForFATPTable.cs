namespace ATEVersions_Management.Migrations.ProductionDB
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addNewColForFATPTable : DbMigration
    {
        public override void Up()
        {
            /*AddColumn("dbo.FATP_TABLE", "PC_DATE", c => c.DateTime());
            AddColumn("dbo.FATP_TABLE", "CPK_RESULTS", c => c.String());
            AddColumn("dbo.FATP_TABLE", "LOSS_MEASUREMENT", c => c.String());*/
        }
        
        public override void Down()
        {
            /*DropColumn("dbo.FATP_TABLE", "LOSS_MEASUREMENT");
            DropColumn("dbo.FATP_TABLE", "CPK_RESULTS");
            DropColumn("dbo.FATP_TABLE", "PC_DATE");*/
        }
    }
}
