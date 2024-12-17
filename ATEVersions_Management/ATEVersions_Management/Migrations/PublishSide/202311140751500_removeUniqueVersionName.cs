namespace ATEVersions_Management.Migrations.PublishSide
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeUniqueVersionName : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.VERSION", "Idx_VersionName");
        }
        
        public override void Down()
        {
            CreateIndex("dbo.VERSION", "VersionName", unique: true, name: "Idx_VersionName");
        }
    }
}
