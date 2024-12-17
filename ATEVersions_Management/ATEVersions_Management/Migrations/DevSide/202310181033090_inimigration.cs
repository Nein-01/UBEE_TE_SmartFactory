namespace ATEVersions_Management.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class inimigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ATE_CHECKLIST",
                c => new
                {
                    CheckListID = c.Int(nullable: false, identity: true),
                    VersionID = c.Int(nullable: false),
                    CheckListCode = c.String(maxLength: 250),
                    ProductHW_SW = c.String(maxLength: 250),
                    PreparedBy = c.String(maxLength: 250),
                    CheckedBy = c.String(maxLength: 250),
                    ApprovedBy = c.String(maxLength: 250),
                    StoredTime = c.Double(),
                    Status = c.Int(),
                    CreatedAt = c.DateTime(),
                    UpdatedAt = c.DateTime(),
                })
                .PrimaryKey(t => t.CheckListID)
                .ForeignKey("dbo.VERSION", t => t.VersionID, cascadeDelete: true)
                .Index(t => t.VersionID);

            CreateTable(
                "dbo.CHECKLIST_DETAIL",
                c => new
                {
                    CheckListID = c.Int(nullable: false),
                    ItemID = c.Int(nullable: false),
                    Result = c.Int(),
                })
                .PrimaryKey(t => new { t.CheckListID, t.ItemID })
                .ForeignKey("dbo.ATE_CHECKLIST", t => t.CheckListID, cascadeDelete: true)
                .ForeignKey("dbo.CHECKLIST_ITEM", t => t.ItemID, cascadeDelete: true)
                .Index(t => t.CheckListID)
                .Index(t => t.ItemID);

            CreateTable(
                "dbo.CHECKLIST_ITEM",
                c => new
                {
                    ItemID = c.Int(nullable: false, identity: true),
                    ItemName = c.String(),
                    CheckMethod = c.String(),
                    Status = c.Int(),
                    CreatedAt = c.DateTime(),
                    UpdatedAt = c.DateTime(),
                })
                .PrimaryKey(t => t.ItemID);

            CreateTable(
                "dbo.VERSION",
                c => new
                {
                    VersionID = c.Int(nullable: false, identity: true),
                    ProgramID = c.Int(nullable: false),
                    VersionName = c.String(maxLength: 150),
                    Engineer = c.String(maxLength: 250),
                    BuildTime = c.DateTime(),
                    ReleaseTime = c.DateTime(),
                    ReleaseNote = c.String(),
                    Status = c.Int(),
                    CreatedAt = c.DateTime(),
                    UpdatedAt = c.DateTime(),
                })
                .PrimaryKey(t => t.VersionID)
                .ForeignKey("dbo.PROGRAM", t => t.ProgramID, cascadeDelete: true)
                .Index(t => t.ProgramID);

            CreateTable(
                "dbo.PROGRAM",
                c => new
                {
                    ProgramID = c.Int(nullable: false, identity: true),
                    ProgramName = c.String(nullable: false, maxLength: 250),
                    DevelopTool = c.String(nullable: false, maxLength: 250),
                    Status = c.Int(),
                    CreatedAt = c.DateTime(),
                    UpdatedAt = c.DateTime(),
                })
                .PrimaryKey(t => t.ProgramID);

            CreateTable(
                "dbo.PERMISSION",
                c => new
                {
                    PermissionID = c.Int(nullable: false, identity: true),
                    PermissionName = c.String(maxLength: 100),
                })
                .PrimaryKey(t => t.PermissionID);

            CreateTable(
                "dbo.ROLES",
                c => new
                {
                    RoleID = c.Int(nullable: false, identity: true),
                    RoleName = c.String(maxLength: 100),
                })
                .PrimaryKey(t => t.RoleID);

            CreateTable(
                "dbo.USERS",
                c => new
                {
                    UserID = c.Int(nullable: false, identity: true),
                    RoleID = c.Int(nullable: false),
                    UserName = c.String(maxLength: 150),
                    Password = c.String(maxLength: 250),
                    FullName = c.String(maxLength: 250),
                    PhoneNumber = c.String(maxLength: 20),
                    Avatar = c.String(),
                    Status = c.Int(),
                    CreatedAt = c.DateTime(),
                    UpdatedAt = c.DateTime(),
                })
                .PrimaryKey(t => t.UserID)
                .ForeignKey("dbo.ROLES", t => t.RoleID, cascadeDelete: true)
                .Index(t => t.RoleID);

            CreateTable(
                "dbo.ROLE_PERMISSION",
                c => new
                {
                    PermissionID = c.Int(nullable: false),
                    RoleID = c.Int(nullable: false),
                })
                .PrimaryKey(t => new { t.PermissionID, t.RoleID })
                .ForeignKey("dbo.PERMISSION", t => t.PermissionID, cascadeDelete: true)
                .ForeignKey("dbo.ROLES", t => t.RoleID, cascadeDelete: true)
                .Index(t => t.PermissionID)
                .Index(t => t.RoleID);

        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ROLE_PERMISSION", "RoleID", "dbo.ROLES");
            DropForeignKey("dbo.ROLE_PERMISSION", "PermissionID", "dbo.PERMISSION");
            DropForeignKey("dbo.USERS", "RoleID", "dbo.ROLES");
            DropForeignKey("dbo.VERSION", "ProgramID", "dbo.PROGRAM");
            DropForeignKey("dbo.ATE_CHECKLIST", "VersionID", "dbo.VERSION");
            DropForeignKey("dbo.CHECKLIST_DETAIL", "ItemID", "dbo.CHECKLIST_ITEM");
            DropForeignKey("dbo.CHECKLIST_DETAIL", "CheckListID", "dbo.ATE_CHECKLIST");
            DropIndex("dbo.ROLE_PERMISSION", new[] { "RoleID" });
            DropIndex("dbo.ROLE_PERMISSION", new[] { "PermissionID" });
            DropIndex("dbo.USERS", new[] { "RoleID" });
            DropIndex("dbo.VERSION", new[] { "ProgramID" });
            DropIndex("dbo.CHECKLIST_DETAIL", new[] { "ItemID" });
            DropIndex("dbo.CHECKLIST_DETAIL", new[] { "CheckListID" });
            DropIndex("dbo.ATE_CHECKLIST", new[] { "VersionID" });
            DropTable("dbo.ROLE_PERMISSION");
            DropTable("dbo.USERS");
            DropTable("dbo.ROLES");
            DropTable("dbo.PERMISSION");
            DropTable("dbo.PROGRAM");
            DropTable("dbo.VERSION");
            DropTable("dbo.CHECKLIST_ITEM");
            DropTable("dbo.CHECKLIST_DETAIL");
            DropTable("dbo.ATE_CHECKLIST");
        }
    }
}
