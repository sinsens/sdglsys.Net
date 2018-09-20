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
                Response.Write(XUtils.CheckPasswd(txt, hash)?true:false);
                Response.End();
            }
            if (txt != null&&txt.Length>0) {
                var msg = new Msg();
                msg.content = new
                {
                    text = txt,
                    bcrypt_hash = XUtils.hashpwd(txt),
                };
                Response.Write(msg.ToJson());
                Response.End();
            }
            return View();
        }

        public void Stat() {
            string cookie= null;
            if (Request.Cookies.Get("Session_ID") == null)
            {
                Response.SetCookie(new HttpCookie("Session_ID", Session.SessionID));
                cookie = Session.SessionID;
            }
            else {
                cookie = Request.Cookies.Get("Session_ID").Value;
            }
            
            Response.Write(new Msg {
                content = new {
                    Is_NewSession = Session.IsNewSession,
                    Session_Id = Session.SessionID,
                    Cookie_Session_Id = cookie ,
                     Add_ten_minutes = DateTime.Now.Subtract(DateTime.Now.AddMinutes(10)).TotalMinutes
                 }
            }.ToJson());
        }
    }
}