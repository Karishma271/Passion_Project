namespace Passion_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class Studio : DbMigration
    {


        public override void Up()
        {
            CreateTable(
                "dbo.Studios",
                c => new
                {
                    StudioID = c.Int(nullable: false, identity: true),
                    Name = c.String(),
                    Location = c.String(),
                    Capacity = c.Int(nullable: false),
                    Facilities = c.String(),
                })
                .PrimaryKey(t => t.StudioID);

            CreateTable(
                "dbo.Users",
                c => new
                {
                    UserID = c.Int(nullable: false, identity: true),
                    Username = c.String(),
                    Password = c.String(),
                    Email = c.String(),
                })
                .PrimaryKey(t => t.UserID);

            CreateTable(
                "dbo.Classes",
                c => new
                {
                    ClassID = c.Int(nullable: false, identity: true),
                    StudioID = c.Int(nullable: false),
                    Name = c.String(),
                    ClassName = c.String(),
                    Instructor = c.String(),
                    Schedule = c.DateTime(nullable: false),
                    Duration = c.Int(nullable: false),
                    Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                    Status = c.String(),
                })
                .PrimaryKey(t => t.ClassID)
                .ForeignKey("dbo.Studios", t => t.StudioID, cascadeDelete: true)
                .Index(t => t.StudioID);

            CreateTable(
    "dbo.Bookings",
    c => new
    {
        BookingID = c.Int(nullable: false, identity: true),
        UserID = c.Int(nullable: false),
        ClassID = c.Int(nullable: false),
        BookingDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
        ClassDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
        Status = c.String(),
    })
    .PrimaryKey(t => t.BookingID)
    .ForeignKey("dbo.Classes", t => t.ClassID, cascadeDelete: true)
    .ForeignKey("dbo.Users", t => t.UserID, cascadeDelete: true)
    .Index(t => t.UserID)
    .Index(t => t.ClassID);

        }

        public override void Down()
        {
            DropForeignKey("dbo.Bookings", "UserID", "dbo.Users");
            DropForeignKey("dbo.Bookings", "ClassID", "dbo.Classes");
            DropForeignKey("dbo.Classes", "StudioID", "dbo.Studios");
            DropIndex("dbo.Bookings", new[] { "ClassID" });
            DropIndex("dbo.Bookings", new[] { "UserID" });
            DropIndex("dbo.Classes", new[] { "StudioID" });
            DropTable("dbo.Bookings");
            DropTable("dbo.Classes");
            DropTable("dbo.Users");
            DropTable("dbo.Studios");
        }
    }
}
