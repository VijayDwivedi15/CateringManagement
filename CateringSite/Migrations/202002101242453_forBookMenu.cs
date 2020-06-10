namespace CateringSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class forBookMenu : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MenuBookDetails",
                c => new
                    {
                        BookingDetailID = c.Long(nullable: false, identity: true),
                        BookingID = c.Long(nullable: false),
                        DetailMenuID = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.BookingDetailID);
            
            CreateTable(
                "dbo.MenuBookMains",
                c => new
                    {
                        BookingID = c.Long(nullable: false, identity: true),
                        ClientName = c.String(),
                        NoofPeople = c.String(),
                        BookDate = c.DateTime(nullable: false),
                        BookTime = c.Time(nullable: false, precision: 7),
                        Email = c.String(),
                        MobileNo = c.String(),
                        Address = c.String(),
                        BookedOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.BookingID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.MenuBookMains");
            DropTable("dbo.MenuBookDetails");
        }
    }
}
