using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using sdglsys.DbHelper;
using sdglsys.Web;

namespace sdglsys.Web.Controllers
{
    public class ToolsController : Controller
    {
        // GET: Tools
        public ActionResult Index()
        {
            var txt = Request["txt"];
            if (txt != null&&txt.Length>0) {
                Response.Write(Utils.hashpwd(txt));
                Response.End();
            }
            return View();
        }
    }
}