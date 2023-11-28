using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace THPTUDWeb
{
	public class MvcApplication : System.Web.HttpApplication
	{
		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);
		}
		//Tạo mới Session
		protected void Session_Start()
		{
            //Lưu mã người đăng nhập quản lý
            Session["UserID"] = "1";
            Session["MyCart"] = "";//Khởi tạo giá trị = NULL
			//Lưu thông tin đăng nhập của người dùng
            Session["UserCustomer"] = "";

        }

    }
}
