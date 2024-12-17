namespace ATEVersions_Management.Migrations.ProductionDB
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_FATPTable_OPMonitorCols : DbMigration
    {
        public override void Up()
        {
            /*AddColumn("dbo.FATP_TABLE", "OP_ID", c => c.String());
            AddColumn("dbo.FATP_TABLE", "OP_RECORD", c => c.String());*/
        }
        
        public override void Down()
        {
            /*DropColumn("dbo.FATP_TABLE", "OP_RECORD");
            DropColumn("dbo.FATP_TABLE", "OP_ID");*/
        }
    }
}
