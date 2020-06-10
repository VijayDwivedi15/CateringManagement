namespace CateringSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class formenuitems : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MenuCategoryMasters",
                c => new
                    {
                        CategoryID = c.Long(nullable: false, identity: true),
                        CategoryName = c.Long(nullable: false),
                        CategoryImage = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.CategoryID);
            
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
            
        }
        
        public override void Down()
        {
            DropTable("dbo.MenuItems");
            DropTable("dbo.MenuCategoryMasters");
        }
    }
}
