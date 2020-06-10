using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CateringSite.Models
{
    public class AllMenus
    {
        public Int64 MainMenuID { get; set; }
        public Int64 CategoryID { get; set; }
        public Int64 DetailMenuID { get; set; }
        public string CategoryName { get; set; }
        public string ItemName { get; set; }
        public string ItemImage { get; set; }
        public string ItemDescription { get; set; }
    }
}