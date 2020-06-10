using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace CateringSite.DAL
{
    public class Global
    {
        public static string result = String.Empty;
        public static string WebsiteUrl()
        {

            switch (ConfigurationManager.AppSettings["Environment"].ToString().ToLower())
            {

                case "local":
                    result = "http://localhost:2226/";
                    break;
                case "development":
                    result = "http://jainmachinery.com/";
                    break;
                case "production":
                    result = "http://jainmachinery.jihuzzur.com/";
                    break;
                default:
                    result = "http://www.hihuzurweb.com/";
                    break;
            }

            return result;
        }
    }
}