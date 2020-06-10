using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace CateringSite.Models
{
    public class UserContact
    {
        [Key]
        public Int64 ContactID { get; set; }
        public string Name { get; set; }
        public string MobileNo { get; set; }
        public string EmailID { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class MenuCategoryMaster
    {
        [Key]

        public Int64 CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string CategoryImage { get; set; } 
    }

    public class MenuItemsMain
    {
        [Key]
        public Int64 MainMenuID { get; set; }
        public Int64 CategoryID { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
    }

    public class MenuItemDetail
    {
        [Key]
        public Int64 DetailMenuID { get; set; }
        public Int64 MainMenuID { get; set; }
        public string ItemName { get; set; }
        public string ItemImage { get; set; }
        public string ItemDescription { get; set; }
    }


    public class SpecialDishes
    {
        [Key]
        public Int64 DishID { get; set; }
        public string DishName { get; set; }
        public string DishImage { get; set; }
        public DateTime CreatedOn { get; set; }
    }

    public class MenuBookMain
    {
        [Key]
        public Int64 BookingID { get; set; }
        public string ClientName { get; set; }
        public string NoofPeople { get; set; }
        public DateTime BookDate { get; set; }
        public TimeSpan BookTime { get; set; }
        public string Email { get; set; }
        public string MobileNo { get; set; }
        public string Address { get; set; }
        public DateTime BookedOn { get; set; }
    }


    public class MenuBookDetail
    {
        [Key]

        public Int64 BookingDetailID { get; set; }
        public Int64 BookingID { get; set; }
        public Int64 DetailMenuID { get; set; }
    }
}