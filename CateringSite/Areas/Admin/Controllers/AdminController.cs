using CateringSite.DAL;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CateringSite.Areas.Admin.Controllers
{
    public class AdminController : Controller
    {
        UserContext db = new UserContext();
        AdminManager admin = new AdminManager();

        //
        // GET: /Admin/Admin/
        public ActionResult Index()
        {
            ViewBag.totalcontact = db.UserContact.Count();
            ViewBag.TotalMenu = db.MenuCategoryMaster.Count();
            ViewBag.AllMenuItems = db.MenuItemDetail.Count();
            ViewBag.MenuBooked = db.MenuBookMain.Count();


            return View();
        }



        [HttpPost]
        public ActionResult LogOff()
        {
            Session.RemoveAll();
            Session.Abandon();
            Session.Clear();
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            //return RedirectToAction("UserCP", "Dashboard", new { area = "Admin" });
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }



        public ActionResult AllContactForm()
        {
            ViewData["AllContacts"] = db.UserContact.OrderByDescending(s => s.ContactID).ToList();
            return View();
        }

        public ActionResult DeleteWebsiteContacts(Int64 ContactID = 0)
        {
            string Status = "NA";

            var StatusExist = db.UserContact.Find(ContactID);
            if (StatusExist != null)
            {

                db.UserContact.Remove(StatusExist);
                int result = db.SaveChanges();
                if (result == 1)
                    Status = "Deleted";
                else

                    Status = "Unsucceeded";
            }
            else
            {

                Status = "Unsucceeded";
            }

            TempData["Example"] = Status;
            return RedirectToAction("AllContactForm", "Admin", new { area = "Admin" });
        }




        //--------------------------Menu Master Form------------------------

        public ActionResult MenuCatgory(Int64 CategoryID=0)
        {
            Models.MenuCategoryMaster pm = new Models.MenuCategoryMaster();

            if (CategoryID > 0)
            {
                var exist = db.MenuCategoryMaster.Where(s => s.CategoryID == CategoryID).FirstOrDefault();
                pm.CategoryName = exist.CategoryName;
                pm.CategoryID = CategoryID;
                pm.CategoryImage = exist.CategoryImage;
                ViewBag.CategoryID = CategoryID;
            }

            ViewData["MenuMaster"] = db.MenuCategoryMaster.ToList();
            return View(pm);
           
        }


        [HttpPost]

        public ActionResult MenuCatgory(HttpPostedFileBase CategoryImage, Int64 CategoryID = 0, string CategoryName = null)
        {
            string userid = User.Identity.GetUserId();
            string Status = "";

            string productimg = null;
            productimg = "MenuImages/" + CategoryImage.FileName;

            Models.MenuCategoryMaster pm = new Models.MenuCategoryMaster();

            var Exist = db.MenuCategoryMaster.Where(s => s.CategoryID == CategoryID).FirstOrDefault();

            if (Exist == null)
            {
                pm.CategoryImage = productimg;
                pm.CategoryName = CategoryName;


                db.MenuCategoryMaster.Add(pm);
                int i = db.SaveChanges();

                Status = "Succeeded";
                string path = Server.MapPath("~/MenuImages/");

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                CategoryImage.SaveAs(path + CategoryImage.FileName);

            }
            else
            {
               
                Exist.CategoryImage = productimg;
                Exist.CategoryName = CategoryName;
               

                db.SaveChanges();

                Status = "Succeeded";
                string path = Server.MapPath("~/MenuImages/");

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                CategoryImage.SaveAs(path + CategoryImage.FileName);
            }

            TempData["Example"] = Status;
            return RedirectToAction("MenuCatgory", "Admin", new { area = "Admin" });

        }


        public ActionResult DeleteMenuCategory(Int64 CategoryID = 0)
        {
            string Status = "NA";

            var StatusExist = db.MenuCategoryMaster.Find(CategoryID);
            if (StatusExist != null)
            {

                db.MenuCategoryMaster.Remove(StatusExist);
                int result = db.SaveChanges();
                if (result == 1)
                    Status = "Deleted";
                else

                    Status = "Unsucceeded";
            }
            else
            {

                Status = "Unsucceeded";
            }
            TempData["Deleteexample"] = Status;
            return RedirectToAction("MenuCatgory", "Admin", new { area = "Admin" });
        }


        private void PopulateCategory(object selectedvalue = null)
        {
            var productlist = db.MenuCategoryMaster.ToList();
            var product = new SelectList(productlist, "CategoryID", "CategoryName", selectedvalue);
            ViewBag.CategoryID = product;
        }

        public ActionResult MenuItems()
        {
            PopulateCategory();
            return View();
        }

        [HttpPost]
        public ActionResult MenuItems(HttpPostedFileBase[] ItemImage, Int64[] DetailMenuID, string[] ItemName, string[] ItemDescription, Int64 CategoryID = 0)
        {
            string userid = User.Identity.GetUserId();
            string Status = "";

            Models.MenuItemsMain main = new Models.MenuItemsMain();
            Models.MenuItemDetail detail = new Models.MenuItemDetail();

            main.CreatedBy = userid;
            main.CreatedOn = DateTime.Now;
            main.CategoryID = CategoryID;

            db.MenuItemsMain.Add(main);
            int i = db.SaveChanges();
            int k = 0;

            Int64 Lastid = 0;

            if (i > 0)
            {
                Lastid = db.MenuItemsMain.Where(p => p.CreatedBy == userid).Max(p => p.MainMenuID);

                for (int j = 0; j < ItemName.Length; j++)
                {


                    detail.MainMenuID = Lastid;
                    detail.ItemName = ItemName[j];
                    detail.ItemImage = ItemImage[j].FileName;
                    detail.ItemDescription = ItemDescription[j];

                    db.MenuItemDetail.Add(detail);
                    k = db.SaveChanges();
                    Status = "Succeeded";

                    string path = Server.MapPath("~/MenuImages/");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    ItemImage[j].SaveAs(path + Path.GetFileName(ItemImage[j].FileName));

                }

            }

            TempData["Example"] = Status;
            return RedirectToAction("AllMenuItems", "Admin", new { area = "Admin" });
        }


        public ActionResult AllMenuItems()
        {
            var allmenuitems = admin.GetAllMenuItems();
            ViewData["MenuItems"] = allmenuitems;

            return View();
        }




        //--------------------------Special Dishes------------------------------

        public ActionResult SpecialDish(Int64 DishID=0)
        {
            Models.SpecialDishes pm = new Models.SpecialDishes();

            if (DishID > 0)
            {
                var exist = db.SpecialDishes.Where(s => s.DishID == DishID).FirstOrDefault();
                pm.DishName = exist.DishName;
                pm.DishID = DishID;
                pm.DishImage = exist.DishImage;
                ViewBag.DishID = DishID;
            }

            ViewData["SpecialDish"] = db.SpecialDishes.ToList();
            return View(pm);
        }

        [HttpPost]
        public ActionResult SpecialDish(HttpPostedFileBase DishImage, Int64 DishID = 0, string DishName = null)
        {
            string userid = User.Identity.GetUserId();
            string Status = "";

            string productimg = null;
            productimg = "MenuImages/" + DishImage.FileName;

            Models.SpecialDishes pm = new Models.SpecialDishes();

            var Exist = db.SpecialDishes.Where(s => s.DishID == DishID).FirstOrDefault();

            if (Exist == null)
            {
                pm.DishImage = productimg;
                pm.DishName = DishName;
                pm.CreatedOn = DateTime.Now;

                db.SpecialDishes.Add(pm);
                int i = db.SaveChanges();

                Status = "Succeeded";
                string path = Server.MapPath("~/MenuImages/");

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                DishImage.SaveAs(path + DishImage.FileName);

            }
            else
            {

                Exist.DishImage = productimg;
                Exist.DishName = DishName;
                Exist.CreatedOn = DateTime.Now;


                db.SaveChanges();

                Status = "Succeeded";
                string path = Server.MapPath("~/MenuImages/");

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                DishImage.SaveAs(path + DishImage.FileName);
            }

            TempData["Example"] = Status;
            return RedirectToAction("SpecialDish", "Admin", new { area = "Admin" });
        }


        public ActionResult DeleteSpecialDish(Int64 DishID = 0)
        {
            string Status = "NA";

            var StatusExist = db.SpecialDishes.Find(DishID);
            if (StatusExist != null)
            {

                db.SpecialDishes.Remove(StatusExist);
                int result = db.SaveChanges();
                if (result == 1)
                    Status = "Deleted";
                else

                    Status = "Unsucceeded";
            }
            else
            {

                Status = "Unsucceeded";
            }
            TempData["Deleteexample"] = Status;
            return RedirectToAction("SpecialDish", "Admin", new { area = "Admin" });
        }


        //------------- Booked Menu Detail-------------------------

        public ActionResult BookedMenu()
        {
            ViewData["AllBookedMenu"] = db.MenuBookMain.ToList();
            return View();  
        }

        public ActionResult PrintMenuOrder(Int64 BookingID = 0)
        {
            string UserID = User.Identity.GetUserId();
            ReportViewer rptViewer = new ReportViewer();
            rptViewer.LocalReport.ReportPath = "Report/SunshineCatering.rdlc";
            string thisConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection thisConnection = new SqlConnection(thisConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = thisConnection;

            if (BookingID != 0)
                cmd.Parameters.Add(new SqlParameter("@BookingID", BookingID));
            string MyDataSource1 = "Get_BookMenuMainReport";
            cmd.CommandText = string.Format(MyDataSource1);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            SqlDataAdapter daN = new SqlDataAdapter(cmd);
            System.Data.DataSet DataSet1 = new System.Data.DataSet();
            daN.Fill(DataSet1);
            ReportDataSource reportDataSource = new ReportDataSource();
            reportDataSource.Name = "DataSet1";
            reportDataSource.Value = DataSet1.Tables[0];
            ReportParameter[] parms = new ReportParameter[1];
            parms[0] = new ReportParameter("BookingID", BookingID.ToString());

            SqlCommand cmd2 = new SqlCommand();
            cmd2.Connection = thisConnection;
            if (BookingID != 0)
                cmd2.Parameters.Add(new SqlParameter("@BookingID", BookingID));
            string MyDataSource2 = "Get_BookMenuItemsReport";
            cmd2.CommandText = string.Format(MyDataSource2);
            cmd2.CommandType = System.Data.CommandType.StoredProcedure;
            SqlDataAdapter daN2 = new SqlDataAdapter(cmd2);
            System.Data.DataSet DataSet2 = new System.Data.DataSet();
            daN2.Fill(DataSet2);
            ReportDataSource reportDataSource2 = new ReportDataSource();
            reportDataSource2.Name = "DataSet2";
            reportDataSource2.Value = DataSet2.Tables[0];
            ReportParameter[] parms2 = new ReportParameter[1];
            parms2[0] = new ReportParameter("BookingID", BookingID.ToString());



            rptViewer.LocalReport.EnableExternalImages = true;
            rptViewer.LocalReport.SetParameters(parms);
            rptViewer.LocalReport.SetParameters(parms2);
            rptViewer.LocalReport.DataSources.Add(reportDataSource);
            rptViewer.LocalReport.DataSources.Add(reportDataSource2);

            rptViewer.ProcessingMode = ProcessingMode.Local;
            rptViewer.SizeToReportContent = true;
            rptViewer.ZoomMode = ZoomMode.PageWidth;
            //rptViewer.Width = Unit.Percentage(99);
            //rptViewer.Height = Unit.Pixel(1000);
            var reList = rptViewer.LocalReport.ListRenderingExtensions();
            string mimeType = string.Empty;
            string encoding = string.Empty;
            rptViewer.LocalReport.Refresh();
            string extension = string.Empty;
            byte[] bytes = rptViewer.LocalReport.Render("PDF", null);
            Response.Buffer = true;
            Response.Clear();
            Response.ContentType = mimeType;
            Response.AddHeader("content-disposition", "inline; filename=?SunshineCatering.pdf");
            Response.BinaryWrite(bytes); // create the file new
            Response.Flush();
            return View();
        }

	}
}