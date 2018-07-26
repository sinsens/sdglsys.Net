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
            var hash = Request["hash"];
            var txt = Request["txt"];
            if (hash != null&&hash.Length>20) {
                Response.Write(Utils.checkpw(txt, hash)?true:false);
                Response.End();
            }
            if (txt != null&&txt.Length>0) {
                var msg = new Msg();
                msg.content = new
                {
                    text = txt,
                    bcrypt_hash = Utils.hashpwd(txt),
                };
                Response.Write(msg.ToJson());
                Response.End();
            }
            return View();
        }
    }
}