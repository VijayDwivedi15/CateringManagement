using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using CateringSite.Models;
using CateringSite.Migrations;

namespace CateringSite.DAL
{
    public class UserContext :DbContext
    {
        public UserContext()
            : base("DefaultConnection")
        {
            this.Configuration.LazyLoadingEnabled = false;
        }

        public virtual DbSet<CateringSite.Models.UserContact> UserContact { get; set; }
        public virtual DbSet<CateringSite.Models.MenuCategoryMaster> MenuCategoryMaster { get; set; }
        public virtual DbSet<CateringSite.Models.MenuItemsMain> MenuItemsMain { get; set; }
        public virtual DbSet<CateringSite.Models.MenuItemDetail> MenuItemDetail { get; set; }
        public virtual DbSet<CateringSite.Models.SpecialDishes> SpecialDishes { get; set; }

        public virtual DbSet<CateringSite.Models.MenuBookMain> MenuBookMain { get; set; }
        public virtual DbSet<CateringSite.Models.MenuBookDetail> MenuBookDetail { get; set; }

    }
}