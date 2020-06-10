using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using CateringSite.Models;
using System.Net;
using CateringSite.DAL;
using System.IO;
using System.Web.Script.Serialization;
using System.Web.Security;
using System.Text.RegularExpressions;
using Microsoft.Reporting.WebForms;
using System.Configuration;
using System.Data.SqlClient;

namespace CateringSite.Controllers
{
    public class HomeController : Controller
    {
        UserContext db = new UserContext();
        AdminManager admin = new AdminManager();
        public ActionResult Index()
        {
            ViewData["SpecialDishes"] = db.SpecialDishes.OrderByDescending(s => s.DishID).ToList();

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


        [HttpPost]

        public ActionResult Contact(Int64 ContactID = 0, string Name = null, string MobileNo = null, string EmailID = null, string Subject = null, string Message = null)
        {
            string Status = null;
            Models.UserContact contact = new UserContact();

            contact.CreatedDate = DateTime.Now;
            contact.EmailID = EmailID;
            contact.Message = Message;
            contact.MobileNo = MobileNo;
            contact.Name = Name;
            contact.Subject = Subject;

            db.UserContact.Add(contact);
            db.SaveChanges();
            Status = "Succeeded";

            TempData["example"] = Status;
            return RedirectToAction("Contact", "Home");

        }

        public ActionResult Menu()
        {
            ViewData["Menu"] = db.MenuCategoryMaster.ToList();
            return View();
        }

        [HttpPost]
        public ActionResult GetMenuItem(Int64 Category = 0)
        {
            List<SelectListItem> ItemId = new List<SelectListItem>();
            var members = admin.GetAllMenuItems().Where(e => e.CategoryID == Category).ToList();

            return Json(members, JsonRequestBehavior.AllowGet);
        }

        public ActionResult MenuOrderOnline()
        {
            ViewData["Menu"] = db.MenuCategoryMaster.ToList();
            ViewData["Allmenu"] = admin.GetAllMenuItems().ToList();
            return View();
        }

        public ActionResult MenuOrderDetail(string MenuItemID = null, int len = 0)
        {
            var menulist = admin.GetAllMenuItems().ToList();

            ViewData["Menumaster"] = db.MenuCategoryMaster.ToList();

            List<Int64> add_list = new List<Int64>();

            for (int i = 0; i < len; i++)
            {
                string clientid = MenuItemID.Split(',')[i].ToString();
                string client_Id = Regex.Replace(clientid, @"[^0-9]+", " ");
                add_list.Add(Convert.ToInt64(client_Id));
                Int64 clientids = Convert.ToInt64(client_Id);
                var clientdetails = admin.GetAllMenuItems().Where(c => c.DetailMenuID == clientids).FirstOrDefault();

            }


            menulist = menulist.Where(x => x.DetailMenuID != 0 && add_list.Contains((Int64)x.DetailMenuID)).ToList();
            ViewData["MenuList"] = menulist;


            return View();
        }


        [HttpPost]

        public ActionResult PostBookMenu(DateTime BookDate, TimeSpan BookTime, Int64[] DetailMenuID, string ClientName = null, string NoofPeople = null, string Email = null, string MobileNo = null, string Address = null)
        {
            string Status = "";

            Models.MenuBookMain main = new MenuBookMain();
            Models.MenuBookDetail detail = new MenuBookDetail();

            main.Address = Address;
            main.BookDate = BookDate;
            main.BookedOn = DateTime.Now;
            main.BookTime = BookTime;
            main.ClientName = ClientName;
            main.Email = Email;
            main.MobileNo = MobileNo;
            main.NoofPeople = NoofPeople;

            db.MenuBookMain.Add(main);
            int i = db.SaveChanges();

            Int64 lastid = 0;

            if(i>0)
            {
                lastid = db.MenuBookMain.Max(s=>s.BookingID);

                for (int j = 0; j < DetailMenuID.Length; j++)
                {
                    detail.DetailMenuID = DetailMenuID[j];
                    detail.BookingID = lastid;
                    db.MenuBookDetail.Add(detail);
                    db.SaveChanges();
                    Status = "Succeeded";

                }
            }

            TempData["Example"] = Status;
            TempData["Result"] = lastid;
            return Json(Status, JsonRequestBehavior.AllowGet);
        }


        public ActionResult PrintMenuOrder(Int64 BookingID=0)
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



        public ActionResult Gallery()
        {
            return View();
        }

        public ActionResult GalleryImages()
        {
            return View();
        }


        public ActionResult Services()
        {
            return View();
        }



    }
}