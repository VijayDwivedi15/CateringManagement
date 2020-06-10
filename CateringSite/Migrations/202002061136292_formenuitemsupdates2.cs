namespace CateringSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class formenuitemsupdates2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MenuItemDetails",
                c => new
                    {
                        DetailMenuID = c.Long(nullable: false, identity: true),
                        MainMenuID = c.Long(nullable: false),
                        ItemName = c.String(),
                        ItemImage = c.String(),
                        ItemPrice = c.Single(),
                        ItemDescription = c.String(),
                    })
                .PrimaryKey(t => t.DetailMenuID);
            
            CreateTable(
                "dbo.MenuItemsMains",
                c => new
                    {
                        MainMenuID = c.Long(nullable: false, identity: true),
                        CategoryID = c.Long(nullable: false),
                        CreatedBy = c.String(),
                        CreatedOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.MainMenuID);
            
            DropTable("dbo.MenuItems");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.MenuItems",
                c => new
                    {
                        MenuID = c.Long(nullable: false, identity: true),
                        CategoryID = c.Long(nullable: false),
                        ItemName = c.String(),
                        Price = c.Single(),
                        ItemImage = c.String(),
                        ItemDescription = c.String(),
                        CreatedBy = c.String(),
                        CreatedOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.MenuID);
            
            DropTable("dbo.MenuItemsMains");
            DropTable("dbo.MenuItemDetails");
        }
    }
}
