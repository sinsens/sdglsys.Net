using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using sdglsys.BLL;

namespace sdglsys.Web.Controllers
{
    public class ToolsController : Controller
    {
        // GET: Tools
        public ActionResult Index()
        {
            var txt = Request["txt"];
            if (txt != null) {
                Response.Write(Utils.hashpwd(txt));
                Response.End();
            }
            return View();
        }
    }
}