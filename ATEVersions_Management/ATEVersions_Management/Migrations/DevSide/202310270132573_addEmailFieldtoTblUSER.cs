namespace ATEVersions_Management.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addEmailFieldtoTblUSER : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.USERS", "Email", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            DropColumn("dbo.USERS", "Email");
        }
    }
}
