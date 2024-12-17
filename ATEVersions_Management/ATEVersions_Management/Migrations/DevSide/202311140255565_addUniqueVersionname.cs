namespace ATEVersions_Management.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addUniqueVersionname : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.VERSION", "VersionName", c => c.String(nullable: false, maxLength: 150));
            CreateIndex("dbo.VERSION", "VersionName", unique: true, name: "Idx_VersionName");
        }
        
        public override void Down()
        {
            DropIndex("dbo.VERSION", "Idx_VersionName");
            AlterColumn("dbo.VERSION", "VersionName", c => c.String(maxLength: 150));
        }
    }
}
