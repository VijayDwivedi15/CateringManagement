namespace CateringSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class formenuitemsupdates : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.MenuCategoryMasters", "CategoryName", c => c.String());
            AlterColumn("dbo.MenuCategoryMasters", "CategoryImage", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.MenuCategoryMasters", "CategoryImage", c => c.Long(nullable: false));
            AlterColumn("dbo.MenuCategoryMasters", "CategoryName", c => c.Long(nullable: false));
        }
    }
}
