using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace sdglsys.Web.Controllers
{
    public class DefaultController : Controller
    {
        // GET: Default
        public ActionResult Errors()
        {
            var statusCode = Response.StatusCode;
            ViewBag.code = statusCode;
            switch (statusCode) {
                case 404:
                    ViewBag.msg = "找不到该页面，可能是输入的参数有误";
                    break;
                case 500:
                    ViewBag.msg = "发生了系统内部错误";
                    break;
                default:
                    ViewBag.msg = Response.StatusDescription;
                    break;
            }
            return View();
        }
    }
}