using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace sdglsys.Web.Controllers
{
    public class ErrorController : Controller
    {
        /// <summary>
        /// 自定义错误页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(int code)
        {
            ViewBag.code = code;
            switch (code)
            {
                case 404:
                    ViewBag.msg = "找不到该页面，可能是输入的参数有误";
                    break;
                case 500:
                    ViewBag.msg = "发生了系统内部错误";
                    break;
                default:
                    ViewBag.msg = "发生了未知错误,错误代码："+code;
                    break;
            }
            
            return View();
        }
    }
}