using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ThueXeVn.Models;
using PagedList;
using PagedList.Mvc;

namespace ThueXeVn.Controllers
{
    public class CongTacVienController : Controller
    {
        private thuexevnEntities db = new thuexevnEntities();

        // GET: CongTacVien
        public ActionResult Index()
        {
            if (Config.getCookie("ctvlogged") == "") return RedirectToRoute("congtacviendangnhap");

            var id_ctv = Config.getCookie("ctvlogged").Split('-').Last();
            var ctv = db.ctv_tiepthi.Find(id_ctv);
            if (ctv == null)
            {
                return RedirectToRoute("congtacviendangnhap");
            }

            string url = Request.Url.Authority + Request.RawUrl.ToString();            
            if (Request.ServerVariables["HTTPS"] == "on")
            {
                url = "https://" + url;
            }
            else
            {
                url = "http://" + url;
            }

            string link = url + "/dat-xe?utm=" + ctv.ctv_phone;
            ViewBag.linkshare = link;
            return View();
        }
    }
}