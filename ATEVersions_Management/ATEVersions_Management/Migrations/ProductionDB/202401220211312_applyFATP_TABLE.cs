namespace ATEVersions_Management.Migrations.ProductionDB
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class applyFATP_TABLE : DbMigration
    {
        public override void Up()
        {
            /*CreateTable(
                "dbo.FATP_TABLE",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ATE_PC = c.String(maxLength: 50),
                        ATE_IP = c.String(maxLength: 50),
                        ATE_MAC = c.String(maxLength: 50),
                        LINE = c.String(maxLength: 50),
                        STATION = c.String(maxLength: 50),
                        MODEL = c.String(maxLength: 50),
                        POST_DATE = c.DateTime(),
                        PASS_NUM = c.Int(),
                        FAIL_NUM = c.Int(),
                        ERROR_LIST = c.String(),
                        COUNTERS = c.String(),
                        EQUIPMENTS = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.PC_INFORS",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        ATE = c.String(maxLength: 30, unicode: false),
                        IP_ADDRESS = c.String(maxLength: 30, unicode: false),
                        OS = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.TABLE_DATA_FAIL",
                c => new
                    {
                        MODEL_NAME = c.String(nullable: false, maxLength: 50),
                        STATION = c.String(nullable: false, maxLength: 10),
                        SN = c.String(nullable: false, maxLength: 100),
                        MAC = c.String(nullable: false, maxLength: 100),
                        MO = c.String(nullable: false, maxLength: 50),
                        VERSION = c.String(nullable: false, maxLength: 50),
                        ERROR_CODE = c.String(nullable: false, maxLength: 100),
                        ERROR_DETAIL = c.String(nullable: false, maxLength: 254),
                        WORK_DATE = c.DateTime(nullable: false),
                        FAIL_VALUE = c.String(maxLength: 254),
                    })
                .PrimaryKey(t => new { t.MODEL_NAME, t.STATION, t.SN, t.MAC, t.MO, t.VERSION, t.ERROR_CODE, t.ERROR_DETAIL, t.WORK_DATE });
            
            CreateTable(
                "dbo.TE_TEST_DATA",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        VIRTUAL_SN = c.String(maxLength: 254),
                        WORK_DATE = c.String(maxLength: 254),
                        WORK_SECTION = c.Int(),
                        FULL_WORK_DATE = c.String(maxLength: 254),
                        CFT = c.String(maxLength: 254),
                        PROJECT = c.String(maxLength: 254),
                        MODEL = c.String(maxLength: 254),
                        LINE = c.String(maxLength: 254),
                        STATION = c.String(maxLength: 254),
                        ATE = c.String(maxLength: 254),
                        FIXTURE_CODE = c.String(maxLength: 254),
                        CARRIER_CODE = c.String(maxLength: 254),
                        TEST_MODE = c.Int(),
                        TEST_VERSION = c.String(maxLength: 254),
                        MO = c.String(maxLength: 254),
                        BOARD_SN = c.String(maxLength: 254),
                        SN = c.String(maxLength: 254),
                        START_TIME = c.DateTime(),
                        END_TIME = c.DateTime(precision: 7, storeType: "datetime2"),
                        ELAPSE_TIME = c.Int(),
                        STATUS = c.Int(),
                        ERROR_CODE = c.String(maxLength: 254),
                        TEST_ITEM = c.String(maxLength: 254),
                        VALUE_TYPE = c.String(maxLength: 254),
                        VALUE = c.String(maxLength: 254),
                        LSL = c.String(maxLength: 254),
                        USL = c.String(maxLength: 254),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.TE_TEST_FINAL_DATA",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        VIRTUAL_SN = c.String(maxLength: 50),
                        WORK_DATE = c.String(maxLength: 8),
                        WORK_SECTION = c.Int(),
                        FULL_WORK_DATE = c.String(maxLength: 10),
                        CFT = c.String(maxLength: 50),
                        PROJECT = c.String(maxLength: 50),
                        MODEL = c.String(maxLength: 50),
                        LINE = c.String(maxLength: 50),
                        STATION = c.String(maxLength: 50),
                        ATE = c.String(maxLength: 50),
                        FIXTURE_CODE = c.String(maxLength: 50),
                        CARRIER_CODE = c.String(maxLength: 50),
                        MO = c.String(maxLength: 50),
                        BOARD_SN = c.String(maxLength: 50),
                        SN = c.String(maxLength: 50),
                        TEST_MODE = c.String(maxLength: 50),
                        TEST_VERSION = c.String(maxLength: 50),
                        START_TIME = c.DateTime(precision: 7, storeType: "datetime2"),
                        END_TIME = c.DateTime(precision: 7, storeType: "datetime2"),
                        ELAPSE_TIME = c.Double(),
                        STATUS = c.String(maxLength: 50),
                        ERROR_CODE = c.String(maxLength: 50),
                        TEST_ITEM_MAP = c.String(),
                        FACTORY = c.String(maxLength: 254),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.TE_TEST_FINAL_DATA_V2",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        FACTORY = c.String(maxLength: 254),
                        CFT = c.String(maxLength: 254),
                        PROJECT = c.String(maxLength: 254),
                        MODEL = c.String(maxLength: 254),
                        LINE = c.String(maxLength: 254),
                        STATION = c.String(maxLength: 254),
                        ATE = c.String(maxLength: 254),
                        WORK_DATE = c.String(maxLength: 254),
                        WORK_SECTION = c.String(maxLength: 254),
                        FULL_WORK_DATE = c.String(maxLength: 254),
                        TEST_ITEM = c.String(maxLength: 254),
                        VALUES = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.TE_TEST_ITEM_DATA",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        VIRTUAL_SN = c.String(maxLength: 254),
                        WORK_DATE = c.String(maxLength: 254),
                        WORK_SECTION = c.Int(),
                        FULL_WORK_DATE = c.String(maxLength: 254),
                        CFT = c.String(maxLength: 254),
                        PROJECT = c.String(maxLength: 254),
                        MODEL = c.String(maxLength: 254),
                        LINE = c.String(maxLength: 254),
                        STATION = c.String(maxLength: 254),
                        ATE = c.String(maxLength: 254),
                        FIXTURE_CODE = c.String(maxLength: 254),
                        CARRIER_CODE = c.String(maxLength: 254),
                        TEST_MODE = c.Int(),
                        TEST_VERSION = c.String(maxLength: 254),
                        MO = c.String(maxLength: 254),
                        BOARD_SN = c.String(maxLength: 254),
                        SN = c.String(maxLength: 254),
                        START_TIME = c.DateTime(precision: 7, storeType: "datetime2"),
                        END_TIME = c.DateTime(precision: 7, storeType: "datetime2"),
                        STATUS = c.Int(),
                        STEP_NUMBER = c.Int(),
                        TEST_ITEM = c.String(maxLength: 254),
                        VALUE_TYPE = c.String(maxLength: 254),
                        VALUE = c.String(maxLength: 254),
                        LSL = c.String(maxLength: 254),
                        USL = c.String(maxLength: 254),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.TEST_LOGS_DATA",
                c => new
                    {
                        MODEL_NAME = c.String(nullable: false, maxLength: 50),
                        TSN = c.String(nullable: false, maxLength: 20),
                        LOGS = c.String(nullable: false, maxLength: 350),
                        MO = c.String(maxLength: 20),
                        RSN = c.String(maxLength: 254),
                        GROUP_NAME = c.String(maxLength: 20),
                        TIME_START = c.DateTime(),
                        TIME_END = c.DateTime(),
                        TEST_TIME = c.Double(),
                        ERROR_CODE = c.String(maxLength: 20),
                        EQUIPMENT = c.String(maxLength: 150),
                        STATION_NAME = c.String(maxLength: 50),
                        MAC = c.String(maxLength: 254),
                        PROGRAM_VERSION = c.String(maxLength: 50, unicode: false),
                        REASON = c.String(maxLength: 1000),
                        OWNER = c.String(maxLength: 100, unicode: false),
                        BUILD_TIME = c.String(maxLength: 25, unicode: false),
                    })
                .PrimaryKey(t => new { t.MODEL_NAME, t.TSN, t.LOGS });
            */
        }
        
        public override void Down()
        {
            /*DropTable("dbo.TEST_LOGS_DATA");
            DropTable("dbo.TE_TEST_ITEM_DATA");
            DropTable("dbo.TE_TEST_FINAL_DATA_V2");
            DropTable("dbo.TE_TEST_FINAL_DATA");
            DropTable("dbo.TE_TEST_DATA");
            DropTable("dbo.TABLE_DATA_FAIL");
            DropTable("dbo.PC_INFORS");
            DropTable("dbo.FATP_TABLE");*/
        }
    }
}
