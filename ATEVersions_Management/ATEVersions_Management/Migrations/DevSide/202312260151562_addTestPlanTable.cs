﻿namespace ATEVersions_Management.Migrations.DevSide
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addTestPlanTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TEST_PLAN",
                c => new
                    {
                        TestPlanID = c.Int(nullable: false, identity: true),
                        UserID = c.Int(nullable: false),
                        ModelName = c.String(nullable: true, maxLength: 50),
                        TestPlanVersion = c.String(nullable: true, maxLength: 50),
                        Author = c.String(nullable: true, maxLength: 50),
                        ModifyNote = c.String(nullable: true, maxLength: 250),
                        StoredDir = c.String(nullable: true, maxLength: 350),
                        Status = c.Int(),
                        CreatedAt = c.DateTime(),
                        UpdatedAt = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.TestPlanID)
                .ForeignKey("dbo.USERS", t => t.UserID, cascadeDelete: true)
                .Index(t => t.UserID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TEST_PLAN", "UserID", "dbo.USERS");
            DropIndex("dbo.TEST_PLAN", new[] { "UserID" });
            DropTable("dbo.TEST_PLAN");
        }
    }
}
