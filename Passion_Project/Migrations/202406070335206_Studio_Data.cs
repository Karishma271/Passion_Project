namespace Passion_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Studio_Data : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Bookings");
            AddColumn("dbo.Bookings", "BookingID", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.Classes", "StudioName", c => c.String());
            AddPrimaryKey("dbo.Bookings", "BookingID");
            DropColumn("dbo.Bookings", "ReservationID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Bookings", "ReservationID", c => c.Int(nullable: false, identity: true));
            DropPrimaryKey("dbo.Bookings");
            DropColumn("dbo.Classes", "StudioName");
            DropColumn("dbo.Bookings", "BookingID");
            AddPrimaryKey("dbo.Bookings", "ReservationID");
        }
    }
}
