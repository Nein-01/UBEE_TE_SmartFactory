namespace ATEVersions_Management.Migrations.ProductionDB
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addLowestCPKColForFATPTable : DbMigration
    {
        public override void Up()
        {
            /*AddColumn("dbo.FATP_TABLE", "CPK_LOWEST_ITEM", c => c.String());*/
        }
        
        public override void Down()
        {
            /*DropColumn("dbo.FATP_TABLE", "CPK_LOWEST_ITEM");*/
        }
    }
}
